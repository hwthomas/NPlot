//
// NPlot - A charting library for .NET
// 
// HorizontalLine.cs
// Copyright (C) 2003-2006 Matt Howlett and others
// Port to Xwt 2012-2013 : Hywel Thomas <hywel.w.thomas@gmail.com>
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
	/// Encapsulates functionality for drawing a horizontal line on a plot surface
	/// </summary>
	public class HorizontalLine : IPlot
	{

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="ordinateValue">ordinate (Y) value of line</param>
		public HorizontalLine (double ordinateValue)
		{
			value_ = ordinateValue;
			color_ = Colors.Black;
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="ordinateValue">ordinate (Y) value of line.</param>
		/// <param name="color">line color.</param>
		public HorizontalLine (double ordinateValue, Color color )
		{
			value_ = ordinateValue;
			color_ = color;
		}

		/// <summary>
		/// Draws a representation of the horizontal line in the legend.
		/// </summary>
		/// <param name="ctx">The Drawing Context with which to draw</param>
		/// <param name="startEnd">A rectangle specifying the bounds of the area in the legend set aside for drawing</param>
		public void DrawInLegend (Context ctx, Rectangle startEnd)
		{
			ctx.Save ();
			ctx.SetColor (color_);
			ctx.SetLineWidth (1);
			ctx.MoveTo (startEnd.Left, (startEnd.Top + startEnd.Bottom)/2);
			ctx.LineTo (startEnd.Right, (startEnd.Top + startEnd.Bottom)/2);
			ctx.Stroke ();
			ctx.Restore ();
		}

		/// <summary>
		/// The Color to draw the line with
		/// </summary>
		public Color Color
		{
			get { return color_; }
			set { color_ = value; }
		}
		private Color color_ = Colors.Black;

		/// <summary>
		/// A label to associate with the plot - used in the legend.
		/// </summary>
		public string Label
		{
			get { return label_; }
			set { label_ = value; }
		}
		private string label_ = "";

		/// <summary>
		/// Whether or not to include an entry for this plot in the legend if it exists.
		/// </summary>
		public bool ShowInLegend
		{
			get { return showInLegend_; }
			set { showInLegend_ = value; }
		}
		private bool showInLegend_ = false;

		/// <summary>
		/// Returns null indicating that x extremities of the line are variable.
		/// </summary>
		/// <returns>null</returns>
		public Axis SuggestXAxis()
		{
			return null;
		}

		/// <summary>
		/// Returns a y-axis that is suitable for drawing this plot.
		/// </summary>
		/// <returns>A suitable y-axis.</returns>
		public Axis SuggestYAxis()
		{
			return new LinearAxis( value_, value_ );
		}

		/// <summary>
		/// Writes text data describing the horizontal line object to the supplied string builder. It is 
		/// possible to specify that the data will be written only if the line is in the specified 
		/// region.
		/// </summary>
		/// <param name="sb">the StringBuilder object to write to.</param>
		/// <param name="region">a region used if onlyInRegion is true.</param>
		/// <param name="onlyInRegion">If true, data will be written only if the line is in the specified region.</param>
		public void WriteData (System.Text.StringBuilder sb, Rectangle region, bool onlyInRegion)
		{
			// return if line is not in plot region and 
			if (value_ > region.Y+region.Height || value_ < region.Y) {
				if (onlyInRegion) {
					return;
				}
			}

			sb.Append ( "Label: " );
			sb.Append ( Label );
			sb.Append ( "\r\n" );
			sb.Append ( value_.ToString() );
			sb.Append ( "\r\n" );
		}

		/// <summary>
		/// Draws the horizontal line plot using the Context and the x and y axes specified
		/// </summary>
		/// <param name="ctx">The Context with which to draw.</param>
		/// <param name="xAxis">The X-Axis to draw against.</param>
		/// <param name="yAxis">The Y-Axis to draw against.</param>
		public void Draw (Context ctx, PhysicalAxis xAxis, PhysicalAxis yAxis)
		{
			double xMin = xAxis.PhysicalMin.X;
			double xMax = xAxis.PhysicalMax.X;
			
			xMin += pixelIndent_;
			xMax -= pixelIndent_;

			double length = Math.Abs (xMax - xMin);
			double lengthDiff = length - length*scale_;
			double indentAmount = lengthDiff/2;

			xMin += indentAmount;
			xMax -= indentAmount;

			double yPos = yAxis.WorldToPhysical (value_, false).Y;

			ctx.Save ();
			ctx.SetLineWidth (1);
			ctx.SetColor (color_);
			ctx.MoveTo (xMin, yPos);
			ctx.LineTo (xMax, yPos);
			ctx.Stroke ();
			ctx.Restore ();

			// todo:  clip and proper logic for flipped axis min max.
		}

		/// <summary>
		/// ordinate (Y) value to draw horizontal line at.
		/// </summary>
		public double OrdinateValue
		{
			get { return value_; }
			set { value_ = value; }
		}
		private double value_;

		/// <summary>
		/// Each end of the line is indented by this many pixels. 
		/// </summary>
		public double PixelIndent
		{
			get { return pixelIndent_; }
			set { pixelIndent_ = value; }
		}
		private double pixelIndent_ = 0;

		/// <summary>
		/// The line length is multiplied by this amount. Default
		/// corresponds to a value of 1.0.
		/// </summary>
		public double LengthScale
		{
			get { return scale_; }
			set { scale_ = value; }
		}
		private double scale_ = 1.0;
	}
}
