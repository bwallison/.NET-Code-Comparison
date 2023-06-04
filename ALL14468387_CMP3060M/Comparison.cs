using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ALL14468387_CMP3060M
{
    public class Comparison
    {
        public float LevenshteinDistance(String studentFile, String referenceFile)
        {
            char[] charArray;
            int studentFileLength, referenceFileLength;
            int[] previousCost, costArray, swapArray; 

            charArray = studentFile.ToCharArray();
            studentFileLength = charArray.Length;
            referenceFileLength = referenceFile.Length;

            previousCost = new int[studentFileLength + 1];
            costArray = new int[studentFileLength + 1];

            if (studentFileLength == 0 || referenceFileLength == 0)
            {
                if (studentFileLength == referenceFileLength)
                    return 1;
                else
                    return 0;
            }


            int i, j, cost;
            char reference_j; 

            for (i = 0; i <= studentFileLength; i++)
                previousCost[i] = i;

            for (j = 1; j <= referenceFileLength; j++)
            {
                reference_j = referenceFile[j - 1];
                costArray[0] = j;

                for (i = 1; i <= studentFileLength; i++)
                {
                    cost = charArray[i - 1] == reference_j ? 0 : 1;
                    costArray[i] = Math.Min(Math.Min(costArray[i - 1] + 1, previousCost[i] + 1), previousCost[i - 1] + cost);
                }

                swapArray = previousCost;
                previousCost = costArray;
                costArray = swapArray;
            }
            return 1.0f - ((float)previousCost[studentFileLength] / Math.Max(referenceFile.Length, charArray.Length));
        }

        public float JaroWinklerDistance(String studentFile, String referenceFile)
        {
            String Max, Min;
            int i, si;

            if (studentFile.Length == 0 || referenceFile.Length == 0)
                if (studentFile.Length == referenceFile.Length)
                    return 1;
                else
                    return 0;

            if (studentFile.Length > referenceFile.Length)
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

            for (i = 0; i < matchIndexes.Length; i++)
                matchIndexes[i] = -1;

            var matchFlags = new bool[Max.Length];
            var matches = 0;

            for (var mi = 0; mi < Min.Length; mi++)
            {
                var c1 = Min[mi];
                for (int xi = Math.Max(mi - range, 0), xn = Math.Min(mi + range + 1, Max.Length); xi < xn; xi++)
                {
                    if (matchFlags[xi] || c1 != Max[xi]) continue;

                    matchIndexes[mi] = xi;
                    matchFlags[xi] = true;
                    matches++;
                    break;
                }
            }

            var ms1 = new char[matches];
            var ms2 = new char[matches];

            for (i = 0, si = 0; i < Min.Length; i++)
            {
                if (matchIndexes[i] != -1)
                {
                    ms1[si] = Min[i];
                    si++;
                }
            }

            for (i = 0, si = 0; i < Max.Length; i++)
            {
                if (matchFlags[i])
                {
                    ms2[si] = Max[i];
                    si++;
                }
            }

            var transpositions = ms1.Where((t, mi) => t != ms2[mi]).Count();

            var prefix = 0;
            for (var mi = 0; mi < Min.Length; mi++)
            {
                if (studentFile[mi] == referenceFile[mi])
                    prefix++;
                else
                    break;
            }

            float[] mtp = { matches, transpositions / 2, prefix, Max.Length };
            float m = mtp[0], Threshold = 0.7f;

            if (m == 0)
                return 0f;

            float j = ((m / studentFile.Length + m / referenceFile.Length + (m - mtp[1]) / m)) / 3;
            float jw = j < Threshold ? j : j + Math.Min(0.1f, 1f / mtp[3]) * mtp[2] * (1 - j);
            return jw;

        }

        private int nGramsize;

        public void nGramSize(int size)
        {
            this.nGramsize = size;
        }

        public void nGramSize()
        {
            this.nGramsize = 2;
        }

        public float nGramDistance(String studentFile, String referenceFile)
        {
            int studentFileLength = studentFile.Length;
            int referenceFileLength = referenceFile.Length;

            if (studentFileLength == 0 || referenceFileLength == 0)
            {
                if (studentFileLength == referenceFileLength)
                    return 1;
                else
                    return 0;
            }

            int cost = 0;
            int i, ni, j;

            if (studentFileLength < nGramsize || referenceFileLength < nGramsize)
            {
                for (i = 0, ni = Math.Min(studentFileLength, referenceFileLength); i < ni; i++)
                {
                    if (studentFile[i] == referenceFile[i])
                        cost++;
                }
                return (float)cost / Math.Max(studentFileLength, referenceFileLength);
            }
            char[] sa = new char[studentFileLength + nGramsize - 1];
            float[] previousCost, costArray, swapArray;

            for (i = 0; i < sa.Length; i++)
            {
                if (i < nGramsize - 1)
                    sa[i] = (char)0;
                else
                    sa[i] = studentFile[i - nGramsize + 1];
            }

            previousCost = new float[studentFileLength + 1];
            costArray = new float[studentFileLength + 1];

            char[] t_j = new char[nGramsize];

            for (i = 0; i <= studentFileLength; i++)
                previousCost[i] = i;

            for (j = 1; j <= referenceFileLength; j++)
            {
                if (j < nGramsize)
                {
                    for (int ti = 0; ti < nGramsize - j; ti++)
                    {
                        t_j[ti] = (char)0;
                    }
                    for (int ti = nGramsize - j; ti < nGramsize; ti++)
                    {
                        t_j[ti] = referenceFile[ti - (nGramsize - j)];
                    }
                }
                else
                {
                    t_j = referenceFile.Substring(j - nGramsize, nGramsize).ToCharArray();
                }
                costArray[0] = j;
                for (i = 1; i <= studentFileLength; i++)
                {
                    cost = 0;
                    int tn = nGramsize;
                    for (ni = 0; ni < nGramsize; ni++)
                    {
                        if (sa[i - 1 + ni] != t_j[ni])
                            cost++;
                        else if (sa[i - 1 + ni] == 0)
                            tn--;
                    }
                    float ec = (float)cost / tn;
                    costArray[i] = Math.Min(Math.Min(costArray[i - 1] + 1, previousCost[i] + 1), previousCost[i - 1] + ec);
                }
                swapArray = previousCost;
                previousCost = costArray;
                costArray = swapArray;
            }
            return 1.0f - previousCost[studentFileLength] / Math.Max(referenceFileLength, studentFileLength);
        }
    }
}
