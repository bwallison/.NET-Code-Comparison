using System;
using System.IO;
using System.Linq;

class mainGrades //class mainGrades where the calculations and main method are located
{
	public static void Main () //main method
	{	
		try //try keyword to try and catch any errors
		{
			string[] grades = System.IO.File.ReadAllLines(@"..\Part B\grades_multiple.txt");	//open and read the file by lines, then create a new array for the lines of the text file
			string[] allGrades = new string[6]; //create a Array of six elements to hold the strings of the corresponding course grades
			string[] courseTitle = new string[6]; //create a Array of six elements to hold the strings of the course titles	
			double[] gradeA = new double[6]; //create Array for grades what are 'A'
			double[] gradeB = new double[6]; //create Array for grades what are 'B'
			double[] gradeC = new double[6]; //create Array for grades what are 'C'
			double[] gradeD = new double[6]; //create Array for grades what are 'D'
			double[] gradeE = new double[6]; //create Array for grades what are 'E'
			double[] gradeF = new double[6]; //create Array for grades what are 'F'
			string[] asteriskA = new string[6]; //create Array for grades what are 'A'
			string[] asteriskB = new string[6]; //create Array for grades what are 'B'
			string[] asteriskC = new string[6]; //create Array for grades what are 'C'
			string[] asteriskD = new string[6]; //create Array for grades what are 'D'
			string[] asteriskE = new string[6]; //create Array for grades what are 'E'
			string[] asteriskF = new string[6]; //create Array for grades what are 'F'
			double[] totalGrade = new double[6];
			int counter = 0; //create the variable for the counter of the switch statement to determine line number
			bool flag = true; //create a boolean flag to loop the original question to prompt a input, if incorrect it will ask again
			print myPrint = new print(); //create a object to call the method from the class "print"
		
			Console.WriteLine("{0}", mainGrades.readGrades(counter, grades, allGrades, courseTitle)); //initialise by calling to method to read the grades by passing the variables stated to it
			Console.WriteLine("{0}", mainGrades.countGrades(allGrades, gradeA, gradeB, gradeC, gradeD, gradeE, gradeF, totalGrade)); //count the amount of each grade by calling the method by passing the variables stated to it
			Console.WriteLine("{0}", mainGrades.calculatePercentage(gradeA, gradeB, gradeC, gradeD, gradeE, gradeF, totalGrade)); //call the calculatePercentage method which calculates the percentage of each course using these variables and Arrays
			Console.WriteLine("{0}", mainGrades.convertasterisk(gradeA, gradeB, gradeC, gradeD, gradeE, gradeF, asteriskA, asteriskB, asteriskC, asteriskD, asteriskE, asteriskF, counter)); //call the convertasterisk method which converts the percentage into the appropriate number of asterisk’s
		
			while(flag == true) //while statement to loop question if input is wrong
			{
				Console.WriteLine("Please Enter 'c' for console graphs or 'w' for a web page display"); //prompt the user to either enter 'c' or 'w'
				string input = Convert.ToString(Console.ReadLine()); //converts to a string just to make sure the input is no other variable type
		
				if(input == "c") //if the user input is 'c' entered
				{
					flag = false; //exit loop statement when 'c' is entered
					myPrint.printConsole(asteriskA, asteriskB, asteriskC, asteriskD, asteriskE, asteriskF, courseTitle); //when 'c' is entered the printConsole method from the print class is called
				}
				
				if(input == "w") //if the user input is 'w' entered
				{
					flag = false;; //exit loop statement when 'w' is entered
					myPrint.printWebpage(asteriskA, asteriskB, asteriskC, asteriskD, asteriskE, asteriskF, courseTitle); //when 'w' is entered the printWebpage method from the print class is called
				}
				if(input != "w" && input != "c") //tell the user is the input is wrong to prompt them to enter a correct value
				Console.WriteLine("The value you entered was not a 'c' or 'w'"); //console writeline to tell them
			}
		}
		catch(FileNotFoundException) //FileNotFoundException is caught, if the file is not in the correct folder with the program
		{
			Console.WriteLine("The file was not found please place the file in the folder with the program or the name is not 'grades_multiple'"); //tells the user the file is not in the correct folder to read or the name is wrong
		}
	}
	
	public static string readGrades(int counter, string[] grades, string[] allGrades, string[] courseTitle) //method for reading the grades in the text file
	{
			foreach(string line in grades) //loops through every line in the Array 'grades' which contains the string from the text document
			{
			counter++; //increase counter by one before every new line
			switch(counter) //switch statement for the line counter, this allows each line with grades to be assigned to a element of the Array 'marks'
				{
				case 1: //switch at line 1 of the original text file
				courseTitle[0] = line; //assigns courseTitle[0] element as the second line
				break;
					
				case 2: //switch at line 2 of the original text file
				allGrades[0] = line; //assigns allGrades[0] element as the second line
				break;
					
				case 3: //switch at line 3 of the original text file
				courseTitle[1] = line; //assigns courseTitle[1] element as the second line
				break;
				
				case 4: //switch at line 4 of the original text file
				allGrades[1] = line; //assigns allGrades[1] element as the fourth line
				break;
					
				case 5: //switch at line 5 of the original text file
				courseTitle[2] = line; //assigns courseTitle[2] element as the second line
				break;
			
				case 6: //switch at line 6 of the original text file
				allGrades[2] = line; //assigns allGrades[2] element as the sixth line 
				break;
					
				case 7: //switch at line 7 of the original text file
				courseTitle[3] = line; //assigns courseTitle[3] element as the second line
				break;
			
				case 8: //switch at line 8 of the original text file
				allGrades[3] = line; //assigns allGrades[3] element as the eighth line
				break;
					
				case 9: //switch at line 9 of the original text file
				courseTitle[4] = line; //assigns courseTitle[4] element as the second line
				break;
			
				case 10: //switch at line 10 of the original text file
				allGrades[4] = line; //assigns allGrades[4] element as the tenth line 
				break;
					
				case 11: //switch at line 11 of the original text file
				courseTitle[5] = line; //assigns courseTitle[5] element as the second line
				break;
			
				case 12: //switch at line 12 of the original text file
				allGrades[5] = line; //assigns allGrades[5] element as the twelfth line
				break;
			
				default: //default for switch
				break;
				}
			}
			return "Reading the grades..."; //returns a string to confirm what the program is doing at this stage
	}
	
	public static string countGrades(string[] allGrades, double[] gradeA, double[] gradeB, double[] gradeC, double[] gradeD, double[] gradeE, double[] gradeF, double[] totalGrade) //method which counts the amount of each character in the grade Arrays
	{
			for(int i = 0; i < 6; i++) //for loop which loops through all elements of the array allGrades[] and also to loop through the elements of the Arrays gradeA[], GradeB[], gradeC[], gradeD[], gradeE[] and gradeF[]
			{
				foreach(char x in allGrades[i]) //foreach loop to read the strings of each element in the Array allGrades[], it then adds 1 to the corresponding Array of that grade
				{
					if( x == 'A' ) //if the char in the Array is a 'A'
					gradeA[i]++; //adds one onto the gradeA array counter element
					if( x == 'B' ) //if the char in the Array is a 'A'
					gradeB[i]++; //adds one onto the gradeB array counter element
					if( x == 'C' ) //if the char in the Array is a 'A'
					gradeC[i]++; //adds one onto the gradeC array counter element
					if( x == 'D' ) //if the char in the Array is a 'A'
					gradeD[i]++; //adds one onto the gradeD array counter element
					if( x == 'E' ) //if the char in the Array is a 'A'
					gradeE[i]++; //adds one onto the gradeE array counter element
					if( x == 'F' ) //if the char in the Array is a 'A'
					gradeF[i]++; //adds one onto the gradeF array counter element
					if( x == 'A' || x == 'B' || x == 'C' || x == 'D' || x == 'E' || x == 'F') //if the char is any of the grade possibilities
					totalGrade[i]++; //total grade count which is used later in calculating the percentage of grades
				}
			}
			return "Counting amount of each grade..."; //returns a string letting the user know the program has counted the grades
	}
	
	public static string calculatePercentage(double[] gradeA, double[] gradeB, double[] gradeC, double[] gradeD, double[] gradeE, double[] gradeF, double[] totalGrade) //method which calculates the percentage of the characters between 0 and 100 inclusive
	{
		try //try to catch any possible errors
		{
			for(int i = 0; i < 6; i++) //for loop to loop easily through all Array elements of gradeA, gradeB, gradeC, gradeD, gradeE, gradeF and totalGrade
			{
				gradeA[i] = (gradeA[i]/totalGrade[i])*100; //calculates percentage by taking the GradeA element 'i' in the loop dividing it by the total amount of grades in the course 'totalGrade' and *100 to give a percentage
				gradeB[i] = (gradeB[i]/totalGrade[i])*100; //calculates percentage by taking the GradeB element 'i' in the loop dividing it by the total amount of grades in the course 'totalGrade' and *100 to give a percentage	
				gradeC[i] = (gradeC[i]/totalGrade[i])*100; //calculates percentage by taking the GradeC element 'i' in the loop dividing it by the total amount of grades in the course 'totalGrade' and *100 to give a percentage
				gradeD[i] = (gradeD[i]/totalGrade[i])*100; //calculates percentage by taking the GradeD element 'i' in the loop dividing it by the total amount of grades in the course 'totalGrade' and *100 to give a percentage
				gradeE[i] = (gradeE[i]/totalGrade[i])*100; //calculates percentage by taking the GradeE element 'i' in the loop dividing it by the total amount of grades in the course 'totalGrade' and *100 to give a percentage
				gradeF[i] = (gradeF[i]/totalGrade[i])*100; //calculates percentage by taking the GradeF element 'i' in the loop dividing it by the total amount of grades in the course 'totalGrade' and *100 to give a percentage
			}
		}
		catch(DivideByZeroException) //divide by zero exception is caught, if there is not grades in the totalGrade element which is a possibility the error is caught
		{
		Console.WriteLine("The total of the grades of a course was 0, therefore the percentage cannot be calculated"); //tells the user what specifically was wrong e.g. totalGrades has not value is was null
		}
		
		return "Calculating percentage of each grade..."; //returns a string telling the user that percentage has been calculated
	}
	
	public static string convertasterisk(double[] gradeA, double[] gradeB, double[] gradeC, double[] gradeD, double[] gradeE, double[] gradeF, string[] asteriskA, string[] asteriskB, string[] asteriskC, string[] asteriskD, string[] asteriskE, string[] asteriskF, int counter) //method for converting the percentage into asterisk’s for printing onto a graph
	{		
		for(int i = 0; i < 6; i++) //for loop to go through every element of percentage of grades and asterisk elements
		{
			if(gradeA[i] >= 2) //if gradeA has a percentage equal or greater than 2, this eliminates printing any asterisk’s if it is not above 2% which is one asterisk’s on the graph
			{
				while(counter < gradeA[i]/2) //while statement, if the counter goes above half of the percentage it will exit, this ensures every asterisk represents 2% on the graph
				{
					counter++; //counter which has been brought and reused from readGrades is used to keep track for while statement the counting is correct
					asteriskA[i] += "*"; //adds a asterisk every time the while statement is gone through, it is half of the amount of the percentage as a asterisk is 2% on the graphs
				}
			}
			counter = 0; //resets the counter for gradeB percentage to be converted to asterisk’s
			
			if(gradeB[i] >= 2) //if gradeB has a percentage equal or greater than 2, this eliminates printing any asterisk’s if it is not above 2% which is one asterisk’s on the graph
			{
				while(counter < gradeB[i]/2) //while statement, if the counter goes above half of the percentage it will exit, this ensures every asterisk represents 2% on the graph
				{
					counter++; //counter which has been brought and reused from readGrades is used to keep track for while statement the counting is correct
					asteriskB[i] += "*"; //adds a asterisk every time the while statement is gone through to the asteriskB Array
				}
			}
			counter = 0; //resets the counter for gradeC percentage to be converted to asterisk’s

			if(gradeC[i] >= 2) //if gradeC has a percentage equal or greater than 2, this eliminates printing any asterisk’s if it is not above 2% which is one asterisk’s on the graph
			{
				while(counter < gradeC[i]/2) //while statement, if the counter goes above half of the percentage it will exit, this ensures every asterisk represents 2% on the graph
				{ 
					counter++; //counter which has been brought and reused from readGrades is used to keep track for while statement the counting is correct
					asteriskC[i] += "*"; //adds a asterisk every time the while statement is gone through to the asteriskC Array
				}
			}
			counter = 0; //resets the counter for gradeD percentage to be converted to asterisk’s
		
			if(gradeD[i] >= 2) //if gradeD has a percentage equal or greater than 2, this eliminates printing any asterisk’s if it is not above 2% which is one asterisk’s on the graph
			{
				while(counter < gradeD[i]/2) //while statement, if the counter goes above half of the percentage it will exit, this ensures every asterisk represents 2% on the graph
				{
					counter++; //counter which has been brought and reused from readGrades is used to keep track for while statement the counting is correct
					asteriskD[i] += "*"; //adds a asterisk every time the while statement is gone through to the asteriskD Array
				}
			}
			counter = 0; //resets the counter for gradeE percentage to be converted to asterisk’s
		
			if(gradeE[i] >= 2) //if gradeE has a percentage equal or greater than 2, this eliminates printing any asterisk’s if it is not above 2% which is one asterisk’s on the graph
			{
				while(counter < gradeE[i]/2) //while statement, if the counter goes above half of the percentage it will exit, this ensures every asterisk represents 2% on the graph
				{
					counter++; //counter which has been brought and reused from readGrades is used to keep track for while statement the counting is correct
					asteriskE[i] += "*"; //adds a asterisk every time the while statement is gone through to the asteriskE Array
				}
			}
			counter = 0; //resets the counter on the next loop for gradeF  percentage to be converted to asterisk’s
		
			if(gradeF[i] >= 2) //if gradeF has a percentage equal or greater than 2, this eliminates printing any asterisk’s if it is not above 2% which is one asterisk’s on the graph
			{
				while(counter < gradeF[i]/2) //while statement, if the counter goes above half of the percentage it will exit, this ensures every asterisk represents 2% on the graph
				{
					counter++;  //counter which has been brought and reused from readGrades is used to keep track for while statement the counting is correct
					asteriskF[i] += "*"; //adds a asterisk every time the while statement is gone through to the asteriskF Array
				}
			}
			counter = 0; //resets the counter on the next loop for gradeA  percentage to be converted to asterisk’s
		
		if((gradeA[i] % 2) != 0) //if statement, if the percentage is a even number i.e modulus of it has got a remainder
		asteriskA[i] = asteriskA[i].Remove(0,1); //accommodates for having a odd number of grades so removes one asterisk to make sure the percentage is correct
		if((gradeB[i] % 2) != 0) //if statement, if the percentage is a even number i.e modulus of it has got a remainder
		asteriskB[i] = asteriskB[i].Remove(0,1); //accommodates for having a odd number of grades so removes one asterisk from asteriskB it is a even percentage
		if((gradeC[i] % 2) != 0) //if statement, if the percentage is a even number i.e modulus of it has got a remainder
		asteriskC[i] = asteriskC[i].Remove(0,1); //accommodates for having a odd number of grades so removes one asterisk from the start of the Array asteriskC 
		if((gradeD[i] % 2) != 0) //if statement, if the percentage is a even number i.e modulus of it has got a remainder
		asteriskD[i] = asteriskD[i].Remove(0,1); //accommodates for having a odd number of grades so removes one asterisk from the start of the Array asteriskD
		if((gradeE[i] % 2) != 0) //if statement, if the percentage is a even number i.e modulus of it has got a remainder
		asteriskE[i] = asteriskE[i].Remove(0,1); //accommodates for having a odd number of grades so removes one asterisk from the start of the Array asteriskE
		if((gradeF[i] % 2) != 0) //if statement, if the percentage is a even number i.e modulus of it has got a remainder
		asteriskF[i] = asteriskF[i].Remove(0,1); //accommodates for having a odd number of grades so removes one asterisk from the start of the Array asteriskF
		
		}
		
	return "Converting percentage to asterisk’s..."; //returns a string saying that percentage has been converted to asterisk’s for the print stage
	}
}

class print //second class for printing off to console or webpage
{
	public string printConsole(string[] a, string[] b, string[] c, string[] d, string[] e, string[] f, string[] g) //print bar graph to console screen method	
	{
		for(int i = 0; i < 6; i++) //for loop looping through the six elements of courseTitle[] and the elements of the Arrays gradeA[], GradeB[], gradeC[], gradeD[], gradeE[] and gradeF[]
		{
		Console.WriteLine("\n"); //formatting to leave a line between the graphs
		Console.WriteLine("{0}", g[i]); //writes in console the course title of the corresponding course graph, the number 'i' corresponds to the what course it is e.g. 0 = CMP1000, 1 = CMP1001
		Console.WriteLine("0   10   20   30   40   50   60   70   80   90   100"); //key to read the percentages off the graph
		Console.WriteLine("|   |    |    |    |    |    |    |    |    |    |"); //indications for the number to show where that percentage lies on the graph
		Console.WriteLine("**************************************************"); //50 '*' to show the full length of the graph
		Console.WriteLine("{0} {1}", a[i], "A"); //as the loop continues the string of asterisk’s for the grade 'A' display on the console, the number 'i' corresponds to the what course it is e.g. 0 = CMP1000, 1 = CMP1001
		Console.WriteLine("{0} {1}", b[i], "B"); //as the loop continues the string of asterisk’s for the grade 'B' display on the console
		Console.WriteLine("{0} {1}", c[i], "C"); //as the loop continues the string of asterisk’s for the grade 'C' display on the console
		Console.WriteLine("{0} {1}", d[i], "D"); //as the loop continues the string of asterisk’s for the grade 'D' display on the console
		Console.WriteLine("{0} {1}", e[i], "E"); //as the loop continues the string of asterisk’s for the grade 'E' display on the console
		Console.WriteLine("{0} {1}", f[i], "F"); //as the loop continues the string of asterisk’s for the grade 'F' display on the console
		}
		return "You selected a console display, the program has now finished"; //tells the user that the program has ended
	}	
	
	public string printWebpage(string[] a, string[] b, string[] c, string[] d, string[] e, string[] f, string[] g) //print bar graph to webpage method and opens the webpage
	{
		StreamWriter index = new StreamWriter("index.html"); //streamWriter which from this line to index.Close() any text will be written to new file created index.html
		index.WriteLine("<!DOCTYPE html>"); //writes this text on the first line of the html file, this is to specify it is a html document
		index.WriteLine("<html>"); //adds '<html>' to the second line to start the html document
		index.WriteLine("<body>"); //adds '</body>' to open the body section
			for(int i = 0; i < 6; i++) //for loop looping through the six elements of courseTitle[] and the elements of the Arrays gradeA[], GradeB[], gradeC[], gradeD[], gradeE[] and gradeF[]
			{
			index.WriteLine("{0}{1}{2}", "<p>", g[i], "</p>"); //writes in console the course title of the graph, the number 'i' is the element number of that course e.g. 0 = CMP1000, 1 = CMP1001
			index.WriteLine("<pre>0   10   20   30   40   50   60   70   80   90   100</pre>"); //key to read the percentages off the graph
			index.WriteLine("<pre>|   |    |    |    |    |    |    |    |    |    |</pre>"); //indications for the number to show where that percentage lies on the graph
			index.WriteLine("<pre>**************************************************</pre>");  //50 '*' to show the full length of the graph, '<p>' is for a html paragraph and </p> closes the paragraph
			index.WriteLine("{0}{1} {2}{3}","<p>", a[i], "A","</p>"); //as it loops the strings of asterisk’s for the grade 'A' display on the webpage, the number 'i' corresponds to the what course it is e.g. 0 = CMP1000
			index.WriteLine("{0}{1} {2}{3}","<p>", b[i], "B","</p>"); //as it loops the strings of asterisk’s for the grade 'B' display on the webpage
			index.WriteLine("{0}{1} {2}{3}","<p>", c[i], "C","</p>"); //as it loops the strings of asterisk’s for the grade 'C' display on the webpage
			index.WriteLine("{0}{1} {2}{3}","<p>", d[i], "D","</p>"); //as it loops the strings of asterisk’s for the grade 'D' display on the webpage
			index.WriteLine("{0}{1} {2}{3}","<p>", e[i], "E","</p>"); //as it loops the strings of asterisk’s for the grade 'E' display on the webpage
			index.WriteLine("{0}{1} {2}{3}","<p>", f[i], "F","</p>"); //as it loops the strings of asterisk’s for the grade 'F' display on the webpage
			}
		index.WriteLine("</body>"); //adds '</body>' to close the body section
		index.WriteLine("</html>"); //adds '</html>' to close the html document
		index.Close(); //stops the streamWriter of the document index.html
		System.Diagnostics.Process.Start("index.html"); //launches the webpage
		return "You selected a web display, the program has now finished"; //tells the user that the program has ended
	}	
}