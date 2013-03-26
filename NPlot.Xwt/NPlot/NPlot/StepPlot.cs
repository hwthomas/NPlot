//
// NPlot - A charting library for .NET
// 
// StepPlot.cs
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

namespace NPlot.Xwt
{

	/// <summary>
	/// Encapsulates functionality for plotting data as a stepped line.
	/// </summary>
	public class StepPlot : BaseSequencePlot, IPlot, ISequencePlot
	{

		/// <summary>
		/// Constructor.
		/// </summary>
		public StepPlot()
		{
			Center = false;
		}


		/// <summary>
		/// Draws the step plot using a Drawing Context against the provided x and y axes.
		/// </summary>
		/// <param name="ctx">The Drawing Context with which to draw.</param>
		/// <param name="xAxis">The X-Axis to draw against.</param>
		/// <param name="yAxis">The Y-Axis to draw against.</param>
		public virtual void Draw (Context ctx, PhysicalAxis xAxis, PhysicalAxis yAxis )
		{
			SequenceAdapter data = 
				new SequenceAdapter (DataSource, DataMember, OrdinateData, AbscissaData);

			double leftCutoff = xAxis.PhysicalToWorld(xAxis.PhysicalMin, false);
			double rightCutoff = xAxis.PhysicalToWorld(xAxis.PhysicalMax, false);

			for (int i=0; i<data.Count; ++i) {
				Point p1 = data[i];
				if (Double.IsNaN(p1.X) || Double.IsNaN(p1.Y)) {
					continue;
				}

				Point p2;
				Point p3;
				if (i+1 != data.Count) {
					p2 = data[i+1];
					if (Double.IsNaN(p2.X) || Double.IsNaN(p2.Y)) {
						continue;
					}
					p2.Y = p1.Y;
					p3 = data[i+1];
				}
				else {
					// Check that we are not dealing with a DataSource of 1 point.
					// This check is done here so it is only checked on the end
					// condition and not for every point in the DataSource.
					if (data.Count > 1) {
						p2 = data[i - 1];
					}
					else {
						// TODO: Once log4net is set up post a message to the user that a step-plot of 1 really does not make any sense.
						p2 = p1;
					}

					double offset = p1.X - p2.X;
					p2.X = p1.X + offset;
					p2.Y = p1.Y;
					p3 = p2; 
				}

				if (center_) {
					double offset = ( p2.X - p1.X ) / 2.0;
					p1.X -= offset;
					p2.X -= offset;
					p3.X -= offset;
				}

				Point xPos1 = xAxis.WorldToPhysical (p1.X, false);
				Point yPos1 = yAxis.WorldToPhysical (p1.Y, false);
				Point xPos2 = xAxis.WorldToPhysical (p2.X, false);
				Point yPos2 = yAxis.WorldToPhysical (p2.Y, false);
				Point xPos3 = xAxis.WorldToPhysical (p3.X, false);
				Point yPos3 = yAxis.WorldToPhysical (p3.Y, false);

				// do horizontal clipping here, to speed up
				if ((p1.X<leftCutoff && p2.X<leftCutoff && p3.X<leftCutoff) || (p1.X>rightCutoff && p2.X>rightCutoff && p3.X>rightCutoff)) {
					continue;
				}

				ctx.Save ();
				ctx.SetColor (Color);
				ctx.SetLineWidth (1);
				if (!this.hideHorizontalSegments_) {
					if (scale_ != 1) {
						double middle = (xPos2.X + xPos1.X) / 2;
						double width = xPos2.X - xPos1.X;
						width *= this.scale_;
						ctx.MoveTo (middle-width/2, yPos1.Y);
						ctx.LineTo (middle+width/2, yPos2.Y);
					}
					else {
						ctx.MoveTo (xPos1.X, yPos1.Y);
						ctx.LineTo (xPos2.X, yPos2.Y);
					}
					ctx.Stroke ();
				}
				
				if (!this.hideVerticalSegments_) {
					ctx.MoveTo (xPos2.X, yPos2.Y);
					ctx.LineTo (xPos3.X, yPos3.Y);
					ctx.Stroke ();
				}
				ctx.Restore ();
			}
		}  


		/// <summary>
		/// Returns an X-axis suitable for use by this plot. The axis will be one that is just long
		/// enough to show all data.
		/// </summary>
		/// <returns>X-axis suitable for use by this plot.</returns>
		public Axis SuggestXAxis()
		{
			SequenceAdapter data = 
				new SequenceAdapter (DataSource, DataMember, OrdinateData, AbscissaData );

			if (data.Count < 2) {
				return data.SuggestXAxis();
			}

			// else

			Axis a = data.SuggestXAxis();

			Point p1 = data[0];
			Point p2 = data[1];
			Point p3 = data[data.Count-2];
			Point p4 = data[data.Count-1];

			double offset1;
			double offset2;

			if (!center_) {
				offset1 = 0;
				offset2 = p4.X - p3.X;
			}
			else {
				offset1 = (p2.X - p1.X)/2;
				offset2 = (p4.X - p3.X)/2;
			}

			a.WorldMin -= offset1;
			a.WorldMax += offset2;

			return a;
		}


		/// <summary>
		/// Returns an Y-axis suitable for use by this plot. The axis will be one that is just long
		/// enough to show all data.
		/// </summary>
		/// <returns>Y-axis suitable for use by this plot.</returns>
		public Axis SuggestYAxis()
		{
			SequenceAdapter data = new SequenceAdapter (DataSource, DataMember, OrdinateData, AbscissaData);
			return data.SuggestYAxis();
		}


		/// <summary>
		/// Gets or sets whether or not steps should be centered. If true, steps will be centered on the
		/// X abscissa values. If false, the step corresponding to a given x-value will be drawn between 
		/// this x-value and the next x-value at the current y-height.
		/// </summary>
		public bool Center
		{
			get { return center_; }
			set { center_ = value; }
		}
		private bool center_;


		/// <summary>
		/// Draws a representation of this plot in the legend.
		/// </summary>
		/// <param name="g">The graphics surface on which to draw.</param>
		/// <param name="startEnd">A rectangle specifying the bounds of the area in the legend set aside for drawing.</param>
		public virtual void DrawInLegend (Context ctx, Rectangle startEnd)
		{
			ctx.Save ();
			ctx.SetLineWidth (1);
			ctx.SetColor (color_);
			ctx.MoveTo (startEnd.Left, (startEnd.Top + startEnd.Bottom)/2);
			ctx.LineTo (startEnd.Right, (startEnd.Top + startEnd.Bottom)/2);
			ctx.Stroke ();
			ctx.Restore ();
		}


		/// <summary>
		/// The color of the pen used to draw lines in this plot.
		/// </summary>
		public Color Color
		{
			get { return color_; }
			set { color_ = value;}
		}
		private Color color_;


		/// <summary>
		/// If true, then vertical lines are hidden.
		/// </summary>
		public bool HideVerticalSegments
		{
			get { return hideVerticalSegments_; }
			set { hideVerticalSegments_ = value; }
		}
		bool hideVerticalSegments_ = false;


		/// <summary>
		/// If true, then vertical lines are hidden.
		/// </summary>
		public bool HideHorizontalSegments
		{
			get { return hideHorizontalSegments_; }
			set { hideHorizontalSegments_ = value; }
		}
		bool hideHorizontalSegments_ = false;


		/// <summary>
		/// The horizontal line length is multiplied by this amount. Default
		/// corresponds to a value of 1.0.
		/// </summary>
		public double WidthScale
		{
			get { return scale_; }
			set { scale_ = value; }
		}
		private double scale_ = 1;
	}
}
