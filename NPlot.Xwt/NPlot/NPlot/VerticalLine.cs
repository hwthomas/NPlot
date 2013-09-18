//
// NPlot - A charting library for .NET
// 
// VerticalLine.cs
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
	/// Encapsulates functionality for drawing a vertical line on a plot surface.
	/// </summary>
	public class VerticalLine : IPlot
	{
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="abscissaValue">abscissa (X) value of line.</param>
		/// <param name="color">draw the line using this color.</param>
		public VerticalLine (double abscissaValue, Color color)
		{
			AbscissaValue = abscissaValue;
			Color = color;
			PixelIndent = 0;
			LengthScale = 1;
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="abscissaValue">abscissa (X) value of line.</param>
		public VerticalLine (double abscissaValue) : this (abscissaValue, Colors.Black) { }

		/// <summary>
		/// abscissa (X) value to draw horizontal line at.
		/// </summary>
		public double AbscissaValue { get; set; }

		/// <summary>
		/// Color used to draw the vertical line.
		/// </summary>
		public Color Color { get; set; }

		/// <summary>
		/// A label to associate with the plot - used in the legend.
		/// </summary>
		public string Label { get; set; }

		/// <summary>
		/// Whether or not to include an entry for this plot in the legend if it exists.
		/// </summary>
		public bool ShowInLegend { get; set; }

		/// <summary>
		/// Draws a representation of the line in the legend
		/// </summary>
		/// <param name="ctx">The Drawing Context with which to draw</param>
		/// <param name="startEnd">A rectangle specifying the bounds of the area in the legend set aside for drawing</param>
		public void DrawInLegend (Context ctx, Rectangle startEnd)
		{
			ctx.Save ();
			ctx.MoveTo (startEnd.Left, (startEnd.Top + startEnd.Bottom)/2);
			ctx.LineTo (startEnd.Right, (startEnd.Top + startEnd.Bottom)/2);
			ctx.SetColor (Color);
			ctx.SetLineWidth (1);
			ctx.Stroke ();
			ctx.Restore ();
		}

		/// <summary>
		/// Returns an x-axis that is suitable for drawing this plot.
		/// </summary>
		/// <returns>A suitable x-axis.</returns>
		public Axis SuggestXAxis ()
		{
			return new LinearAxis (AbscissaValue, AbscissaValue);
		}

		/// <summary>
		/// Returns null indicating that y extremities of the line are variable.
		/// </summary>
		/// <returns>null</returns>
		public Axis SuggestYAxis()
		{
			return null;
		}

		/// <summary>
		/// Writes text data describing the vertical line object to the supplied string builder. It is 
		/// possible to specify that the data will be written only if the line is in the specified 
		/// region.
		/// </summary>
		/// <param name="sb">the StringBuilder object to write to.</param>
		/// <param name="region">a region used if onlyInRegion is true.</param>
		/// <param name="onlyInRegion">If true, data will be written only if the line is in the specified region.</param>
		public void WriteData (System.Text.StringBuilder sb, Rectangle region, bool onlyInRegion)
		{
			// return if line is not in plot region and 
			if (AbscissaValue > region.X+region.Width || AbscissaValue < region.X) {
				if (onlyInRegion) {
					return;
				}
			}

			sb.Append ("Label: ");
			sb.Append (Label);
			sb.Append ("\r\n");
			sb.Append (AbscissaValue.ToString());
			sb.Append ("\r\n");
		}


		/// <summary>
		/// Draws the vertical line using the Context and the x and y axes specified
		/// </summary>
		/// <param name="ctx">The Context with which to draw.</param>
		/// <param name="xAxis">The X-Axis to draw against.</param>
		/// <param name="yAxis">The Y-Axis to draw against.</param>
		public void Draw (Context ctx, PhysicalAxis xAxis, PhysicalAxis yAxis)
		{
			double yMin = yAxis.PhysicalMin.Y;
			double yMax = yAxis.PhysicalMax.Y;
			
			yMin -= PixelIndent;
			yMax += PixelIndent;

			double length = Math.Abs (yMax - yMin);
			double lengthDiff = length - length*LengthScale;
			double indentAmount = lengthDiff/2;

			yMin -= indentAmount;
			yMax += indentAmount;

			double xPos = xAxis.WorldToPhysical (AbscissaValue, false).X;
		
			ctx.Save ();
			ctx.SetLineWidth (1);
			ctx.SetColor (Color);
			ctx.MoveTo (xPos, yMin);
			ctx.LineTo (xPos, yMax);
			ctx.Stroke ();
			ctx.Restore ();
			// todo:  clip and proper logic for flipped axis min max.
		}

		/// <summary>
		/// Each end of the line is indented by this many pixels. 
		/// </summary>
		public double PixelIndent { get; set; }

		/// <summary>
		/// The line length is multiplied by this amount. Default
		/// corresponds to a value of 1.0.
		/// </summary>
		public double LengthScale { get; set; }

	}
}
