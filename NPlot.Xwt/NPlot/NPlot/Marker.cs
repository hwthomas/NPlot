//
// XwPlot - A cross-platform charting library using the Xwt toolkit
// 
// Marker.cs
// Copyright (C) 2003-2006 Matt Howlett and others.
// Port to Xwt 2013 : Hywel Thomas <hywel.w.thomas@gmail.com>
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
	/// Encapsulates functionality relating to markers used by the PointPlot class.
	/// </summary>
	public class Marker
	{
		/// <summary>
		/// Enumeration of all different types of marker.
		/// </summary>
		public enum MarkerType
		{
			/// <summary>
			/// A simple cross marker (x).
			/// </summary>
			Cross1,

			/// <summary>
			/// Another simple cross marker (+).
			/// </summary>
			Cross2,

			/// <summary>
			/// A circle marker.
			/// </summary>
			Circle,

			/// <summary>
			/// A filled circle
			/// </summary>
			FilledCircle,

			/// <summary>
			/// A square marker.
			/// </summary>
			Square,

			/// <summary>
			/// A filled square
			/// </summary>
			FilledSquare,

			/// <summary>
			/// A triangle marker (upwards).
			/// </summary>
			Triangle,

			/// <summary>
			/// A filled triangle
			/// </summary>
			FilledTriangle,

			/// <summary>
			/// A triangle marker (downwards).
			/// </summary>
			TriangleDown,

			/// <summary>
			/// A diamond,
			/// </summary>
			Diamond,

			/// <summary>
			/// A small flag (up)
			/// </summary>
			Flag,

			/// <summary>
			/// A small filled flag
			/// </summary>
			FilledFlag,

			/// <summary>
			/// A small flag (down)
			/// </summary>
			FlagDown,

			/// <summary>
			/// No marker
			/// </summary>
			None
		}

		private MarkerType markerType;
		private double size;
		private double h;
		private bool filled = false;
		private bool dropLine = false;


		/// <summary>
		/// The type of marker.
		/// </summary>
		public MarkerType Type
		{
			get { return markerType; }
			set { markerType = value; }
		}


		/// <summary>
		/// Whether or not to draw a dropline.
		/// </summary>
		public bool DropLine
		{
			get { return dropLine; }
			set { dropLine = value; }
		}


		/// <summary>
		/// The marker size.
		/// </summary>
		public double Size
		{
			get { return size; }
			set { 
				size = value;
				h = size/2;
			}
		}


		/// <summary>
		/// Sets the line width
		/// </summary>
		public double LineWidth
		{
			set { lineWidth = value; }
			get { return lineWidth; }
		}
		private double lineWidth = 1;


		/// <summary>
		/// Sets the line color
		/// </summary>
		public Color LineColor
		{
			set { lineColor = value; }
			get { return lineColor; }
		}
		private Color lineColor = Colors.Black;


		/// <summary>
		/// Sets the fill color
		/// </summary>
		public Color FillColor
		{
			set { fillColor = value; }
			get { return fillColor; }
		}
		private Color fillColor = Colors.Black;


		/// <summary>
		/// Fill with color.
		/// </summary>
		public bool Filled
		{
			get { return filled; }
			set { filled = value; }
		}


		/// <summary>
		/// Default constructor.
		/// </summary>
		public Marker ()
		{
			markerType = MarkerType.Square;
			Size = 4;
			filled = false;
		}


		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="markertype">The marker type.</param>
		public Marker (MarkerType markertype)
		{
			markerType = markertype;
			Size = 4;
			filled = false;
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="markertype">The marker type.</param>
		/// <param name="size">The marker size.</param>
		public Marker (MarkerType markertype, double size )
		{
			markerType = markertype;
			Size = size;
			filled = false;
		}


		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="markertype">The marker type.</param>
		/// <param name="size">The marker size.</param>
		/// <param name="color">The marker color.</param>
		public Marker (MarkerType markertype, double size, Color color)
		{
			markerType = markertype;
			Size = size;
			LineColor = color;
			filled = false;
		}


		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="markertype">The marker type.</param>
		/// <param name="size">The marker size.</param>
		/// <param name="color">The marker color</param>
		/// <param name="fill">The fill flag.</param>
		public Marker (MarkerType markertype, double size, Color color, bool fill)
		{
			markerType = markertype;
			Size = size;
			LineColor = color;
			filled = fill;
		}



		/// <summary>
		/// Draws the marker at the given position
		/// </summary>
		/// <param name="ctx">The Drawing Context with which to draw.</param>
		/// <param name="x">The [physical] x position to draw the marker.</param>
		/// <param name="y">The [physical] y position to draw the marker.</param>
		public void Draw (Context ctx, double x, double y )
		{
			ctx.Save ();
			ctx.SetLineWidth (lineWidth);
			ctx.SetColor (lineColor);

			switch (markerType) {
			case MarkerType.Cross1:
				ctx.MoveTo (x-h, y+h);
				ctx.LineTo (x+h, y-h);
				ctx.MoveTo (x+h, y+h);
				ctx.LineTo (x-h, y-h);
				ctx.Stroke ();
				break;

			case MarkerType.Cross2:
				ctx.MoveTo (x, y-h);
				ctx.LineTo (x, y+h);
				ctx.MoveTo (x-h, y);
				ctx.LineTo (x+h, y);
				ctx.Stroke ();
				break;

			case MarkerType.Circle:
				ctx.MoveTo (x+h,y);
				ctx.Arc (x, y, h, 0, 360);
				ctx.ClosePath ();
				if (filled ) {
					ctx.SetColor (fillColor);
					ctx.FillPreserve ();
				}
				ctx.SetColor (lineColor);
				ctx.Stroke ();
				break;

			case MarkerType.Square:
				ctx.Rectangle (x-h, y-h, size, size);
				ctx.ClosePath ();
				if (filled) {
					ctx.SetColor (fillColor);
					ctx.FillPreserve ();
				}
				ctx.SetColor (lineColor);
				ctx.Stroke ();
				break;

			case MarkerType.Triangle:
				ctx.MoveTo (x-h, y+h);
				ctx.LineTo (x, y-h);
				ctx.LineTo (x+h, y+h);
				ctx.ClosePath ();
				if (filled) {
					ctx.SetColor (fillColor);
					ctx.FillPreserve ();
				}
				ctx.SetColor (lineColor);
				ctx.Stroke ();
				break;

			case MarkerType.TriangleDown:
				ctx.MoveTo (x-h, y-h);
				ctx.LineTo (x, y+h);
				ctx.LineTo (x+h, y-h);
				ctx.ClosePath ();
				if (filled) {
					ctx.SetColor (fillColor);
					ctx.FillPreserve ();
				}
				ctx.SetColor (lineColor);
				ctx.Stroke ();
				break;
			
			case MarkerType.FilledCircle:
				ctx.MoveTo (x+h,y);
				ctx.Arc (x, y, h, 0, 360);
				ctx.ClosePath ();
				ctx.SetColor (fillColor);
				ctx.FillPreserve ();
				ctx.SetColor (lineColor);
				ctx.Stroke ();
				break;

			case MarkerType.FilledSquare:
				ctx.Rectangle (x-h, y-h, size, size);
				ctx.ClosePath ();
				ctx.SetColor (fillColor);
				ctx.FillPreserve ();
				ctx.SetColor (lineColor);
				ctx.Stroke ();
				break;

			case MarkerType.FilledTriangle:
				ctx.MoveTo (x-h, y+h);
				ctx.LineTo (x, y-h);
				ctx.LineTo (x+h, y+h);
				ctx.ClosePath ();
				ctx.SetColor (fillColor);
				ctx.FillPreserve ();
				ctx.SetColor (lineColor);
				ctx.Stroke ();
				break;

			case MarkerType.Diamond:
				ctx.MoveTo (x-h, y);
				ctx.LineTo (x, y-h);
				ctx.LineTo (x+h, y);
				ctx.LineTo (x, y+h);
				ctx.ClosePath ();
				if (filled) {
					ctx.SetColor (fillColor);
					ctx.FillPreserve ();
				}
				ctx.SetColor (lineColor);
				ctx.Stroke ();
				break;

			case MarkerType.Flag:
				ctx.MoveTo (x, y-size);
				ctx.LineTo (x+size, y-size+size/3);
				ctx.LineTo (x, y-size+2*size/3);
				ctx.ClosePath ();
				ctx.MoveTo (x, y);
				ctx.LineTo (x, y-size);
				if (filled) {
					ctx.SetColor (fillColor);
					ctx.FillPreserve ();
				}
				ctx.SetColor (lineColor);
				ctx.Stroke ();
				break;

			case MarkerType.FlagDown:
				ctx.MoveTo (x, y+size);
				ctx.LineTo (x+size, y+size-size/3);
				ctx.LineTo (x, y+size-2*size/3);
				ctx.ClosePath ();
				ctx.MoveTo (x, y);
				ctx.LineTo (x, y+size);
				if (filled) {
					ctx.SetColor (fillColor);
					ctx.FillPreserve ();
				}
				ctx.SetColor (lineColor);
				ctx.Stroke ();
				break;

			case MarkerType.FilledFlag:
				ctx.MoveTo (x, y-size);
				ctx.LineTo (x+size, y-size+size/3);
				ctx.LineTo (x, y-size+2*size/3);
				ctx.ClosePath ();
				ctx.MoveTo (x, y);
				ctx.LineTo (x, y-size);
				ctx.SetColor (fillColor);
				ctx.FillPreserve ();
				ctx.SetColor (lineColor);
				ctx.Stroke ();
				break;

			case MarkerType.None:
					break;
			}
			ctx.Restore ();
		}

	}

}
