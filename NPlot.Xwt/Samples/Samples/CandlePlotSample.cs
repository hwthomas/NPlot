//
// XwPlot - A cross-platform charting library using the Xwt toolkit
// 
// CandlePlotSample.cs
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

