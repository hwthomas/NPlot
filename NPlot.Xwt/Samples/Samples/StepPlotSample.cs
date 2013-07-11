using System;
using System.IO;
using System.Reflection;

using Xwt;
using Xwt.Drawing;
using NPlot;

namespace Samples
{
	public class StepPlotSample : PlotSample
	{
		public StepPlotSample () : base ()
		{
			infoText = "";
			infoText += "Sound Wave Example. Demonstrates - \n";
			infoText += " * StepPlot (centered) and HorizontalLine IDrawables \n";
			//infoText += " * AxisDrag Interaction - try left clicking and dragging X or Y axes \n";
			//infoText += " * Vertical & Horizontal GuideLines - without fragmentation problems! \n";
			//infoText += " * Rubberband Selection - click and drag to zoom an area of the plot \n";
			//infoText += " * Key actions : +,- zoom, left/right/up/down pan, Home restores original scale and origin";

			Assembly asm = Assembly.GetExecutingAssembly ();

			Stream file = asm.GetManifestResourceStream ("Samples.Resources.sound.wav");

			System.Int16[] w = new short[5000];
			byte[] a = new byte[10000];

			file.Read( a, 0, 10000 );
			for (int i=100; i<5000; ++i) {
				w[i] = BitConverter.ToInt16(a,i*2);
			}
			file.Close();

			plotCanvas.Clear();
		  
			//plotCanvas.AddInteraction (new AxisDrag ());
			//plotCanvas.AddInteraction (new KeyActions ());
			//plotCanvas.AddInteraction (new NPlot.PlotSelection (Color.Gray));
			//plotCanvas.AddInteraction (new VerticalGuideline (Color.Gray));
			//plotCanvas.AddInteraction (new HorizontalGuideline (Color.Gray));
  
			plotCanvas.Add (new HorizontalLine (2500.0, Colors.LightBlue));
			
			StepPlot sp = new StepPlot ();
			sp.DataSource = w;
			sp.Color = Colors.Yellow;
			sp.Center = true;
			plotCanvas.Add( sp );

			plotCanvas.YAxis1.FlipTicksLabel = true;

			plotCanvas.Canvas.BackgroundColor = new Color (0.375, 0.375, 0.375);
			plotCanvas.PlotBackColor = Colors.DarkBlue;
			plotCanvas.XAxis1.LineColor = Colors.White;
			plotCanvas.YAxis1.LineColor = Colors.White;
			
			PackStart (plotCanvas.Canvas, true);
			Label la = new Label (infoText);
			PackStart (la);
		
		}
	}
}

