//
// XwPlot - A cross-platform charting library using the Xwt toolkit
// 
// HistogramSample.cs
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
	public class HistogramSample : PlotSample
	{
		public HistogramSample () : base ()
		{
			infoText = "";
			infoText += "Gaussian Example. Demonstrates - \n";
			infoText += "  * HistogramPlot and LinePlot" ;

			plotCanvas.Clear();
	
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
			plotCanvas.Add (sp);
			plotCanvas.Add (lp);
			plotCanvas.Legend = new Legend ();
			plotCanvas.YAxis1.WorldMin = 0.0;
			plotCanvas.Title = "Histogram Plot";

			PackStart (plotCanvas.Canvas, true);
			Label la = new Label (infoText);
			PackStart (la);
		}
	}
}

