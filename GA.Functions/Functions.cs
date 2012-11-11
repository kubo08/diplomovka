using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MathNet.Numerics;
using Alg = MathNet.Numerics.LinearAlgebra;
using matica = MathNet.Numerics.LinearAlgebra.Double.Matrix;
using MathNet.Numerics.LinearAlgebra.Double;

namespace GA
{
    public class Functions
    {
        Random rand = new Random();
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
            
            matica NewPop = OldPop;
            int lstring = OldPop.ColumnCount;
            int lpop = OldPop.RowCount;
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
                    j =  Convert.ToInt32(Math.Ceiling((lpop-1) * rand.NextDouble())); //nahodne najde retazec s flagom 0, co este nebol vybrany (index j)
                    while (flag[j] != 0)
                    {
                        j = Convert.ToInt32(Math.Ceiling((lpop-1) * rand.NextDouble()));
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
                List<int> v=new List<int>();
                double n = lstring * (1 - (pts - 1) * 0.15);        //max. usek
                int p = Convert.ToInt32(Math.Ceiling(rand.NextDouble() * n));   //1. usek
                if (p == lstring)
                {
                    p = lstring - 1;
                }
                v.Add(p);

                for (int k = 0; k < pts - 1; k++)
                {
                    int h = Convert.ToInt32(Math.Ceiling(rand.NextDouble() * n));
                    if (h == 1)
                        h = 2;
                    p = p + h;
                    if (p >= lstring)
                        break;
                    v.Add(p);            //zapis bodu krizenia do pola
                }

                //krizenie
                NewPop = skriz(OldPop, NewPop, v.ToArray(), lstring, i, j);
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
                    listPrvkov1.Add(OldPop.Row(i, newV[a], newV[a + 1] - newV[a]).ToArray());
                    listPrvkov2.Add(OldPop.Row(j, newV[a], newV[a + 1] - newV[a]).ToArray());
                }
                else
                {
                    listPrvkov1.Add(OldPop.Row(j, newV[a], newV[a + 1] - newV[a]).ToArray());
                    listPrvkov2.Add(OldPop.Row(i, newV[a], newV[a + 1] - newV[a]).ToArray());
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
            int lstring = Space.ColumnCount;
            matica NewPop = new DenseMatrix(popSize, lstring);

            for (int i = 0; i < popSize; i++)
            {
                for (int j = 0; j < lstring; j++)
                {
                    double d = Space[1, j] - Space[0, j];
                    NewPop[i, j] = rand.NextDouble() * d + Space[0, j];

                    //podmienky na ohranicenie hodnoty genu
                    if (NewPop[i, j] < Space[0, j])
                        NewPop[i, j] = Space[0, j];
                    if (NewPop[i, j] > Space[1, j])
                        NewPop[i, j] = Space[1, j];
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

        public matica muta(matica OldPop, double factor, matica Amps, matica Space)
        {
            int lstring = OldPop.ColumnCount;
            int lpop = OldPop.RowCount;

            if (factor > 1)
                factor = 1.0;
            if (factor < 0)
                factor = 0.0;
            
            int n = Convert.ToInt32(Math.Ceiling(lpop * lstring * factor * rand.NextDouble())); //nahodne generujem pocet mutovanych genov podla miery mutacie (factor)

            matica NewPop = OldPop;

            for (int i = 0; i < n; i++) //cyklus na pocet mutacii
            {
                int r = Convert.ToInt32(Math.Ceiling(rand.NextDouble() * (lpop-1)));
                int s = Convert.ToInt32(Math.Ceiling(rand.NextDouble() * (lstring-1)));
                NewPop[r, s] = OldPop[r, s] + (2 * rand.NextDouble() - 1) * Amps[0, s];    //k danemu genu pripocitam nahodne cislo z def. intervalu 
                if (NewPop[r, s] < Space[0, s])
                    NewPop[r, s] = Space[0, s];
                if (NewPop[r, s] > Space[1, s])
                    NewPop[r, s] = Space[1, s];
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
            int lstring = OldPop.ColumnCount;
            int lpop = OldPop.RowCount;

            if (factor > 1)
                factor = 1.0;
            if (factor < 0)
                factor = 0.0;

            int n = Convert.ToInt32(Math.Ceiling(lpop * lstring * factor * rand.NextDouble()));     //nahodne generujem pocet mutovanych genov podla miery mutacie (factor)
            matica NewPop = OldPop;

            for (int i = 0; i < n; i++)
            {
                int r = Convert.ToInt32(Math.Ceiling(rand.NextDouble() * (lpop-1)));
                int s = Convert.ToInt32(Math.Ceiling(rand.NextDouble() * (lstring-1)));

                double d = Space[1, s] - Space[0, s];
                NewPop[r, s] = rand.NextDouble() * d + Space[0, s];     //dany gen nahradim novym z def. intervalu
                if (NewPop[r, s] < Space[0, s])
                    NewPop[r, s] = Space[0, s];
                if (NewPop[r, s] > Space[1, s])
                    NewPop[r, s] = Space[1, s];
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
            int count = 0;
            for (int i = 0; i < Nums.Length; i++)
            {
                count += Nums[i];
            }

            matica Newpop0 = new DenseMatrix(Nums.Length, OldPop.ColumnCount);            
            matica NewPop = new DenseMatrix(count, OldPop.ColumnCount);
            double[] NewFit0 = new double[Nums.Length];
            double[] NewFit = new double[count];

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
                NewFit0[i] = Fit[i];
            }

            int r = 0;
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < Nums[i]; j++)
                {
                    NewPop.SetRow(r, Newpop0.Row(i));
                    NewFit[r] = NewFit0[i];
                    r++;
                }
            }
            PopFit result = new PopFit { Pop = NewPop, Fit = NewFit };
            
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
            int lstring = OldPop.ColumnCount;
            int lpop = OldPop.RowCount;
            int j;
            matica NewPop = new DenseMatrix(num, OldPop.ColumnCount);
            double[] NewFit = new double[num];

            for (int i = 0; i < num; i++)
            {
                j = Convert.ToInt32(Math.Ceiling((lpop-1) * rand.NextDouble()));
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
            int lstring = OldPop.ColumnCount;
            int lpop = OldPop.RowCount;
            int j,k;
            matica NewPop = new DenseMatrix(num, OldPop.ColumnCount);
            double[] NewFit = new double[num];

            for (int i = 0; i < num; i++)
            {
                j = Convert.ToInt32(Math.Ceiling((lpop-1) * rand.NextDouble()));
                k = Convert.ToInt32(Math.Ceiling((lpop-1) * rand.NextDouble()));

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

        //TODO: otestovat nove funkcie
        //** nove funkcie **//

        /// <summary>
        /// The function selects from the old population into the new population
        ///	the required number of best strings and also sorts this strings according
        /// their fitness from the most fit to the least fit. The most fit is the string 
        ///	with the lowest value of the objective function and vice-versa.
        /// </summary>
        /// <param name="Oldpop">old population</param>
        /// <param name="Oldfit">fitness vector of Oldpop</param>
        /// <param name="Num">number of selected strings</param>
        /// <returns> Newpop - new selected population
        ///           Newfit - fitness vector of Newpop</returns>
        public PopFit selSort(matica OldPop, double[] oldFit, int num)
        {
            matica NewPop = new DenseMatrix(num, OldPop.ColumnCount);
            var sorted = oldFit
                .Select((x, i) => new KeyValuePair<double, int>(x, i))
                .OrderBy(x => x.Key)
                .ToList();
            double[] Fit = sorted.Select(x => x.Key).ToArray();
            int[] nix = sorted.Select(x => x.Value).ToArray();

            for (int j = 0; j < num; j++)
            {
                NewPop.SetRow(j, OldPop.Row(nix[j]));
            }
            return new PopFit { Pop = NewPop, Fit = Fit };
        }

        /// <summary>
        /// The function selects from the old population a required number of strings using
        ///	the "weighted roulette wheel selection". Under this selection method the individuals
        ///	have a direct-proportional probability to their fitness to be selected into the 
        /// new population.
        /// </summary>
        /// <param name="Oldpop">old population</param>
        /// <param name="Fvpop">fitness vector of Oldpop</param>
        /// <param name="num">number of selected strings</param>
        /// <returns> Newpop - new selected population
        ///           Newfit - fitness vector of Newpop</returns>
        public PopFit selwRul(matica OldPop, double[] Fvpop, int num)
        {
            int lstring = OldPop.ColumnCount;
            int lpop = OldPop.RowCount;
            matica NewPop = new DenseMatrix(num, OldPop.ColumnCount);
            double[] NewFit = new double[num];

            double sumfv = Fvpop.Sum();

            double[] w0 = new double[lpop+1];

            for (int i = 0; i < lpop; i++)
            {
                if (Fvpop[i] == 0)
                    Fvpop[i] = 0.001;
                double men = Fvpop[i] * sumfv;
                if (men == 0)
                    men = 0.0000001;
                w0[i] = 1 / men;        //tvorba inverznych vah
            }
            w0[lpop] = 0;
            double[] w = new double[lpop + 1];
            for (int i = lpop-1; i >= 0; i--)
            {
                w[i] = w[i + 1] + w0[i];
            }
            double maxW = w.Max();

            if (maxW == 0)
                maxW = 0.00001;


            //TODO: najst lepsie riesenie
            for (int j = 0; j < w.Length; j++)
            {
                w[j] = (w[j] / maxW) * 100;     //vahovaci vektor
            }

            for (int i = 0; i < num; i++)
            {
                double q = rand.NextDouble() * 100;
                for (int j = 0; j < lpop; j++)
                {
                    if ((q < w[j]) && (q > w[j + 1]))
                    {                        
                        NewPop.SetRow(i, OldPop.Row(j));
                        NewFit[i] = Fvpop[j];
                        break;
                    }
                }                
            }
            return new PopFit { Pop = NewPop, Fit = NewFit };
        }

        /// <summary>
        /// The function returns a population with random changed order of strings. 
        ///	The strings are without any changes. The intensity of shaking depends on the 
        ///	parameter rate.
        /// </summary>
        /// <param name="Oldpop">old population</param>
        /// <param name="rate">shaking intensity from 0 to 1</param>
        /// <returns> Newpop - new selected population</returns>
        public matica shake(matica OldPop, double rate)
        {
            int lchrom = OldPop.ColumnCount;
            int lpop = OldPop.RowCount;            

            if (rate < 0) rate = 0;
            if (rate > 1) rate = 1;

            matica NewPop = OldPop;
            matica Hlp = OldPop;
            double n = lpop * rate;

            for (int i = 0; i < n; i++)
            {
                int ch1 = 0, ch2 = 0;
                while (ch1 == ch2)
                {
                    ch1 = Convert.ToInt32(Math.Ceiling(rand.NextDouble() * lpop));
                    ch2 = Convert.ToInt32(Math.Ceiling(rand.NextDouble() * lpop));
                }
                NewPop.SetRow(ch1, Hlp.Row(ch2));
                NewPop.SetRow(ch2, Hlp.Row(ch1));
                Hlp.SetRow(ch1, NewPop.Row(ch1));
                Hlp.SetRow(ch2, NewPop.Row(ch2));
            }
            return NewPop;
        }

        /// <summary>
        /// The function mutates the population of strings with the intensity
        ///	proportional to the parameter rate from interval <0;1>. Only a few genes  
        ///	from a few strings are mutated in the population. The mutations are realized
        ///	by multiplication of the mutated genes with real numbers from bounded intervals.
        ///	The intervals are defined in the two-row matrix Amps. The first row of the 
        ///	matrix represents the lower boundaries and the second row represents the upper 
        /// boundaries of the multiplication constants. Next the mutated strings
        ///	are limited using boundaries defined in a similar two-row matrix Space.
        /// </summary>
        /// <param name="Oldpop">old population</param>
        /// <param name="Amps">matrix of multiplicative constant boundaries in the form:
        ///			[real-number vector of lower limits;
        ///         real-number vector of upper limits];</param>
        /// <param name="Space">matrix of gene boundaries in the form: 
        ///	                [real-number vector of lower limits of genes;
        ///                 real-number vector of upper limits of genes];</param>
        /// <param name="rate">mutation intensity, 0 =< rate =< 1</param>
        /// <returns> Newpop - new selected population</returns>
        public matica mutm(matica OldPop, double rate, matica Amps, matica Space)
        {
            int lstring = OldPop.ColumnCount;
            int lpop = OldPop.RowCount;

            if (rate > 1) rate = 1;
            if (rate < 0) rate = 0;

            int n = Convert.ToInt32(Math.Ceiling(lpop * lstring * rate * rand.NextDouble()));

            matica NewPop = OldPop;

            for (int i = 0; i < n; i++)
            {
                int r = Convert.ToInt32(Math.Ceiling(lpop * rand.NextDouble()));
                int s = Convert.ToInt32(Math.Ceiling(lstring * rand.NextDouble()));
                double d = Amps[1, s] - Amps[0, s];
                NewPop[r, s] = OldPop[r, s] * (rand.NextDouble() * d + Amps[0, s]);
                if (NewPop[r, s] < Space[0, s]) NewPop[r, s] = Space[0, s];
                if (NewPop[r, s] < Space[1, s]) NewPop[r, s] = Space[1, s];
            }

            return NewPop;
        }

        /// <summary>
        /// The function creates a new population of the strings, which rises after
        ///	intermediate crossover operation of all (couples of) strings of the old
        ///	population. The selection of strings into couples is either random or
        ///	the neighbouring strings are selected, depending on the parameter sel.
        ///	From each couple of parents will be calculated a new couple of offsprings
        ///	as follows: 
        ///
        ///	Offspring = (Parent1+Parent2)/2 +(-) alfa(Parent_distance)/2
        /// </summary>
        /// <param name="Oldpop">old population</param>
        /// <param name="sel">selection type of crossover couples:
        ///		0-random couples
        ///		1-neighbouring strings in the population</param>
        /// <param name="Space">matrix of gene boundaries in the form: 
        ///	                [real-number vector of lower limits of genes;
        ///                 real-number vector of upper limits of genes];</param>
        /// <param name="alfa">enlargement parameter,  0.1<alfa<10, (usually: alfa=1.25; or 0.75<alfa<2)</param>
        /// <returns> Newpop - new selected population</returns>
        public matica around(matica OldPop, int sel, double alfa, matica Space)
        {
            int lstring = OldPop.ColumnCount;
            int lpop = OldPop.RowCount;
            double[] b = new double[lstring];
            double[] c = new double[lstring];
            double[] d = new double[lstring];

            matica NewPop = OldPop;
            double[] flag = new double[lpop];
            double num = Math.Truncate(lpop / 2.0);
            int i = 0;
            int m = 0;
            int j = 0;

            for (int cyk = 1; cyk < num; cyk++)
            {
                if (sel == 0)
                {
                    while (flag[i] != 0)
                    {
                        i += 1;
                    }
                    flag[i] = 1;
                    j = Convert.ToInt32(Math.Ceiling(lpop * rand.NextDouble()));
                    while (flag[j] != 0)
                    {
                        j = Convert.ToInt32(Math.Ceiling(lpop * rand.NextDouble()));
                    }
                    flag[j] = 2;
                }
                else if (sel == 1)
                {
                    i = 2 * cyk - 1;
                    j = i + 1;
                }
                for (int k = 0; k < lstring; k++)
                {
                    b[k] = Math.Min(OldPop[i, k], OldPop[j, k]);
                    d[k] = Math.Max(OldPop[i, k], OldPop[j, k]) - b[k];
                    c[k] = (OldPop[i, k] + OldPop[j, k]) / 2;
                }
                for (int k = 0; k < lstring; k++)
                {
                    NewPop[m, k] = c[k] + alfa * (2 * rand.NextDouble() - 1) * d[k] / 2;
                    NewPop[m + 1, k] = c[k] + alfa * (2 * rand.NextDouble() - 1) * d[k] / 2;
                    if (NewPop[m, k] < Space[0, k]) NewPop[m, k] = Space[0, k];
                    if (NewPop[m, k] > Space[1, k]) NewPop[m, k] = Space[1, k];
                    if (NewPop[m + 1, k] < Space[0, k]) NewPop[m + 1, k] = Space[0, k];
                    if (NewPop[m + 1, k] > Space[1, k]) NewPop[m + 1, k] = Space[1, k];
                }
                m += 2;
            }
            return NewPop;
        }

        /// <summary>
        /// The function exchanges (mutates) the order of some random selected genes
        ///	in random selected strings in the population. The mutation intensity depends
        ///	on the parameter rate.
        /// </summary>
        /// <param name="Oldpop">old population</param>
        /// <param name="rate">mutation intensity, 0 =< rate =< 1</param>
        /// <returns> Newpop - new selected population</returns>
        public matica swapgen(matica OldPop, double rate)
        {
            int lstring = OldPop.ColumnCount;
            int lpop = OldPop.RowCount;
            if (rate < 0) rate = 0;
            if (rate > 1) rate = 1;

            int n = Convert.ToInt32(Math.Ceiling(lpop * lstring * rate * rand.NextDouble()));

            matica newpop = OldPop;

            for (int i = 0; i < n; i++)
            {
                int r = Convert.ToInt32(Math.Ceiling(rand.NextDouble() * lpop));
                int s1 = Convert.ToInt32(Math.Ceiling(rand.NextDouble() * lstring));
                int s2 = Convert.ToInt32(Math.Ceiling(rand.NextDouble() * lstring));
                if (s1 == s2) s2 = Convert.ToInt32(Math.Ceiling(rand.NextDouble() * lstring));

                double str = newpop[r, s1];
                newpop[r, s1] = newpop[r, s2];
                newpop[r, s2] = str;
            }

            return newpop;
        }

        /// <summary>
        /// The function exchanges the order of two substrings, which will arise after 
        ///	spliting a string in two parts. The number of such modificated strings in
        ///	the population depends on the parameter rate.
        /// </summary>
        /// <param name="Oldpop">old population</param>
        /// <param name="rate">mutation intensity, 0 =< rate =< 1</param>
        /// <returns> Newpop - new selected population</returns>
        public matica swapPart(matica OldPop, double rate)
        {
            int lstring = OldPop.ColumnCount;
            int lpop = OldPop.RowCount;
            if (rate < 0) rate = 0;
            if (rate > 1) rate = 1;

            int n = Convert.ToInt32(Math.Ceiling(lpop * lstring * rand.NextDouble()));
            matica NewPop = OldPop;

            for (int i = 0; i < n; i++)
            {
                int r = Convert.ToInt32(Math.Ceiling(lpop * rand.NextDouble()));
                int s = Convert.ToInt32(Math.Ceiling(lstring * rand.NextDouble()));

                List<double[]> prvky = new List<double[]>();
                prvky.Add(OldPop.Row(r,s-1,lstring-r-1).ToArray());
                prvky.Add(OldPop.Row(r,0,s-1).ToArray());
                NewPop.SetRow(r, spoj(prvky));
            }

            return NewPop;
        }

        /// <summary>
        /// Vytvori novu populaciu retazcov ktora vznikne skrizenim vsetkych 
        /// retazcov starej populacie 2-bodovym krizenim permutacneho typu. 
        /// Krizene su vsetky retazce (ak je ich parny pocet).
        /// </summary>
        /// <param name="Oldpop">old population</param>
        /// <param name="sel">sposob vyberu dvojic: 0 - nahodny, 1 - susedne dvojice v populacii</param>
        /// <returns> Newpop - new selected population</returns>
        public matica crosord(matica OldPop, int sel)
        {
            int lstring = OldPop.ColumnCount;
            int lpop = OldPop.RowCount;

            matica NewPop = OldPop;
            int[] flag = new int[lpop];
            int num = Convert.ToInt32(Math.Truncate(lpop / 2.0));
            int i = 0, j=0;

            for (int cyk = 0; cyk < num; cyk++)
            {
                if (sel == 0)
                {
                    while (flag[i] != 0)
                    {
                        i = i + 1;
                    }
                    flag[i] = 1;
                     j = Convert.ToInt32(Math.Ceiling(lpop * rand.NextDouble()));
                    while(flag[j]!=0)
                        j = Convert.ToInt32(Math.Ceiling(lpop * rand.NextDouble()));
                    flag[j] = 2;
                }
                else if (sel == 1)
                {
                    i = 2 * cyk - 1;
                    j = i + 1;
                }
                int p1 = Convert.ToInt32(Math.Ceiling(0.0001 + rand.NextDouble() * (lstring - 2)));
                int p2 = Convert.ToInt32(Math.Ceiling(0.0001 + rand.NextDouble() * (lstring - p1)) + p1);
                if (p2 > lstring) p2 = lstring;
                if ((p1 == 1) && (p2 >= (lstring - 1))) p2 = lstring - 2;
                if ((p1 == 2) && (p2 >= lstring)) p2 = lstring - 1;

                List<double[]> prvky = new List<double[]>();
                prvky.Add(NewPop.Row(i, 0, p1).ToArray());
                prvky.Add(OldPop.Row(i, p1, p2 - p1).ToArray());
                prvky.Add(NewPop.Row(i, p2, lstring - p2).ToArray());
                NewPop.SetRow(i, spoj(prvky));
                prvky = new List<double[]>();
                prvky.Add(NewPop.Row(j, 0, p1).ToArray());
                prvky.Add(OldPop.Row(j, p1, p2 - p1).ToArray());
                prvky.Add(NewPop.Row(j, p2, lstring - p2).ToArray());
                NewPop.SetRow(j, spoj(prvky));

                int nxch = lstring - (p2 - p1 + 1);

                int pos = 1, all = 0;
                while (all == 0)
                {
                    for (int k2 = 0; k2 < lstring; k2++)
                    {
                        if (pos == p1) pos = p2 + 1;
                        bool nasiel = false;
                        for (int k1 = p1 - 1; k1 < p2; k1++)        //TODO: p1-1?
                        {
                            if (OldPop[i, k1] == OldPop[j, k2])
                                nasiel = true;
                        }
                        if (nasiel == false)
                        {
                            NewPop[i, pos] = OldPop[j, k2];
                            pos++;
                            if (pos >= nxch + 1) all = 1;
                        }
                    }
                }

                pos = 1; all = 0;
                while (all == 0)
                {
                    for (int k2 = 0; k2 < lstring; k2++)
                    {
                        if (pos == p1) pos = p2 + 1;
                        bool nasiel = false;
                        for (int k1 = p1 - 1; k1 < p2; k1++)
                        {
                            if (OldPop[j, k1] == OldPop[i, k2])
                                nasiel = true;
                        }
                        if (nasiel == false)
                        {
                            NewPop[j, pos] = OldPop[i, k2];
                            pos++;
                            if (pos >= nxch + 1) all = 1;
                        }
                    }
                }                
            }

            return NewPop;
        }
    }

    public class PopFit
    {
        public matica Pop { get; set; }
        public double[] Fit { get; set; }
    }
}
