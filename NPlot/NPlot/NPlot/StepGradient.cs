/*
 * NPlot - A charting library for .NET
 * 
 * StepGradient.cs
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
	/// Class for creating a rainbow legend.
	/// </summary>
	public class StepGradient : IGradient
	{

		/// <summary>
		/// Types of step gradient defined.
		/// </summary>
		public enum Type
		{
			/// <summary>
			/// Rainbow gradient type (colors of the rainbow)
			/// </summary>
			Rainbow,

			/// <summary>
			/// RGB gradient type (red, green blud).
			/// </summary>
			RGB
		}


		/// <summary>
		/// Sets the type of step gradient.
		/// </summary>
		public Type StepType
		{
			get
			{
				return stepType_;
			}
			set
			{
				stepType_ = value;
			}
		}
		Type stepType_ = Type.RGB;


		/// <summary>
		/// Default Constructor
		/// </summary>
		public StepGradient()
		{
		}


		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="stepType">type of gradient</param>
		public StepGradient( Type stepType )
		{
			stepType_  = stepType;
		}


		/// <summary>
		/// Gets a color corresponding to a number between 0.0 and 1.0 inclusive. The color will
		/// be a linear interpolation of the min and max colors.
		/// </summary>
		/// <param name="prop">the number to get corresponding color for (between 0.0 and 1.0)</param>
		/// <returns>The color corresponding to the supplied number.</returns>
		public Color GetColor( double prop )
		{
			switch (stepType_)
			{
				case Type.RGB:
				{
					if (prop < 1.0 / 3.0)
					{
						return Color.Red;
					}
					if (prop < 2.0 / 3.0)
					{
						return Color.Green;
					}
					return Color.Blue;
				}
				case Type.Rainbow:
				{
					if (prop < 0.125) 
					{ 
						return Color.Red; 
					}
					if (prop < 0.25)
					{ 
						return Color.Orange;
					}
					if (prop < 0.375)
					{ 
						return Color.Yellow;
					}
					if (prop < 0.5)
					{ 
						return Color.Green;
					}
					if (prop < 0.625)
					{ 
						return Color.Cyan;
					}
					if (prop < 0.75)
					{ 
						return Color.Blue;
					}
					if (prop < 0.825)
					{ 
						return Color.Purple;
					}
					return Color.Pink;
				}
				default:
				{
					return Color.Black;
				}
			}
		}
	} 
}
