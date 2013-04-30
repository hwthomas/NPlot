//
// NPlot - A charting library for .NET
// 
// PointPlot.cs
// Copyright (C) 2003-2006 Matt Howlett and others.
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
	/// Encapsulates functionality for drawing data as a series of points.
	/// </summary>
	public class PointPlot : BaseSequencePlot, ISequencePlot, IPlot
	{
		private Marker marker;

		/// <summary>
		/// Default Constructor
		/// </summary>
		public PointPlot ()
		{
			marker = new Marker ();
		}

		/// <summary>
		/// Constructor for the marker plot.
		/// </summary>
		/// <param name="marker">The marker to use.</param>
		public PointPlot (Marker marker)
		{
			this.marker = marker;
		}


		/// <summary>
		/// Draws the point plot using the Drawing Context and x and y axes supplied
		/// </summary>
		/// <param name="ctx">The Drawing Context with which to draw.</param>
		/// <param name="xAxis">The X-Axis to draw against.</param>
		/// <param name="yAxis">The Y-Axis to draw against.</param>
		public virtual void Draw (Context ctx, PhysicalAxis xAxis, PhysicalAxis yAxis)
		{
			SequenceAdapter data_ = new SequenceAdapter (DataSource, DataMember, OrdinateData, AbscissaData );

			double leftCutoff_ = xAxis.PhysicalMin.X - marker.Size;
			double rightCutoff_ = xAxis.PhysicalMax.X + marker.Size;

			ctx.Save ();
			ctx.SetColor (marker.LineColor);
			ctx.SetLineWidth (marker.LineWidth);

			for (int i=0; i<data_.Count; ++i) {
				if (!Double.IsNaN(data_[i].X) && !Double.IsNaN(data_[i].Y)) {
					Point xPos = xAxis.WorldToPhysical (data_[i].X, false);
					if (xPos.X < leftCutoff_ || rightCutoff_ < xPos.X) {
						continue;
					}

					Point yPos = yAxis.WorldToPhysical (data_[i].Y, false);
					marker.Draw (ctx, xPos.X, yPos.Y);
					if (marker.DropLine) {
						Point yMin = new Point (data_[i].X, Math.Max (0.0, yAxis.Axis.WorldMin));
						Point yStart = yAxis.WorldToPhysical (yMin.Y, false);
						ctx.MoveTo (xPos.X, yStart.Y);
						ctx.LineTo (xPos.X, yPos.Y);
						ctx.Stroke ();
					}
				}
			}
			ctx.Restore ();
		}


		/// <summary>
		/// Returns an x-axis that is suitable for drawing this plot.
		/// </summary>
		/// <returns>A suitable x-axis.</returns>
		public Axis SuggestXAxis()
		{
			SequenceAdapter data_ = new SequenceAdapter (DataSource, DataMember, OrdinateData, AbscissaData);
			return data_.SuggestXAxis();
		}


		/// <summary>
		/// Returns a y-axis that is suitable for drawing this plot.
		/// </summary>
		/// <returns>A suitable y-axis.</returns>
		public Axis SuggestYAxis()
		{
			SequenceAdapter data_ = new SequenceAdapter (DataSource, DataMember, OrdinateData, AbscissaData);
			return data_.SuggestYAxis();
		}


		/// <summary>
		/// Draws a representation of this plot in the legend.
		/// </summary>
		/// <param name="ctx">The Drawing Context with which to draw.</param>
		/// <param name="startEnd">A rectangle specifying the bounds of the area in the legend set aside for drawing.</param>
		public void DrawInLegend (Context ctx, Rectangle startEnd )
		{
			ctx.Save ();

			if (marker.Size > 0) {
				marker.Draw (ctx, (startEnd.Left + startEnd.Right) / 2, (startEnd.Top + startEnd.Bottom) / 2);
			}
			else if (marker.LineWidth > 0) {
				ctx.SetLineWidth (marker.LineWidth);
				ctx.SetColor (marker.LineColor);
				ctx.MoveTo ((startEnd.Left + startEnd.Right)/2, (startEnd.Top + startEnd.Bottom - marker.LineWidth)/2);
				ctx.LineTo ((startEnd.Left + startEnd.Right)/2, (startEnd.Top + startEnd.Bottom + marker.LineWidth)/2);
				ctx.Stroke ();
			}
			ctx.Restore ();
		}


		/// <summary>
		/// The Marker object used for the plot.
		/// </summary>
		public Marker Marker
		{
			get { return marker; }
			set { marker = value; }
		}
	}
}
