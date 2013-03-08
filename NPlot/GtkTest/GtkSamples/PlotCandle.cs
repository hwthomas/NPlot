using System;
using System.Drawing;
using System.IO;
using System.Reflection;

using NPlot;

namespace GtkSamples
{
    public class PlotCandle : PlotSample
    {
        public PlotCandle ()
        {
            infoText = "";
            infoText += "Simple CandlePlot example. Demonstrates - \n";
            infoText += " * Setting candle plot datapoints using arrays \n";
            infoText += " * Plot Zoom interaction using MouseWheel ";

            plotSurface.Clear();

            FilledRegion fr = new FilledRegion(
                new VerticalLine(1.2),
                new VerticalLine(2.4));
            fr.Brush = Brushes.BlanchedAlmond;
            plotSurface.Add(fr);

            // note that arrays can be of any type you like.
            int[] opens =  { 1, 2, 1, 2, 1, 3 };
            double[] closes = { 2, 2, 2, 1, 2, 1 };
            float[] lows =   { 0, 1, 1, 1, 0, 0 };
            System.Int64[] highs =  { 3, 2, 3, 3, 3, 4 };
            int[] times =  { 0, 1, 2, 3, 4, 5 };

            CandlePlot cp = new CandlePlot();
            cp.CloseData = closes;
            cp.OpenData = opens;
            cp.LowData = lows;
            cp.HighData = highs;
            cp.AbscissaData = times;
            plotSurface.Add(cp);

            HorizontalLine line = new HorizontalLine( 1.2 );
            line.LengthScale = 0.89f;
            plotSurface.Add( line, -10 );

            VerticalLine line2 = new VerticalLine( 1.2 );
            line2.LengthScale = 0.89f;
            plotSurface.Add( line2 );
            
            plotSurface.AddInteraction (new PlotZoom());

            plotSurface.Title = "Line in the Title Number 1\nFollowed by another title line\n and another";
            plotSurface.Refresh();
        }
    }
}

