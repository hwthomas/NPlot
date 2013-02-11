/*
 * NPlot - A charting library for .NET
 * 
 * Swf.AxisTestsForm.cs
 * Copyright (C) 2003-2009 Matt Howlett and others.
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
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

using NPlot;

namespace NPlotDemo
{
	/// <summary>
	/// Summary description for Tests.
	/// </summary>
	public class AxisTestsForm : System.Windows.Forms.Form
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public AxisTestsForm()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//

			this.Paint += new PaintEventHandler(Tests_Paint);

		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support
		/// </summary>
		private void InitializeComponent()
		{
			// 
			// AxisTestsForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(480, 320);
			this.Name = "AxisTestsForm";
			this.Text = "Tests";

		}
		#endregion
		#region Axis Tests
		//
		// The actual Axis tests
		//
		private void Tests_Paint(object sender, PaintEventArgs e)
		{
			System.Drawing.Rectangle boundingBox;
			Graphics g = e.Graphics;
			
			NPlot.LinearAxis a = new LinearAxis(0, 10);

			a.Draw( g, new Point(30,10), new Point(30, 200), out boundingBox );

			a.Reversed = true;
			a.Draw( g, new Point(60,10), new Point(60, 200), out boundingBox );

			a.SmallTickSize = 0;
			a.Draw( g, new Point(90,10), new Point(90, 200), out boundingBox );

			a.LargeTickStep = 2.5;
			a.Draw( g, new Point(120,10), new Point(120,200), out boundingBox );

			a.NumberOfSmallTicks = 5;
			a.SmallTickSize = 2;
			a.Draw( g, new Point(150,10), new Point(150,200), out boundingBox );

			a.AxisColor = Color.Cyan;
			a.Draw( g, new Point(180,10), new Point(180,200), out boundingBox );

			a.TickTextColor= Color.Cyan;
			a.Draw( g, new Point(210,10), new Point(210,200), out boundingBox );

			a.TickTextBrush = Brushes.Black;
			a.AxisPen = Pens.Black;
			a.Draw( g, new Point(240,10), new Point(300,200), out boundingBox );

			a.WorldMax = 100000;
			a.WorldMin = -3;
			a.LargeTickStep = double.NaN;
			a.Draw( g, new Point(330,10), new Point(330,200), out boundingBox );

			a.NumberFormat = "{0:0.0E+0}";
			a.Draw( g, new Point(380,10), new Point(380,200), out boundingBox );
			
			// Test for default TicksAngle on positive X-axis, ie Ticks below X-axis
			NPlot.LinearAxis aX = new LinearAxis(0, 10);
			aX.Draw( g, new Point(30,240), new Point(380, 240), out boundingBox );
			
			// Set TicksAngle to PI/4 anti-clockwise from positive X-axis direction
			aX.TicksAngle = (float)Math.PI / 4.0f;
			aX.Draw( g, new Point(30,280), new Point(380, 280), out boundingBox );
			
		}
		#endregion
	
	}
}
