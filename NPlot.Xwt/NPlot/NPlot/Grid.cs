//
// XwPlot - A cross-platform charting library using the Xwt toolkit
// 
// Grid.cs
// 
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
using System.Collections;

using Xwt;
using Xwt.Drawing;

namespace NPlot
{

	/// <summary>
	/// Encapsulates a Grid IDrawable object. Instances of this
	/// can be added to a PlotSurface2D to produce a grid.
	/// </summary>
	public class Grid : IDrawable
	{
		private GridType horizontalGridType;
		private GridType verticalGridType;
		private Color majorGridColor;
		private Color minorGridColor;
		private Color gridColor;
		private double [] majorGridDash = {3.0, 1.0};
		private double [] minorGridDash = {1.0, 2.0};
		private double [] gridDash = {3.0, 1.0};

		public enum GridType
		{
			/// <summary>
			/// No grid.
			/// </summary>
			None = 0,
			/// <summary>
			/// Coarse grid. Lines at large tick positions only.
			/// </summary>
			Coarse = 1,
			/// <summary>
			/// Fine grid. Lines at both large and small tick positions.
			/// </summary>
			Fine = 2
		}


		/// <summary>
		/// Default constructor
		/// </summary>
		public Grid ()
		{
			minorGridColor = Colors.LightGray;
			majorGridColor = Colors.LightGray;
			horizontalGridType = GridType.Coarse;
			verticalGridType = GridType.Coarse;
		}


		/// <summary>
		/// Specifies the horizontal grid type (none, coarse or fine).
		/// </summary>
		public GridType HorizontalGridType
		{
			get { return horizontalGridType; }
			set { horizontalGridType = value; }
		}


		/// <summary>
		/// Specifies the vertical grid type (none, coarse, or fine).
		/// </summary>
		public GridType VerticalGridType
		{
			get { return verticalGridType; }
			set { verticalGridType = value; }
		}


		/// <summary>
		/// The Color used to draw major (coarse) grid lines.
		/// </summary>
		public Color MajorGridColor
		{
			get { return majorGridColor; }
			set { majorGridColor = value; }
		}


		/// <summary>
		/// The Color used to draw minor (fine) grid lines.
		/// </summary>
		public Color MinorGridColor
		{
			get { return minorGridColor; }
			set { minorGridColor = value; }
		}

		/// <summary>
		/// Does all the work in drawing grid lines.
		/// </summary>
		/// <param name="ctx">The graphics context with which to draw</param>
		/// <param name="axis">TODO</param>
		/// <param name="orthogonalAxis">TODO</param>
		/// <param name="a">the list of world values to draw grid lines at.</param>
		/// <param name="horizontal">true if want horizontal lines, false otherwise.</param>
		/// <param name="color">the color to draw the grid lines.</param>
		private void DrawGridLines (Context ctx,
			PhysicalAxis axis, PhysicalAxis orthogonalAxis,
			System.Collections.ArrayList a, bool horizontal)
		{
			for (int i=0; i<a.Count; ++i) {
				Point p1 = axis.WorldToPhysical ((double)a[i], true);
				Point p2 = p1;
				Point p3 = orthogonalAxis.PhysicalMax;
				Point p4 = orthogonalAxis.PhysicalMin;
				if (horizontal) {
					p1.Y = p4.Y;
					p2.Y = p3.Y;
				}
				else {
					p1.X = p4.X;
					p2.X = p3.X;
				}
				ctx.MoveTo (p1);
				ctx.LineTo (p2);
				// note: casting all drawing was necessary for sane display. why?
				//g.DrawLine( p, (int)p1.X, (int)p1.Y, (int)p2.X, (int)p2.Y );
			}
			ctx.SetLineWidth (1);
			ctx.SetColor (gridColor);
			ctx.SetLineDash (0, gridDash);
			ctx.Stroke ();
		}

		/// <summary>
		/// Draws the grid
		/// </summary>
		/// <param name="ctx">The Drawing Context with which to draw</param>
		/// <param name="xAxis">The physical x axis to draw horizontal lines parallel to.</param>
		/// <param name="yAxis">The physical y axis to draw vertical lines parallel to.</param>
		public void Draw (Context ctx, PhysicalAxis xAxis, PhysicalAxis yAxis)
		{
			ArrayList xLargePositions = null;
			ArrayList yLargePositions = null;
			ArrayList xSmallPositions = null;
			ArrayList ySmallPositions = null;

			ctx.Save ();

			// Draw MajorGrid
			gridColor = MajorGridColor;
			gridDash = majorGridDash;
			if (horizontalGridType != GridType.None) {
				xAxis.Axis.WorldTickPositions_FirstPass (xAxis.PhysicalMin, xAxis.PhysicalMax, out xLargePositions, out xSmallPositions);
				DrawGridLines (ctx, xAxis, yAxis, xLargePositions, true);	
			}

			if (verticalGridType != GridType.None) {
				yAxis.Axis.WorldTickPositions_FirstPass (yAxis.PhysicalMin, yAxis.PhysicalMax, out yLargePositions, out ySmallPositions);
				DrawGridLines (ctx, yAxis, xAxis, yLargePositions, false);
			}

			// Draw MinorGrid
			gridColor = MinorGridColor;
			gridDash = minorGridDash;
			if (horizontalGridType == GridType.Fine) {
				xAxis.Axis.WorldTickPositions_SecondPass (xAxis.PhysicalMin, xAxis.PhysicalMax, xLargePositions, ref xSmallPositions);
				DrawGridLines(ctx, xAxis, yAxis, xSmallPositions, true);
			}

			if (verticalGridType == GridType.Fine) {
				yAxis.Axis.WorldTickPositions_SecondPass (yAxis.PhysicalMin, yAxis.PhysicalMax, yLargePositions, ref ySmallPositions);
				DrawGridLines (ctx, yAxis, xAxis, ySmallPositions, false);
			}

			ctx.Restore ();
		}
	}
}