using System;
using System.Drawing;
using System.IO;
using System.Reflection;

using NPlot;

namespace SwfSamples
{
	public class PlotWave : PlotSample
	{
		public PlotWave() : base ()
		{
			infoText = "";
			infoText += "Sound Wave Example. Demonstrates - \n";
			infoText += " * StepPlot (centered) and HorizontalLine IDrawables \n";
			infoText += " * AxisDrag Interaction - try left clicking and dragging X or Y axes \n";
			infoText += " * Vertical & Horizontal GuideLines - without fragmentation problems! \n";
			infoText += " * Rubberband Selection - click and drag to zoom an area of the plot \n";
			infoText += " * Key actions : +,- zoom, left/right/up/down pan, Home restores original scale and origin";
			
			System.IO.Stream file =
				Assembly.GetExecutingAssembly().GetManifestResourceStream("SwfTest.Resources.sound.wav");

			System.Int16[] w = new short[5000];
			byte[] a = new byte[10000];

			file.Read( a, 0, 10000 );
			for (int i=100; i<5000; ++i) {
				w[i] = BitConverter.ToInt16(a,i*2);
			}
			file.Close();

			plotSurface.Clear();
		  
			plotSurface.AddInteraction (new AxisDrag ());
			plotSurface.AddInteraction (new KeyActions ());
			plotSurface.AddInteraction (new NPlot.PlotSelection (Color.Gray));
			plotSurface.AddInteraction (new VerticalGuideline (Color.Gray));
			plotSurface.AddInteraction (new HorizontalGuideline (Color.Gray));
  
			plotSurface.Add(new HorizontalLine(0.0, Color.LightBlue));
			
			StepPlot sp = new StepPlot ();
			sp.DataSource = w;
			sp.Color = Color.Yellow;
			sp.Center = true;
			plotSurface.Add( sp );

			plotSurface.YAxis1.FlipTicksLabel = true;

			plotSurface.PlotBackColor = Color.DarkBlue;
			plotSurface.Canvas.BackColor = Color.FromArgb (100, 100, 100);
			plotSurface.XAxis1.Color = Color.White;
			plotSurface.YAxis1.Color = Color.White;

			plotSurface.Refresh();

		}

	}
}

