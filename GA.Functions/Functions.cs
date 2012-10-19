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

        /// <summary>
        /// The function mutates the population of strings with the intensity
        ///	proportional to the parameter rate from interval <0;1>. Only a few genes  
        ///	from a few strings are mutated in the population. The mutated values are
        ///	selected from the bounded real-number space, which is defined by the two-row 
        ///	matrix Space. The first row of the matrix represents the lower boundaries and the 
        ///	second row represents the upper boundaries of corresponding genes in the strings.
        /// </summary>
        /// <param name="Oldpop">old population</param>
        /// <param name="Amps">vector of absolute values of real-number boundaries</param>
        /// <param name="Space">matrix of boundaries in the form: [vector of lower limits of genes;
        ///                                                          vector of upper limits of genes];</param>
        /// <param name="factor">mutation intensity, 0 =< rate =< 1</param>
        /// <returns> Newpop - new mutated population</returns>
        public matica mutx(matica OldPop, double factor, matica Space)
        {
            Random rand = new Random();

            int lpop = OldPop.ColumnCount;
            int lstring = OldPop.RowCount;

            if (factor > 1)
                factor = 1.0;
            if (factor < 0)
                factor = 0.0;

            int n = Convert.ToInt32(Math.Ceiling(lpop * lstring * factor * rand.NextDouble()));     //nahodne generujem pocet mutovanych genov podla miery mutacie (factor)
            matica NewPop = OldPop;

            for (int i = 0; i < n; i++)
            {
                int r = Convert.ToInt32(Math.Ceiling(rand.NextDouble() * lpop));
                int s = Convert.ToInt32(Math.Ceiling(rand.NextDouble() * lstring));

                double d = Space[2, s] - Space[1, s];
                NewPop[r, s] = rand.NextDouble() * d + Space[1, s];     //dany gen nahradim novym z def. intervalu
                if (NewPop[r, s] < Space[1, s])
                    NewPop[r, s] = Space[1, s];
                if (NewPop[r, s] > Space[2, s])
                    NewPop[r, s] = Space[2, s];
            }

            return NewPop;
        }

        /// <summary>
        ///	The function copies from the old population into the new population
        ///	required a number of strings according to their fitness. The number of the
        ///	selected strings depends on the vector Nums as follows:
        ///	Nums=[number of copies of the best string, ... ,
        ///             number of copies of the i-th best string, ...]
        ///	The best string is the string with the lowest value of its objective function.
        /// </summary>
        /// <param name="Oldpop">old population</param>
        /// <param name="FvPop">fitness vector of Oldpop</param>
        /// <param name="Nums">vector in the form: Nums=[number of copies of the best string, ... ,
        ///                                          number of copies of the i-th best string, ...]</param>
        /// <returns> Newpop - new selected population
        ///           Newfit - fitness vector of Newpop</returns>
        public PopFit selBest(matica OldPop, double[] FvPop, int[] Nums)
        {
            matica Newpop0 = new DenseMatrix(OldPop.RowCount, OldPop.ColumnCount);
            matica NewPop = new DenseMatrix(OldPop.RowCount, OldPop.ColumnCount);
            double[] NewFit0 = new double[FvPop.Length];
            double[] NewFit = new double[FvPop.Length];

            int N = Nums.Length;
            //[Fit, fix] = sort(FvPop);
            var sorted = FvPop
                .Select((x, i) => new KeyValuePair<double, int>(x, i))
                .OrderBy(x => x.Key)
                .ToList();
            double[] Fit = sorted.Select(x => x.Key).ToArray();
            int[] nix = sorted.Select(x => x.Value).ToArray();

            for (int i = 0; i < N; i++)
            {
                Newpop0.SetRow(i, OldPop.Row(nix[i]));        //vytvorenie populacie z N najlepsimi retazcami, z ktorych sa budu robit kopie 
                NewFit0[N] = Fit[N];
            }

            int r = 0;
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < Nums[i]; j++)
                {
                    NewPop.SetRow(r, Newpop0.Row(i));
                    NewFit[r] = NewFit0[i];
                }
            }
            PopFit result = new PopFit { Pop = NewPop, Fit = Fit };
            
            return result;
        }

        /// <summary>
        /// The function selects randomly from the old population a required number
        ///	of strings. 
        /// </summary>
        /// <param name="Oldpop">old population</param>
        /// <param name="Oldfit">fitness vector of Oldpop</param>
        /// <param name="Num">number of selected strings</param>
        /// <returns> Newpop - new selected population
        ///           Newfit - fitness vector of Newpop</returns>
        public PopFit selRand(matica OldPop, double[] OldFit, int num)
        {
            Random rand = new Random();
            int lpop = OldPop.ColumnCount;
            int lstring = OldPop.RowCount;
            int j;
            matica NewPop = new DenseMatrix(OldPop.RowCount, OldPop.ColumnCount);
            double[] NewFit = new double[OldFit.Length];

            for (int i = 0; i < num; i++)
            {
                j = Convert.ToInt32(Math.Ceiling(lpop * rand.NextDouble()));
                NewPop.SetRow(i, OldPop.Row(j));
                NewFit[i] = OldFit[j];
            }
            PopFit result = new PopFit { Pop = NewPop, Fit = NewFit };

            return result;
        }

        /// <summary>
        /// The function selects using tournament selection from the old population 
        ///       a required number of strings.
        /// </summary>
        /// <param name="Oldpop">old population</param>
        /// <param name="Oldfit">fitness vector of Oldpop</param>
        /// <param name="Num">number of selected strings</param>
        /// <returns> Newpop - new selected population
        ///           Newfit - fitness vector of Newpop</returns>
        public PopFit selTourn(matica OldPop, double[] OldFit, int num)
        {
            Random rand=new Random();
            int lpop = OldPop.ColumnCount;
            int lstring = OldPop.RowCount;
            int j,k;
            matica NewPop = new DenseMatrix(OldPop.RowCount, OldPop.ColumnCount);
            double[] NewFit = new double[OldFit.Length];

            for (int i = 0; i < num; i++)
            {
                j = Convert.ToInt32(Math.Ceiling(lpop * rand.NextDouble()));
                k = Convert.ToInt32(Math.Ceiling(lpop * rand.NextDouble()));

                if (OldFit[j] <= OldFit[k])
                {
                    NewPop.SetRow(i, OldPop.Row(j));
                    NewFit[i] = OldFit[j];
                }
                else
                {
                    NewPop.SetRow(i, OldPop.Row(k));
                    NewFit[i] = OldFit[k];
                }
            }
            PopFit result = new PopFit { Pop = NewPop, Fit = NewFit };

            return result;
        }
    }

    public class PopFit
    {
        public matica Pop { get; set; }
        public double[] Fit { get; set; }
    }
}
