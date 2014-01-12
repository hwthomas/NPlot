//
// NPlot - A charting library for .NET
// 
// FilledRegion.cs
// 
// Copyright (C) 2003-2006 Matt Howlett and others
// Port to Xwt 2012-2013 : Hywel Thomas <hywel.w.thomas@gmail.com>
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
using Xwt;
using Xwt.Drawing;

namespace NPlot
{
	/// <summary>
	/// A quick and dirty Filled region plottable object
	/// </summary>
	/// <remarks>
	/// This could be integrated with LinePlot at some stage since a FilledRegion
	/// can be bounded by two LinePlots.  Describing the Paths once and then both
	/// doing a FillPreserve () and a Stroke () would be more efficient. In addition,
	/// the current approach risks missing the line plot (if done before the fill).
	/// </remarks>
	public class FilledRegion : IDrawable
	{
		private VerticalLine vl1;
		private VerticalLine vl2;

		private HorizontalLine hl1;
		private HorizontalLine hl2;

		private LinePlot lp1;
		private LinePlot lp2;

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="lp1">LinePlot that provides bounds to filled region [upper or lower]</param>
		/// <param name="lp2">LinePlot that provides bounds to filled region [upper or lower]</param>
		/// <remarks>TODO: make this work with other plot types.</remarks>
		public FilledRegion (LinePlot plot1, LinePlot plot2)
		{
			lp1 = plot1;
			lp2 = plot2;
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="l1">Vertical line to provide bounds for filled region</param>
		/// <param name="l2">The other Vertical line to provide bounds for filled region</param>
		public FilledRegion (VerticalLine line1, VerticalLine line2)
		{
			vl1 = line1;
			vl2 = line2;
		}


		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="l1">Vertical line to provide bounds for filled region</param>
		/// <param name="l2">The other Vertical line to provide bounds for filled region</param>
		public FilledRegion (HorizontalLine line1, HorizontalLine line2)
		{
			hl1 = line1;
			hl2 = line2;
		}

		/// <summary>
		/// Use this Color for the Filled Region.
		/// </summary>
		public Color FillColor { get; set; }

		/// <summary>
		/// Draw the filled region
		/// </summary>
		/// <param name="g">The Drawing Context with which to draw.</param>
		/// <param name="xAxis">The X-Axis to draw against.</param>
		/// <param name="yAxis">The Y-Axis to draw against.</param>
		public void Draw (Context ctx, PhysicalAxis xAxis, PhysicalAxis yAxis)
		{
			ITransform2D t = Transform2D.GetTransformer (xAxis, yAxis);

			if (hl1 != null && hl2 != null) {
				ctx.MoveTo (t.Transform (xAxis.Axis.WorldMin, hl1.OrdinateValue));
				ctx.LineTo (t.Transform (xAxis.Axis.WorldMax, hl1.OrdinateValue));
				ctx.LineTo (t.Transform (xAxis.Axis.WorldMax, hl2.OrdinateValue));
				ctx.LineTo (t.Transform (xAxis.Axis.WorldMin, hl2.OrdinateValue));
			} else if (vl1 != null && vl2 != null) {
				ctx.MoveTo (t.Transform (vl1.AbscissaValue, yAxis.Axis.WorldMin));
				ctx.LineTo (t.Transform (vl1.AbscissaValue, yAxis.Axis.WorldMax));
				ctx.LineTo (t.Transform (vl2.AbscissaValue, yAxis.Axis.WorldMax));
				ctx.LineTo (t.Transform (vl2.AbscissaValue, yAxis.Axis.WorldMin));
			} else if (lp1 != null && lp2 != null) {
				SequenceAdapter a1 = new SequenceAdapter (lp1.DataSource, lp1.DataMember, lp1.OrdinateData, lp1.AbscissaData);
				SequenceAdapter a2 = new SequenceAdapter (lp2.DataSource, lp2.DataMember, lp2.OrdinateData, lp2.AbscissaData);
				// Start at first point of LinePlot 1
				ctx.MoveTo (t.Transform (a1 [0]));
				// Join LinePlot 1 points in ascending order
				for (int i = 1; i < a1.Count; ++i) {
					ctx.LineTo (t.Transform (a1[i]));
				}
				// Then join LinePlot 2 points in descending order
				for (int i = a2.Count-1; i >= 0; --i) {
					ctx.LineTo (t.Transform (a2[i]));
				}
			}
			else {
				throw new NPlotException ("Filled Region bounds not defined");
			}

			ctx.Save ();
			ctx.SetColor (FillColor);
			ctx.Fill ();
			ctx.Restore ();
		}
	}
}
