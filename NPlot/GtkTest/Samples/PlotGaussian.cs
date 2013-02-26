using System;
using System.Drawing;
using System.IO;
using System.Reflection;

using Gtk;
using NPlot;

namespace GtkTest
{
    public class PlotGaussian : PlotSample
    {
        public PlotGaussian ()
        {
            infoText = "";
            infoText += "Gaussian Example. Demonstrates - \n";
            infoText += "  * HistogramPlot and LinePlot." ;

            plotSurface.Clear();
    
            System.Random r = new Random();
            
            int len = 35;
            double[] a = new double[len];
            double[] b = new double[len];

            for (int i=0; i<len; ++i) 
            {
                int j = len-1-i;
                a[i] = (double)Math.Exp(-(double)(i-len/2)*(double)(i-len/2)/50.0f);
                b[i] = a[i] + (r.Next(10)/50.0f)-0.05f;
                if (b[i] < 0.0f) 
                {
                    b[i] = 0;
                }
            }

            HistogramPlot sp = new HistogramPlot();
            sp.DataSource = b;
            sp.Pen = Pens.DarkBlue;
            sp.Filled = true;
            sp.RectangleBrush = new RectangleBrushes.HorizontalCenterFade( Color.Lavender, Color.Gold );
            sp.BaseWidth = 0.5f;
            sp.Label = "Random Data";
            LinePlot lp = new LinePlot();
            lp.DataSource = a;
            lp.Pen = new Pen( Color.Blue, 3.0f );
            lp.Label = "Gaussian Function";
            plotSurface.Add( sp );
            plotSurface.Add( lp );
            plotSurface.Legend = new Legend();
            plotSurface.YAxis1.WorldMin = 0.0f;
            plotSurface.Title = "Histogram Plot";
            plotSurface.Refresh();
        }
    }
}

