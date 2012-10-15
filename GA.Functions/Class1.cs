using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
        /// <param name="num">the number of crossover points from 1 to 4</param>
        /// <param name="sel">type of the string-couple selection:
	    ///           0 - random 
	    ///           1 - neighbouring strings in the population</param>
		/// <returns>Newpop - new population</returns>
        public strukt crossov(strukt OldPop, int num, int sel)
        {
            tralala
        }

        public double[,] genrPop(int popSize, double[,] Space)
        {
            int lstring = Space.Length;
        }

    }
}
