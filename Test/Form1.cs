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
using ZedGraph;

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

            int[] selbest = txtSelbest.Text.Split(',').ToList().ConvertAll<int>(s => Convert.ToInt32(s)).ToArray();

            if ((selbest.Sum() + Convert.ToInt32(txtSelrand.Text) + Convert.ToInt32(txtNewPop.Text) + Convert.ToInt32(txtSeltourn.Text)) != Convert.ToInt32(txtLpop.Text))
            {
                txtError.Text = "selbest + selrand + nova populacia + seltourn sa musi rovnat velkosti populacie";
                return;
            }

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
                Best = Ga.selBest(Pop, FitPop, selbest);
                Old1 = Ga.selRand(Pop, FitPop, Convert.ToInt32(txtSelrand.Text));
                Old2 = Ga.genrPop(Convert.ToInt32(txtNewPop.Text), SpaceAll);
                Work = Ga.selwRul(Pop, FitPop, 14).Pop;
                //Work = Ga.selSort(Pop, FitPop, 14).Pop;
                //Work = Ga.shake(Work, 0.3);
                //Work = Ga.mutm(Work, 0.2, SpaceMut, SpaceAll);
                //Work = Ga.around(Work, 0, 1.25, SpaceAll);
                //Work = Ga.swapgen(Work, 0.25);
                //Work = Ga.swapPart(Work, 0.35);
                //Work = Ga.crosord(Work, 0);
                Work = Ga.selTourn(Pop, FitPop, Convert.ToInt32(txtSeltourn.Text)).Pop;
                Work = Ga.crossov(Work, 3, 0);
                Work = Ga.mutx(Work, Convert.ToDouble(txtMutx.Text.Replace('.',',')), SpaceAll);
                Work = Ga.muta(Work, Convert.ToDouble(txtMuta.Text.Replace('.', ',')), SpaceMut, SpaceAll);
                Pop.SetSubMatrix(0, Best.Pop.RowCount, 0, Pop.ColumnCount, Best.Pop);
                Pop.SetSubMatrix(Best.Pop.RowCount, Work.RowCount, 0, Pop.ColumnCount, Work);
                Pop.SetSubMatrix(Best.Pop.RowCount + Work.RowCount, Old1.Pop.RowCount, 0, Pop.ColumnCount, Old1.Pop);
                Pop.SetSubMatrix(Best.Pop.RowCount + Work.RowCount + Old1.Pop.RowCount, Old2.RowCount, 0, Pop.ColumnCount, Old2);
            }

            FitPop = schwefel.schwefelFunc(Pop);
            Best = Ga.selBest(Pop, FitPop, new int[1] { 1 });

            lblBestFit.Text = FitPop[0].ToString();
            lblBestPop.Text = Best.Pop.Row(0).ToString();

            //graf
            GraphPane myPane = zg1.GraphPane;
            PointPairList list = new PointPairList();
            for (int i = 0; i < FitTrend.Count; i++)
            {
                double x = i;
                double y = FitTrend[i];
                list.Add(x, y);
            }

            // Generate a red curve with diamond symbols, and "Alpha" in the legend
            LineItem myCurve = myPane.AddCurve("Alpha",
                list, Color.Red, SymbolType.None);
            // Fill the symbols with white
            myCurve.Symbol.Fill = new Fill(Color.White);


            // Show the x axis grid
            myPane.XAxis.MajorGrid.IsVisible = true;

            // Make the Y axis scale red
            myPane.YAxis.Scale.FontSpec.FontColor = Color.Red;
            myPane.YAxis.Title.FontSpec.FontColor = Color.Red;
            // turn off the opposite tics so the Y tics don't show up on the Y2 axis
            myPane.YAxis.MajorTic.IsOpposite = false;
            myPane.YAxis.MinorTic.IsOpposite = false;
            // Don't display the Y zero line
            myPane.YAxis.MajorGrid.IsZeroLine = false;
            // Align the Y axis labels so they are flush to the axis
            myPane.YAxis.Scale.Align = AlignP.Inside;
            // Manually set the axis range
            myPane.YAxis.Scale.Min = -30;
            myPane.YAxis.Scale.Max = 30;

            // Enable the Y2 axis display
            myPane.Y2Axis.IsVisible = true;
            // Make the Y2 axis scale blue
            myPane.Y2Axis.Scale.FontSpec.FontColor = Color.Blue;
            myPane.Y2Axis.Title.FontSpec.FontColor = Color.Blue;
            // turn off the opposite tics so the Y2 tics don't show up on the Y axis
            myPane.Y2Axis.MajorTic.IsOpposite = false;
            myPane.Y2Axis.MinorTic.IsOpposite = false;
            // Display the Y2 axis grid lines
            myPane.Y2Axis.MajorGrid.IsVisible = true;
            // Align the Y2 axis labels so they are flush to the axis
            myPane.Y2Axis.Scale.Align = AlignP.Inside;

            // Fill the axis background with a gradient
            myPane.Chart.Fill = new Fill(Color.White, Color.LightGray, 45.0f);

            // Add a text box with instructions
            TextObj text = new TextObj(
                "Zoom: left mouse & drag\nPan: middle mouse & drag\nContext Menu: right mouse",
                0.05f, 0.95f, CoordType.ChartFraction, AlignH.Left, AlignV.Bottom);
            text.FontSpec.StringAlignment = StringAlignment.Near;
            myPane.GraphObjList.Add(text);

            // Enable scrollbars if needed
            zg1.IsShowHScrollBar = true;
            zg1.IsShowVScrollBar = true;
            zg1.IsAutoScrollRange = true;
            zg1.IsScrollY2 = true;

            // OPTIONAL: Show tooltips when the mouse hovers over a point
            zg1.IsShowPointValues = true;
            zg1.PointValueEvent += new ZedGraphControl.PointValueHandler(MyPointValueHandler);

            // OPTIONAL: Add a custom context menu item
            zg1.ContextMenuBuilder += new ZedGraphControl.ContextMenuBuilderEventHandler(
                            MyContextMenuBuilder);

            // OPTIONAL: Handle the Zoom Event
            zg1.ZoomEvent += new ZedGraphControl.ZoomEventHandler(MyZoomEvent);

            // Size the control to fit the window
            SetSize();

            // Tell ZedGraph to calculate the axis ranges
            // Note that you MUST call this after enabling IsAutoScrollRange, since AxisChange() sets
            // up the proper scrolling parameters
            zg1.GraphPane.YAxis.Scale.MinAuto = true;
            zg1.AxisChange();
            // Make sure the Graph gets redrawn
            zg1.Invalidate();
        }

        /// <summary>
        /// Customize the context menu by adding a new item to the end of the menu
        /// </summary>
        private void MyContextMenuBuilder(ZedGraphControl control, ContextMenuStrip menuStrip,
                        Point mousePt, ZedGraphControl.ContextMenuObjectState objState)
        {
            ToolStripMenuItem item = new ToolStripMenuItem();
            item.Name = "add-beta";
            item.Tag = "add-beta";
            item.Text = "Add a new Beta Point";
            item.Click += new System.EventHandler(AddBetaPoint);

            menuStrip.Items.Add(item);
        }

        /// <summary>
        /// Handle the "Add New Beta Point" context menu item.  This finds the curve with
        /// the CurveItem.Label = "Beta", and adds a new point to it.
        /// </summary>
        private void AddBetaPoint(object sender, EventArgs args)
        {
            // Get a reference to the "Beta" curve IPointListEdit
            IPointListEdit ip = zg1.GraphPane.CurveList["Beta"].Points as IPointListEdit;
            if (ip != null)
            {
                double x = ip.Count * 5.0;
                double y = Math.Sin(ip.Count * Math.PI / 15.0) * 16.0 * 13.5;
                ip.Add(x, y);
                zg1.AxisChange();
                zg1.Refresh();
            }
        }

        /// <summary>
        /// Display customized tooltips when the mouse hovers over a point
        /// </summary>
        private string MyPointValueHandler(ZedGraphControl control, GraphPane pane,
                        CurveItem curve, int iPt)
        {
            // Get the PointPair that is under the mouse
            PointPair pt = curve[iPt];

            return curve.Label.Text + " is " + pt.Y.ToString("f2") + " units at " + pt.X.ToString("f1") + " days";
        }

        // Respond to a Zoom Event
        private void MyZoomEvent(ZedGraphControl control, ZoomState oldState,
                    ZoomState newState)
        {
            // Here we get notification everytime the user zooms
        }
        private void SetSize()
        {
            // Leave a small margin around the outside of the control
            zg1.Size = new Size(this.ClientRectangle.Width - 20,
                    this.ClientRectangle.Height - 20);
        }
    }
}
