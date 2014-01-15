//
// XwPlot - A cross-platform charting library using the Xwt toolkit
// 
// ArrowItem.cs
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
using Xwt;
using Xwt.Drawing;

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
			textFont_ = Font.FromName ("Tahoma 10");
		}

		/// <summary>
		/// Default constructor : 
		/// text = ""
		/// angle = 45 degrees anticlockwise from horizontal.
		/// </summary>
		/// <param name="position">The position the arrow points to.</param>
		public ArrowItem (Point position)
		{
			to_ = position;
			Init();
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="position">The position the arrow points to.</param>
		/// <param name="angle">angle of arrow with respect to x axis.</param>
		public ArrowItem (Point position, double angle)
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
		public ArrowItem (Point position, double angle, string text)
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
			get { return text_; }
			set { text_ = value; }
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
			get { return -angle_; }
			set { angle_ = -value; }
		}
		private double angle_ = -45.0;

		/// <summary>
		/// Physical length of the arrow. 
		/// </summary>
		public double PhysicalLength
		{
			get { return physicalLength_; }
			set { physicalLength_ = value; }
		}
		private double physicalLength_ = 40.0;

		/// <summary>
		/// The point the arrow points to.
		/// </summary>
		public Point To
		{
			get { return to_; }
			set { to_ = value; }
		}
		private Point to_;

		/// <summary>
		/// Size of the arrow head sides in pixels.
		/// </summary>
		public double HeadSize
		{
			get { return headSize_; }
			set { headSize_ = value; }
		}
		private double headSize_ = 10.0;

		/// <summary>
		/// angle between sides of arrow head in degrees
		/// </summary>
		public double HeadAngle
		{
			get { return headAngle_; }
			set { headAngle_ = value; }
		}
		private double headAngle_ = 40.0;

		/// <summary>
		/// Set the text to be drawn with a solid brush of this color.
		/// </summary>
		public Color TextColor
		{
			get { return textColor_; }
			set	{ textColor_ =  value; }
		}
		private Color textColor_ =  Colors.Black;


		/// <summary>
		/// The color of the pen used to draw the arrow.
		/// </summary>
		public Color ArrowColor
		{
			get { return arrowColor_; }
			set { arrowColor_ = value; }
		}
		private Color arrowColor_ = Colors.Black;
			
		
		/// <summary>
		/// The font used to draw the text associated with the arrow.
		/// </summary>
		public Font TextFont
		{
			get { return textFont_; }
			set { textFont_ = value; }
		}
		private Font textFont_;


		/// <summary>
		/// Offset the whole arrow back in the arrow direction this many pixels from the point it's pointing to.
		/// </summary>
		public double HeadOffset
		{
			get { return headOffset_; }
			set { headOffset_ = value; }
		}
		private double headOffset_ = 2;


		/// <summary>
		/// Draws the arrow on a plot surface.
		/// </summary>
		/// <param name="ctx">the Drawing Context with which to draw</param>
		/// <param name="xAxis">The X-Axis to draw against.</param>
		/// <param name="yAxis">The Y-Axis to draw against.</param>
		public void Draw (Context ctx, PhysicalAxis xAxis, PhysicalAxis yAxis )
		{
			if (To.X > xAxis.Axis.WorldMax || To.X < xAxis.Axis.WorldMin) {
				return;
			}
			if (To.Y > yAxis.Axis.WorldMax || To.Y < yAxis.Axis.WorldMin) {
				return;
			}

			ctx.Save ();

			TextLayout layout = new TextLayout ();
			layout.Font = textFont_;
			layout.Text = text_;

			double angle = angle_;
			if (angle_ < 0.0) {
				int mul = -(int)(angle_ / 360.0) + 2;
				angle = angle_ + 360.0 * (double)mul;
			}

			double normAngle = (double)angle % 360.0;	// angle in range 0 -> 360.

			Point toPoint = new Point ( 
				xAxis.WorldToPhysical (to_.X, true).X,
				yAxis.WorldToPhysical (to_.Y, true).Y);


			double xDir = Math.Cos (normAngle * 2.0 * Math.PI / 360.0);
			double yDir = Math.Sin (normAngle * 2.0 * Math.PI / 360.0);

			toPoint.X += xDir*headOffset_;
			toPoint.Y += yDir*headOffset_;

			double xOff = physicalLength_ * xDir;
			double yOff = physicalLength_ * yDir;

			Point fromPoint = new Point(
				(int)(toPoint.X + xOff),
				(int)(toPoint.Y + yOff) );

			ctx.SetLineWidth (1);
			ctx.SetColor (arrowColor_);
			ctx.MoveTo (fromPoint);
			ctx.LineTo (toPoint);
			ctx.Stroke ();

			xOff = headSize_ * Math.Cos ((normAngle-headAngle_/2) * 2.0 * Math.PI / 360.0);
			yOff = headSize_ * Math.Sin ((normAngle-headAngle_/2) * 2.0 * Math.PI / 360.0);

			ctx.LineTo (toPoint.X + xOff, toPoint.Y + yOff);

			double xOff2 = headSize_ * Math.Cos ((normAngle+headAngle_/2) * 2.0 * Math.PI / 360.0);
			double yOff2 = headSize_ * Math.Sin ((normAngle+headAngle_/2) * 2.0 * Math.PI / 360.0);

			ctx.LineTo (toPoint.X + xOff2, toPoint.Y + yOff2);
			ctx.LineTo (toPoint);
			ctx.ClosePath ();
			ctx.SetColor (arrowColor_);
			ctx.Fill ();

			Size textSize = layout.GetSize ();
			Size halfSize = new Size (textSize.Width/2, textSize.Height/2);

			double quadrantSlideLength = halfSize.Width + halfSize.Height;

			double quadrantD = normAngle / 90.0;		// integer part gives quadrant.
			int quadrant = (int)quadrantD;				// quadrant in. 
			double prop = quadrantD - (double)quadrant;	// proportion of way through this qadrant. 
			double dist = prop * quadrantSlideLength;	// distance along quarter of bounds rectangle.
			
			// now find the offset from the middle of the text box that the
			// rear end of the arrow should end at (reverse this to get position
			// of text box with respect to rear end of arrow).
			//
			// There is almost certainly an elgant way of doing this involving
			// trig functions to get all the signs right, but I'm about ready to 
			// drop off to sleep at the moment, so this blatent method will have 
			// to do.
			Point offsetFromMiddle = new Point (0, 0);
			switch (quadrant) {
			case 0:
				if (dist > halfSize.Height) {
					dist -= halfSize.Height;
					offsetFromMiddle = new Point ( -halfSize.Width + dist, halfSize.Height );
				}
				else {
					offsetFromMiddle = new Point ( -halfSize.Width, - dist );
				}
				break;
			case 1:
				if (dist > halfSize.Width) {
					dist -= halfSize.Width;
					offsetFromMiddle = new Point ( halfSize.Width, halfSize.Height - dist );
				}
				else {
					offsetFromMiddle = new Point ( dist, halfSize.Height );
				}
				break;
			case 2:
				if (dist > halfSize.Height) {
					dist -= halfSize.Height;
					offsetFromMiddle = new Point ( halfSize.Width - dist, -halfSize.Height );
				}
				else {
					offsetFromMiddle = new Point ( halfSize.Width, -dist );
				}
				break;
			case 3:
				if (dist > halfSize.Width) {
					dist -= halfSize.Width;
					offsetFromMiddle = new Point ( -halfSize.Width, -halfSize.Height + dist );
				}
				else {
					offsetFromMiddle = new Point ( -dist, -halfSize.Height );
				}
				break;
			default:
				throw new NPlotException( "Programmer error." );
			}

			ctx.SetColor (textColor_);
			double x = fromPoint.X - halfSize.Width - offsetFromMiddle.X;
			double y = fromPoint.Y - halfSize.Height + offsetFromMiddle.Y;
			ctx.DrawTextLayout (layout, x, y);

			ctx.Restore ();

		}

	}
}
