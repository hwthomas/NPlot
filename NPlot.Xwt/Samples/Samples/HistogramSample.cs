using System;
using System.IO;
using System.Reflection;
using Xwt;
using Xwt.Drawing;

using NPlot;

namespace Samples
{
	public class HistogramSample : PlotSample
	{
		public HistogramSample () : base ()
		{
			infoText = "";
			infoText += "Gaussian Example. Demonstrates - \n";
			infoText += "  * HistogramPlot and LinePlot" ;

			plotSurface.Clear();
	
			System.Random r = new Random ();
			
			int len = 35;
			double[] a = new double [len];
			double[] b = new double [len];

			for (int i=0; i<len; ++i) {
				a[i] = Math.Exp (-(double)(i-len/2)*(double)(i-len/2)/50.0);
				b[i] = a[i] + (r.Next(10)/50.0f)-0.05f;
				if (b[i] < 0) {
					b[i] = 0;
				}
			}

			HistogramPlot sp = new HistogramPlot ();
			sp.DataSource = b;
			sp.BorderColor = Colors.DarkBlue;
			sp.Filled = true;
			sp.FillColor = Colors.Gold; //Gradient (Colors.Lavender, Color.Gold );
			sp.BaseWidth = 0.5;
			sp.Label = "Random Data";

			LinePlot lp = new LinePlot ();
			lp.DataSource = a;
			lp.LineColor = Colors.Blue;
			lp.LineWidth = 3;
			lp.Label = "Gaussian Function";
			plotSurface.Add (sp);
			plotSurface.Add (lp);
			plotSurface.Legend = new Legend ();
			plotSurface.YAxis1.WorldMin = 0.0;
			plotSurface.Title = "Histogram Plot";

			PackStart (plotSurface.Canvas, BoxMode.FillAndExpand);
			Label la = new Label (infoText);
			PackStart (la, BoxMode.None);
		}
	}
}

