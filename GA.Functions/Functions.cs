using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MathNet.Numerics;
using Alg = MathNet.Numerics.LinearAlgebra;
using matica = MathNet.Numerics.LinearAlgebra.Double.Matrix;
using MathNet.Numerics.LinearAlgebra.Double;

namespace GA.Functions
{
    public class Functions
    {
        /// <summary>
		/// The function creates a new population of strings, which rises after
        ///	1- to 4-point crossover operation of all (couples of) strings of the old
        ///	population. The selection of strings into couples is either random or
        ///	the neighbouring strings are selected, depending on the parameter sel.
		/// </summary>
		/// <param name="Oldpop">old population</param>
        /// <param name="pts">the number of crossover points from 1 to 4</param>
        /// <param name="sel">type of the string-couple selection:
	    ///           0 - random 
	    ///           1 - neighbouring strings in the population</param>
		/// <returns>Newpop - new population</returns>
        public matica crossov(matica OldPop, int pts, int sel)
        {
            int i = 0;
            Random rand = new Random();

            matica NewPop = OldPop;
            int lstring = OldPop.RowCount;
            int lpop=OldPop.ColumnCount;
            int[] flag = new int[lpop];    //vytvorim pole flagov ci bol retazec z populacie vybrany  
            num = fix(lpop / 2);
            for (int cyk = 0; cyk < num; cyk++)
            {
                if (sel == 0)
                {
                    while (flag[i] != 0) //najde prveho s flagom 0, co este nebol vybrany (index i)
                    {
                        i += 1;
                    }
                    flag[i] = 1;
                    int j = ceil(lpop * rand.NextDouble()); //nahodne najde retazec s flagom 0, co este nebol vybrany (index j)
                    while (flag[j] != 0)
                    {
                        j = ceil(lpop * rand.NextDouble());
                    }
                    flag[j] = 2;
                }
                else if (sel == 1)
                {
                    i = 2 * cyk - 1;
                    int j = i + 1;
                }
                if (pts > 4)
                    pts = 4;
                double n = lstring * (1 - (pts - 1) * 0.15);
                double p = ceil(rand.NextDouble() * n);
                if (p == lstring)
                {
                    p = lstring - 1;
                }
                double[] v = p;

                for (int k = 0; k < pts - 1; k++)
                {
                    double h = ceil(rand.NextDouble() * n);
                    if (h == 1)
                        h = 2;
                    p = p + h;
                    if (p >= lstring)
                        break;
                    //v=[v,p];            //zapis bodu krizenia do pola
                }

                //krizenie
                int lv = v.Length;      //pocet bodov krizenia
                if (lv == 4)
                {
                    double[] row=[OldPop[i,0:v[0]];
                    NewPop.SetRow(i,
                }
            }
        }

        /// <summary>
        /// The function generates a population of random real-coded strings
        ///	which items (genes) are limited by a two-row matrix Space. The first
        ///	row of the matrix Space consists of the lower limits and the second row 
        ///	consists of the upper limits of the possible values of genes. 
        ///	The length of the string is equal to the length of rows of the matrix Space.
        /// </summary>
        /// <param name="popSize">required number of strings in the population</param>
        /// <param name="Space">2-row matrix, which 1-st row is the vector of the lower limits
        ///     and the 2-nd row is the vector of the upper limits of the
        ///	   gene values.</param>
        /// <returns> Newpop - random generated population</returns>
        public matica genrPop(int popSize, matica Space)
        {            
            Random random = new Random();
            int lstring = Space.RowCount;
            matica NewPop = new DenseMatrix(popSize, lstring);

            for (int i = 0; i < popSize; i++)
            {
                for (int j = 0; j < lstring; j++)
                {
                    double d = Space[2, j] - Space[1, j];
                    NewPop[i, j] = random.NextDouble() * d + Space[1, j];

                    //podmienky na ohranicenie hodnoty genu
                    if (NewPop[i, j] < Space[1, j])
                        NewPop[i, j] = Space[1, j];
                    if (NewPop[i, j] > Space[2, j])
                        NewPop[i, j] = Space[2, j];
                }
            }

            return NewPop;
        }

    }
}
