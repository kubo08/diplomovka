using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MathNet.Numerics.LinearAlgebra.Double;
using GA;
using Test.testing_Functions;

namespace Test
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Functions Ga = new Functions();
            int numgen = Convert.ToInt32(txtNumgen.Text);
            int lpop= Convert.ToInt32(txtLpop.Text);

            Matrix SpaceAll = new DenseMatrix(2, 10);
            Matrix SpaceMut = new DenseMatrix(2, 10);
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    SpaceAll[i, j] = 500 * Math.Pow(-1,i+1);
                    SpaceMut[i, j] = 2 - i;
                }
            }

            Matrix Pop = Ga.genrPop(lpop, SpaceAll);
            double[] FitPop=new double[lpop];
            List<double> FitTrend = new List<double>();
            PopFit Best, Old1;
            Matrix Old2, Work;

            for (int gen = 0; gen < numgen; gen++)
            {
                FitPop = schwefel.schwefelFunc(Pop);
                FitTrend.Add(FitPop.Min());                
                Best = Ga.selBest(Pop, FitPop, new int[2] { 1, 1 });
                Old1 = Ga.selRand(Pop, FitPop, 7);
                Old2 = Ga.genrPop(7, SpaceAll);
                Work=Ga.selTourn(Pop, FitPop, 14).Pop;
                Work = Ga.crossov(Work, 3, 0);
                Work = Ga.mutx(Work, 0.25, SpaceAll);
                Work = Ga.muta(Work, 0.2, SpaceMut, SpaceAll);
                Pop.SetSubMatrix(0, Best.Pop.RowCount, 0, Pop.ColumnCount, Best.Pop);
                Pop.SetSubMatrix(Best.Pop.RowCount + 1, Work.RowCount, 0, Pop.ColumnCount, Work);
                Pop.SetSubMatrix(Best.Pop.RowCount + Work.RowCount + 1, Old1.Pop.RowCount, 0, Pop.ColumnCount, Old1.Pop);
                Pop.SetSubMatrix(Best.Pop.RowCount + Work.RowCount + Old1.Pop.RowCount + 1, Old2.RowCount, 0, Pop.ColumnCount, Old2);
            }

            FitPop = schwefel.schwefelFunc(Pop);
            Best = Ga.selBest(Pop, FitPop, new int[1] { 1 });
        }
    }
}
