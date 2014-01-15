//
// XwPlot - A cross-platform charting library using the Xwt toolkit
// 
// StepPlotSample.cs
// Copyright (C) 2003-2006 Matt Howlett and others
// Ported from NPlot to Xwt 2012-2014 : Hywel Thomas <hywel.w.thomas@gmail.com>
//
// All rights reserved.
// 
// Redistribution and use in source and binary forms, with or without modification,
// are permitted provided that the following conditions are met:
// 
// 1. Redistributions of source code must retain the above copyright notice, this
//	  list of conditions and the following disclaimer.
// 2. Redistributions in binary form must reproduce the above copyright notice,
//	  this list of conditions and the following disclaimer in the documentation
//	  and/or other materials provided with the distribution.
// 3. Neither the name of NPlot nor the names of its contributors may
//	  be used to endorse or promote products derived from this software without
//	  specific prior written permission.
// 
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
// ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
// WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED.
// IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT,
// INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING,
// BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
// DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF
// LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE
// OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED
// OF THE POSSIBILITY OF SUCH DAMAGE.
//
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

			file.Read (a, 0, 10000);
			for (int i=100; i<5000; ++i) {
				w[i] = BitConverter.ToInt16 (a,i*2);
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

			plotCanvas.YAxis1.FlipTickText = true;

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

