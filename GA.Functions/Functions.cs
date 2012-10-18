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
            int i = 0, j=0;
            Random rand = new Random();
            
            matica NewPop = OldPop;
            int lstring = OldPop.RowCount;
            int lpop=OldPop.ColumnCount;
            int num;
            int[] flag = new int[lpop];    //vytvorim pole flagov ci bol retazec z populacie vybrany  
            num = Convert.ToInt32(Math.Truncate((double)lpop / 2.0));
            for (int cyk = 0; cyk < num; cyk++)
            {
                if (sel == 0)
                {
                    while (flag[i] != 0) //najde prveho s flagom 0, co este nebol vybrany (index i)
                    {
                        i += 1;
                    }
                    flag[i] = 1;
                    j =  Convert.ToInt32(Math.Ceiling((double)lpop * rand.NextDouble())); //nahodne najde retazec s flagom 0, co este nebol vybrany (index j)
                    while (flag[j] != 0)
                    {
                        j = Convert.ToInt32(Math.Ceiling((double)lpop * rand.NextDouble()));
                    }
                    flag[j] = 2;
                }
                else if (sel == 1)
                {
                    i = 2 * cyk - 1;
                    j = i + 1;
                }
                if (pts > 4)
                    pts = 4;
                int[] v=new int[pts];
                double n = lstring * (1 - (pts - 1) * 0.15);
                int p = Convert.ToInt32(Math.Ceiling(rand.NextDouble() * n));
                if (p == lstring)
                {
                    p = lstring - 1;
                }
                v[0] = p;

                for (int k = 0; k < pts - 1; k++)
                {
                    int h = Convert.ToInt32(Math.Ceiling(rand.NextDouble() * n));
                    if (h == 1)
                        h = 2;
                    p = p + h;
                    if (p >= lstring)
                        break;
                    v[k + 1] = p;            //zapis bodu krizenia do pola
                }

                //krizenie
                NewPop = skriz(OldPop, NewPop, v, lstring, i, j);
            }

            return NewPop;
        }

        //skrizi jedincov
        private matica skriz(matica OldPop, matica NewPop, int[] v, int lstring, int i, int j)
        {
            List<double[]> listPrvkov1 = new List<double[]>();
            List<double[]> listPrvkov2 = new List<double[]>();
            int[] newV = new int[v.Length + 2];
            v.CopyTo(newV,1);
            newV[0]=0;
            newV[newV.Length-1]=lstring;
            for (int a = 0; a < newV.Length - 1; a++)
            {
                if (a % 2 == 0)
                {
                    listPrvkov1.Add(OldPop.Row(i, v[a], v[a + 1]).ToArray());
                    listPrvkov2.Add(OldPop.Row(j, v[a], v[a + 1]).ToArray());
                }
                else
                {
                    listPrvkov1.Add(OldPop.Row(j, v[a], v[a + 1]).ToArray());
                    listPrvkov2.Add(OldPop.Row(i, v[a], v[a + 1]).ToArray());
                }
            }
            double[] row = spoj(listPrvkov1);
            NewPop.SetRow(i, row);
            row = spoj(listPrvkov2);
            NewPop.SetRow(j, row);

            return NewPop;
        }


        //spoji polia v liste
        private double[] spoj(List<double[]> prvky)
        {
            int pozicia = 0;
            double[] vysledok = new double[prvky.Sum(t => t.Length)];

            foreach (var prvok in prvky)
            {
                prvok.CopyTo(vysledok, pozicia);
                pozicia += prvok.Length;
            }
            
            return vysledok;
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

        /// <summary>
        /// The function mutates the population of strings with the intensity
        ///	proportional to the parameter rate from interval <0;1>. Only a few genes  
        ///	from a few strings are mutated in the population. The mutations are realized
        ///	by addition or substraction of random real-numbers to the mutated genes. The 
        ///	absolute values of the added constants are limited by the vector Amp. 
        ///	Next the mutated strings are limited using boundaries defined in 
        ///	a two-row matrix Space. The first row of the matrix represents the lower 
        ///	boundaries and the second row represents the upper boundaries of corresponding 
        ///	genes.
        /// </summary>
        /// <param name="Oldpop">old population</param>
        /// <param name="Amps">vector of absolute values of real-number boundaries</param>
        /// <param name="Space">matrix of gene boundaries in the form: 
        /// 	                [real-number vector of lower limits of genes
        ///                     real-number vector of upper limits of genes];</param>
        /// <param name="factor">mutation intensity, 0 =< rate =< 1</param>
        /// <returns> Newpop - new, mutated population</returns>

        public matica muta(matica OldPop, double factor, double[] Amps, matica Space)
        {
            Random rand = new Random();
            int lpop = OldPop.ColumnCount;
            int lstring = OldPop.RowCount;

            if (factor > 1)
                factor = 1.0;
            if (factor < 0)
                factor = 0.0;

            int n = Convert.ToInt32(Math.Ceiling(lpop * lstring * factor * rand.NextDouble())); //nahodne generujem pocet mutovanych genov podla miery mutacie (factor)

            matica NewPop = OldPop;

            for (int i = 0; i < n; i++) //cyklus na pocet mutacii
            {
                int r = Convert.ToInt32(Math.Ceiling(rand.NextDouble() * lpop));
                int s = Convert.ToInt32(Math.Ceiling(rand.NextDouble() * lstring));
                NewPop[r, s] = OldPop[r, s] + (2 * rand.NextDouble() - 1) * Amps[s];    //k danemu genu pripocitam nahodne cislo z def. intervalu 
                if (NewPop[r, s] < Space[1, s])
                    NewPop[r, s] = Space[1, s];
                if (NewPop[r, s] > Space[2, s])
                    NewPop[r, s] = Space[2, s];
            }

            return NewPop;
        }

        fdsgdgdsgsgs
    }
}
