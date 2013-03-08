using System;
using System.Drawing;
using System.IO;
using System.Reflection;

using NPlot;

namespace GtkSamples
{
    public class PlotABC : PlotSample
    {
        public PlotABC ()
        {
            infoText = "";
            infoText += "ABC (logo for australian broadcasting commission) Example. Demonstrates - \n";
            infoText += " * How to set the background of a plotsurface as an image. \n";
            infoText += " * EqualAspectRatio axis constraint \n";
            infoText += " * Plot Zoom with Mouse Wheel, and mouse position Focus point";
           
            plotSurface.Clear();
            const int size = 200;
            float [] xs = new float [size];
            float [] ys = new float [size];
            for (int i=0; i<size; i++)
            {
                xs[i] = (float)Math.Sin((double)i/(double)(size-1)*2.0*Math.PI);
                ys[i] = (float)Math.Cos((double)i/(double)(size-1)*6.0*Math.PI);
            }

            LinePlot lp = new LinePlot();
            lp.OrdinateData = ys;
            lp.AbscissaData = xs;
            Pen linePen = new Pen( Color.Yellow, 2.0f );
            lp.Pen = linePen;
            plotSurface.Add(lp);
            plotSurface.Title = "AxisConstraint.EqualScaling in action...";

            // Image downloaded from http://squidfingers.com. Thanks!
            Assembly a = Assembly.GetExecutingAssembly();
            System.IO.Stream file =
                a.GetManifestResourceStream( "GtkTest.Resources.pattern01.jpg" );
            System.Drawing.Image im = System.Drawing.Image.FromStream( file );
            plotSurface.PlotBackImage = new Bitmap( im );

            plotSurface.AddInteraction (new PlotZoom());
            plotSurface.AddInteraction (new KeyActions());
            plotSurface.AddAxesConstraint (new AxesConstraint.AspectRatio( 1.0, PlotSurface2D.XAxisPosition.Top, PlotSurface2D.YAxisPosition.Left ) );
            
            plotSurface.XAxis1.WorldMin = plotSurface.YAxis1.WorldMin;
            plotSurface.XAxis1.WorldMax = plotSurface.YAxis1.WorldMax;
            plotSurface.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            
            plotSurface.Refresh();
        }
    }
}

