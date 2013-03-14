using System;
using System.Drawing;
using System.IO;
using System.Reflection;

using NPlot;

namespace GtkSamples
{
    public class PlotLogLin : PlotSample
    {
        public PlotLogLin ()
        {
            infoText = "";
            infoText += "LogLin Example. Demonstrates - \n";
            infoText += "  * How to chart data against log axes and linear axes at the same time.";

            plotSurface.Clear();

            // draw a fine grid. 
            Grid fineGrid = new Grid();
            fineGrid.VerticalGridType = Grid.GridType.Fine;
            fineGrid.HorizontalGridType = Grid.GridType.Fine;
            plotSurface.Add( fineGrid );

            const int npt = 101;
            float[] x = new float[npt];
            float[] y = new float[npt];
            float step = 0.1f;
            for (int i=0; i<npt; ++i)
            {
                x[i] = i*step - 5.0f;
                y[i] = (float)Math.Pow( 10.0, x[i] );
            }
            float xmin = x[0];
            float xmax = x[npt-1];
            float ymin = (float)Math.Pow( 10.0, xmin );
            float ymax = (float)Math.Pow( 10.0, xmax );

            LinePlot lp = new LinePlot();
            lp.OrdinateData = y;
            lp.AbscissaData = x;
            lp.Pen = new Pen( Color.Red );
            plotSurface.Add( lp );

            LogAxis loga = new LogAxis( plotSurface.YAxis1 );
            loga.WorldMin = ymin;
            loga.WorldMax = ymax;
            loga.AxisColor = Color.Red;
            loga.LabelColor = Color.Red;
            loga.TickTextColor = Color.Red;
            loga.LargeTickStep = 1.0f;
            loga.Label = "10^x";
            plotSurface.YAxis1 = loga;

            LinePlot lp1 = new LinePlot();
            lp1.OrdinateData = y;
            lp1.AbscissaData = x;
            lp1.Pen = new Pen( Color.Blue );
            plotSurface.Add( lp1, XAxisPosition.Bottom, YAxisPosition.Right );
            LinearAxis lin = new LinearAxis( plotSurface.YAxis2 );
            lin.WorldMin = ymin;
            lin.WorldMax = ymax;
            lin.AxisColor = Color.Blue;
            lin.LabelColor = Color.Blue;
            lin.TickTextColor = Color.Blue;
            lin.Label = "10^x";
            plotSurface.YAxis2 = lin;
 
            LinearAxis lx = (LinearAxis)plotSurface.XAxis1;
            lx.WorldMin = xmin;
            lx.WorldMax = xmax;
            lx.Label = "x";

            //((LogAxis)plotSurface.YAxis1).LargeTickStep = 2;

            plotSurface.Title = "Mixed Linear/Log Axes";

            //plotSurface.XAxis1.LabelOffset = 20.0f;

            plotSurface.Refresh();
        }
    }
}

