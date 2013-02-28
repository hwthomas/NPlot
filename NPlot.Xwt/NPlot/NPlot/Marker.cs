/*
 * NPlot - A charting library for .NET
 * 
 * Marker.cs
 * Copyright (C) 2003-2006 Matt Howlett and others.
 * All rights reserved.
 * 
 * Redistribution and use in source and binary forms, with or without modification,
 * are permitted provided that the following conditions are met:
 * 
 * 1. Redistributions of source code must retain the above copyright notice, this
 *    list of conditions and the following disclaimer.
 * 2. Redistributions in binary form must reproduce the above copyright notice,
 *    this list of conditions and the following disclaimer in the documentation
 *    and/or other materials provided with the distribution.
 * 3. Neither the name of NPlot nor the names of its contributors may
 *    be used to endorse or promote products derived from this software without
 *    specific prior written permission.
 * 
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
 * ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
 * WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED.
 * IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT,
 * INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING,
 * BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
 * DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF
 * LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE
 * OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED
 * OF THE POSSIBILITY OF SUCH DAMAGE.
 */

using System;
using System.Drawing;
using System.Drawing.Drawing2D;

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
			/// A square marker.
			/// </summary>
			Square,

			/// <summary>
			/// A triangle marker (upwards).
			/// </summary>
			Triangle,

			/// <summary>
			/// A triangle marker (upwards).
			/// </summary>
			TriangleUp,

			/// <summary>
			/// A triangle marker (upwards).
			/// </summary>
			TriangleDown,

			/// <summary>
			/// A diamond,
			/// </summary>
			Diamond,

			/// <summary>
			/// A filled circle
			/// </summary>
			FilledCircle,

			/// <summary>
			/// A filled square
			/// </summary>
			FilledSquare,

			/// <summary>
			/// A filled triangle
			/// </summary>
			FilledTriangle,

			/// <summary>
			/// A small flag (up)
			/// </summary>
			Flag,

			/// <summary>
			/// A small flag (up)
			/// </summary>
			FlagUp,

			/// <summary>
			/// A small flag (down)
			/// </summary>
			FlagDown,

			/// <summary>
			/// No marker
			/// </summary>
			None
		}

		private MarkerType markerType_;
		private int size_;
		private int h_;
		private System.Drawing.Pen pen_ = new Pen( Color.Black );
		private System.Drawing.Brush brush_ = new SolidBrush( Color.Black );
		private bool filled_ = false;
		private bool dropLine_ = false;


		/// <summary>
		/// The type of marker.
		/// </summary>
		public MarkerType Type
		{
			get
			{
				return markerType_;
			}
			set
			{
				markerType_ = value;
			}
		}


		/// <summary>
		/// Whether or not to draw a dropline.
		/// </summary>
		public bool DropLine
		{
			get
			{
				return dropLine_;
			}
			set
			{
				dropLine_ = value;
			}
		}


        /// <summary>
		/// The marker size.
		/// </summary>
		public int Size
		{
			get
			{
				return size_;
			}
			set
			{
				size_ = value;
				h_ = size_/2;
			}
		}


		/// <summary>
		/// The brush used to fill the marker.
		/// </summary>
		public Brush FillBrush
		{
			get
			{
				return brush_;
			}
			set
			{
				brush_ = value;
			}
		}


		/// <summary>
		/// Fill with color.
		/// </summary>
		public bool Filled
		{
			get
			{
				return filled_;
			}
			set
			{
				filled_ = value;
			}
		}


		/// <summary>
		/// Sets the pen color and fill brush to be solid with the specified color.
		/// </summary>
		public System.Drawing.Color Color
		{
			set
			{
				pen_.Color = value;
				brush_ = new SolidBrush( value );
			}
			get
			{
				return pen_.Color;
			}
		}


		/// <summary>
		/// The Pen used to draw the marker.
		/// </summary>
		public System.Drawing.Pen Pen
		{
			set
			{
				pen_ = value;
			}
			get
			{
				return pen_;
			}
		}


		/// <summary>
		/// Default constructor.
		/// </summary>
		public Marker()
		{
			markerType_ = MarkerType.Square;
			Size = 4;
			filled_ = false;
		}


		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="markertype">The marker type.</param>
		public Marker( MarkerType markertype )
		{
			markerType_ = markertype;
			Size = 4;
			filled_ = false;
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="markertype">The marker type.</param>
		/// <param name="size">The marker size.</param>
		public Marker( MarkerType markertype, int size )
		{
			markerType_ = markertype;
			Size = size;
			filled_ = false;
		}


		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="markertype">The marker type.</param>
		/// <param name="size">The marker size.</param>
		/// <param name="color">The marker color.</param>
		public Marker( MarkerType markertype, int size, Color color )
		{
			markerType_ = markertype;
			Size = size;
			Color = color;
			filled_ = false;
		}


		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="markertype">The marker type.</param>
		/// <param name="size">The marker size.</param>
		/// <param name="pen">The marker Pen.</param>
		public Marker( MarkerType markertype, int size, Pen pen )
		{
			markerType_ = markertype;
			Size = size;
			Pen = pen;
			filled_ = false;
		}


		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="markertype">The marker type.</param>
		/// <param name="size">The marker size.</param>
		/// <param name="pen">The marker Pen.</param>
		/// <param name="fill">The fill flag.</param>
		public Marker( MarkerType markertype, int size, Pen pen, bool fill )
		{
			markerType_ = markertype;
			Size = size;
			Pen = pen;
			filled_ = fill;
		}



		/// <summary>
		/// Draws the marker at the given position
		/// </summary>
		/// <param name="g">The graphics surface on which to draw.</param>
		/// <param name="x">The [physical] x position to draw the marker.</param>
		/// <param name="y">The [physical] y position to draw the marker.</param>
		public void Draw( Graphics g, int x, int y )
		{
			switch (markerType_)
			{
				case MarkerType.Cross1:
					g.DrawLine( pen_, x-h_, y+h_, x+h_, y-h_ );
					g.DrawLine( pen_, x+h_, y+h_, x-h_, y-h_ );
					break;

				case MarkerType.Cross2:
					g.DrawLine( pen_, x, y-h_, x, y+h_ );
					g.DrawLine( pen_, x-h_, y, x+h_, y );
					break;

				case MarkerType.Circle:
					g.DrawEllipse( pen_, x-h_, y-h_, size_, size_ );
					if ( this.filled_ ) 
					{
						g.FillEllipse( brush_, x-h_, y-h_, size_, size_ );
					}
					break;

				case MarkerType.Square:
					g.DrawRectangle( pen_, x-h_, y-h_, size_, size_ );
					if ( this.filled_ ) 
					{
						g.FillRectangle( brush_, x-h_, y-h_, size_, size_ );
					}
					break;

				case MarkerType.Triangle:
				case MarkerType.TriangleDown:
				{
					Point p1 = new Point( x-h_, y-h_ );
					Point p2 = new Point( x, y+h_ );
					Point p3 = new Point( x+h_, y-h_ );
					Point [] pts = new Point [3] { p1, p2, p3 };
					GraphicsPath gp = new GraphicsPath();
					gp.AddPolygon( pts );
					g.DrawPath( pen_, gp );
					if (this.filled_)
					{
						g.FillPath( brush_, gp );
					}
					break;
				}
				case MarkerType.TriangleUp:
				{
					Point p1 = new Point( x-h_, y+h_ );
					Point p2 = new Point( x, y-h_ );
					Point p3 = new Point( x+h_, y+h_ );
					Point [] pts = new Point [3] { p1, p2, p3 };
					GraphicsPath gp = new GraphicsPath();
					gp.AddPolygon( pts );
					g.DrawPath( pen_, gp );
					if (this.filled_) 
					{
						g.FillPath( brush_, gp );
					}
					break;
				}
				case MarkerType.FilledCircle:
					g.DrawEllipse( pen_, x-h_, y-h_, size_, size_ );
					g.FillEllipse( brush_, x-h_, y-h_, size_, size_ );
					break;

				case MarkerType.FilledSquare:
					g.DrawRectangle( pen_, x-h_, y-h_, size_, size_ );
					g.FillRectangle( brush_, x-h_, y-h_, size_, size_ );
					break;

				case MarkerType.FilledTriangle:
				{
					Point p1 = new Point( x-h_, y-h_ );
					Point p2 = new Point( x, y+h_ );
					Point p3 = new Point( x+h_, y-h_ );
					Point [] pts = new Point [3] { p1, p2, p3 };
					GraphicsPath gp = new GraphicsPath();
					gp.AddPolygon( pts );
					g.DrawPath( pen_, gp );
					g.FillPath( brush_, gp );
					break;
				}
				case MarkerType.Diamond:
				{
					Point p1 = new Point( x-h_, y );
					Point p2 = new Point( x, y-h_ );
					Point p3 = new Point( x+h_, y );
					Point p4 = new Point( x, y+h_ );
					Point [] pts = new Point [4] { p1, p2, p3, p4 };
					GraphicsPath gp = new GraphicsPath();
					gp.AddPolygon( pts );
					g.DrawPath( pen_, gp );
					if (this.filled_)
					{
						g.FillPath( brush_, gp );
					}
					break;
				}
				case MarkerType.Flag:
				case MarkerType.FlagUp:
				{
					Point p1 = new Point( x, y );
					Point p2 = new Point( x, y-size_ );
					Point p3 = new Point( x+size_, y-size_+size_/3 );
					Point p4 = new Point( x, y-size_+2*size_/3 );
					g.DrawLine( pen_, p1, p2 );
					Point [] pts = new Point [3] { p2, p3, p4 };
					GraphicsPath gp = new GraphicsPath();
					gp.AddPolygon( pts );
					g.DrawPath( pen_, gp );
					if (this.filled_)
					{
						g.FillPath( brush_, gp );
					}
					break;
				}
				case MarkerType.FlagDown:
				{
					Point p1 = new Point( x, y );
					Point p2 = new Point( x, y+size_ );
					Point p3 = new Point( x+size_, y+size_-size_/3 );
					Point p4 = new Point( x, y+size_-2*size_/3 );
					g.DrawLine( pen_, p1, p2 );
					Point [] pts = new Point [3] { p2, p3, p4 };
					GraphicsPath gp = new GraphicsPath();
					gp.AddPolygon( pts );
					g.DrawPath( pen_, gp );
					if (this.filled_)
					{
						g.FillPath( brush_, gp );
					}
					break;
				}
				case MarkerType.None:
					break;
			}
		}
	}
}
