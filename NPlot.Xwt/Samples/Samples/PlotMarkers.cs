//
// XwPlot - A cross-platform charting library using the Xwt toolkit
// 
// PlotMarkers.cs
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
	public class PlotMarkerSample : PlotSample
	{
		public PlotMarkerSample () : base ()
		{
			infoText = "";
			infoText += "Markers Example. Demonstrates - \n";
			infoText += " * PointPlot and the available marker types \n";
			infoText += " * Legends, and how to place them.";

			plotCanvas.Clear();
			
			double[] y = new double[1] {1.0};
			foreach (object i in Enum.GetValues (typeof(Marker.MarkerType))) {
				Marker m = new Marker( (Marker.MarkerType)Enum.Parse(typeof(Marker.MarkerType), i.ToString()), 8 );
				m.FillColor = Colors.Red;
				double[] x = new double[1];
				x[0] = (double) m.Type;
				PointPlot pp = new PointPlot();
				pp.OrdinateData = y;
				pp.AbscissaData = x;
				pp.Marker = m;
				pp.Label = m.Type.ToString();
				plotCanvas.Add (pp);
			}
			plotCanvas.Title = "Markers";
			plotCanvas.YAxis1.Label = "Index";
			plotCanvas.XAxis1.Label = "Marker";
			plotCanvas.YAxis1.WorldMin = 0.0;
			plotCanvas.YAxis1.WorldMax = 2.0;
			plotCanvas.XAxis1.WorldMin -= 1.0;
			plotCanvas.XAxis1.WorldMax += 1.0;

			Legend legend = new Legend();
			legend.AttachTo( XAxisPosition.Top, YAxisPosition.Right );
			legend.VerticalEdgePlacement = Legend.Placement.Outside;
			legend.HorizontalEdgePlacement = Legend.Placement.Inside;
			legend.XOffset = 5; // note that these numbers can be negative.
			legend.YOffset = 0;
			plotCanvas.Legend = legend;

			PackStart (plotCanvas.Canvas, true);
			Label la = new Label (infoText);
			PackStart (la);
		
		}
	}
}

