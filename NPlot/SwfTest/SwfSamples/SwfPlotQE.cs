using System;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

using NPlot;

namespace SwfSamples
{
	public class PlotQE : PlotSample
	{
		private bool qeExampleTimerEnabled = false;
		private System.Windows.Forms.Timer qeExampleTimer;
		private double[] PlotQEExampleValues;
		private string[] PlotQEExampleTextValues;

		public PlotQE ()
		{
			infoText = "";
			infoText += "Cs2Te Photocathode QE evolution Example. Demonstrates - \n";
			infoText += "  * LabelPointPlot (allows text to be associated with points) \n";
			infoText += "  * PointPlot droplines \n";
			infoText += "  * LabelAxis \n";
			infoText += "  * PhysicalSpacingMin property of LabelAxis \n";
			infoText += "You cannot interact with this chart";

			// This doesn't currently work, as the Timer is not in a Window
			qeExampleTimer = new System.Windows.Forms.Timer ();
			qeExampleTimer.Interval = 500;
			qeExampleTimer.Tick += new EventHandler (qeExampleTimer_Tick);
			qeExampleTimerEnabled = true;

			plotSurface.Clear();
			
			int len = 24;
			string[] s = new string[len];
			PlotQEExampleValues = new double[len];
			PlotQEExampleTextValues = new string[len];

			Random r = new Random();

			for (int i=0; i<len;i++) {
				PlotQEExampleValues[i] = 8.0f + 12.0f * (double)r.Next(10000) / 10000.0f;
				if (PlotQEExampleValues[i] > 18.0f) {
					PlotQEExampleTextValues[i] = "KCsTe";
				}
				else {
					PlotQEExampleTextValues[i] = "";
				}
				s[i] = i.ToString("00") + ".1";
			}

			PointPlot pp = new PointPlot();
			pp.DataSource = PlotQEExampleValues;
			pp.Marker = new Marker( Marker.MarkerType.Square, 10 );
			pp.Marker.DropLine = true;
			pp.Marker.Pen = Pens.CornflowerBlue;
			pp.Marker.Filled = false;
			plotSurface.Add( pp );

			LabelPointPlot tp1 = new LabelPointPlot();
			tp1.DataSource = PlotQEExampleValues;
			tp1.TextData = PlotQEExampleTextValues;
			tp1.LabelTextPosition = LabelPointPlot.LabelPositions.Above;
			tp1.Marker = new Marker( Marker.MarkerType.None, 10 );
			plotSurface.Add( tp1 );

			LabelAxis la = new LabelAxis( plotSurface.XAxis1 );
			for (int i=0; i<len; ++i) {
				la.AddLabel( s[i], i );
			}
			FontFamily ff = new FontFamily( "Verdana" );
			la.TickTextFont = new Font( ff, 7 );
			la.PhysicalSpacingMin = 25;
			plotSurface.XAxis1 = la;

			plotSurface.Title = "Cs2Te Photocathode QE evolution";
			plotSurface.TitleFont = new Font(ff,15);
			plotSurface.XAxis1.WorldMin = -1.0f;
			plotSurface.XAxis1.WorldMax = len;
			plotSurface.XAxis1.LabelFont = new Font( ff, 10 );
			plotSurface.XAxis1.Label = "Cathode ID";
			plotSurface.YAxis1.Label = "QE [%]";
			plotSurface.YAxis1.LabelFont = new Font( ff, 10 );
			plotSurface.YAxis1.TickTextFont = new Font( ff, 10 );

			plotSurface.YAxis1.WorldMin = 0.0;
			plotSurface.YAxis1.WorldMax= 25.0;

			plotSurface.XAxis1.TicksLabelAngle = 60.0f;

			plotSurface.Refresh();
			
		}

		/// <summary>
		/// Callback for QE example timer tick.
		/// </summary>
		/// <param name="sender">unused</param>
		/// <param name="e">unused</param>
		private void qeExampleTimer_Tick (object sender, System.EventArgs e)
		{
			Random r = new Random();

			for (int i=0; i<PlotQEExampleValues.Length; ++i) {
				PlotQEExampleValues[i] = 8.0f + 12.0f * (double)r.Next(10000) / 10000.0f;
				if ( PlotQEExampleValues[i] > 18.0f ) {
					PlotQEExampleTextValues[i] = "KCsTe";
				}
				else {
					PlotQEExampleTextValues[i] = "";
				}
			}
			plotSurface.Refresh();
		}

	}
}

