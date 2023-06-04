using Project.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Project.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index(HttpPostedFileBase studentFile, HttpPostedFileBase markerFile, string setting)
        {
            var postedstudentFile = Request.Files["studentFile"]; //get the files from the input and store it
            var postedmarkerFile = Request.Files["markerFile"]; //get the files for marker from  the input and store it in
            string error = "File Upload unsuccessful - please validate that both files are the same type and are both of either .cs .c or .cpp extensions"; //error message, re-written if it is sucessful

            string studentFileString = ""; //string to hold line text for student file output   
            string markerFileString = ""; //string to hold line text for marker file output
            string stats = ""; //string to hold text for statistics

            string[] studentFileStringArray = new string[0];
            string[] markerFileStringArray = new string[0];

            List<string> tempList = new List<string>();
            List<float> tempListScores = new List<float>();
            List<string> tempLines = new List<string>();
            List<float> tempListScoresMethod = new List<float>();
            List<string> tempLinesMethod = new List<string>();
            List<float> averageScore = new List<float>();
            List<float> averageScoreLocation = new List<float>();
            List<float> numInEachMethod = new List<float>();
            List<int> methodLocations = new List<int>();

            if (String.IsNullOrEmpty(setting)) //if no setting is selected
                setting = "Jaro-Winkler";

            float currentScore = 0;

            if (postedstudentFile != null && postedmarkerFile != null) //if files are not empty
            {
                if (IsFileSupported(postedstudentFile) == true && IsFileSupported(postedmarkerFile) == true) //if both files are of a valid format, calles IsFileSupported method
                {
                    if (Path.GetExtension(postedstudentFile.FileName).ToString() == Path.GetExtension(postedmarkerFile.FileName).ToString()) //if both files are the same format
                    {
                        using (StreamReader reader1 = new StreamReader(postedstudentFile.InputStream)) //read input stream of student file
                        {
                            string line;
                            while ((line = reader1.ReadLine()) != null) //if line is not null add the line to the list
                            {
                                tempList.Add(line);
                            }
                        }
                        studentFileStringArray = tempList.ToArray(); //copy from temp to permenant string array
                        tempList.Clear(); //clear temp list

                        using (StreamReader reader2 = new StreamReader(postedmarkerFile.InputStream))  //read input stream of marker file
                        {
                            string line;
                            while ((line = reader2.ReadLine()) != null) //if line is not null add the line to the list
                            {
                                tempList.Add(line);
                            }
                        }
                        markerFileStringArray = tempList.ToArray(); //copy from temp to permenant string array

                        for (int i = 0; i < markerFileStringArray.Length; i++) //initialises all relevant list to a length which is big enough to hold all variables
                        {
                            tempListScores.Add(0);
                            tempLines.Add("null");
                            tempListScoresMethod.Add(0);
                            tempLinesMethod.Add("null");
                            averageScore.Add(0);
                            averageScoreLocation.Add(0);
                            methodLocations.Add(0);
                            numInEachMethod.Add(0);
                        }

                        for (int i = 0; i < markerFileStringArray.Length; i++) //initialises all relevant list to a length which is big enough to hold all variables
                        {
                            averageScore.Add(0);
                            averageScoreLocation.Add(0);
                            numInEachMethod.Add(0);
                        }

                        error = "File Upload successful"; //file is valid and has been read in
                        float average = 0; //average variable
                        int numOfValuesStudent = 0; //count for num of significant lines
                        int numOfValuesMarker = 0; //for for nu  of significant lines

                        nGramSize(2); //initialises a ngram encase using n-gram measure

                        for (int i = 0; i < markerFileStringArray.Length; i++) //go through all marker elements
                        {
                            if (markerFileStringArray[i].Replace(" ", "") == "{" || markerFileStringArray[i].Replace(" ", "") == "}" || markerFileStringArray[i].Replace(" ", "") == "") //if the line is empty or is a singular bracket
                            {
                                tempListScores[i] = 2.00f; //error value
                                tempLines[i] = "N/A"; //error value
                            }
                            else
                            {
                                for (int j = 0; j < studentFileStringArray.Length; j++) //go through all student elements
                                {
                                    if (studentFileStringArray[j].Replace(" ","") != "{" || studentFileStringArray[j].Replace(" ", "") != "}" || studentFileStringArray[j].Replace(" ", "") == "") //compare if it is a significant line
                                    {
                                        if(setting == "N-Grams") //if n-gram call n-gram method
                                            currentScore = nGramDistance(markerFileStringArray[i], studentFileStringArray[j]);
                                        else
                                            if(setting == "Levenshtein") //if Levenshtein call Levenshtein method
                                                currentScore = LevenshteinDistance(markerFileStringArray[i], studentFileStringArray[j]);
                                            else //if all else fails call Jaro-Winkler
                                                currentScore = JaroWinklerDistance(markerFileStringArray[i], studentFileStringArray[j]);

                                        if (currentScore > tempListScores[i]) //Store the best line score and lcoation
                                        {
                                            tempListScores[i] = currentScore;
                                            tempLines[i] = j.ToString();
                                        }
                                    }
                                    currentScore = 0; //reset for next marker element
                                }

                                if (tempListScores[i] != 2.00) //if not error value
                                {
                                    average = average + tempListScores[i]; //sum for the average score
                                    numOfValuesStudent++; //counting how many student lines
                                }
                            }
                        }

                        for (int i = 0; i < studentFileStringArray.Length; i++) //go through student file
                        {
                            if (studentFileStringArray[i].Replace(" ", "") != "{" && studentFileStringArray[i].Replace(" ", "") != "}" && studentFileStringArray[i].Replace(" ", "") != "") //count all significant lines
                            {
                                numOfValuesMarker++; //counting how many marker lines
                            }
                        }

                        average = average / numOfValuesStudent; //calculate average final

                        int markerBracket = 0;
                        int studentBracket = 0;

                        int studentBracketBefore = 0;
                        int markerBracketBefore = 0;

                        int markerMethodLines = 0;
                        int studentMethodLines = 0;

                        int markerMethodLineStart = 0;
                        int studentMethodLineStart = 0;

                        float bestScore = 0;
                        int currentStudentMethod = 0;
                        int currentMarkerMethod = 0;

                        int numOfStudentMethods = 0;

                        for (int i = 0; i < markerFileStringArray.Length; i++) // go through all marker lines
                        {
                            markerBracketBefore = markerBracket; //set bracket before to last bracket

                            if (markerFileStringArray[i].Contains("{")) //if open bracket increment by 1
                                markerBracket++;

                            if (markerFileStringArray[i].Contains("}")) //if close bracket decrease by 1
                                markerBracket--;

                            if (markerBracket >= 3 && markerBracketBefore == 2) //if start of method, store method start location
                                markerMethodLineStart = i;

                            if (markerBracket >= 3) //if a method compare to the other file
                            {
                                markerMethodLines++; //increase marker method line count for each line iterated

                                for (int j = 0; j < studentFileStringArray.Length; j++)  //go through all student lines
                                {
                                    studentBracketBefore = studentBracket;  //set bracket before to last bracket

                                    if (studentFileStringArray[j].Contains("{")) //if open bracket increment by 1
                                        studentBracket++;

                                    if (studentFileStringArray[j].Contains("}")) //if close bracket decrease by 1
                                        studentBracket--;

                                    if (studentBracket >= 3 && studentBracketBefore == 2) //if start of method, store method start location
                                        studentMethodLineStart = j;

                                    if (studentBracket >= 3) //if a method compare to the other file
                                    {
                                        studentMethodLines++;

                                        if (setting == "N-Grams") //if n-gram call n-gram method
                                            currentScore = nGramDistance(markerFileStringArray[i], studentFileStringArray[j]);
                                        else
                                            if (setting == "Levenshtein") //if Levenshtein call Levenshtein method
                                                currentScore = LevenshteinDistance(markerFileStringArray[i], studentFileStringArray[j]);
                                            else //if all else fails call Jaro-Winkler
                                                currentScore = JaroWinklerDistance(markerFileStringArray[i], studentFileStringArray[j]);

                                        if(currentScore > bestScore) //store best match for the line of marker method in this current student method
                                            bestScore = currentScore;
                                    }
                                    else
                                        if(studentBracketBefore == 3) //if exiting a method
                                        {
                                            numInEachMethod[currentStudentMethod] = studentMethodLines; //store the length of student method
                                            averageScore[currentStudentMethod] += bestScore; //store sum of best scores of this student method compared to marker method
                                            averageScoreLocation[currentStudentMethod] = studentMethodLineStart; //store location of this student method compared to marker method
                                            bestScore = 0; //reset best score
                                            studentMethodLines = 0; //reset student method lines
                                            if (currentMarkerMethod == 0 && i == markerMethodLineStart) //used for counting and storing how many student methods there are
                                                numOfStudentMethods++;
                                            currentStudentMethod++; //used to iterate through lists
                                        }
                                }
                                currentStudentMethod = 0; //reset to 0 when compared all method of student
                            }
                            else
                                if (markerBracketBefore == 3) //if exiting method of marker file
                                {
                                    for(int m = 0; m < averageScore.Count; m++) //go through average results for this current marker method
                                    {
                                        if (tempListScoresMethod[currentMarkerMethod] < averageScore[m] / markerMethodLines) //if average is better for this method store location and score 
                                        {
                                            tempListScoresMethod[currentMarkerMethod] = averageScore[m] / markerMethodLines; 
                                            tempLinesMethod[currentMarkerMethod] = averageScoreLocation[m].ToString();
                                        }
                                    }
                                    methodLocations[currentMarkerMethod] = markerMethodLineStart; //store the location of marker methods
                                    markerMethodLines = 0; //reset line count as starting next method
                                    currentMarkerMethod++; //increment method of marker
                                    for (int m = 0; m < averageScore.Count; m++) //reset average score for next marker method
                                        averageScore[m] = 0;
                            }
                        }

                        //writing the stats to a string for output
                        stats = "Similarity Measure: " + setting + "\nAverage Similarity Score of Lines:\t\t\t[" + average.ToString() + "]" + "\nAverage Similarity Score of Methods:\t\t\t[" + (tempListScoresMethod.Sum()/currentMarkerMethod).ToString() + "]" + "\nStudent File Length Of Significant Lines:\t\t" + numOfValuesMarker + "\nMarker File Length Of Significant Lines:\t\t" + numOfValuesStudent + "\n";
                        stats = stats + "Number of Marker Methods:\t\t\t\t" + currentMarkerMethod + "\nNumber of Student Methods:\t\t\t\t" + numOfStudentMethods + "\n";


                        for (int i = 0; i < currentMarkerMethod; i++) //loop through the arrays writing the location of the method match start based off the position of the bracket
                        {
                            int numStud = Int32.Parse(tempLinesMethod[i]);
                            int numMark = methodLocations[i];
                            if (studentFileStringArray[numStud].Replace(" ", "").Length == 1)
                            {
                                if(markerFileStringArray[numMark].Replace(" ", "").Length == 1)
                                    stats += "\nMarker Method:\t\t" + markerFileStringArray[numMark - 1] + "\nStudent Match Method:\t" + studentFileStringArray[numStud - 1] + "\nLine of matching Method:" + tempLinesMethod[i] + "\tScore of Method: " + tempListScoresMethod[i] + "\n" ;
                                else
                                    stats += "\nMarker Method:\t\t" + markerFileStringArray[numMark] + "\nStudent Match Method:\t" + studentFileStringArray[numStud - 1] + "\nLine of matching Method:" + tempLinesMethod[i] + "\tScore of Method: " + tempListScoresMethod[i] + "\n";
                            }
                            else
                            {
                                if (markerFileStringArray[numMark].Replace(" ", "").Length == 1)
                                    stats += "\nMarker Method:\t\t" + markerFileStringArray[numMark - 1] + "\nStudent Match Method:\t" + studentFileStringArray[numStud] + "\nLine of matching Method:" + tempLinesMethod[i] + "\tScore of Method: " + tempListScoresMethod[i] + "\n";
                                else
                                    stats += "\nMarker Method:\t\t" + markerFileStringArray[numMark] + "\nStudent Match Method:\t" + studentFileStringArray[numStud] + "\nLine of matching Method:" + tempLinesMethod[i] + "\tScore of Method: " + tempListScoresMethod[i] + "\n";
                            }
                        }

                        for (int i = 0; i < studentFileStringArray.Length; i++) //count how many values within the marker file 
                            if (studentFileStringArray[i].Replace(" ", "") != "{" || studentFileStringArray[i].Replace(" ", "") != "}" || studentFileStringArray[i].Replace(" ", "") != "" )
                                numOfValuesMarker++;

                        for (int i = 0; i < markerFileStringArray.Length; i++)
                            markerFileStringArray[i] = "Line:[" + tempLines[i] + "]\tScore:[" + tempListScores[i].ToString("#0.00", CultureInfo.InvariantCulture).Replace("2.00", "N/A") + "]\t\t" + markerFileStringArray[i];

                        for (int i = 0; i < studentFileStringArray.Length; i++)
                            studentFileStringArray[i] = "Line:[" + i.ToString() + "]\t" + studentFileStringArray[i];
 

                        studentFileString = string.Join("\n", studentFileStringArray);
                        markerFileString = string.Join("\n", markerFileStringArray);


                    }
                    else
                        error = "File Upload unsuccessful - Both files were not of the same file extension"; //if both file were not same format
                }
                else
                    error = "File Upload unsuccessful - One or more files did not contain the correct file extension, please ensure both files are the same type and are both of either .cs .c or .cpp extensions"; //if one or more files were the wrong format
            }
            else
                return View(); //return view if they were null


            var result = new modelText() { studentFileText = studentFileString, markerFileText = markerFileString, errorMessage = error, statistics = stats, setting = setting }; //create a model for view

            return View(result); //return view to user

        }

        public bool IsFileSupported(HttpPostedFileBase file)
        {
            var isSupported = false;

            switch (Path.GetExtension(file.FileName).ToString()) //check file extension for supported file extensions
            {
                case (".cs"):
                    isSupported = true;
                    break;

                case (".cpp"):
                    isSupported = true;
                    break;

                case (".c"):
                    isSupported = true;
                    break;
            }

            return isSupported;
        }

        public float LevenshteinDistance(string studentFile, string markerFile)
        {
            char[] charArray;
            int studentFileLength, markerFileLength;
            int[] previousCost, costArray, swapArray;

            charArray = studentFile.ToCharArray(); //convert student string to char array
            studentFileLength = charArray.Length; //count number of chars
            markerFileLength = markerFile.Length; //count number of chars

            previousCost = new int[studentFileLength + 1]; //create array one larger than student file for previous cost
            costArray = new int[studentFileLength + 1]; //create array one larger than student file for cost

            if (studentFileLength == 0 || markerFileLength == 0) //if either are empty
            {
                if (studentFileLength == markerFileLength) //if they are the same return match
                    return 1;
                else
                    return 0; //if not return 0
            }


            int i, j, cost;
            char reference_j;

            for (i = 0; i <= studentFileLength; i++) //go through student string
                previousCost[i] = i; //store value of i for each element - 1,2,3,4 etc.

            for (j = 1; j <= markerFileLength; j++) //go through marker string
            {
                reference_j = markerFile[j - 1];
                costArray[0] = j;

                for (i = 1; i <= studentFileLength; i++) //for length of student string + 1
                {
                    cost = charArray[i - 1] == reference_j ? 0 : 1; //return 0 or 1 based off condition
                    costArray[i] = Math.Min(Math.Min(costArray[i - 1] + 1, previousCost[i] + 1), previousCost[i - 1] + cost); //calculate min and add to cost
                }

                swapArray = previousCost; //store previous cost
                previousCost = costArray; //replace previous cost
                costArray = swapArray; //store cost array
            }
            return 1.0f - ((float)previousCost[studentFileLength] / Math.Max(markerFile.Length, charArray.Length)); //calculate and return the cost metric
        }

        public float JaroWinklerDistance(string studentFile, string referenceFile)
        {
            String Max, Min;
            int i, si;

            if (studentFile.Length == 0 || referenceFile.Length == 0) //if either are empty
                if (studentFile.Length == referenceFile.Length)  //if they are the same return match
                    return 1;
                else
                    return 0; //if not return 0

            if (studentFile.Length > referenceFile.Length) //work out which length is the max file length and which is min file length
            {
                Max = studentFile;
                Min = referenceFile;
            }
            else
            {
                Max = referenceFile;
                Min = studentFile;
            }

            var range = Math.Max(Max.Length / 2 - 1, 0);
            var matchIndexes = new int[Min.Length]; 

            for (i = 0; i < matchIndexes.Length; i++) //initialise values for matching indexes
                matchIndexes[i] = -1;

            var matchFlag = new bool[Max.Length]; //array of bool flags
            var matches = 0; //count matches

            for (var a = 0; a < Min.Length; a++) //loop through minimum string array
            {
                var c1 = Min[a]; //take current min string letter
                for (int b = Math.Max(a - range, 0), xn = Math.Min(a + range + 1, Max.Length); b < xn; b++) //go through max elements till end of min elements
                {
                    if (matchFlag[b] || c1 != Max[b]) continue; //match flag or current min string letter eqauls max string letter

                    matchIndexes[a] = b; //record match index
                    matchFlag[b] = true; //record there was a match
                    matches++; //increase count of matches
                    break;
                }
            }

            var match1 = new char[matches]; //store min matches
            var match2 = new char[matches]; //store max matches

            for (i = 0, si = 0; i < Min.Length; i++)
            {
                if (matchIndexes[i] != -1) //if there is a match in min for each char
                {
                    match1[si] = Min[i];
                    si++;
                }
            }

            for (i = 0, si = 0; i < Max.Length; i++)
            {
                if (matchFlag[i]) //if there is a match in max for each char
                {
                    match2[si] = Max[i];
                    si++;
                }
            }

            var transpositions = match1.Where((t, mi) => t != match2[mi]).Count(); //filters the matches based on condition and counts them to add to transpositions

            var prefix = 0; //initialise prefix var
            for (var a = 0; a < Min.Length; a++) //loop through min
            {
                if (studentFile[a] == referenceFile[a]) //if equal to other file increment prefix
                    prefix++;
                else
                    break; //if not exit loop
            }

            float[] matchesTranspositionPrefix = { matches, transpositions / 2, prefix, Max.Length }; //store the data collected on matches, transpositions and prefixes of chars
            float thresh = matchesTranspositionPrefix[0], Threshold = 0.7f; //filter matches with threshold

            if (thresh == 0) //if threshold set to 0, all will be 0
                return 0f;

            float j = ((thresh / studentFile.Length + thresh / referenceFile.Length + (thresh - matchesTranspositionPrefix[1]) / thresh)) / 3; //cost value of change of characters
            float score = j < Threshold ? j : j + Math.Min(0.1f, 1f / matchesTranspositionPrefix[3]) * matchesTranspositionPrefix[2] * (1 - j); //calculating the score based off the values gathered
            return score; //return total cost

        }

        private int nGramsize; //ngram var

        public void nGramSize(int size) //ngram initialiser with specified size
        {
            this.nGramsize = size;
        }

        public void nGramSize() //if not specified create one of size 2
        {
            this.nGramsize = 2;
        }

        public float nGramDistance(string studentFile, string referenceFile)
        {
            int studentFileLength = studentFile.Length;
            int referenceFileLength = referenceFile.Length;
            int cost = 0;
            int i, minI, j;

            if (studentFileLength == 0 || referenceFileLength == 0) //if either are empty
            {
                if (studentFileLength == referenceFileLength) //if they are the same return match
                    return 1;
                else
                    return 0; //if not return 0
            }

            if (studentFileLength < nGramsize || referenceFileLength < nGramsize) //if files are smaller than ngram size return max 0 or 1
            {
                for (i = 0, minI = Math.Min(studentFileLength, referenceFileLength); i < minI; i++)
                {
                    if (studentFile[i] == referenceFile[i])
                        cost++;
                }
                return (float)cost / Math.Max(studentFileLength, referenceFileLength); //return the max value of them
            }

            char[] sa = new char[studentFileLength + nGramsize - 1]; //create char array of studentfile length + ngramsize
            float[] previousCost, costArray, swapArray; //float arrays to store costs

            for (i = 0; i < sa.Length; i++) //go through new char array
            {
                if (i < nGramsize - 1) //if less than N-Gram size store 0
                    sa[i] = (char)0;
                else //if not store the studentFile value char
                    sa[i] = studentFile[i - nGramsize + 1]; 
            }

            previousCost = new float[studentFileLength + 1];
            costArray = new float[studentFileLength + 1];

            char[] refJ = new char[nGramsize]; //create ngram char

            for (i = 0; i <= studentFileLength; i++) //initilise previous costs with i
                previousCost[i] = i;

            for (j = 1; j <= referenceFileLength; j++) //loop through reference file
            {
                if (j < nGramsize) //when loop gets to char which are n-gram size
                {
                    for (int ti = 0; ti < nGramsize - j; ti++) //if less than N-Gram size store 0
                    {
                        refJ[ti] = (char)0;
                    }
                    for (int ti = nGramsize - j; ti < nGramsize; ti++) //if not less than N-Gram size store the referenceFile value char
                    {
                        refJ[ti] = referenceFile[ti - (nGramsize - j)];
                    }
                }
                else
                {
                    refJ = referenceFile.Substring(j - nGramsize, nGramsize).ToCharArray(); //create new n-gram from position
                }

                costArray[0] = j;

                for (i = 1; i <= studentFileLength; i++) //loop thjrough student file
                {
                    cost = 0; //reset cost value each time
                    int refN = nGramsize; //get size for ngram
                    for (minI = 0; minI < nGramsize; minI++) //go through till reaches bigger than nGramsize
                    {
                        if (sa[i - 1 + minI] != refJ[minI])
                            cost++;
                        else if (sa[i - 1 + minI] == 0)
                            refN--;
                    }
                    float ec = (float)cost / refN;
                    costArray[i] = Math.Min(Math.Min(costArray[i - 1] + 1, previousCost[i] + 1), previousCost[i - 1] + ec); //return the smallest cost either new or previous cost for each n-gram
                }

                swapArray = previousCost; //swap previousCost with swapArray
                previousCost = costArray; //put new cost into previous cost
                costArray = swapArray; //store last cost in cost array
            }
            return 1.0f - previousCost[studentFileLength] / Math.Max(referenceFileLength, studentFileLength); //return the cost value with the previous cost value divided by the max values taken from 1
        }
    }
}