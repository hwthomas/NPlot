using System;
using System.Drawing;
using System.IO;
using System.Reflection;

using NPlot;

namespace SwfSamples
{
    public class PlotLabelAxis : PlotSample
    {
        public PlotLabelAxis ()
        {
            infoText = "";
            infoText += "Internet Usage Example. Demonstrates - \n";
            infoText += " * Label Axis with angled text. \n";
            infoText += " * RectangleBrushes.";
            
            plotSurface.Clear();

            Grid mygrid = new Grid();
            mygrid.VerticalGridType = Grid.GridType.Coarse;
            Pen majorGridPen = new Pen( Color.LightGray );
            float[] pattern = { 1.0f, 2.0f };
            majorGridPen.DashPattern = pattern;
            mygrid.MajorGridPen = majorGridPen;
            plotSurface.Add( mygrid );

            float[] xs = {20.0f, 31.0f, 27.0f, 38.0f, 24.0f, 3.0f, 2.0f };
            float[] xs2 = {7.0f, 10.0f, 42.0f, 9.0f, 2.0f, 79.0f, 70.0f };
            float[] xs3 = {1.0f, 20.0f, 20.0f, 25.0f, 10.0f, 30.0f, 30.0f };

            HistogramPlot hp = new HistogramPlot();
            hp.DataSource = xs;
            hp.BaseWidth = 0.6f;
            hp.RectangleBrush =
                new RectangleBrushes.HorizontalCenterFade( Color.FromArgb(255,255,200), Color.White );
            hp.Filled = true;
            hp.Label = "Developer Work";
            
            HistogramPlot hp2 = new HistogramPlot();
            hp2.DataSource = xs2;
            hp2.Label = "Web Browsing";
            hp2.RectangleBrush = RectangleBrushes.Horizontal.FaintGreenFade;
            hp2.Filled = true;
            hp2.StackedTo( hp );
            
            HistogramPlot hp3 = new HistogramPlot();
            hp3.DataSource = xs3;
            hp3.Label = "P2P Downloads";
            hp3.RectangleBrush = RectangleBrushes.Vertical.FaintBlueFade;
            hp3.Filled = true;
            hp3.StackedTo( hp2 );
            
            plotSurface.Add( hp );
            plotSurface.Add( hp2 );
            plotSurface.Add( hp3 );
            
            plotSurface.Legend = new Legend();

            LabelAxis la = new LabelAxis( plotSurface.XAxis1 );
            la.AddLabel( "Monday", 0.0f );
            la.AddLabel( "Tuesday", 1.0f );
            la.AddLabel( "Wednesday", 2.0f );
            la.AddLabel( "Thursday", 3.0f );
            la.AddLabel( "Friday", 4.0f );
            la.AddLabel( "Saturday", 5.0f );
            la.AddLabel( "Sunday", 6.0f );
            la.Label = "Days";
            la.TickTextFont = new Font( "Courier New", 8 );
            la.TicksBetweenText = true;

            plotSurface.XAxis1 = la;
            plotSurface.YAxis1.WorldMin = 0.0;
            plotSurface.YAxis1.Label = "MBytes";
            ((LinearAxis)plotSurface.YAxis1).NumberOfSmallTicks = 1;

            plotSurface.Title = "Internet useage for user:\n johnc 09/01/03 - 09/07/03";

            plotSurface.XAxis1.TicksLabelAngle = 30.0f;

            plotSurface.PlotBackBrush = RectangleBrushes.Vertical.FaintRedFade;
            plotSurface.Refresh();
        }
    }
}

