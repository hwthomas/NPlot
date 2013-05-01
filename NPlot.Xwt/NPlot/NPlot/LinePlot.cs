//
// NPlot - A charting library for .NET
// 
// LinePlot.cs
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
using System.Diagnostics;
using Xwt;
using Xwt.Drawing;

namespace NPlot
{

	/// <summary>
	/// Encapsulates functionality for plotting data as a line chart.
	/// </summary>
	public class LinePlot : BaseSequencePlot, IPlot, ISequencePlot
	{
		/// <summary>
		/// Default constructor
		/// </summary>
		public LinePlot ()
		{
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="dataSource">The data source to associate with this plot</param>
		public LinePlot (object dataSource)
		{
			DataSource = dataSource;
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="ordinateData">the ordinate data to associate with this plot.</param>
		/// <param name="abscissaData">the abscissa data to associate with this plot.</param>
		public LinePlot (object ordinateData, object abscissaData)
		{
			OrdinateData = ordinateData;
			AbscissaData = abscissaData;
		}


		/// <summary>
		/// Draws the line plot using the Context and Physical Axes provided
		/// </summary>
		/// <param name="ctx">The Drawing Context with which to draw.</param>
		/// <param name="xAxis">The X-Axis to draw against.</param>
		/// <param name="yAxis">The Y-Axis to draw against.</param>
		/// <param name="drawShadow">If true draw the shadow for the line. If false, draw line.</param>
		public void DrawLineOrShadow (Context ctx, PhysicalAxis xAxis, PhysicalAxis yAxis, bool drawShadow)
		{
			SequenceAdapter data = new SequenceAdapter (DataSource, DataMember, OrdinateData, AbscissaData);

			ITransform2D t = Transform2D.GetTransformer (xAxis, yAxis);
			
			int numberPoints = data.Count;
			
			if (data.Count == 0) {
				return;
			}

			ctx.Save ();
			ctx.SetLineWidth (lineWidth_);

			// clipping is now handled assigning a clip region in the
			// graphic object before this call
			if (numberPoints == 1) {
				Point physical = t.Transform (data[0]);
				
				if (drawShadow) {
					ctx.SetColor (shadowColor_);
					ctx.MoveTo (physical.X - 0.5 + ShadowOffset.X, physical.Y + ShadowOffset.Y);
					ctx.LineTo (physical.X + 0.5 + ShadowOffset.X, physical.Y + ShadowOffset.Y);
					ctx.Stroke ();
				}
				else {
					ctx.SetColor (lineColor_);
					ctx.MoveTo (physical.X-0.5, physical.Y);
					ctx.LineTo (physical.X+0.5, physical.Y);
					ctx.Stroke ();
				}
			}
			else {
				// prepare for clipping
				double leftCutoff = xAxis.PhysicalToWorld (xAxis.PhysicalMin, false);
				double rightCutoff = xAxis.PhysicalToWorld (xAxis.PhysicalMax, false);
				if (leftCutoff > rightCutoff) {
					Utils.Swap (ref leftCutoff, ref rightCutoff);
				}
				if (drawShadow) {
					// correct cut-offs
					double shadowCorrection =
						xAxis.PhysicalToWorld (ShadowOffset, false) - xAxis.PhysicalToWorld (new Point(0,0), false);
					leftCutoff -= shadowCorrection;
					rightCutoff -= shadowCorrection;
				}

				for (int i = 1; i < numberPoints; ++i) {
					// check to see if any values null. If so, then continue.
					double dx1 = data[i-1].X;
					double dx2 = data[i].X;
					double dy1 = data[i-1].Y;
					double dy2 = data[i].Y;
					if (Double.IsNaN(dx1) || Double.IsNaN(dy1) || Double.IsNaN(dx2) || Double.IsNaN(dy2)) {
						continue;
					}

					// do horizontal clipping here, to speed up
					if ((dx1 < leftCutoff && dx2 < leftCutoff) || (rightCutoff < dx1 && rightCutoff < dx2)) {
						continue;
					}

					// else draw line.	
					Point p1 = t.Transform (data[i-1]);
					Point p2 = t.Transform (data[i]);
					
					// when very far zoomed in, points can fall ontop of each other,
					// and g.DrawLine throws an overflow exception
					if (p1.Equals(p2)) {
						continue;
					}

					if (drawShadow) {
						ctx.SetColor (shadowColor_);
						ctx.MoveTo (p1.X + ShadowOffset.X, p1.Y + ShadowOffset.Y);
						ctx.LineTo (p2.X + ShadowOffset.X, p2.Y + ShadowOffset.Y);
						ctx.Stroke ();
					}
					else {
						ctx.SetColor (lineColor_);
						ctx.MoveTo (p1.X, p1.Y);
						ctx.LineTo (p2.X, p2.Y);
						ctx.Stroke ();
					}
				}
			}
			ctx.Restore ();
		}


		/// <summary>
		/// Draws the line plot using the Context and Physical Axes provided
		/// </summary>
		/// <param name="ctx">The Drawing Context with which to draw.</param>
		/// <param name="xAxis">The X-Axis to draw against.</param>
		/// <param name="yAxis">The Y-Axis to draw against.</param>
		public void Draw (Context ctx, PhysicalAxis xAxis, PhysicalAxis yAxis)
		{
			if (shadow_) {
				DrawLineOrShadow (ctx, xAxis, yAxis, true);
			}
			DrawLineOrShadow (ctx, xAxis, yAxis, false);
		}


		/// <summary>
		/// Returns an x-axis that is suitable for drawing this plot.
		/// </summary>
		/// <returns>A suitable x-axis.</returns>
		public Axis SuggestXAxis ()
		{
			SequenceAdapter data_ = 
				new SequenceAdapter (DataSource, DataMember, OrdinateData, AbscissaData);

			return data_.SuggestXAxis();
		}


		/// <summary>
		/// Returns a y-axis that is suitable for drawing this plot.
		/// </summary>
		/// <returns>A suitable y-axis.</returns>
		public Axis SuggestYAxis ()
		{
			SequenceAdapter data_ = 
				new SequenceAdapter (DataSource, DataMember, OrdinateData, AbscissaData);

			return data_.SuggestYAxis();
		}


		/// <summary>
		/// If true, draw a shadow under the line.
		/// </summary>
		public bool Shadow
		{
			get { return shadow_; }
			set { shadow_ = value; }
		}
		private bool shadow_ = false;
	

		/// <summary>
		/// The color used to draw lines in this plot.
		/// </summary>
		public Color LineColor
		{
			get { return lineColor_; }
			set { lineColor_ = value; }
		}
		private Color lineColor_ = Colors.Black;

		/// <summary>
		/// The line width used in this plot.
		/// </summary>
		public double LineWidth
		{
			get { return lineWidth_; }
			set { lineWidth_ = value; }
		}
		private double lineWidth_ = 1;

		/// <summary>
		/// Color of line shadow if drawn. Use Shadow method to turn shadow on and off.
		/// </summary>
		public Color ShadowColor
		{
			get { return shadowColor_; }
			set { shadowColor_ = value; }
		}
		private Color shadowColor_ = new Color (100, 100, 100);


		/// <summary>
		/// Offset of shadow line from primary line.
		/// </summary>
		public Point ShadowOffset
		{
			get { return shadowOffset_; }
			set { shadowOffset_ = value; }
		}
		private Point shadowOffset_ = new Point (1, 1);


		/// <summary>
		/// Draws a representation of this plot in the legend.
		/// </summary>
		/// <param name="ctx">The Drawing Context with which to draw.</param>
		/// <param name="startEnd">A rectangle specifying the bounds of the area in the legend set aside for drawing.</param>
		public virtual void DrawInLegend (Context ctx, Rectangle startEnd)
		{
			ctx.Save ();
			ctx.SetLineWidth (lineWidth_);
			ctx.SetColor (lineColor_);
			ctx.MoveTo (startEnd.Left, (startEnd.Top + startEnd.Bottom)/2);
			ctx.LineTo (startEnd.Right, (startEnd.Top + startEnd.Bottom)/2);
			ctx.Stroke ();
			ctx.Restore ();
		}

	}
}
