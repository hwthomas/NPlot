using System;
using System.Drawing;
using System.IO;
using System.Reflection;

using NPlot;

namespace SwfSamples
{
    public class PlotLogLog : PlotSample
    {
        public PlotLogLog ()
        {
            infoText = "";
            infoText += "LogLog Example. Demonstrates - \n";
            infoText += " * How to chart data against log axes and linear axes at the same time. \n";
            infoText += " * Plot Drag (Vertical) - try clicking and dragging in the Plot area \n";
            infoText += " * Plot Drag (Vertical) - try Ctrl + clicking and dragging in the Plot area";
    
            // log log plot
            plotSurface.Clear();

            Grid mygrid = new Grid();
            mygrid.HorizontalGridType = Grid.GridType.Fine;
            mygrid.VerticalGridType = Grid.GridType.Fine;
            plotSurface.Add(mygrid);

            int npt = 101;
            float [] x = new float[npt];
            float [] y = new float[npt];

            float step=0.1f;

            // plot a power law on the log-log scale
            for (int i=0; i<npt; ++i)
            {
                x[i] = (i+1)*step;
                y[i] = x[i]*x[i];
            }
            float xmin = x[0];
            float xmax = x[npt-1];
            float ymin = y[0];
            float ymax = y[npt-1];

            LinePlot lp = new LinePlot();
            lp.OrdinateData = y;
            lp.AbscissaData = x; 
            lp.Pen = new Pen( Color.Red );
            plotSurface.Add( lp );
            // axes
            // x axis
            LogAxis logax = new LogAxis( plotSurface.XAxis1 );
            logax.WorldMin = xmin;
            logax.WorldMax = xmax;
            logax.AxisColor = Color.Red;
            logax.LabelColor = Color.Red;
            logax.TickTextColor = Color.Red;
            logax.LargeTickStep = 1.0f;
            logax.Label = "x";
            plotSurface.XAxis1 = logax;
            // y axis
            LogAxis logay = new LogAxis( plotSurface.YAxis1 );
            logay.WorldMin = ymin;
            logay.WorldMax = ymax;
            logay.AxisColor = Color.Red;
            logay.LabelColor = Color.Red;
            logay.TickTextColor = Color.Red;
            logay.LargeTickStep = 1.0f;
            logay.Label = "x^2";
            plotSurface.YAxis1 = logay;

            LinePlot lp1 = new LinePlot();
            lp1.OrdinateData = y;
            lp1.AbscissaData = x;
            lp1.Pen = new Pen( Color.Blue );
            plotSurface.Add( lp1, XAxisPosition.Top, YAxisPosition.Right );
            // axes
            // x axis (lin)
            LinearAxis linx = (LinearAxis) plotSurface.XAxis2;
            linx.WorldMin = xmin;
            linx.WorldMax = xmax;
            linx.AxisColor = Color.Blue;
            linx.LabelColor = Color.Blue;
            linx.TickTextColor = Color.Blue;
            linx.Label = "x";
            plotSurface.XAxis2 = linx;
            // y axis (lin)
            LinearAxis liny = (LinearAxis) plotSurface.YAxis2;
            liny.WorldMin = ymin;
            liny.WorldMax = ymax;
            liny.AxisColor = Color.Blue;
            liny.LabelColor = Color.Blue;
            liny.TickTextColor = Color.Blue;
            liny.Label = "x^2";
            plotSurface.YAxis2 = liny;

            plotSurface.Title = "x^2 plotted with log(red)/linear(blue) axes";
            plotSurface.AddInteraction ( new PlotDrag(false,true));

            plotSurface.Refresh();
        }
    }
}

