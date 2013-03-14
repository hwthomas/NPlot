using System;
using System.Drawing;
using System.IO;
using System.Reflection;

using NPlot;

namespace SwfSamples
{
    public class PlotCircular : PlotSample
    {
        public PlotCircular ()
        {
            infoText = "";
            infoText += "Circular Example. Demonstrates - \n";
            infoText += "  * PiAxis, Horizontal and Vertical Lines. \n";
            infoText += "  * Placement of legend";

            plotSurface.Clear();
            plotSurface.Add( new HorizontalLine( 0.0, Color.LightGray ) );
            plotSurface.Add( new VerticalLine( 0.0, Color.LightGray ) );

            const int N = 400;
            const double start = -Math.PI * 7.0;
            const double end = Math.PI * 7.0;

            double[] xs = new double[N];
            double[] ys = new double[N];

            for (int i=0; i<N; ++i)
            {
                double t = ((double)i*(end - start)/(double)N + start);
                xs[i] = 0.5 * (t - 2.0 * Math.Sin(t));
                ys[i] = 2.0 * (1.0 - 2.0 * Math.Cos(t));
            }

            LinePlot lp = new LinePlot( ys, xs );
            lp.Pen = new Pen( Color.DarkBlue, 2.0f );
            lp.Label = "Circular Line"; // no legend, but still useful for copy data to clipboard.
            plotSurface.Add( lp );

            plotSurface.XAxis1 = new PiAxis( plotSurface.XAxis1 );

            plotSurface.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            plotSurface.Legend = new Legend();
            plotSurface.Legend.AttachTo( XAxisPosition.Bottom, YAxisPosition.Right);
            plotSurface.Legend.HorizontalEdgePlacement = Legend.Placement.Inside;
            plotSurface.Legend.VerticalEdgePlacement = Legend.Placement.Inside;
            plotSurface.Legend.XOffset = -10;
            plotSurface.Legend.YOffset = -10;

            plotSurface.Refresh();
        }
    }
}

