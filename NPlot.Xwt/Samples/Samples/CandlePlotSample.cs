using System;
using System.IO;
using System.Reflection;

using Xwt;
using Xwt.Drawing;

using NPlot;

namespace Samples
{
	public class CandlePlotSample : PlotSample
	{
		public CandlePlotSample () : base ()
		{
			infoText = "";
			infoText += "Simple CandlePlot example. Demonstrates - \n";
			infoText += " * Setting candle plot datapoints using arrays \n";

			plotCanvas.Clear();

			//FilledRegion fr = new FilledRegion (new VerticalLine (1.2), new VerticalLine (2.4));
			//fr.Brush = Brushes.BlanchedAlmond;
			//plotCanvas.Add (fr);

			// note that arrays can be of any type you like.
			int[] opens =  {1, 2, 1, 2, 1, 3};
			double[] closes = {2, 2, 2, 1, 2, 1};
			float[] lows =   {0, 1, 1, 1, 0, 0};
			System.Int64[] highs =  {3, 2, 3, 3, 3, 4};
			int[] times =  {0, 1, 2, 3, 4, 5};

			CandlePlot cp = new CandlePlot ();
			cp.CloseData = closes;
			cp.OpenData = opens;
			cp.LowData = lows;
			cp.HighData = highs;
			cp.AbscissaData = times;
			plotCanvas.Add (cp);

			HorizontalLine line = new HorizontalLine (1.2);
			line.LengthScale = 0.89;
			plotCanvas.Add (line, -10);

			//VerticalLine line2 = new VerticalLine ( 1.2 );
			//line2.LengthScale = 0.89;
			//plotCanvas.Add (line2);
			
			plotCanvas.Title = "Line in the Title Number 1\nFollowed by another title line\n and another";
			plotCanvas.Refresh ();

			plotCanvas.Legend = new Legend(	);
			plotCanvas.LegendZOrder = 1; // default zorder for adding idrawables is 0, so this puts legend on top.

			PackStart (plotCanvas.Canvas, true);
			Label la = new Label (infoText);
			PackStart (la);
		}
	}
}

