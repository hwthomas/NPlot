using System;
using System.Drawing;
using System.IO;
using System.Reflection;

using NPlot;

namespace GtkSamples
{
	public class PlotMarkers : PlotSample
	{
		public PlotMarkers ()
		{
			infoText = "";
			infoText += "Markers Example. Demonstrates - \n";
			infoText += " * PointPlot and the available marker types \n";
			infoText += " * Legends, and how to place them.";

			plotSurface.Clear();
			
			double[] y = new double[1] {1.0f};
			foreach (object i in Enum.GetValues(typeof(Marker.MarkerType)))
			{
				Marker m = new Marker( (Marker.MarkerType)Enum.Parse(typeof(Marker.MarkerType), i.ToString()), 8 );
				double[] x = new double[1];
				x[0] = (double) m.Type;
				PointPlot pp = new PointPlot();
				pp.OrdinateData = y;
				pp.AbscissaData = x;
				pp.Marker = m;
				pp.Label = m.Type.ToString();
				plotSurface.Add( pp );
			}
			plotSurface.Title = "Markers";
			plotSurface.YAxis1.Label = "Index";
			plotSurface.XAxis1.Label = "Marker";
			plotSurface.YAxis1.WorldMin = 0.0f;
			plotSurface.YAxis1.WorldMax = 2.0f;
			plotSurface.XAxis1.WorldMin -= 1.0f;
			plotSurface.XAxis1.WorldMax += 1.0f;

			Legend legend = new Legend();
			legend.AttachTo( XAxisPosition.Top, YAxisPosition.Right );
			legend.VerticalEdgePlacement = Legend.Placement.Outside;
			legend.HorizontalEdgePlacement = Legend.Placement.Inside;
			legend.XOffset = 5; // note that these numbers can be negative.
			legend.YOffset = 0;
			plotSurface.Legend = legend;

			plotSurface.Refresh();
		}
	}
}

