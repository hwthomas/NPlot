/*
 * NPlot - A charting library for .NET
 * 
 * ArrowItem.cs
 * Copyright (C) 2003-2006 Matt Howlett and others.
 * All rights reserved.
 * 
 * Redistribution and use in source and binary forms, with or without modification,
 * are permitted provided that the following conditions are met:
 * 
 * 1. Redistributions of source code must retain the above copyright notice, this
 *	  list of conditions and the following disclaimer.
 * 2. Redistributions in binary form must reproduce the above copyright notice,
 *	  this list of conditions and the following disclaimer in the documentation
 *	  and/or other materials provided with the distribution.
 * 3. Neither the name of NPlot nor the names of its contributors may
 *	  be used to endorse or promote products derived from this software without
 *	  specific prior written permission.
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

namespace NPlot
{

	/// <summary>
	/// An Arrow IDrawable, with a text label that is automatically
	/// nicely positioned at the non-pointy end of the arrow. Future
	/// feature idea: have constructor that takes a dataset, and have
	/// the arrow know how to automatically set it's angle to avoid 
	/// the data.
	/// </summary>
	public class ArrowItem : IDrawable
	{

		private void Init()
		{
			FontFamily fontFamily = new FontFamily("Arial");
			font_ = new Font(fontFamily, 10, FontStyle.Regular, GraphicsUnit.Pixel);
		}


		/// <summary>
		/// Default constructor : 
		/// text = ""
		/// angle = 45 degrees anticlockwise from horizontal.
		/// </summary>
		/// <param name="position">The position the arrow points to.</param>
		public ArrowItem( PointD position )
		{
			to_ = position;
			Init();
		}


		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="position">The position the arrow points to.</param>
		/// <param name="angle">angle of arrow with respect to x axis.</param>
		public ArrowItem( PointD position, double angle )
		{
			to_ = position;
			angle_ = -angle;
			Init();
		}


		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="position">The position the arrow points to.</param>
		/// <param name="angle">angle of arrow with respect to x axis.</param>
		/// <param name="text">The text associated with the arrow.</param>
		public ArrowItem( PointD position, double angle, string text )
		{
			to_ = position;
			angle_ = -angle;
			text_ = text;
			Init();
		}


		/// <summary>
		/// Text associated with the arrow.
		/// </summary>
		public string Text 
		{
			get
			{
				return text_;
			}
			set
			{
				text_ = value;
			}
		}
		private string text_ = "";


		/// <summary>
		/// Angle of arrow anti-clockwise to right horizontal in degrees.
		/// </summary>
		/// <remarks>The code relating to this property in the Draw method is
		/// a bit weird. Internally, all rotations are clockwise [this is by 
		/// accient, I wasn't concentrating when I was doing it and was half
		/// done before I realised]. The simplest way to make angle represent
		/// anti-clockwise rotation (as it is normal to do) is to make the 
		/// get and set methods negate the provided value.</remarks>
		public double Angle
		{
			get
			{
				return -angle_;
			}
			set
			{
				angle_ = -value;
			}
		}
		private double angle_ = -45.0;


		/// <summary>
		/// Physical length of the arrow. 
		/// </summary>
		public float PhysicalLength
		{
			get
			{
				return physicalLength_;
			}
			set
			{
				physicalLength_ = value;
			}
		}
		private float physicalLength_ = 40.0f;


		/// <summary>
		/// The point the arrow points to.
		/// </summary>
		public PointD To
		{
			get
			{
				return to_;
			}
			set
			{
				to_ = value;
			}
		}
		private PointD to_;


		/// <summary>
		/// Size of the arrow head sides in pixels.
		/// </summary>
		public float HeadSize
		{
			get
			{
				return headSize_;
			}
			set
			{
				headSize_ = value;
			}
		}
		private float headSize_ = 10.0f;


		/// <summary>
		/// angle between sides of arrow head in degrees
		/// </summary>
		public float HeadAngle
		{
			get
			{
				return headAngle_;
			}
			set
			{
				headAngle_ = value;
			}
		}
		private float headAngle_ = 40.0f;


		/// <summary>
		/// Draws the arrow on a plot surface.
		/// </summary>
		/// <param name="g">graphics surface on which to draw</param>
		/// <param name="xAxis">The X-Axis to draw against.</param>
		/// <param name="yAxis">The Y-Axis to draw against.</param>
		public void Draw( System.Drawing.Graphics g, PhysicalAxis xAxis, PhysicalAxis yAxis )
		{
			if (this.To.X > xAxis.Axis.WorldMax || this.To.X < xAxis.Axis.WorldMin)
			{
				return;
			}

			if (this.To.Y > yAxis.Axis.WorldMax || this.To.Y < yAxis.Axis.WorldMin)
			{
				return;
			}

			double angle = this.angle_;

			if (this.angle_ < 0.0)
			{
				int mul = -(int)(this.angle_ / 360.0) + 2;
				angle = angle_ + 360.0 * (double)mul;
			}

			double normAngle = (double)angle % 360.0;	// angle in range 0 -> 360.

			Point toPoint = new Point( 
				(int)xAxis.WorldToPhysical( to_.X, true ).X,
				(int)yAxis.WorldToPhysical( to_.Y, true ).Y );


			float xDir = (float)Math.Cos( normAngle * 2.0 * Math.PI / 360.0 );
			float yDir = (float)Math.Sin( normAngle * 2.0 * Math.PI / 360.0 );

			toPoint.X += (int)(xDir*headOffset_);
			toPoint.Y += (int)(yDir*headOffset_);

			float xOff = physicalLength_ * xDir;
			float yOff = physicalLength_ * yDir;

			Point fromPoint = new Point(
				(int)(toPoint.X + xOff),
				(int)(toPoint.Y + yOff) );

			g.DrawLine( pen_, toPoint, fromPoint );

			Point[] head = new Point[3];

			head[0] = toPoint;

			xOff = headSize_ * (float)Math.Cos( (normAngle-headAngle_/2.0f) * 2.0 * Math.PI / 360.0 );
			yOff = headSize_ * (float)Math.Sin( (normAngle-headAngle_/2.0f) * 2.0 * Math.PI / 360.0 );

			head[1] = new Point(
				(int)(toPoint.X + xOff),
				(int)(toPoint.Y + yOff) );

			float xOff2 = headSize_ * (float)Math.Cos( (normAngle+headAngle_/2.0f) * 2.0 * Math.PI / 360.0 );
			float yOff2 = headSize_ * (float)Math.Sin( (normAngle+headAngle_/2.0f) * 2.0 * Math.PI / 360.0 );

			head[2] = new Point(
				(int)(toPoint.X + xOff2),
				(int)(toPoint.Y + yOff2) );

			g.FillPolygon( arrowBrush_, head );

			SizeF textSize = g.MeasureString( text_, font_ );
			SizeF halfSize = new SizeF( textSize.Width / 2.0f, textSize.Height / 2.0f );

			float quadrantSlideLength = halfSize.Width + halfSize.Height;

			float quadrantF = (float)normAngle / 90.0f;		  // integer part gives quadrant.
			int quadrant = (int)quadrantF;			  // quadrant in. 
			float prop = quadrantF - (float)quadrant; // proportion of way through this qadrant. 
			float dist = prop * quadrantSlideLength;	  // distance along quarter of bounds rectangle.
			
			// now find the offset from the middle of the text box that the
			// rear end of the arrow should end at (reverse this to get position
			// of text box with respect to rear end of arrow).
			//
			// There is almost certainly an elgant way of doing this involving
			// trig functions to get all the signs right, but I'm about ready to 
			// drop off to sleep at the moment, so this blatent method will have 
			// to do.
			PointF offsetFromMiddle = new PointF( 0.0f, 0.0f );
			switch (quadrant)
			{
				case 0:
					if (dist > halfSize.Height)
					{
						dist -= halfSize.Height;
						offsetFromMiddle = new PointF( -halfSize.Width + dist, halfSize.Height );
					}
					else
					{
						offsetFromMiddle = new PointF( -halfSize.Width, - dist );
					}
					break;

				case 1:
					if (dist > halfSize.Width)
					{
						dist -= halfSize.Width;
						offsetFromMiddle = new PointF( halfSize.Width, halfSize.Height - dist );
					}
					else
					{
						offsetFromMiddle = new PointF( dist, halfSize.Height );
					}
					break;

				case 2:
					if (dist > halfSize.Height)
					{
						dist -= halfSize.Height;
						offsetFromMiddle = new PointF( halfSize.Width - dist, -halfSize.Height );
					}
					else
					{
						offsetFromMiddle = new PointF( halfSize.Width, -dist );
					}
					break;

				case 3:
					if (dist > halfSize.Width)
					{
						dist -= halfSize.Width;
						offsetFromMiddle = new PointF( -halfSize.Width, -halfSize.Height + dist );
					}
					else
					{
						offsetFromMiddle = new PointF( -dist, -halfSize.Height );
					}
					break;

				default:
					throw new NPlotException( "Programmer error." );

			}

			g.DrawString( 
				text_, font_, textBrush_,
				(int)(fromPoint.X - halfSize.Width - offsetFromMiddle.X),
				(int)(fromPoint.Y - halfSize.Height + offsetFromMiddle.Y) );

		}


		/// <summary>
		/// The brush used to draw the text associated with the arrow.
		/// </summary>
		public Brush TextBrush
		{
			get
			{
				return textBrush_;
			}
			set
			{
				textBrush_ = value;
			}
		}

	
		/// <summary>
		/// Set the text to be drawn with a solid brush of this color.
		/// </summary>
		public Color TextColor
		{
			set
			{
				textBrush_ = new SolidBrush( value );
			}
		}


		/// <summary>
		/// The color of the pen used to draw the arrow.
		/// </summary>
		public Color ArrowColor
		{
			get
			{
				return pen_.Color;
			}
			set
			{
				pen_.Color = value;
				arrowBrush_ = new SolidBrush( value );
			}
		}
			
		
		/// <summary>
		/// The font used to draw the text associated with the arrow.
		/// </summary>
		public Font TextFont
		{
			get
			{
				return this.font_;
			}
			set
			{
				this.font_ = value;
			}
		}


		/// <summary>
		/// Offset the whole arrow back in the arrow direction this many pixels from the point it's pointing to.
		/// </summary>
		public int HeadOffset
		{
			get
			{
				return headOffset_;
			}
			set
			{
				headOffset_ = value;
			}
		}
		private int headOffset_ = 2;


		private Brush arrowBrush_ = new SolidBrush( Color.Black );
		private Brush textBrush_ = new SolidBrush( Color.Black );
		private Pen pen_ = new Pen( Color.Black );
		private Font font_;
	}
}
