/*
 * NPlot - A charting library for .NET
 * 
 * RectangleBrushes.cs
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
using System.Drawing.Drawing2D;

namespace NPlot
{

	/// <summary>
	/// Classes that implement this interface can provide a brush 
	/// sized according to a given rectangle.
	/// </summary>
	public interface IRectangleBrush
	{

		/// <summary>
		/// Gets a brush according to the supplied rectangle.
		/// </summary>
		/// <param name="rectangle">the rectangle used to construct the brush</param>
		/// <returns>The brush</returns>
		Brush Get( Rectangle rectangle );

		/// <summary>
		/// Gets a brush according to the supplied rectangle.
		/// </summary>
		/// <param name="rectangle">the rectangle used to construct the brush</param>
		/// <returns>The brush</returns>
		Brush Get(RectangleF rectangle);
	}

	/// <summary>
	/// Collection of useful brushes.
	/// </summary>
	public class RectangleBrushes
	{

		/// <summary>
		/// A solid brush
		/// </summary>
		public class Solid : IRectangleBrush
		{
			Brush brush_;

			/// <summary>
			/// Constructor
			/// </summary>
			/// <param name="c">brush color</param>
			public Solid( Color c )
			{
				brush_ = new SolidBrush( c );
			}

			/// <summary>
			/// Gets a brush according to the supplied rectangle.
			/// </summary>
			/// <param name="rectangle">the rectangle used to construct the brush</param>
			/// <returns>The solid brush</returns>
			public Brush Get( Rectangle rectangle )
			{
				return brush_;
			}

			public Brush Get(RectangleF rectangle)
			{
				return brush_;
			}

			#region Default Brushes
			/// <summary>
			/// AliceBlue solid brush.
			/// </summary>
			public static Solid AliceBlue
			{
				get
				{
					return new Solid( Color.AliceBlue );
				}
			}

			/// <summary>
			/// AntiqueWhite solid brush.
			/// </summary>
			public static Solid AntiqueWhite
			{
				get
				{
					return new Solid( Color.AntiqueWhite );
				}
			}

			/// <summary>
			/// Aqua solid brush.
			/// </summary>
			public static Solid Aqua
			{
				get
				{
					return new Solid( Color.Aqua );
				}
			}

			/// <summary>
			/// Aquamarine solid brush.
			/// </summary>
			public static Solid Aquamarine
			{
				get
				{
					return new Solid( Color.Aquamarine );
				}
			}

			/// <summary>
			/// Azure solid brush.
			/// </summary>
			public static Solid Azure
			{
				get
				{
					return new Solid( Color.Azure );
				}
			}

			/// <summary>
			/// Beige solid brush.
			/// </summary>
			public static Solid Beige
			{
				get
				{
					return new Solid( Color.Beige );
				}
			}

			/// <summary>
			/// Bisque solid brush.
			/// </summary>
			public static Solid Bisque
			{
				get
				{
					return new Solid( Color.Bisque );
				}
			}

			/// <summary>
			/// Black solid brush.
			/// </summary>
			public static Solid Black
			{
				get
				{
					return new Solid( Color.Black );
				}
			}

			/// <summary>
			/// BlanchedAlmond solid brush.
			/// </summary>
			public static Solid BlanchedAlmond
			{
				get
				{
					return new Solid( Color.BlanchedAlmond );
				}
			}

			/// <summary>
			/// Blue solid brush.
			/// </summary>
			public static Solid Blue
			{
				get
				{
					return new Solid( Color.Blue );
				}
			}

			/// <summary>
			/// BlueViolet solid brush.
			/// </summary>
			public static Solid BlueViolet
			{
				get
				{
					return new Solid( Color.BlueViolet );
				}
			}

			/// <summary>
			/// Brown solid brush.
			/// </summary>
			public static Solid Brown
			{
				get
				{
					return new Solid( Color.Brown );
				}
			}

			/// <summary>
			/// BurlyWood solid brush.
			/// </summary>
			public static Solid BurlyWood
			{
				get
				{
					return new Solid( Color.BurlyWood );
				}
			}

			/// <summary>
			/// CadetBlue solid brush.
			/// </summary>
			public static Solid CadetBlue
			{
				get
				{
					return new Solid( Color.CadetBlue );
				}
			}

			/// <summary>
			/// Chartreuse solid brush.
			/// </summary>
			public static Solid Chartreuse
			{
				get
				{
					return new Solid( Color.Chartreuse );
				}
			}

			/// <summary>
			/// Chocolate solid brush.
			/// </summary>
			public static Solid Chocolate
			{
				get
				{
					return new Solid( Color.Chocolate );
				}
			}

			/// <summary>
			/// Coral solid brush.
			/// </summary>
			public static Solid Coral
			{
				get
				{
					return new Solid( Color.Coral );
				}
			}

			/// <summary>
			/// CornflowerBlue solid brush.
			/// </summary>
			public static Solid CornflowerBlue
			{
				get
				{
					return new Solid( Color.CornflowerBlue );
				}
			}

			/// <summary>
			/// Cornsilk solid brush.
			/// </summary>
			public static Solid Cornsilk
			{
				get
				{
					return new Solid( Color.Cornsilk );
				}
			}

			/// <summary>
			/// Crimson solid brush.
			/// </summary>
			public static Solid Crimson
			{
				get
				{
					return new Solid( Color.Crimson );
				}
			}

			/// <summary>
			/// Cyan solid brush.
			/// </summary>
			public static Solid Cyan
			{
				get
				{
					return new Solid( Color.Cyan );
				}
			}

			/// <summary>
			/// DarkBlue solid brush.
			/// </summary>
			public static Solid DarkBlue
			{
				get
				{
					return new Solid( Color.DarkBlue );
				}
			}

			/// <summary>
			/// DarkCyan solid brush.
			/// </summary>
			public static Solid DarkCyan
			{
				get
				{
					return new Solid( Color.DarkCyan );
				}
			}

			/// <summary>
			/// DarkGoldenrod solid brush.
			/// </summary>
			public static Solid DarkGoldenrod
			{
				get
				{
					return new Solid( Color.DarkGoldenrod );
				}
			}

			/// <summary>
			/// DarkGray solid brush.
			/// </summary>
			public static Solid DarkGray
			{
				get
				{
					return new Solid( Color.DarkGray );
				}
			}

			/// <summary>
			/// DarkGreen solid brush.
			/// </summary>
			public static Solid DarkGreen
			{
				get
				{
					return new Solid( Color.DarkGreen );
				}
			}

			/// <summary>
			/// DarkKhaki solid brush.
			/// </summary>
			public static Solid DarkKhaki
			{
				get
				{
					return new Solid( Color.DarkKhaki );
				}
			}

			/// <summary>
			/// DarkMagenta solid brush.
			/// </summary>
			public static Solid DarkMagenta
			{
				get
				{
					return new Solid( Color.DarkMagenta );
				}
			}

			/// <summary>
			/// DarkOliveGreen solid brush.
			/// </summary>
			public static Solid DarkOliveGreen
			{
				get
				{
					return new Solid( Color.DarkOliveGreen );
				}
			}

			/// <summary>
			/// DarkOrange solid brush.
			/// </summary>
			public static Solid DarkOrange
			{
				get
				{
					return new Solid( Color.DarkOrange );
				}
			}

			/// <summary>
			/// DarkOrchid solid brush.
			/// </summary>
			public static Solid DarkOrchid
			{
				get
				{
					return new Solid( Color.DarkOrchid );
				}
			}

			/// <summary>
			/// DarkRed solid brush.
			/// </summary>
			public static Solid DarkRed
			{
				get
				{
					return new Solid( Color.DarkRed );
				}
			}

			/// <summary>
			/// DarkSalmon solid brush.
			/// </summary>
			public static Solid DarkSalmon
			{
				get
				{
					return new Solid( Color.DarkSalmon );
				}
			}

			/// <summary>
			/// DarkSeaGreen solid brush.
			/// </summary>
			public static Solid DarkSeaGreen
			{
				get
				{
					return new Solid( Color.DarkSeaGreen );
				}
			}

			/// <summary>
			/// DarkSlateBlue solid brush.
			/// </summary>
			public static Solid DarkSlateBlue
			{
				get
				{
					return new Solid( Color.DarkSlateBlue );
				}
			}

			/// <summary>
			/// DarkSlateGray solid brush.
			/// </summary>
			public static Solid DarkSlateGray
			{
				get
				{
					return new Solid( Color.DarkSlateGray );
				}
			}

			/// <summary>
			/// DarkTurquoise solid brush.
			/// </summary>
			public static Solid DarkTurquoise
			{
				get
				{
					return new Solid( Color.DarkTurquoise );
				}
			}

			/// <summary>
			/// DarkViolet solid brush.
			/// </summary>
			public static Solid DarkViolet
			{
				get
				{
					return new Solid( Color.DarkViolet );
				}
			}

			/// <summary>
			/// DeepPink solid brush.
			/// </summary>
			public static Solid DeepPink
			{
				get
				{
					return new Solid( Color.DeepPink );
				}
			}

			/// <summary>
			/// DeepSkyBlue solid brush.
			/// </summary>
			public static Solid DeepSkyBlue
			{
				get
				{
					return new Solid( Color.DeepSkyBlue );
				}
			}

			/// <summary>
			/// DimGray solid brush.
			/// </summary>
			public static Solid DimGray
			{
				get
				{
					return new Solid( Color.DimGray );
				}
			}

			/// <summary>
			/// DodgerBlue solid brush.
			/// </summary>
			public static Solid DodgerBlue
			{
				get
				{
					return new Solid( Color.DodgerBlue );
				}
			}

			/// <summary>
			/// Firebrick solid brush.
			/// </summary>
			public static Solid Firebrick
			{
				get
				{
					return new Solid( Color.Firebrick );
				}
			}

			/// <summary>
			/// FloralWhite solid brush.
			/// </summary>
			public static Solid FloralWhite
			{
				get
				{
					return new Solid( Color.FloralWhite );
				}
			}

			/// <summary>
			/// ForestGreen solid brush.
			/// </summary>
			public static Solid ForestGreen
			{
				get
				{
					return new Solid( Color.ForestGreen );
				}
			}

			/// <summary>
			/// Fuchsia solid brush.
			/// </summary>
			public static Solid Fuchsia
			{
				get
				{
					return new Solid( Color.Fuchsia );
				}
			}

			/// <summary>
			/// Gainsboro solid brush.
			/// </summary>
			public static Solid Gainsboro
			{
				get
				{
					return new Solid( Color.Gainsboro );
				}
			}

			/// <summary>
			/// GhostWhite solid brush.
			/// </summary>
			public static Solid GhostWhite
			{
				get
				{
					return new Solid( Color.GhostWhite );
				}
			}

			/// <summary>
			/// Gold solid brush.
			/// </summary>
			public static Solid Gold
			{
				get
				{
					return new Solid( Color.Gold );
				}
			}

			/// <summary>
			/// Goldenrod solid brush.
			/// </summary>
			public static Solid Goldenrod
			{
				get
				{
					return new Solid( Color.Goldenrod );
				}
			}

			/// <summary>
			/// Gray  solid brush.
			/// </summary>
			public static Solid Gray 
			{
				get
				{
					return new Solid( Color.Gray );
				}
			}

			/// <summary>
			/// Green solid brush.
			/// </summary>
			public static Solid Green
			{
				get
				{
					return new Solid( Color.Green );
				}
			}

			/// <summary>
			/// GreenYellow solid brush.
			/// </summary>
			public static Solid GreenYellow
			{
				get
				{
					return new Solid( Color.GreenYellow );
				}
			}

			/// <summary>
			/// Honeydew solid brush.
			/// </summary>
			public static Solid Honeydew
			{
				get
				{
					return new Solid( Color.Honeydew );
				}
			}

			/// <summary>
			/// HotPink solid brush.
			/// </summary>
			public static Solid HotPink
			{
				get
				{
					return new Solid( Color.HotPink );
				}
			}

			/// <summary>
			/// IndianRed solid brush.
			/// </summary>
			public static Solid IndianRed
			{
				get
				{
					return new Solid( Color.IndianRed );
				}
			}

			/// <summary>
			/// Indigo solid brush.
			/// </summary>
			public static Solid Indigo
			{
				get
				{
					return new Solid( Color.Indigo );
				}
			}

			/// <summary>
			/// Ivory solid brush.
			/// </summary>
			public static Solid Ivory
			{
				get
				{
					return new Solid( Color.Ivory );
				}
			}

			/// <summary>
			/// Khaki solid brush.
			/// </summary>
			public static Solid Khaki
			{
				get
				{
					return new Solid( Color.Khaki );
				}
			}

			/// <summary>
			/// Lavender solid brush.
			/// </summary>
			public static Solid Lavender
			{
				get
				{
					return new Solid( Color.Lavender );
				}
			}

			/// <summary>
			/// LavenderBlush solid brush.
			/// </summary>
			public static Solid LavenderBlush
			{
				get
				{
					return new Solid( Color.LavenderBlush );
				}
			}

			/// <summary>
			/// LawnGreen solid brush.
			/// </summary>
			public static Solid LawnGreen
			{
				get
				{
					return new Solid( Color.LawnGreen );
				}
			}

			/// <summary>
			/// LemonChiffon solid brush.
			/// </summary>
			public static Solid LemonChiffon
			{
				get
				{
					return new Solid( Color.LemonChiffon );
				}
			}

			/// <summary>
			/// LightBlue solid brush.
			/// </summary>
			public static Solid LightBlue
			{
				get
				{
					return new Solid( Color.LightBlue );
				}
			}

			/// <summary>
			/// LightCoral solid brush.
			/// </summary>
			public static Solid LightCoral
			{
				get
				{
					return new Solid( Color.LightCoral );
				}
			}

			/// <summary>
			/// LightCyan solid brush.
			/// </summary>
			public static Solid LightCyan
			{
				get
				{
					return new Solid( Color.LightCyan );
				}
			}

			/// <summary>
			/// LightGoldenrodYellow solid brush.
			/// </summary>
			public static Solid LightGoldenrodYellow
			{
				get
				{
					return new Solid( Color.LightGoldenrodYellow );
				}
			}

			/// <summary>
			/// LightGray solid brush.
			/// </summary>
			public static Solid LightGray
			{
				get
				{
					return new Solid( Color.LightGray );
				}
			}

			/// <summary>
			/// LightGreen solid brush.
			/// </summary>
			public static Solid LightGreen
			{
				get
				{
					return new Solid( Color.LightGreen );
				}
			}

			/// <summary>
			/// LightPink solid brush.
			/// </summary>
			public static Solid LightPink
			{
				get
				{
					return new Solid( Color.LightPink );
				}
			}

			/// <summary>
			/// LightSalmon solid brush.
			/// </summary>
			public static Solid LightSalmon
			{
				get
				{
					return new Solid( Color.LightSalmon );
				}
			}

			/// <summary>
			/// LightSeaGreen solid brush.
			/// </summary>
			public static Solid LightSeaGreen
			{
				get
				{
					return new Solid( Color.LightSeaGreen );
				}
			}

			/// <summary>
			/// LightSkyBlue solid brush.
			/// </summary>
			public static Solid LightSkyBlue
			{
				get
				{
					return new Solid( Color.LightSkyBlue );
				}
			}

			/// <summary>
			/// LightSlateGray solid brush.
			/// </summary>
			public static Solid LightSlateGray
			{
				get
				{
					return new Solid( Color.LightSlateGray );
				}
			}

			/// <summary>
			/// LightSteelBlue solid brush.
			/// </summary>
			public static Solid LightSteelBlue
			{
				get
				{
					return new Solid( Color.LightSteelBlue );
				}
			}

			/// <summary>
			/// LightYellow solid brush.
			/// </summary>
			public static Solid LightYellow
			{
				get
				{
					return new Solid( Color.LightYellow );
				}
			}

			/// <summary>
			/// Lime solid brush.
			/// </summary>
			public static Solid Lime
			{
				get
				{
					return new Solid( Color.Lime );
				}
			}

			/// <summary>
			/// LimeGreen solid brush.
			/// </summary>
			public static Solid LimeGreen
			{
				get
				{
					return new Solid( Color.LimeGreen );
				}
			}

			/// <summary>
			/// Color.Linen solid brush.
			/// </summary>
			public static Solid Linen
			{
				get
				{
					return new Solid( Color.Linen );
				}
			}

			/// <summary>
			/// Color.Magenta solid brush.
			/// </summary>
			public static Solid Magenta
			{
				get
				{
					return new Solid( Color.Magenta );
				}
			}

			/// <summary>
			/// Maroon solid brush.
			/// </summary>
			public static Solid Maroon
			{
				get
				{
					return new Solid( Color.Maroon );
				}
			}

			/// <summary>
			/// MediumAquamarine solid brush.
			/// </summary>
			public static Solid MediumAquamarine
			{
				get
				{
					return new Solid( Color.MediumAquamarine );
				}
			}

			/// <summary>
			/// MediumBlue solid brush.
			/// </summary>
			public static Solid MediumBlue
			{
				get
				{
					return new Solid( Color.MediumBlue );
				}
			}

			/// <summary>
			/// MediumOrchid  solid brush.
			/// </summary>
			public static Solid MediumOrchid 
			{
				get
				{
					return new Solid( Color.MediumOrchid );
				}
			}

			/// <summary>
			/// MediumPurple solid brush.
			/// </summary>
			public static Solid MediumPurple
			{
				get
				{
					return new Solid( Color.MediumPurple );
				}
			}

			/// <summary>
			/// MediumSeaGreen solid brush.
			/// </summary>
			public static Solid MediumSeaGreen
			{
				get
				{
					return new Solid( Color.MediumSeaGreen );
				}
			}

			/// <summary>
			/// MediumSlateBlue solid brush.
			/// </summary>
			public static Solid MediumSlateBlue
			{
				get
				{
					return new Solid( Color.MediumSlateBlue );
				}
			}

			/// <summary>
			/// MediumSpringGreen solid brush.
			/// </summary>
			public static Solid MediumSpringGreen
			{
				get
				{
					return new Solid( Color.MediumSpringGreen );
				}
			}

			/// <summary>
			/// MediumTurquoise solid brush.
			/// </summary>
			public static Solid MediumTurquoise
			{
				get
				{
					return new Solid( Color.MediumTurquoise );
				}
			}

			/// <summary>
			/// MediumVioletRed solid brush.
			/// </summary>
			public static Solid MediumVioletRed
			{
				get
				{
					return new Solid( Color.MediumVioletRed );
				}
			}

			/// <summary>
			/// MidnightBlue  solid brush.
			/// </summary>
			public static Solid MidnightBlue 
			{
				get
				{
					return new Solid( Color.MidnightBlue );
				}
			}

			/// <summary>
			/// MintCream solid brush.
			/// </summary>
			public static Solid MintCream
			{
				get
				{
					return new Solid( Color.MintCream );
				}
			}

			/// <summary>
			/// MistyRose solid brush.
			/// </summary>
			public static Solid MistyRose
			{
				get
				{
					return new Solid( Color.MistyRose );
				}
			}

			/// <summary>
			/// Moccasin solid brush.
			/// </summary>
			public static Solid Moccasin
			{
				get
				{
					return new Solid( Color.Moccasin );
				}
			}

			/// <summary>
			/// NavajoWhite solid brush.
			/// </summary>
			public static Solid NavajoWhite
			{
				get
				{
					return new Solid( Color.NavajoWhite );
				}
			}

			/// <summary>
			/// Navy solid brush.
			/// </summary>
			public static Solid Navy
			{
				get
				{
					return new Solid( Color.Navy );
				}
			}

			/// <summary>
			/// OldLace solid brush.
			/// </summary>
			public static Solid OldLace
			{
				get
				{
					return new Solid( Color.OldLace );
				}
			}

			/// <summary>
			/// Olive solid brush.
			/// </summary>
			public static Solid Olive
			{
				get
				{
					return new Solid( Color.Olive );
				}
			}

			/// <summary>
			/// OliveDrab solid brush.
			/// </summary>
			public static Solid OliveDrab
			{
				get
				{
					return new Solid( Color.OliveDrab );
				}
			}

			/// <summary>
			/// Orange solid brush.
			/// </summary>
			public static Solid Orange
			{
				get
				{
					return new Solid( Color.Orange );
				}
			}

			/// <summary>
			/// OrangeRed solid brush.
			/// </summary>
			public static Solid OrangeRed
			{
				get
				{
					return new Solid( Color.OrangeRed );
				}
			}

			/// <summary>
			/// Orchid solid brush.
			/// </summary>
			public static Solid Orchid
			{
				get
				{
					return new Solid( Color.Orchid );
				}
			}

			/// <summary>
			/// PaleGoldenrod solid brush.
			/// </summary>
			public static Solid PaleGoldenrod
			{
				get
				{
					return new Solid( Color.PaleGoldenrod );
				}
			}

			/// <summary>
			/// PaleGreen solid brush.
			/// </summary>
			public static Solid PaleGreen
			{
				get
				{
					return new Solid( Color.PaleGreen );
				}
			}

			/// <summary>
			/// PaleTurquoise solid brush.
			/// </summary>
			public static Solid PaleTurquoise
			{
				get
				{
					return new Solid( Color.PaleTurquoise );
				}
			}

			/// <summary>
			/// PaleVioletRed solid brush.
			/// </summary>
			public static Solid PaleVioletRed
			{
				get
				{
					return new Solid( Color.PaleVioletRed );
				}
			}

			/// <summary>
			/// PapayaWhip solid brush.
			/// </summary>
			public static Solid PapayaWhip
			{
				get
				{
					return new Solid( Color.PapayaWhip );
				}
			}

			/// <summary>
			/// PeachPuff solid brush.
			/// </summary>
			public static Solid PeachPuff
			{
				get
				{
					return new Solid( Color.PeachPuff );
				}
			}

			/// <summary>
			/// Peru solid brush.
			/// </summary>
			public static Solid Peru
			{
				get
				{
					return new Solid( Color.Peru );
				}
			}

			/// <summary>
			/// Pink solid brush.
			/// </summary>
			public static Solid Pink
			{
				get
				{
					return new Solid( Color.Pink );
				}
			}

			/// <summary>
			/// Plum solid brush.
			/// </summary>
			public static Solid Plum
			{
				get
				{
					return new Solid( Color.Plum );
				}
			}

			/// <summary>
			/// PowderBlue solid brush.
			/// </summary>
			public static Solid PowderBlue
			{
				get
				{
					return new Solid( Color.PowderBlue );
				}
			}

			/// <summary>
			/// Purple solid brush.
			/// </summary>
			public static Solid Purple
			{
				get
				{
					return new Solid( Color.Purple );
				}
			}

			/// <summary>
			/// Red solid brush.
			/// </summary>
			public static Solid Red
			{
				get
				{
					return new Solid( Color.Red );
				}
			}

			/// <summary>
			/// RosyBrown solid brush.
			/// </summary>
			public static Solid RosyBrown
			{
				get
				{
					return new Solid( Color.RosyBrown );
				}
			}

			/// <summary>
			/// RoyalBlue solid brush.
			/// </summary>
			public static Solid RoyalBlue
			{
				get
				{
					return new Solid( Color.RoyalBlue );
				}
			}

			/// <summary>
			/// SaddleBrown solid brush.
			/// </summary>
			public static Solid SaddleBrown
			{
				get
				{
					return new Solid( Color.SaddleBrown );
				}
			}

			/// <summary>
			/// Salmon solid brush.
			/// </summary>
			public static Solid Salmon
			{
				get
				{
					return new Solid( Color.Salmon );
				}
			}

			/// <summary>
			/// SandyBrown solid brush.
			/// </summary>
			public static Solid SandyBrown
			{
				get
				{
					return new Solid( Color.SandyBrown );
				}
			}

			/// <summary>
			/// SeaGreen solid brush.
			/// </summary>
			public static Solid SeaGreen
			{
				get
				{
					return new Solid( Color.SeaGreen );
				}
			}

			/// <summary>
			/// SeaShell solid brush.
			/// </summary>
			public static Solid SeaShell
			{
				get
				{
					return new Solid( Color.SeaShell );
				}
			}

			/// <summary>
			/// Sienna solid brush.
			/// </summary>
			public static Solid Sienna
			{
				get
				{
					return new Solid( Color.Sienna );
				}
			}

			/// <summary>
			/// Silver solid brush.
			/// </summary>
			public static Solid Silver
			{
				get
				{
					return new Solid( Color.Silver );
				}
			}

			/// <summary>
			/// SkyBlue solid brush.
			/// </summary>
			public static Solid SkyBlue
			{
				get
				{
					return new Solid( Color.SkyBlue );
				}
			}

			/// <summary>
			/// SlateBlue solid brush.
			/// </summary>
			public static Solid SlateBlue
			{
				get
				{
					return new Solid( Color.SlateBlue );
				}
			}

			/// <summary>
			/// SlateGray solid brush.
			/// </summary>
			public static Solid SlateGray
			{
				get
				{
					return new Solid( Color.SlateGray );
				}
			}

			/// <summary>
			/// Snow solid brush.
			/// </summary>
			public static Solid Snow
			{
				get
				{
					return new Solid( Color.Snow );
				}
			}

			/// <summary>
			/// SpringGreen solid brush.
			/// </summary>
			public static Solid SpringGreen
			{
				get
				{
					return new Solid( Color.SpringGreen );
				}
			}

			/// <summary>
			/// SteelBlue solid brush.
			/// </summary>
			public static Solid SteelBlue
			{
				get
				{
					return new Solid( Color.SteelBlue );
				}
			}

			/// <summary>
			/// Tan solid brush.
			/// </summary>
			public static Solid Tan
			{
				get
				{
					return new Solid( Color.Tan );
				}
			}

			/// <summary>
			/// Teal solid brush.
			/// </summary>
			public static Solid Teal
			{
				get
				{
					return new Solid( Color.Teal );
				}
			}

			/// <summary>
			/// Thistle solid brush.
			/// </summary>
			public static Solid Thistle
			{
				get
				{
					return new Solid( Color.Thistle );
				}
			}

			/// <summary>
			/// Tomato solid brush.
			/// </summary>
			public static Solid Tomato
			{
				get
				{
					return new Solid( Color.Tomato );
				}
			}

			/// <summary>
			/// Transparent solid brush.
			/// </summary>
			public static Solid Transparent
			{
				get
				{
					return new Solid( Color.Transparent );
				}
			}

			/// <summary>
			/// Turquoise solid brush.
			/// </summary>
			public static Solid Turquoise
			{
				get
				{
					return new Solid( Color.Turquoise );
				}
			}

			/// <summary>
			/// Violet solid brush.
			/// </summary>
			public static Solid Violet
			{
				get
				{
					return new Solid( Color.Violet );
				}
			}

			/// <summary>
			/// Wheat solid brush.
			/// </summary>
			public static Solid Wheat
			{
				get
				{
					return new Solid( Color.Wheat );
				}
			}

			/// <summary>
			/// White solid brush.
			/// </summary>
			public static Solid White
			{
				get
				{
					return new Solid( Color.White );
				}
			}

			/// <summary>
			/// WhiteSmoke solid brush.
			/// </summary>
			public static Solid WhiteSmoke
			{
				get
				{
					return new Solid( Color.WhiteSmoke );
				}
			}

			/// <summary>
			/// Yellow solid brush.
			/// </summary>
			public static Solid Yellow
			{
				get
				{
					return new Solid( Color.Yellow );
				}
			}

			/// <summary>
			/// YellowGreen solid brush.
			/// </summary>
			public static Solid YellowGreen
			{
				get
				{
					return new Solid( Color.YellowGreen );
				}
			}

			#endregion

		}


		/// <summary>
		/// A brush with horizontal gradient.
		/// </summary>
		public class Horizontal : IRectangleBrush
		{

			private Color c1_;
			private Color c2_;
			
			/// <summary>
			/// Constructor
			/// </summary>
			/// <param name="c1">Color on left.</param>
			/// <param name="c2">Color on right.</param>
			public Horizontal( Color c1, Color c2 )
			{
				c1_ = c1;
				c2_ = c2;
			}


			/// <summary>
			/// Gets a brush according to the supplied rectangle.
			/// </summary>
			/// <param name="rectangle">the rectangle used to construct the brush</param>
			/// <returns>The horizontal brush</returns>
			public Brush Get( Rectangle rectangle )
			{
				return new LinearGradientBrush( rectangle, c1_, c2_, LinearGradientMode.Horizontal );
			}


			public Brush Get(RectangleF rectangle)
			{
				return new LinearGradientBrush(rectangle, c1_, c2_, LinearGradientMode.Horizontal);
			}

			#region DefaultBrushes

			/// <summary>
			/// Default brush - fades from faint blue to white.
			/// </summary>
			public static Horizontal FaintBlueFade
			{
				get
				{
					return new Horizontal( Color.FromArgb(200,200,255), Color.FromArgb(255,255,255) );
				}
			}

			/// <summary>
			/// Default brush - fades from faint red to white.
			/// </summary>
			public static Horizontal FaintRedFade
			{
				get
				{
					return new Horizontal( Color.FromArgb(255,200,200), Color.FromArgb(255,255,255) );
				}
			}

			/// <summary>
			/// Default brush - fades from faint red to white.
			/// </summary>
			public static Horizontal FaintGreenFade
			{
				get
				{
					return new Horizontal( Color.FromArgb(200,255,200), Color.FromArgb(255,255,255) );
				}
			}

			#endregion

		}


		/// <summary>
		/// A brush with vertical gradient.
		/// </summary>
		public class Vertical : IRectangleBrush
		{

			private Color c1_;
			private Color c2_;
			
			/// <summary>
			/// Constructor
			/// </summary>
			/// <param name="c1">top color [or bottom?]</param>
			/// <param name="c2">bottom color [or top?]</param>
			public Vertical( Color c1, Color c2 )
			{
				c1_ = c1;
				c2_ = c2;
			}

			/// <summary>
			/// Gets a brush according to the supplied rectangle.
			/// </summary>
			/// <param name="rectangle">the rectangle used to construct the brush</param>
			/// <returns>The vertical brush</returns>
			public Brush Get( Rectangle rectangle )
			{
				return new LinearGradientBrush( rectangle, c1_, c2_, LinearGradientMode.Vertical );
			}


			public Brush Get(RectangleF rectangle)
			{
				return new LinearGradientBrush(rectangle, c1_, c2_, LinearGradientMode.Vertical);
			}

			#region DefaultBrushes

			/// <summary>
			/// Default brush - fades from faint blue to white.
			/// </summary>
			public static Vertical FaintBlueFade
			{
				get
				{
					return new Vertical( Color.FromArgb(200,200,255), Color.FromArgb(255,255,255) );
				}
			}

			/// <summary>
			/// Default brush - fades from faint red to white.
			/// </summary>
			public static Vertical FaintRedFade
			{
				get
				{
					return new Vertical( Color.FromArgb(255,200,200), Color.FromArgb(255,255,255) );
				}
			}

			/// <summary>
			/// Default brush - fades from faint red to white.
			/// </summary>
			public static Vertical FaintGreenFade
			{
				get
				{
					return new Vertical( Color.FromArgb(200,255,200), Color.FromArgb(255,255,255) );
				}
			}

			#endregion
		}


		/// <summary>
		/// A brush with horizontal gradient that fades into center then out again.
		/// </summary>
		public class HorizontalCenterFade : IRectangleBrush
		{

			private Color c1_;
			private Color c2_;

			/// <summary>
			/// Constructor
			/// </summary>
			/// <param name="c1">inner color</param>
			/// <param name="c2">outer color</param>
			public HorizontalCenterFade( Color c1, Color c2 )
			{
				c1_ = c1;
				c2_ = c2;
			}


			/// <summary>
			/// Gets a brush according to the supplied rectangle.
			/// </summary>
			/// <param name="rectangle">the rectangle used to construct the brush</param>
			/// <returns>The horizontal center fade brush</returns>
			public Brush Get( Rectangle rectangle )
			{
				LinearGradientBrush brush = new LinearGradientBrush( rectangle, c1_, c2_, LinearGradientMode.Horizontal );
				float[] relativeIntensities = { 0.0f, 0.9f, 1.0f, 0.9f, 0.0f };
				float[] relativePositions	= { 0.0f, 0.4f, 0.5f, 0.6f, 1.0f };
				Blend blend = new Blend();
				blend.Factors = relativeIntensities;
				blend.Positions = relativePositions;
				brush.Blend = blend;
				return brush;
			}


			public Brush Get(RectangleF rectangle)
			{
				LinearGradientBrush brush = new LinearGradientBrush(rectangle, c1_, c2_, LinearGradientMode.Horizontal);
				float[] relativeIntensities = { 0.0f, 0.9f, 1.0f, 0.9f, 0.0f };
				float[] relativePositions = { 0.0f, 0.4f, 0.5f, 0.6f, 1.0f };
				Blend blend = new Blend();
				blend.Factors = relativeIntensities;
				blend.Positions = relativePositions;
				brush.Blend = blend;
				return brush;
			}

			#region DefaultBrushes

			/// <summary>
			/// Default brush - fades from faint blue to white.
			/// </summary>
			public static HorizontalCenterFade FaintBlueFade
			{
				get
				{
					return new HorizontalCenterFade( Color.FromArgb(200,200,255), Color.FromArgb(255,255,255) );
				}
			}

			/// <summary>
			/// Default brush - fades from faint red to white.
			/// </summary>
			public static HorizontalCenterFade FaintRedFade
			{
				get
				{
					return new HorizontalCenterFade( Color.FromArgb(255,200,200), Color.FromArgb(255,255,255) );
				}
			}

			/// <summary>
			/// Default brush - fades from faint red to white.
			/// </summary>
			public static HorizontalCenterFade FaintGreenFade
			{
				get
				{
					return new HorizontalCenterFade( Color.FromArgb(200,255,200), Color.FromArgb(255,255,255) );
				}
			}

			#endregion
		}



		/// <summary>
		/// Brush with vertical gradient that fades into center then out again.
		/// </summary>
		public class VerticalCenterFade : IRectangleBrush
		{

			private Color c1_;
			private Color c2_;

			/// <summary>
			/// Constructor
			/// </summary>
			/// <param name="c1">inner color</param>
			/// <param name="c2">outer color</param>
			public VerticalCenterFade( Color c1, Color c2 )
			{
				c1_ = c1;
				c2_ = c2;
			}


			/// <summary>
			/// Gets a brush according to the supplied rectangle.
			/// </summary>
			/// <param name="rectangle">the rectangle used to construct the brush</param>
			/// <returns>The vertical center fade brush</returns>
			public Brush Get( Rectangle rectangle )
			{
				LinearGradientBrush brush = new LinearGradientBrush( rectangle, c1_, c2_, LinearGradientMode.Vertical );
				float[] relativeIntensities = { 0.0f, 0.9f, 1.0f, 0.9f, 0.0f };
				float[] relativePositions	= { 0.0f, 0.4f, 0.5f, 0.6f, 1.0f };
				Blend blend = new Blend();
				blend.Factors = relativeIntensities;
				blend.Positions = relativePositions;
				brush.Blend = blend;
				return brush;
			}


			public Brush Get(RectangleF rectangle)
			{
				LinearGradientBrush brush = new LinearGradientBrush(rectangle, c1_, c2_, LinearGradientMode.Vertical);
				float[] relativeIntensities = { 0.0f, 0.9f, 1.0f, 0.9f, 0.0f };
				float[] relativePositions = { 0.0f, 0.4f, 0.5f, 0.6f, 1.0f };
				Blend blend = new Blend();
				blend.Factors = relativeIntensities;
				blend.Positions = relativePositions;
				brush.Blend = blend;
				return brush;
			}

			#region DefaultBrushes

			/// <summary>
			/// Default brush - fades from faint blue to white.
			/// </summary>
			public static VerticalCenterFade FaintBlueFade
			{
				get
				{
					return new VerticalCenterFade( Color.FromArgb(200,200,255), Color.FromArgb(255,255,255) );
				}
			}

			/// <summary>
			/// Default brush - fades from faint red to white.
			/// </summary>
			public static VerticalCenterFade FaintRedFade
			{
				get
				{
					return new VerticalCenterFade( Color.FromArgb(255,200,200), Color.FromArgb(255,255,255) );
				}
			}

			/// <summary>
			/// Default brush - fades from faint red to white.
			/// </summary>
			public static VerticalCenterFade FaintGreenFade
			{
				get
				{
					return new VerticalCenterFade( Color.FromArgb(200,255,200), Color.FromArgb(255,255,255) );
				}
			}

			#endregion
		}
	}
}
