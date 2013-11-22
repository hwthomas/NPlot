using System;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Data;
using System.Collections;

using NPlot;

namespace GtkSamples
{
	public class PlotDataset : PlotSample
	{
		public PlotDataset ()
		{
			infoText = "";
			infoText += "Stock Dataset Sample. Demonstrates - \n";
			infoText += " * CandlePlot, FilledRegion, LinePlot and ArrowItem IDrawables \n";
			infoText += " * DateTime axes \n";
			infoText += " * Horizontal Drag Interaction - try dragging (and Ctrl-dragging) the plot surface \n";
			infoText += " * Axis Drag Interaction - try dragging in the horizontal and vertical Axis areas";

			plotSurface.Clear();
			// [NOTIMP] plotSurface.DateTimeToolTip = true;

			// obtain stock information from xml file
			DataSet ds = new DataSet();
			System.IO.Stream file =
				Assembly.GetExecutingAssembly().GetManifestResourceStream( "GtkTest.Resources.asx_jbh.xml" );
			ds.ReadXml( file, System.Data.XmlReadMode.ReadSchema );
			DataTable dt = ds.Tables[0];
			// DataView dv = new DataView( dt );

			// create CandlePlot.
			CandlePlot cp = new CandlePlot();
			cp.DataSource = dt;
			cp.AbscissaData = "Date";
			cp.OpenData = "Open";
			cp.LowData = "Low";
			cp.HighData = "High";
			cp.CloseData = "Close";
			cp.BearishColor = Color.Red;
			cp.BullishColor = Color.Green;
			cp.Style = CandlePlot.Styles.Filled;

			// calculate 10 day moving average and 2*sd line
			ArrayList av10 = new ArrayList();
			ArrayList sd2_10 = new ArrayList();
			ArrayList sd_2_10 = new ArrayList();
			ArrayList dates = new ArrayList();
			for (int i=0; i<dt.Rows.Count-10; ++i)
			{
				float sum = 0.0f;
				for (int j=0; j<10; ++j)
				{
					sum += (float)dt.Rows[i+j]["Close"];
				}
				float average = sum / 10.0f;
				av10.Add( average );
				sum = 0.0f;
				for (int j=0; j<10; ++j)
				{
					sum += ((float)dt.Rows[i+j]["Close"]-average)*((float)dt.Rows[i+j]["Close"]-average);
				}
				sum /= 10.0f;
				sum = 2.0f*(float)Math.Sqrt( sum );
				sd2_10.Add( average + sum );
				sd_2_10.Add( average - sum );
				dates.Add( (DateTime)dt.Rows[i+10]["Date"] );
			}

			// and a line plot of close values.
			LinePlot av = new LinePlot();
			av.OrdinateData = av10;
			av.AbscissaData = dates;
			av.Color = Color.LightGray;
			av.Pen.Width = 2.0f;

			LinePlot top = new LinePlot();
			top.OrdinateData = sd2_10;
			top.AbscissaData = dates;
			top.Color = Color.LightSteelBlue;
			top.Pen.Width = 2.0f;

			LinePlot bottom = new LinePlot();
			bottom.OrdinateData = sd_2_10;
			bottom.AbscissaData = dates;
			bottom.Color = Color.LightSteelBlue;
			bottom.Pen.Width = 2.0f;

			FilledRegion fr = new FilledRegion( top, bottom );
			fr.RectangleBrush = new RectangleBrushes.Vertical( Color.FromArgb(255,255,240), Color.FromArgb(240,255,255) );
			plotSurface.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

			plotSurface.Add( fr );

			plotSurface.Add( new Grid() );

			plotSurface.Add( av );
			plotSurface.Add( top );
			plotSurface.Add( bottom );
			plotSurface.Add( cp );

			// now make an arrow... 
			ArrowItem arrow = new ArrowItem( new PointD( ((DateTime)dt.Rows[60]["Date"]).Ticks, 2.28 ), -80, "An interesting flat bit" );
			arrow.ArrowColor = Color.DarkBlue;
			arrow.PhysicalLength = 50;
			
			plotSurface.Add (arrow);

			plotSurface.Title = "AU:JBH";
			plotSurface.XAxis1.Label = "Date / Time";
			plotSurface.XAxis1.WorldMin += plotSurface.XAxis1.WorldLength / 4.0;
			plotSurface.XAxis1.WorldMax -= plotSurface.XAxis1.WorldLength / 2.0;
			plotSurface.YAxis1.Label = "Price [$]";

			plotSurface.XAxis1 = new TradingDateTimeAxis( plotSurface.XAxis1 );

			plotSurface.AddInteraction (new PlotDrag(true,false));
			plotSurface.AddInteraction (new AxisDrag());

			
			// make sure plot surface colors are as we expect - the wave example changes them.
			plotSurface.PlotBackColor = Color.White;
			plotSurface.XAxis1.Color = Color.Black;
			plotSurface.YAxis1.Color = Color.Black;

			plotSurface.Refresh();
		}
	}
}

