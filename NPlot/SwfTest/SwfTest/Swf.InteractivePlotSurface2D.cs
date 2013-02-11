/*
 * NPlot - A charting library for .NET
 * 
 * Swf.InteractivePlotSurface2D.cs
 * 
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
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

using NPlot;

namespace NPlot.Swf {

    /// <summary>
    /// Implements an SWF-specific version of NPlot.InteractivePlotSurface2D
    /// remarks>
	public class InteractivePlotSurface2D : NPlot.InteractivePlotSurface2D 
	{

    #region plotControl class

        /// <summary>
        /// SWF Control with appropriate options set up
        /// <summary>
        public class plotControl : System.Windows.Forms.Control
        {
            // The Style options we need are protected members of the Swf Control class,
            // so this class is defined solely  for setting up the correct Style, etc.
            // It is then cast back to a Control for exposing as the drawing Canvas.
            public plotControl()
            {
                this.Visible = true;
                this.Enabled = true;
    
                base.DoubleBuffered = true;
                base.SetStyle (ControlStyles.AllPaintingInWmPaint, true);
                base.SetStyle (ControlStyles.UserPaint, true);
                base.ResizeRedraw = true;
            }
        
            // Windows intercepts a number of standard keys which are
            // needed for plot actions unless they are allowed through
            // by overriding the following method and returning 'true'
            protected override bool IsInputKey(Keys keyData)
            {
                if (keyData == Keys.Left)   return true;
                if (keyData == Keys.Right)  return true;
                if (keyData == Keys.Up)     return true;
                if (keyData == Keys.Down)   return true;
                return base.IsInputKey(keyData);
            }
        
            // The following method provides a hook for any optimisation of the
            // background redrawing, but at present the base method (which just 
            // fills the entire control area with the background colour) is used
            protected override void OnPaintBackground (PaintEventArgs pe )
            {
                base.OnPaintBackground(pe);
            }
        
        }

    #endregion

		plotControl canvas ;	// The local Swf.Control
		
		System.Drawing.Bitmap bitmapCache;	// The Off-screen plot area, and...
		Rectangle allocation;				// its current allocation. 
		bool allocated = false;
		
		/// <summary>
		/// Exposes the local Swf.Control, so that it can be added to any Swf Container
		/// </summary>
		[
		Browsable(false),
		Bindable(false)
		]
		public Control Canvas
		{
			get {return (Control)canvas;}
		}

		
		private System.ComponentModel.IContainer components_;
		private System.Windows.Forms.ToolTip coordinates_;

		/// <summary>
		/// Flag to display a coordinates in a tooltip.
		/// </summary>
		[ 
		Category("PlotSurface2D"),
		Description("Whether or not to show coordinates in a tool tip when the mouse hovers above the plot area."),
		Browsable(true),
		Bindable(true)
		]
		public bool ShowCoordinates
		{
			get {return this.coordinates_.Active;}
			set {this.coordinates_.Active = value;}
		}

		
		/// <summary>
		/// Default Constructor
		/// </summary>
		public InteractivePlotSurface2D () : base()
		{
			// Set up control
			canvas = new plotControl();

			// Now link the event handlers into the plotControl events
			canvas.SizeChanged	+= new EventHandler			(SizeChanged);
			canvas.Paint		+= new PaintEventHandler	(OnPaint);
			canvas.MouseEnter	+= new EventHandler			(MouseEnter);
			canvas.MouseLeave	+= new EventHandler			(MouseLeave);
			canvas.MouseDown	+= new MouseEventHandler	(ButtonPress);
			canvas.MouseMove	+= new MouseEventHandler	(MotionNotify);
			canvas.MouseUp		+= new MouseEventHandler	(ButtonRelease);
			canvas.MouseWheel	+= new MouseEventHandler	(ScrollNotify);
			canvas.KeyDown 		+= new KeyEventHandler		(KeyDown);
			canvas.KeyUp		+= new KeyEventHandler		(KeyUp);
			
			canvas.BackColor = System.Drawing.SystemColors.ControlLightLight;
			canvas.Size = new System.Drawing.Size (328, 272);	// initial size

			this.components_ = new System.ComponentModel.Container();
			this.coordinates_ = new System.Windows.Forms.ToolTip(this.components_);
			
			coordinates_.SetToolTip (this.Canvas, "Coordinates will display here");
	
		}

		#region Input Event Handlers
		
		// Mouse events which have been enabled for the Swf.plotControl are linked 
		// in to the platform-specific handlers here.  The mouse coordinates (X,Y)
		// and keys are converted to the platform-neutral ones defined in 
		// NPlot.ActivePlotSurface2D, and those handlers are then called.
		
		// Platform-specific routine to convert from Swf buttons
		
		private Modifiers MouseInput (MouseEventArgs args)
		{
			Modifiers keys = Modifiers.None;

			if (args.Button == MouseButtons.Left)	keys |= Modifiers.Button1;
			if (args.Button == MouseButtons.Middle)	keys |= Modifiers.Button2;
			if (args.Button == MouseButtons.Right)	keys |= Modifiers.Button3;

			// Get Control, Alt, etc from last KeyDown
			keys |= ControlKeys(lastKeyEventArgs);
			
			return keys;
			
		}
		
		//
		// Group together here the event handlers for the specific OS platform (Swf or Gtk#)
		//

		private void MouseEnter(object o, EventArgs args)
		{
			DoMouseEnter(args);
		}
		

		private void MouseLeave(object o, EventArgs args)
		{
			DoMouseLeave(args);
		}

		
		private void ButtonPress(object o, MouseEventArgs args)
		{
			int X = args.X;
			int Y = args.Y;
			
			Modifiers keys = MouseInput(args);
			DoMouseDown(X, Y, keys);
		}

		
		private void ButtonRelease(object o, MouseEventArgs args)
		{
			int X = args.X;
			int Y = args.Y;
			
			Modifiers keys = MouseInput(args);
			DoMouseUp(X, Y, keys);
		}

		
		private void MotionNotify(object o, MouseEventArgs args)
		{
			int X = args.X;
			int Y = args.Y;
			
			Control c = (System.Windows.Forms.Control)o;
			if(!c.Focused)
			{
				c.Focus();	// Set Focus to receive MouseWheel events
			}

			Modifiers keys = MouseInput(args);
			DoMouseMove(X, Y, keys);
			
			// Update coordinates Tooltip if active. TODO: Fix DateTime properly!
			bool DateTimeToolTip_ = false;

			if (coordinates_.Active) {
				Point p = new Point( X, Y );
				if (this.PlotAreaBoundingBoxCache.Contains(p)) 
				{
					if (this.PhysicalXAxis1Cache == null) 
						return;
					if (this.PhysicalYAxis1Cache == null)
						return;

					double px = this.PhysicalXAxis1Cache.PhysicalToWorld( p, true );
					double py = this.PhysicalYAxis1Cache.PhysicalToWorld( p, true );
					string s = "";
					if (!DateTimeToolTip_)
					{
						s = "(" + px.ToString("g4") + "," + py.ToString("g4") + ")"; 
					}
					else 
					{
						DateTime dateTime = new DateTime((long)px);
						s = dateTime.ToShortDateString() + " " + dateTime.ToLongTimeString() + Environment.NewLine + py.ToString("f4");
					}
					coordinates_.SetToolTip( this.Canvas, s );
				}
				else {
					coordinates_.ShowAlways = false;
				}
			}
		}

	
		private void ScrollNotify(object o, MouseEventArgs args)
		{
			int X = args.X;
			int Y = args.Y;
			int direction = -1;
			
			if(args.Delta > 0)
			{
				direction = +1;
			}
			
			Modifiers keys = MouseInput(args);
			DoMouseScroll(X, Y, direction, keys);
		}
		
		#endregion Input Event Handlers
	
		#region Key Event Handlers
	
		// Platform-specific routines to convert from Swf keys
			
		private Modifiers ControlKeys (KeyEventArgs args)
		{
			Modifiers keys = Modifiers.None;
			if (args == null) 	return keys;
			if (args.Control)	keys |= Modifiers.Control;
			if (args.Alt)		keys |= Modifiers.Alt;
			if (args.Shift)		keys |= Modifiers.Shift;
			return keys;
		}
		
		private Modifiers Key(System.Windows.Forms.Keys input)
		{
			switch (input) {
				case Keys.Home:
					return Modifiers.Home;
				case Keys.Add:
					return Modifiers.Plus;
				case Keys.Subtract:
					return Modifiers.Minus;
				case Keys.Left:
					return Modifiers.Left;
				case Keys.Right:
					return Modifiers.Right;
				case Keys.Up:
					return Modifiers.Up;
				case Keys.Down:
					return Modifiers.Down;
				default:
					return Modifiers.None;
			}
		}

		
		private KeyEventArgs lastKeyEventArgs;
		
		private void KeyDown(object o, KeyEventArgs args )
		{
			lastKeyEventArgs = args;
			
			Modifiers key = Key(args.KeyCode);
			key |= ControlKeys(args);
			DoKeyPress(key, this);
		}

		private void KeyUp(object o, KeyEventArgs args )
		{
			lastKeyEventArgs = args;
			
			Modifiers keys = Key(args.KeyCode);
			keys |= ControlKeys(args);
			DoKeyRelease(keys, this);
		}


		#endregion
		
		#region Display Handlers and overrides

		/// <summary>
		/// Handler for the plotControl SizeChanged Event
		/// </summary>
		private void SizeChanged(object o, EventArgs args)
		{
			Control ctl = (Control)o;
			allocated = true;
			allocation = ctl.Bounds;
			UpdateCache ();	// create offscreen Cache and draw plot
		}

		
		/// <summary>
		/// Handles PaintEvents by obtaining a GC from the PaintEventArgs
		/// and copying	the PlotSurface from the cache to the screen
		/// </summary>
		private void OnPaint (object o, PaintEventArgs args)
		{
			Rectangle clip = args.ClipRectangle;	// the Exposed Area
			Graphics g = args.Graphics;
			if (bitmapCache != null) {
				g.DrawImage(bitmapCache, clip, clip, GraphicsUnit.Pixel);
			}
		}

		
		/// <summary>
		/// Create a new Bitmap cache, and obtain a System.Drawing.Graphics from this
		/// The base Draw method is called to draw the PlotSurface into the cache
		/// and this is then copied to the visible plotControl by Paint Events
		/// </summary>
		void UpdateCache ()
		{
			if (!allocated) return;
			if (bitmapCache != null)
                bitmapCache.Dispose ();
			
			bitmapCache = new System.Drawing.Bitmap(allocation.Width, allocation.Height);
			using (Graphics g = Graphics.FromImage(bitmapCache)) {
				Rectangle plotArea = new Rectangle (0, 0, allocation.Width, allocation.Height);
				base.Draw (g, plotArea);	
			}
		}


		// The InteractivePlotSurface2D class defines a number of virtual methods
		// that are required by the common code which deals with Interactions.
		// These virtual methods are overridden here for the Swf platform.

		/// <summary>
		/// Override ActivePlotSurface2D.Refresh
		/// </summary>
		public override void Refresh ()
		{
			// Update the off-screen drawing cache, then Invalidate () raises a
			// PaintEvent to copy the cache to the screen.  Update forces an
			// immediate PaintEvent, to ensure that rescaling, scrolling, etc
			// are responsive to the mouse movements as they occur
			
			UpdateCache();
			canvas.Invalidate ();
			canvas.Update();
		}

			
		/// <summary>
		/// Display current Cursor style as set by Interactions
		/// </summary>
		public override void ShowCursor(CursorType plotCursor)
		{
			System.Windows.Forms.Cursor ct;
			switch (plotCursor)
			{
				case NPlot.InteractivePlotSurface2D.CursorType.CrossHair:
					ct = Cursors.Cross;				break;
				case NPlot.InteractivePlotSurface2D.CursorType.Hand:
					ct = Cursors.Hand;				break;
				case NPlot.InteractivePlotSurface2D.CursorType.LeftPointer:
					ct = Cursors.Arrow;				break;
				case NPlot.InteractivePlotSurface2D.CursorType.LeftRight:
					ct = Cursors.SizeWE;			break;
				case NPlot.InteractivePlotSurface2D.CursorType.RightPointer:
					ct = Cursors.Arrow;				break;
				case NPlot.InteractivePlotSurface2D.CursorType.UpDown:
					ct = Cursors.SizeNS;			break;
				case NPlot.InteractivePlotSurface2D.CursorType.Zoom:
					ct = Cursors.SizeAll;			break;
				default:
					ct = Cursors.Arrow;					break;
			}
			this.Canvas.Cursor = ct;
		}
		
	
		/// <summary>
		/// Draw an Overlay line on the platform-specific display
		/// using the two Points specified for start and end points.
		/// Overrides ActivePlotSurface2D.DrawOverlayLine for Swf
		/// </summary>
		public override void DrawOverlayLine (Point start, Point end, Color color, bool visible)
		{
			//
			// Overlay drawing is done directly onto the PlotSurface2D.Canvas, and is
			// hidden when required by restoring the original from the offscreen cache.
			// Drawing is by System.Drawing.Graphics classes, but requires a Graphics 
			// context from the Canvas (which is platform-dependent)
			//
	
			Control canvas = this.Canvas;
			if(visible)
			{
				using( Graphics g = canvas.CreateGraphics() )
				{
					Pen pen = new Pen(color);
					g.DrawLine(pen, start, end );
					pen.Dispose();
				}
			}
			else
			{
				int h = Math.Abs(end.Y - start.Y) + 1;
				int w = Math.Abs(end.X - start.X) + 1;
				Rectangle rect = new Rectangle(start.X, start.Y, w, h);
				canvas.Invalidate(rect);
				canvas.Update();
			}
		}

	
		/// <summary>
		/// Draw an Overlay rectangle on the platform-specific display
		/// using the two Points specified for topLeft and bottomRight corners
		/// Overrides ActivePlotSurface2D.DrawOverlayRectangle for Swf
		/// </summary>
		public override void DrawOverlayRectangle (Point topLeft, Point bottomRight, Color color, bool visible)
		{
			Control canvas = this.Canvas;	// the Swf plotting control
			
			// ensure topLeft < bottomRight
 			Point tl = topLeft, br = bottomRight;
 			tl.X = Math.Min (topLeft.X, bottomRight.X);
 			tl.Y = Math.Min (topLeft.Y, bottomRight.Y);
 			br.X = Math.Max (topLeft.X, bottomRight.X);
 			br.Y = Math.Max (topLeft.Y, bottomRight.Y);
 			
 			// Draw (or erase) rectangle as 4 lines

 			if (visible) {
				using( Graphics g = canvas.CreateGraphics() ) {
					Color penColor = Color.FromArgb(0x80, color);
					Pen pen = new Pen(penColor);
					g.DrawLine(pen, tl.X, tl.Y, br.X, tl.Y);
					g.DrawLine(pen, br.X, tl.Y, br.X, br.Y);
					g.DrawLine(pen, tl.X, br.Y, br.X, br.Y);
					g.DrawLine(pen, tl.X, br.Y, tl.X, tl.Y);
					pen.Dispose();
				}
			}
			else {
				int w = br.X - tl.X + 1;
				int h = br.Y - tl.Y + 1;

				canvas.Invalidate( new Rectangle(tl.X, tl.Y, w, 1) );
				canvas.Invalidate( new Rectangle(br.X, tl.Y, 1, h) );
				canvas.Invalidate( new Rectangle(tl.X, br.Y, w, 1) );
				canvas.Invalidate( new Rectangle(tl.X, tl.Y, 1, h) );

				canvas.Update();
			}
		}
		#endregion
	
	} // class Swf.InteractivePlotSurface2D

} // namespace NPlot.Swf
