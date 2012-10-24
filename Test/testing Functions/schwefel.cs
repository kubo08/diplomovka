using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MathNet.Numerics.LinearAlgebra.Double;

namespace Test.testing_Functions
{
    public static class schwefel
    {
        //% Schwefel's objective function
        //% global optimum: x(i)=420.9687 ;  Fit(x)=-n*418.9829 , n-number of variables
        //% -500 < x(i) < 500
        public static double[] schwefelFunc(Matrix Pop)
        {
            int lpop = Pop.RowCount;
            int lstring = Pop.ColumnCount;
            double[] Fit = new double[Pop.RowCount];

            for (int i = 0; i < lpop; i++)
            {
                double[] G = Pop.Row(i).ToArray();
                Fit[i] = 0;
                for (int j = 0; j < lstring; j++)
                {
                    Fit[i]=Fit[i]-G[j]*Math.Sin(Math.Sqrt(Math.Abs(G[j])));
                }
            }

            return Fit;
        }
    }
}
