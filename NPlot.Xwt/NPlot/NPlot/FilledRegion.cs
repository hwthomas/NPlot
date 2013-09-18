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
	public class FilledRegion : IDrawable
	{
		private VerticalLine vl1_;
		private VerticalLine vl2_;

		private HorizontalLine hl1_;
		private HorizontalLine hl2_;

		private LinePlot lp1_;
		private LinePlot lp2_;

		private Brush brush_ = new SolidBrush( Color.GhostWhite );
		private IRectangleBrush areaBrush_ = null;

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="lp1">LinePlot that provides bounds to filled region [upper or lower]</param>
		/// <param name="lp2">LinePlot that provides bounds to filled region [upper or lower]</param>
		/// <remarks>TODO: make this work with other plot types.</remarks>
		public FilledRegion (LinePlot lp1, LinePlot lp2)
		{
			lp1_ = lp1;
			lp2_ = lp2;
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="l1">Vertical line to provide bounds for filled region</param>
		/// <param name="l2">The other Vertical line to provide bounds for filled region</param>
		public FilledRegion (VerticalLine l1, VerticalLine l2)
		{
			vl1_ = l1;
			vl2_ = l2;
		}


		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="l1">Vertical line to provide bounds for filled region</param>
		/// <param name="l2">The other Vertical line to provide bounds for filled region</param>
		public FilledRegion (HorizontalLine l1, HorizontalLine l2)
		{
			hl1_ = l1;
			hl2_ = l2;
		}


		/// <summary>
		/// Draw the filled region
		/// </summary>
		/// <param name="g">The GDI+ surface on which to draw.</param>
		/// <param name="xAxis">The X-Axis to draw against.</param>
		/// <param name="yAxis">The Y-Axis to draw against.</param>
		public void Draw (Context ctx, PhysicalAxis xAxis, PhysicalAxis yAxis)
		{
			ITransform2D t = Transform2D.GetTransformer (xAxis, yAxis);

			Brush b = brush_;
			if (b == null) {
				b = areaBrush_.Get(new Rectangle(xAxis.PhysicalMin.X, yAxis.PhysicalMax.Y, xAxis.PhysicalLength, yAxis.PhysicalLength));
			}

			if (hl1_ != null && hl2_ != null) {
				Point [] points = new Point [4];
				points [0] = t.Transform (xAxis.Axis.WorldMin, hl1_.OrdinateValue);
				points [1] = t.Transform (xAxis.Axis.WorldMax, hl1_.OrdinateValue);
				points [2] = t.Transform (xAxis.Axis.WorldMax, hl2_.OrdinateValue);
				points [3] = t.Transform (xAxis.Axis.WorldMin, hl2_.OrdinateValue);

				g.FillPolygon (b, points);
			}	
			else if (vl1_ != null && vl2_ != null) {
				Point [] points = new Point [4];
				points [0] = t.Transform (vl1_.AbscissaValue, yAxis.Axis.WorldMin);
				points [1] = t.Transform (vl1_.AbscissaValue, yAxis.Axis.WorldMax);
				points [2] = t.Transform (vl2_.AbscissaValue, yAxis.Axis.WorldMax);
				points [3] = t.Transform (vl2_.AbscissaValue, yAxis.Axis.WorldMin);

				g.FillPolygon (b, points);
			}
			else if (lp1_ != null && lp2_ != null) {
				SequenceAdapter a1 = new SequenceAdapter (lp1_.DataSource, lp1_.DataMember, lp1_.OrdinateData, lp1_.AbscissaData);
				SequenceAdapter a2 = new SequenceAdapter (lp2_.DataSource, lp2_.DataMember, lp2_.OrdinateData, lp2_.AbscissaData);

				int count = a1.Count + a2.Count;
				Point [] points = new Point [count];
				for (int i = 0; i < a1.Count; ++i) {
					points [i] = t.Transform (a1[i]);
				}
				for (int i = 0; i < a2.Count; ++i) {
					points[i + a1.Count] = t.Transform (a2[a2.Count - i - 1]);
				}

				g.FillPolygon (b, points);
			}
			else {
				throw new NPlotException ("One of bounds was set to null");
			}
		}


		/// <summary>
		/// Use this brush (and not a RectangleBrush) for drawing.
		/// </summary>
		public Brush Brush
		{
			set {
				brush_ = value;
				areaBrush_ = null;
			}
		}


		/// <summary>
		/// Use this RectangleBrush (and not a normal Brush) for drawing.
		/// </summary>
		public IRectangleBrush RectangleBrush
		{
			set {
				brush_ = null;
				areaBrush_ = value;
			}
		}


	}
}
