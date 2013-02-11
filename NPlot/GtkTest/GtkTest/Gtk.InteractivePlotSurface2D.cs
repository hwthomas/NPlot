/*
 * NPlot - A charting library for .NET
 * 
 * Gtk.PlotSurface2D.cs
 * 
 * Copyright (C) Hywel Thomas, Matt Howlett and others.
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
using System.ComponentModel;
using System.Data;
using System.Drawing;

using Gtk;
using GtkDotNet = Gtk.DotNet;

using NPlot;

namespace NPlot.Gtk {

    /// <summary>
    /// Implements a Gtk#-specific version of NPlot.InteractivePlotSurface2D
    /// remarks>
	public class InteractivePlotSurface2D : NPlot.InteractivePlotSurface2D {

		DrawingArea canvas;		// The local Gtk.DrawingArea
		
		/// <summary>
		/// Exposes the local Gtk.DrawingArea, so that
		/// it can be added to any Gtk Container
		/// </summary>
		[
		Browsable(false),
		Bindable(false)
		]
		public DrawingArea Canvas
		{
			get {return canvas;}
		}

		System.Drawing.Bitmap bitmapCache;	// The drawing cache, and...
		Gdk.Rectangle allocation;			// its allocated size 
		bool allocated = false;

		private bool coordsActive_ = true;	// Shows X,Y in Tooltip

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
			get {return this.coordsActive_;}
			set {this.coordsActive_ = value;}
		}

		
		/// <summary>
		/// Default Constructor
		/// </summary>
		public InteractivePlotSurface2D () : base()
		{
			canvas = new DrawingArea ();		// allocate local DrawingArea
			canvas.CanFocus = true;				// enable to receive the focus
			
			// Link the event handlers into the DrawingArea events (default is none)
			canvas.SizeAllocated	    += new SizeAllocatedHandler		 (SizeAllocated);
			canvas.ExposeEvent		    += new ExposeEventHandler   	 (ExposeEvent);
			canvas.EnterNotifyEvent	    += new EnterNotifyEventHandler   (EnterNotify);
			canvas.LeaveNotifyEvent	    += new LeaveNotifyEventHandler   (LeaveNotify);
			canvas.ButtonPressEvent 	+= new ButtonPressEventHandler   (ButtonPress);
			canvas.MotionNotifyEvent	+= new MotionNotifyEventHandler  (MotionNotify);
			canvas.ButtonReleaseEvent	+= new ButtonReleaseEventHandler (ButtonRelease);
			canvas.ScrollEvent  	 	+= new ScrollEventHandler   	 (ScrollNotify);
			canvas.KeyPressEvent		+= new KeyPressEventHandler		 (KeyPressed);
			canvas.KeyReleaseEvent		+= new KeyReleaseEventHandler	 (KeyReleased);

			// Subscribe to DrawingArea mouse movement and button press events.
			// Enter and Leave notification is necessary to make ToolTips work.
			// Specify PointerMotionHint to prevent being deluged with motion events.
			canvas.AddEvents((int)Gdk.EventMask.EnterNotifyMask);
			canvas.AddEvents((int)Gdk.EventMask.LeaveNotifyMask);
			canvas.AddEvents((int)Gdk.EventMask.ButtonPressMask);
			canvas.AddEvents((int)Gdk.EventMask.ButtonReleaseMask);
			canvas.AddEvents((int)Gdk.EventMask.PointerMotionMask);
			canvas.AddEvents((int)Gdk.EventMask.PointerMotionHintMask);
			canvas.AddEvents((int)Gdk.EventMask.ScrollMask);
			
			canvas.SetSizeRequest (400, 300);		// Set DrawingArea size

			// Set up ToolTips to show coordinates. NB works via Enter/Leave events
			// TODO: ToolTips do not work well yet - needs review of approach
			this.Canvas.TooltipText = "Coordinates will display here";
	
		}
		
        #region Input Event Handlers
		
		// Mouse events which have been enabled for the Gtk.DrawingArea are linked 
		// in to the handlers here.  The mouse coordinates (X,Y)
		// and keys are converted to the platform-neutral ones defined in 
		// NPlot.InteractivePlotSurface2D, and those helper functions are then called.
		// If an Interaction modifies the PlotSurface then it will be redrawn.

		
		// Platform-specific routine to convert from Gtk buttons
		
		private Modifiers MouseInput (Gdk.ModifierType state)
		{
			Modifiers keys = Modifiers.None;
			
			if((state & Gdk.ModifierType.Button1Mask) != 0)	keys |= Modifiers.Button1;
			if((state & Gdk.ModifierType.Button2Mask) != 0)	keys |= Modifiers.Button2;
			if((state & Gdk.ModifierType.Button3Mask) != 0)	keys |= Modifiers.Button3;
			
			keys |= ControlKeys(state);
			
			return keys;
		}

		//
		// Group together here all the event handlers for the specific OS platform (Swf or Gtk#)
		//

		private void EnterNotify (object o, EventArgs args )
		{
			DrawingArea da = (DrawingArea)o;
			if(!da.HasFocus) da.GrabFocus();
			DoMouseEnter(args);
		}

		
		private void LeaveNotify (object o, EventArgs args)
		{
			DoMouseLeave(args);
		}

		
		private void ButtonPress ( object o, ButtonPressEventArgs args )
		{
			int X,Y;
			Gdk.ModifierType state;
			
			args.Event.Window.GetPointer(out X, out Y, out state);
            //    state = args.Event.State;
            //    X = (int)args.Event.X;
            //    Y = (int)args.Event.Y;
			Modifiers keys = MouseInput(state);
			DoMouseDown(X, Y, keys);
		}

		private void ButtonRelease (object o, ButtonReleaseEventArgs args)
		{
			int X,Y;
			Gdk.ModifierType state;

			args.Event.Window.GetPointer(out X, out Y, out state);
			Modifiers keys = MouseInput(state);
			DoMouseUp(X, Y, keys);
		}

		private void MotionNotify (object o, MotionNotifyEventArgs args)
		{
			int X,Y;
			Gdk.ModifierType state;
			
			// Ensure PlotSurface has keyboard focus
			DrawingArea da = (DrawingArea)o;
			if(!da.HasFocus)
			{	
				da.GrabFocus();
			}

			args.Event.Window.GetPointer(out X, out Y, out state);
            //    state = args.Event.State;
            //    X = (int)args.Event.X;
            //    Y = (int)args.Event.Y;
			Modifiers keys = MouseInput(state);

			DoMouseMove (X, Y, keys);

			// Update coordinates Tooltip if active. TODO: Fix DateTime properly!
			bool DateTimeToolTip_ = false;

			if (coordsActive_)
			{
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
					if (!DateTimeToolTip_ ) 
					{
						s = "(" + px.ToString("g4") + "," + py.ToString("g4") + ")"; 
					}
					else 
					{
						DateTime dateTime = new DateTime((long)px);
						s = dateTime.ToShortDateString() + " " + dateTime.ToLongTimeString() + Environment.NewLine + py.ToString("f4");
					}
					this.Canvas.TooltipText = s;
				}
				else {
					this.Canvas.TooltipText = "";
				}
			}
		}
		
		private void ScrollNotify (object o, ScrollEventArgs args)
		{
			int X, Y;
			int direction = -1;
			Gdk.ModifierType state;
	
			args.Event.Window.GetPointer(out X, out Y, out state);
            // state = args.Event.State;
            // X = (int)args.Event.X;
            // Y = (int)args.Event.Y;
			Modifiers keys = MouseInput(state);
			if (args.Event.Direction == Gdk.ScrollDirection.Up) {
				direction = +1;
			}
			DoMouseScroll (X, Y, direction, keys);
		}


		#endregion Input Event Handlers
		
		#region Key Event Handlers
	
		// Platform-specific routines to convert from Gtk keys
	
		private Modifiers ControlKeys (Gdk.ModifierType state)
		{
			Modifiers keys = Modifiers.None;
			if((state & Gdk.ModifierType.ShiftMask) != 0)	keys |= Modifiers.Shift;
			if((state & Gdk.ModifierType.ControlMask) != 0)	keys |= Modifiers.Control;
			if((state & Gdk.ModifierType.Mod1Mask) != 0)	keys |= Modifiers.Alt;
			return keys;
		}
			
		private Modifiers Key(Gdk.Key input)
		{
			switch (input) {
				case Gdk.Key.Home:
				case Gdk.Key.KP_Home:
					return Modifiers.Home;
				case Gdk.Key.KP_Add:
				case Gdk.Key.plus:
					return Modifiers.Plus;
				case Gdk.Key.KP_Subtract:
				case Gdk.Key.minus:
					return Modifiers.Minus;
				case Gdk.Key.KP_Left:
				case Gdk.Key.Left:
					return Modifiers.Left;
				case Gdk.Key.KP_Right:
				case Gdk.Key.Right:
					return Modifiers.Right;
				case Gdk.Key.KP_Up:
				case Gdk.Key.Up:
					return Modifiers.Up;
				case Gdk.Key.KP_Down:
				case Gdk.Key.Down:
					return Modifiers.Down;
				default:
					return Modifiers.None;
			}
		}


		private void KeyPressed(object o, KeyPressEventArgs args )
		{
			Modifiers key = Key(args.Event.Key);
			key |= ControlKeys(args.Event.State);
			
			DoKeyPress(key, this);

			args.RetVal = true;		// Prevents further key processing
		}

		private void KeyReleased(object o, KeyReleaseEventArgs args )
		{
			Modifiers key = Key(args.Event.Key);
			key |= ControlKeys(args.Event.State);
			
			DoKeyRelease(key, this);
			
			args.RetVal = true;
		}


		#endregion
		
		#region Display Handlers and overrides
	
		/// <summary>
		/// Handler for the DrawingArea SizeAllocated Event
		/// </summary>
		private void SizeAllocated (object o, SizeAllocatedArgs args)
		{
			allocated = true;
			allocation = args.Allocation;
			UpdateCache();		// create offscreen Cache and draw plot
		}

		
		/// <summary>
		/// Handles ExposeEvents by obtaining a GC from the Gdk.Window
		/// and copying the PlotSurface from the cache to the screen.
		/// </summary>
		private void ExposeEvent (object o, ExposeEventArgs args)
		{
			Gdk.Window win = args.Event.Window;		// the DrawingArea GdkWindow
			Gdk.Rectangle area = args.Event.Area;	// the Exposed Area
			Rectangle clip = new Rectangle (area.X, area.Y, area.Width, area.Height);
			using (Graphics g = GtkDotNet.Graphics.FromDrawable(win,true))
			{
				g.DrawImage (bitmapCache, clip, clip, GraphicsUnit.Pixel);
			}
		}

		
		/// <summary>
		/// Create a new Bitmap cache, and obtain a System.Drawing.Graphics from this
		/// The base Draw method is called to draw the PlotSurface into the cache
		/// and this is then copied to the visible Gtk.DrawingArea by ExposeEvents
		/// </summary>
		void UpdateCache ()
		{
			if (!allocated) return;
			if (bitmapCache != null) bitmapCache.Dispose ();
			bitmapCache = new System.Drawing.Bitmap(allocation.Width, allocation.Height);
			using(Graphics g = Graphics.FromImage(bitmapCache))
			{
				Rectangle plotArea = new Rectangle(0, 0, allocation.Width, allocation.Height);
				base.Draw (g, plotArea);	
			}
		}
		
		
		//
		// The InteractivePlotSurface2D class defines a number of virtual methods
		// that are required by the common code which deals with Interactions.
		// These virtual methods are overridden here for the Gtk# platform.
		//


		/// <summary>
		/// Override InteractivePlotSurface2D.Refresh
		/// </summary>
		public override void Refresh ()
		{
			// Update the off-screen drawing cache, then QueueDrawArea raises
			// an ExposeEvent to copy the cache to the screen, and this Event
			// is raised immediately by calling ProcessUpdates().
			
			UpdateCache();
			canvas.QueueDrawArea(0, 0, canvas.Allocation.Width, canvas.Allocation.Height);
			if(canvas.IsRealized)
			{
				Gdk.Window win = canvas.GdkWindow;
				win.ProcessUpdates(true);
			}
		}

		
		/// <summary>
		/// Display current Cursor style as set by Interactions
		/// </summary>
		public override void ShowCursor(CursorType plotCursor)
		{
			Gdk.CursorType ct;
			switch (plotCursor)
			{
				case NPlot.InteractivePlotSurface2D.CursorType.CrossHair:
					ct = Gdk.CursorType.Crosshair;			break;
				case NPlot.InteractivePlotSurface2D.CursorType.Hand:
					ct = Gdk.CursorType.Hand2;				break;
				case NPlot.InteractivePlotSurface2D.CursorType.LeftPointer:
					ct = Gdk.CursorType.LeftPtr	;			break;
				case NPlot.InteractivePlotSurface2D.CursorType.LeftRight:
					ct = Gdk.CursorType.SbHDoubleArrow;		break;
				case NPlot.InteractivePlotSurface2D.CursorType.RightPointer:
					ct = Gdk.CursorType.RightPtr;			break;
				case NPlot.InteractivePlotSurface2D.CursorType.UpDown:
					ct = Gdk.CursorType.SbVDoubleArrow;		break;
				case NPlot.InteractivePlotSurface2D.CursorType.Zoom:
					ct = Gdk.CursorType.Fleur;				break;
				default:
					ct = Gdk.CursorType.LeftPtr;			break;
			}
			this.Canvas.GdkWindow.Cursor = new Gdk.Cursor(ct);
		}

	
		/// <summary>
		/// Draw an Overlay line on the platform-specific display
		/// using the two Points specified for start and end points.
		/// Overrides InteractivePlotSurface2D.DrawOverlayLine for Swf
		/// </summary>
		public override void DrawOverlayLine (Point start, Point end, Color color, bool visible)
		{
			//
			// Overlay drawing is done directly onto the PlotSurface2D.Canvas, and is
			// hidden when required by restoring the original from the offscreen cache.
			// Drawing is by System.Drawing.Graphics classes, but requires a Graphics 
			// context from the Canvas (which is platform-dependent) (but see note below)
			//
	
			DrawingArea canvas = this.Canvas;
			if(visible)
			{
				using( Graphics g = GtkDotNet.Graphics.FromDrawable(canvas.GdkWindow) )
				{
					Pen xPen = new Pen(color);
					g.DrawLine(xPen, start, end );
					xPen.Dispose();
				}
			}
			else	// Restore plot from cache
			{
				int h = Math.Abs(end.Y - start.Y) + 1;
				int w = Math.Abs(end.X - start.X) + 1;
				canvas.QueueDrawArea(start.X, start.Y, w, h);
				if(canvas.GdkWindow != null)
				{
					canvas.GdkWindow.ProcessUpdates(true);
				}
			}
		}

			
 		/// <summary>
		/// Draw an Overlay rectangle on the platform-specific display
		/// using the two Points specified for topLeft and bottomRight corners
		/// Overrides InteractivePlotSurface2D.DrawOverlayRectangle for Swf
		/// </summary>
		public override void DrawOverlayRectangle (Point topLeft, Point bottomRight, Color color, bool visible)
		{
			DrawingArea canvas = this.Canvas;	// the Gtk plotting control
			
			// ensure topLeft < bottomRight
 			Point tl = topLeft, br = bottomRight;
 			tl.X = Math.Min (topLeft.X, bottomRight.X);
 			tl.Y = Math.Min (topLeft.Y, bottomRight.Y);
 			br.X = Math.Max (topLeft.X, bottomRight.X);
 			br.Y = Math.Max (topLeft.Y, bottomRight.Y);
 			
 			// Draw (or erase) rectangle as 4 lines
 
			if(visible)
			{
				using( Graphics g = GtkDotNet.Graphics.FromDrawable(canvas.GdkWindow,true) )
				{
					Pen pen = new Pen(color);
					g.DrawLine(pen, tl.X, tl.Y, br.X, tl.Y);
					g.DrawLine(pen, br.X, tl.Y, br.X, br.Y);
					g.DrawLine(pen, tl.X, br.Y, br.X, br.Y);
					g.DrawLine(pen, tl.X, br.Y, tl.X, tl.Y);
					pen.Dispose();
				}
				
			}
			else
			{
				int w = br.X - tl.X + 1;
				int h = br.Y - tl.Y + 1;
				
				canvas.QueueDrawArea(tl.X, tl.Y, w, 1);
				canvas.QueueDrawArea(br.X, tl.Y, 1, h);
				canvas.QueueDrawArea(tl.X, br.Y, w, 1);
				canvas.QueueDrawArea(tl.X, tl.Y, 1, h);

				if(canvas.GdkWindow != null)
				{
					canvas.GdkWindow.ProcessUpdates(true);
				}
			}
		}
 			
		
		

		#endregion
		
	} // class PlotSurface2D
  
} // namespace NPlot.Gtk
