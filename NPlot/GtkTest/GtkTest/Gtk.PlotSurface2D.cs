//
// NPlot - A charting library for .NET
// 
// Gtk.PlotSurface2D.cs
//
// Copyright (C) Hywel Thomas 2009-2013
// Matt Howlett and others 2003-2009
// All rights reserved.
// 
// Redistribution and use in source and binary forms, with or without modification,
// are permitted provided that the following conditions are met:
// 
// 1. Redistributions of source code must retain the above copyright notice, this
//    list of conditions and the following disclaimer.
// 2. Redistributions in binary form must reproduce the above copyright notice,
//    this list of conditions and the following disclaimer in the documentation
//    and/or other materials provided with the distribution.
// 3. Neither the name of NPlot nor the names of its contributors may
//    be used to endorse or promote products derived from this software without
//    specific prior written permission.
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
///

using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;

namespace NPlot.Gtk {
 
    ///<summary>
    /// Extends PlotSurface2D with Interactions which change the plot using mouse and keyboard inputs.
    /// </summary>
   	public class PlotSurface2D : NPlot.PlotSurface2D
	{
    	/// <summary>
		/// Default constructor.
		/// </summary>
		public PlotSurface2D() : base()
		{
			// Create empty InteractionOccurred and PreRefresh Event handlers
	        this.InteractionOccurred += new InteractionHandler( OnInteractionOccurred );
   		    this.PreRefresh += new PreRefreshHandler( OnPreRefresh );
		}


		/// <summary>
		/// Clear the plot and reset to default values.
		/// </summary>
		public new void Clear()
		{
			ClearAxisCache();
			interactions.Clear();
            base.Clear();
		}

		/// <summary>
		/// Raise the PreRefresh event, then let the platform-specific
		/// Refresh methods redraw the plot and copy it to the screen
		/// </summary>
		public void ReDraw()
		{
			this.PreRefresh (this);
			Refresh();
		}


        #region Axis Cache and Axis Range Modification

		private Axis xAxis1ZoomCache_;		// copies of current axes,
		private Axis yAxis1ZoomCache_;		// saved for restoring the
		private Axis xAxis2ZoomCache_;		// original dimensions after
		private Axis yAxis2ZoomCache_;		// zooming, etc

		/// <summary>
		/// Saves the current PlotSurface Axes for restoring at a later stage
		/// </summary>
		public void CacheAxes()
		{
			if (xAxis1ZoomCache_ == null && xAxis2ZoomCache_ == null &&
				yAxis1ZoomCache_ == null && yAxis2ZoomCache_ == null)
			{
				if (this.XAxis1 != null) {
					xAxis1ZoomCache_ = (Axis)this.XAxis1.Clone();
				}
				if (this.XAxis2 != null) {
					xAxis2ZoomCache_ = (Axis)this.XAxis2.Clone();
				}
				if (this.YAxis1 != null) {
					yAxis1ZoomCache_ = (Axis)this.YAxis1.Clone();
				}
				if (this.YAxis2 != null) {
					yAxis2ZoomCache_ = (Axis)this.YAxis2.Clone();
				}
			}
		}

		protected void ClearAxisCache()
		{
			xAxis1ZoomCache_ = null;
			yAxis1ZoomCache_ = null;
			xAxis2ZoomCache_ = null;
			yAxis2ZoomCache_ = null;
		}
		
		/// <summary>
		/// Resets all Axes to those saved in the cache
		/// </summary>
		public void SetOriginalDimensions()
		{
			if (xAxis1ZoomCache_ != null) {
                this.XAxis1 = xAxis1ZoomCache_;
                this.XAxis2 = xAxis2ZoomCache_;
                this.YAxis1 = yAxis1ZoomCache_;
                this.YAxis2 = yAxis2ZoomCache_;

                xAxis1ZoomCache_ = null;
                xAxis2ZoomCache_ = null;
                yAxis1ZoomCache_ = null;
                yAxis2ZoomCache_ = null;
            }					
		}
		
		/// <summary>
		/// Translate all PlotSurface X-Axes by shiftProportion
		/// </summary>
		public void TranslateXAxes (double shiftProportion )
		{
            if (XAxis1 != null) {
                XAxis1.TranslateRange (shiftProportion);
            }
            if (XAxis2 != null) {
                XAxis2.TranslateRange (shiftProportion);
            }
		}
		
        /// <summary>
        /// Translate all PlotSurface Y-Axes by shiftProportion
        /// </summary>
        public void TranslateYAxes (double shiftProportion )
        {
            if (YAxis1 != null) {
                YAxis1.TranslateRange (shiftProportion);
            }
            if (YAxis2 != null) {
                YAxis2.TranslateRange (shiftProportion);
            }
        }
        
		/// <summary>
		/// Zoom all PlotSurface X-Axes about focusPoint by zoomProportion 
		/// </summary>
		public void ZoomXAxes (double zoomProportion, double focusRatio)
		{
			if (XAxis1 != null) {
				XAxis1.IncreaseRange(zoomProportion,focusRatio);
			}
            if (XAxis2 != null) {
                XAxis2.IncreaseRange(zoomProportion,focusRatio);
            }
		}

        /// <summary>
        /// Zoom all PlotSurface Y-Axes about focusPoint by zoomProportion 
        /// </summary>
        public void ZoomYAxes (double zoomProportion, double focusRatio)
        {
            if (YAxis1 != null) {
                YAxis1.IncreaseRange(zoomProportion,focusRatio);
            }
            if (YAxis2 != null) {
                YAxis2.IncreaseRange(zoomProportion,focusRatio);
            }
        }

		/// <summary>
		/// Define all PlotSurface X-Axes to minProportion, maxProportion
		/// </summary>
		public void DefineXAxes (double minProportion, double maxProportion)
		{
			if (XAxis1 != null) {
				XAxis1.DefineRange(minProportion, maxProportion, true);
			}
            if (XAxis2 != null) {
                XAxis2.DefineRange(minProportion, maxProportion, true);
            }
		}

        /// <summary>
        /// Define all PlotSurface Y-Axes to minProportion, maxProportion
        /// </summary>
        public void DefineYAxes (double minProportion, double maxProportion)
        {
            if (YAxis1 != null) {
                YAxis1.DefineRange(minProportion, maxProportion, true);
            }
            if (YAxis2 != null) {
                YAxis2.DefineRange(minProportion, maxProportion, true);
            }
        }
		#endregion

		#region Add/Remove Interaction

		private ArrayList interactions = new ArrayList();

		/// <summary>
		/// Adds a specific interaction to the PlotSurface2D
		/// </summary>
		/// <param name="i">the interaction to add.</param>
		public void AddInteraction(Interaction i)
		{
			interactions.Add(i);
		}

		/// <summary>
		/// Remove a previously added interaction
		/// </summary>
		/// <param name="i">interaction to remove</param>
		public void RemoveInteraction(Interaction i)             
		{
			interactions.Remove(i);
		}

		#endregion Add/Remove Interaction

		#region Interaction Events
		
		/// <summary>
		/// An Event is raised to notify clients that an Interaction has modified
		/// the PlotSurface, and a separate Event is also raised prior to a call
		/// to refresh the PlotSurface.  Currently, the conditions for raising
		/// both Events are the same (ie the PlotSurface has been modified)
		/// </summary>
 		
		/// <summary>
        /// InteractionOccurred event signature
        /// </summary>
        /// <param name="sender"></param>
        public delegate void InteractionHandler(object sender);
        
        /// <summary>
        /// Event raised when an interaction modifies the PlotSurface
        /// </summary>
        public event InteractionHandler InteractionOccurred;

        /// <summary>
        /// Default handler called when Interaction modifies PlotSurface
        /// Override this, or add handler to InteractionOccurred event.
        /// </summary>
        /// <param name="sender"></param>
        protected void OnInteractionOccurred(object sender)
        {
            // default - do nothing
        }
        
        /// <summary>
        /// PreRefresh event handler signature
        /// </summary>
        /// <param name="sender"></param>
        public delegate void PreRefreshHandler(object sender);

        /// <summary>
        /// Event raised prior to Refresh call
        /// </summary>
        public event PreRefreshHandler PreRefresh;

        /// <summary>
        /// Default handler for PreRefresh
        /// Override this, or add handler to PreRefresh event.
        /// </summary>
        /// <param name="sender"></param>
        protected void OnPreRefresh(object sender)
        {
            // default - do nothing.
        }
        
		#endregion

		#region Mouse methods
		
		// The platform-specific methods which handle all the Interaction calls.
		// A reference to the PlotSurface is passed as well as the mouse EventButton details,
        // so that the PlotSurface2D can be updated
		// If any Interactions require the PlotSurface to be redrawn, an 
		// InteractionOccurred event is raised prior to the Refresh.
	
		
		/// <summary>
		/// Common set of button/key flags that can be set from either Windows or Gtk events.
        /// This allows common code to be used for all Mouse and Key Interaction Handlers.
		/// </summary>
		[System.Flags]
		public enum Modifier
		{
			None	= 0x00000,	// no keys
			Shift	= 0x00001,	// the Shift key
			Control	= 0x00002,	// the Control key
			Alt		= 0x00004,	// the Alt key
			Button1	= 0x00008,	// the first (left) mouse button
			Button2	= 0x00010,	// the second (middle) mouse button
			Button3	= 0x00020, 	// the third (right) mouse button
			Spare1	= 0x00040,
			Home	= 0x00080,	// a restricted set of keyboard keys
			End		= 0x00100,	// that NPlot will respond to
			Left	= 0x00200,
			Up		= 0x00400,
			Right	= 0x00800,
			Down	= 0x01000,
			PageUp	= 0x02000,
			PageDn	= 0x04000,
			Plus	= 0x08000,
			Minus	= 0x10000
		}

	
		/// <summary>
		/// Handle MouseEnter for all PlotSurface interactions
		/// </summary>
		/// <returns>true if plot has been modified</returns>
		protected bool DoMouseEnter(EventArgs args)
		{
			bool modified = false;
			foreach (Interaction i in interactions) {
				modified |= i.DoMouseEnter(this);
			}
			ShowCursor(this.plotCursor);	//set by each Interaction
			if (modified) {
				InteractionOccurred (this);
				ReDraw ();
			}
			return(modified);
		}


		/// <summary>
		/// Handle MouseLeave for all PlotSurface interactions
		/// </summary>
		/// <returns>true if plot has been modified</returns>
		protected bool DoMouseLeave(EventArgs args)
		{
			bool modified = false;
			foreach (Interaction i in interactions) {
				modified |= i.DoMouseLeave(this);
			}
			ShowCursor(this.plotCursor);
			if (modified){
				InteractionOccurred (this);
				ReDraw ();
			}
			return(modified);
		}

		/// <summary>
		/// Handle MouseDown for all PlotSurface interactions
		/// </summary>
		/// <param name="X">mouse X position</param>
		/// <param name="Y"> mouse Y position</param>
		/// <param name="keys"> mouse and keyboard modifiers</param>
		/// <returns>true if plot has been modified</returns>
		protected bool DoMouseDown(int X, int Y, Modifier keys)
		{
			bool modified = false;
			foreach (Interaction i in interactions) {
				modified |= i.DoMouseDown (X, Y, keys, this);
			}
			ShowCursor(this.plotCursor);
			if (modified){
				InteractionOccurred (this);
				ReDraw();
			}
			return(modified) ;
		}

		/// <summary>
		/// // Handle MouseUp for all PlotSurface interactions
		/// </summary>
		/// <param name="X">mouse X position</param>
		/// <param name="Y"> mouse Y position</param>
		/// <param name="keys"> mouse and keyboard modifiers</param>
		/// <returns>true if plot has been modified</returns>
		protected bool DoMouseUp(int X, int Y, Modifier keys)
		{
			bool modified = false;
			foreach (Interaction i in interactions) {
				modified |= i.DoMouseUp (X, Y, keys, this);
			}
			ShowCursor(this.plotCursor);
			if (modified){
				InteractionOccurred (this);
				ReDraw();
			}
			return(modified);
		}

		/// <summary>
		/// Handle MouseMove for all PlotSurface interactions
		/// </summary>
		/// <param name="X">mouse X position</param>
		/// <param name="Y"> mouse Y position</param>
		/// <param name="keys"> mouse and keyboard modifiers</param>
		/// <returns>true if plot has been modified</returns>
		protected bool DoMouseMove(int X, int Y, Modifier keys)
		{
			bool modified = false;
			foreach (Interaction i in interactions) {
				modified |= i.DoMouseMove( X, Y, keys, this);
			}
			ShowCursor(this.plotCursor);
			if (modified) {
				InteractionOccurred(this);
				ReDraw();
			}
			return(modified);
		}

		/// <summary>
		/// Handle Mouse Scroll (wheel) for all PlotSurface interactions
		/// </summary>
		/// <param name="X">mouse X position</param>
		/// <param name="Y"> mouse Y position</param>
		/// <param name="direction"> scroll direction</param>
		/// <param name="keys"> mouse and keyboard modifiers</param>
		/// <returns>true if plot has been modified</returns>
		protected bool DoMouseScroll(int X, int Y, int direction, Modifier keys)
		{
			bool modified = false;
			foreach (Interaction i in interactions) {
				modified |= i.DoMouseScroll ( X, Y, direction, keys, this );
			}
			ShowCursor(this.plotCursor);
			if (modified) {
				InteractionOccurred(this);
				ReDraw();
			}
			return(modified);
		}

        /// <summary>
        /// Handle Draw event for all interactions
        /// </summary>
        protected virtual void DoDraw (Graphics g, Rectangle clip)
        {
            foreach (Interaction i in interactions) {
                i.DoDraw (g, clip);
            }
        }

		#endregion Mouse methods
		
		#region Keyboard methods
	
		// The platform-neutral methods which are called by the Swf and Gtk
		// event handlers, and which in turn handle all the Interaction calls

		/// <summary>
		/// Handle KeyPressed for all PlotSurface interactions
		/// </summary>
		protected bool DoKeyPress(Modifier keys, InteractivePlotSurface2D ps)
		{
			bool modified = false;
			foreach (Interaction i in interactions) {
				modified |= i.DoKeyPress(keys,this);
			}
			if (modified) {
				InteractionOccurred(this);
				ReDraw();
			}
			return(modified);
		}

		
		/// <summary>
		/// Handle KeyReleased for all PlotSurface interactions
		/// </summary>
		protected bool DoKeyRelease(Modifier keys, InteractivePlotSurface2D ps)
		{
			bool modified = false;
			foreach (Interaction i in interactions) {
				modified |= i.DoKeyRelease(keys,this);
			}
			if (modified) {
				InteractionOccurred(this);
				ReDraw();
			}
			return(modified);
		}

		
		#endregion
		
		#region Output methods
				
		/// <summary>
		/// Platform-neutral CursorTypes that are useful in Interactions
		/// </summary>
		public enum CursorType
		{
			None,			// no cursor displayed
			LeftPointer,	// standard mouse pointer
			RightPointer,	// same but pointing right
			CrossHair,		// for accurate coordinates
			Hand,			// use for dragging in plot
			LeftRight,		// expanding horizontal axis
			UpDown,			// expanding vertical axis
			Zoom			// expanding both axes
		}

		private CursorType plotcursor;	
	
		public CursorType plotCursor
		{
			get { return plotcursor; }
			set { plotcursor = value; }
		}
		
		/// <summary>
		/// Displays the current plotCursor, set in each interaction
		/// This should be overridden by each implementation so that
		/// the appropriate platform cursor type can be displayed
		/// </summary>
		/// <param name="plotCursor"></param>
		public virtual void ShowCursor (CursorType plotCursor )
		{
		}
		
		
		/// <summary>
		/// Update the entire plot area on the platform-specific output
		/// Override this method for each implementation (Swf, Gtk)
		/// </summary>
		public virtual void Refresh()
		{
		}
		

		/// <summary>
		/// Draw an Overlay line on the platform-specific display
		/// using the two Points specified for start and end points.
		/// Override this method for each implementation (Swf, Gtk)
		/// If [visible] is false, line is removed from the display
		/// </summary>
		public virtual void DrawOverlayLine (Point start, Point end, Color color, bool visible)
		{
		}

		
		/// <summary>
		/// Draw an Overlay rectangle on the platform-specific output
		/// using the two Points specified for topLeft and bottomRight corners
		/// Override this method for each implementation (Swf, Gtk)
		/// If [visible] is false, rectangle is removed from the display
		/// </summary>
		public virtual void DrawOverlayRectangle (Point topLeft, Point bottomRight, Color color, bool visible)
		{
		}

        /// <summary>
        /// Invalidate rectangle specified, and allow the Paint/OnDraw handler to redraw the region
        /// </summary>
        public virtual void QueueDraw (Rectangle dirtyRect)
        {
        }
	
		#endregion

		#region Interactions Base class
		
		/// <summary>
		/// Encapsulates a number of separate "Interactions". An interaction is basically 
		/// a set of handlers for mouse and keyboard events that work in a specific way, eg
		/// rescaling the axes, scrolling the PlotSurface, etc. 
        /// </summary>
 		public class Interaction
		{
			/// Base class for all interactions. All methods are virtual (not abstract)
            /// since not all interactions need to use all methods. All methods return true
            /// if PlotSurface needs to be redrawn as a result of the Interaction. The Draw
            /// method is called from the PlotSurface DoDraw handler so that the Interaction
            /// can add any Overlay content such as Rubber-band lines, etc.

			public virtual bool DoMouseEnter (InteractivePlotSurface2D ps)
			{
				return false;
			}

			public virtual bool DoMouseLeave (InteractivePlotSurface2D ps)
			{
				return false;
			}

			public virtual bool DoMouseDown (int X, int Y, Modifier keys, InteractivePlotSurface2D ps)
			{
				return false;
			}
                
			public virtual bool DoMouseUp (int X, int Y, Modifier keys, InteractivePlotSurface2D ps)
			{
				return false;
			}
                
			public virtual bool DoMouseMove (int X, int Y, Modifier keys, InteractivePlotSurface2D ps)
			{
				return false;
			}
			
    		public virtual bool DoMouseScroll (int X, int Y, int direction, Modifier keys, InteractivePlotSurface2D ps)
			{
				return false;
			}
			
			public virtual bool DoKeyPress (Modifier keys, InteractivePlotSurface2D ps)
			{
				return false;
			}

			public virtual bool DoKeyRelease (Modifier keys, InteractivePlotSurface2D ps)
			{
				return false;
			}

            /// <summary>
            /// Called from the OnDraw/Paint routine so that the interaction can add any
            /// Overlay content (eg rubber-band lines, etc) over the cached background.
            /// </summary>
            /// <returns>
            public virtual void DoDraw (Graphics g, Rectangle dirtyRect)
            {
            }
              
		} // Interaction Base class

		#endregion Interactions Base class

		#region Specific Interactions
		
		/// <summary>
		/// The following Interactions are typical samples of what can be built up using
		/// mouse and keyboard inputs.  The PlotDrag(Horizontal/Vertical) Interaction
		/// allows the plot area to be moved in either/both dimensions by clicking
		/// and dragging, as well as Zooming in either/both axes with Ctrl + Drag.
		/// AxisDrag allows one of the axes to be selected with the mouse Left-Button,
		/// then rescaled by dragging in either direction.  PlotZoom uses the mouse
		/// Scroll-Wheel to achieve zooming in or out of the plot: if the mouse is
		/// positioned outside the plot area then zooming is symmetrical into the centre,
		/// while a mouse position within the plot zooms into this as a focus point.
		/// </summary>
		/// 
	
		public class Guidelines : Interaction
		{
			/// <summary>
			/// Horizontal and Vertical guidelines
			/// </summary>
			
			private bool vertical_ = true;
			private bool horizontal_ = true;
			private int xPos_ = -1;
			private int yPos_ = -1;
			private Point xStart_, xEnd_;
			private Point yStart_, yEnd_;
			private Color lineColor_;
			
			/// <summary>
			/// Constructor
			/// </summary>
			/// <param name="horizontal">enable horizontal guideline</param>
			/// <param name="vertical">enable vertical guideline/param>
			public Guidelines (bool horizontal, bool vertical)
			{
				lineColor_ = Color.White;
				Vertical = vertical;
				Horizontal = horizontal;
			}
				
			/// <summary>
			/// LineColor property
			/// </summary>
			public Color LineColor
			{
				get { return lineColor_; }
                set { lineColor_ = value; }
			}

			/// <summary>
			/// Horizontal guideline property enable/disable
			/// </summary>
			public bool Horizontal
			{
				get { return horizontal_; }
                set { horizontal_ = value; }
			}

			/// <summary>
			/// Vertical guideline enable/disable
			/// </summary>
			public bool Vertical
			{
				get { return vertical_; }
                set { vertical_ = value; }
			}

			
			/// <summary>
			/// MouseMove method for Guidelines
			/// </summary>
			/// <param name="X">mouse X position</param>
			/// <param name="Y"> mouse Y position</param>
			/// <param name="keys"> mouse and keyboard modifiers</param>
			/// <param name="ps">the InteractivePlotSurface2D</param>
			public override bool DoMouseMove (int X, int Y, Modifier keys, InteractivePlotSurface2D ps)
			{
				Rectangle plotArea = ps.PlotAreaBoundingBoxCache;
                Rectangle lineExtent = Rectangle.Empty;

				if (vertical_) {
					if (xPos_ != -1) {
						// set extents to erase current vertical guideline
                        //lineExtent = new Rectangle (yStart_.X, yStart_.Y, 1, (int)Math.Abs (yEnd_.Y - yStart_.Y));
						ps.DrawOverlayLine(yStart_, yEnd_, lineColor_, false);
					}
					
					// Only display guideline when mouse is within the plotArea
					if (plotArea.Contains(X,Y)) {
						xPos_ = X;
						yStart_ = new Point(X, plotArea.Top);
						yEnd_ = new Point(X, plotArea.Bottom);
						ps.DrawOverlayLine(yStart_, yEnd_, lineColor_, true);
					} 
					else {
						xPos_ = -1;
					}

                    //if (xPos_ != -1) {
                    //    // Invalidate line extents, then DoDraw will request a new line to be drawn
                    //    ps.QueueDraw (lineExtent);
                    //}

				}
				if (horizontal_) {
					if (yPos_ != -1) {
                        // set extents to erase current vertical guideline
                        //lineExtent = new Rectangle (xStart_.X, xStart_.Y, (int)Math.Abs (xEnd_.X - xStart_.X), 1);
						ps.DrawOverlayLine(xStart_, xEnd_, lineColor_, false);
					}
					// Only display guideline when mouse is within the plotArea
					if (plotArea.Contains(X,Y)) {
						yPos_ = Y;
						xStart_ = new Point(plotArea.Left,Y);
						xEnd_ = new Point(plotArea.Right,Y);
						ps.DrawOverlayLine(xStart_, xEnd_, lineColor_, true);
					}
					else {
						yPos_ = -1;	
					}

                    //if (yPos_ != -1) {
                    //    // Invalidate line extents, then DoDraw will request a new line to be drawn
                    //    ps.QueueDraw (lineExtent);
                    //}
				}
				return false;
			}
			

			/// <summary>
			/// MouseLeave method for Guidelines
			/// </summary>
			/// <param name="ps">the InteractivePlotSurface2D</param>
			public override bool DoMouseLeave (InteractivePlotSurface2D ps)
			{
                Rectangle lineExtent = Rectangle.Empty;

				if (vertical_) {
					if (xPos_ != -1) {
						// set extents to erase current vertical guideline
                        lineExtent = new Rectangle (yStart_.X, yStart_.Y, 1, (int)Math.Abs (yEnd_.Y - yStart_.Y));
						ps.DrawOverlayLine(yStart_, yEnd_, lineColor_, false);
					}
					xPos_ = -1;
				}
				if (horizontal_) {
					if (yPos_ != -1) {
                        // set extents to erase current vertical guideline
                        lineExtent = new Rectangle (xStart_.X, xStart_.Y, (int)Math.Abs (xEnd_.X - xStart_.X), 1);
						ps.DrawOverlayLine(xStart_, xEnd_, lineColor_, false);
					}
					yPos_ = -1;
				}
				return false;
			}


            public override void DoDraw (Graphics g, Rectangle dirtyRect)
            {
            }

		} // Guidelines
 

        public class RubberBandSelection : Interaction
        {
        			
        	/// <summary>
        	/// Selects an area of the plot to become the new Plot Range
        	/// </summary>

			private Point unset_ = new Point(-1, -1);		// any unset point
 			private Point startPoint_ = new Point(-1, -1);
			private Point endPoint_ = new Point(-1, -1);
			
 			private bool selectionActive_ = false;
 
 			
			/// <summary>
            /// Mouse Down method for RubberBand selection
			/// </summary>
			/// <param name="X">mouse X position</param>
			/// <param name="Y"> mouse Y position</param>
			/// <param name="keys"> mouse and keyboard modifiers</param>
			/// <param name="ps">the InteractivePlotSurface2D</param>
			public override bool DoMouseDown(int X, int Y, Modifier keys, InteractivePlotSurface2D ps)
			{
				// Only start selection if mouse is inside plot area (excluding axes)
				if (ps.PlotAreaBoundingBoxCache.Contains(X,Y)) {
					selectionActive_ = true;
					startPoint_.X = X;
					startPoint_.Y = Y;
				
					// invalidate end point
					endPoint_ = unset_;
				}
				return false;
			}

			/// <summary>
			/// MouseMove method for RubberBand selection
			/// </summary>
			/// <param name="X">mouse X position</param>
			/// <param name="Y"> mouse Y position</param>
			/// <param name="keys"> mouse and keyboard modifiers</param>
			/// <param name="ps">the InteractivePlotSurface2D</param>
			public override bool DoMouseMove(int X, int Y, Modifier keys, InteractivePlotSurface2D ps)
			{
				if (((keys & Modifier.Button1) != 0) && selectionActive_)
				{
					// clip X and Y to PlotArea
					Rectangle bounds = ps.PlotAreaBoundingBoxCache;
					
					X = Math.Max(X, bounds.Left);
					X = Math.Min(X, bounds.Right);
					Y = Math.Max(Y, bounds.Top);
					Y = Math.Min(Y, bounds.Bottom);
					
					Point here = new Point(X,Y);
					
					// delete previous overlay rectangle
					if (endPoint_ != unset_) {
						ps.DrawOverlayRectangle(startPoint_, endPoint_, Color.White, false);
					}
					endPoint_ = here;
					
					// draw the latest rectangle
					ps.DrawOverlayRectangle(startPoint_, endPoint_, Color.White, true);
				}
				return false;
			}
			
			/// <summary>
			/// MouseUp method for RubberBand selection
			/// </summary>
			/// <param name="X">mouse X position</param>
			/// <param name="Y"> mouse Y position</param>
			/// <param name="keys"> mouse and keyboard modifiers</param>
			/// <param name="ps">the InteractivePlotSurface2D</param>
			public override bool DoMouseUp(int X, int Y, Modifier keys, InteractivePlotSurface2D ps)
			{
				bool modified = false;
				
				if (selectionActive_) {
					// erase current (clipped) rectangle
					ps.DrawOverlayRectangle(startPoint_, endPoint_, Color.White, false);

					endPoint_.X = X;
					endPoint_.Y = Y;
	
					Rectangle bounds = ps.PlotAreaBoundingBoxCache;
					
					if (!bounds.Contains(endPoint_)) {
						// MouseUp outside plotArea - cancel selection
						modified = false;
					}
					else {
						ps.CacheAxes();
						
						// Redefine range based on selection. The proportions for
						// Min and Max do not require Min < Max, since they will
						// be re-ordered by Axis.DefineRange if necessary
						
						double xMin = startPoint_.X - bounds.Left;
						double yMin = bounds.Bottom - startPoint_.Y;
					
						double xMax = endPoint_.X - bounds.Left;
						double yMax = bounds.Bottom - endPoint_.Y;
					
						double xMinProp = xMin/bounds.Width;
                        double xMaxProp = xMax/bounds.Width;

                        double yMinProp = yMin/bounds.Height;
						double yMaxProp = yMax/bounds.Height;
					
						ps.DefineAxis(ps.XAxis1, xMinProp, xMaxProp);
                        ps.DefineAxis(ps.XAxis2, xMinProp, xMaxProp);
                        ps.DefineAxis(ps.YAxis1, yMinProp, yMaxProp);
                        ps.DefineAxis(ps.YAxis2, yMinProp, yMaxProp);
						
						modified = true;
					}
					selectionActive_ = false;
					startPoint_ = unset_;
					endPoint_ = unset_;
				}
				return modified;
			}
			

			/// <summary>
			/// MouseLeave method for RubberBand selection
			/// </summary>
			/// <param name="ps">the InteractivePlotSurface2D</param>
			public override bool DoMouseLeave(InteractivePlotSurface2D ps)
			{
				if (selectionActive_) {
					selectionActive_ = false;
					if (endPoint_ != unset_) {
						// erase latest rectangle
						ps.DrawOverlayRectangle(startPoint_, endPoint_, Color.White, false);
					}
					startPoint_ = unset_;
					endPoint_ = unset_;
				}
				return false;
			}
			
        } // Rubber Band Selection
		
		public class PlotDrag : Interaction
		{
			/// <summary>
			/// Allows Plot to be dragged without rescaling in both X and Y
			/// </summary>

			private bool vertical_ = true;
			private bool horizontal_ = true;
			private bool dragInitiated_ = false;
			private Point lastPoint_ = new Point(-1, -1);
			private Point unset_ = new Point(-1, -1);
			private double focusX = 0.5, focusY = 0.5;
			private float sensitivity_ = 2.0f;

			/// <summary>
			/// Constructor
			/// </summary>
			/// <param name="horizontal">enable horizontal guideline</param>
			/// <param name="vertical">enable vertical guideline/param>
			public PlotDrag(bool horizontal, bool vertical)
			{
				Vertical = vertical;
				Horizontal = horizontal;
			}

			/// <summary>
			/// Horizontal Drag enable/disable
			/// </summary>
			public bool Horizontal
			{
				get { return horizontal_; }
                set { horizontal_ = value; }
			}

			/// <summary>
			/// Vertical Drag enable/disable
			/// </summary>
			public bool Vertical
			{
				get { return vertical_; }
                set { vertical_ = value; }
			}

	
			/// <summary>
			/// MouseDown method for PlotDrag interaction
			/// </summary>
			/// <param name="X">mouse X position</param>
			/// <param name="Y"> mouse Y position</param>
			/// <param name="keys"> mouse and keyboard modifiers</param>
			/// <param name="ps">the InteractivePlotSurface2D</param>
			public override bool DoMouseDown(int X, int Y, Modifier keys, InteractivePlotSurface2D ps)
			{
				// Only start drag if mouse is inside plot area (excluding axes)
				Rectangle area = ps.PlotAreaBoundingBoxCache;
				if (area.Contains(X,Y)) {
					dragInitiated_ = true;
					lastPoint_ = new Point(X,Y);
					if (((keys & Modifier.Button1) != 0)) {        // Drag
						if (horizontal_ || vertical_) {
							ps.plotCursor = CursorType.Hand;
						}
						if (((keys & Modifier.Control) != 0)) {    // Zoom
							if (horizontal_)
                                ps.plotCursor = CursorType.LeftRight;
							if (vertical_)
                                ps.plotCursor = CursorType.UpDown;
							if (horizontal_ && vertical_)
                                ps.plotCursor = CursorType.Zoom;
						}
					}
					// evaluate focusPoint about which axis is expanded
					focusX = (double)(X - area.Left)/(double)area.Width;
					focusY = (double)(area.Bottom - Y)/(double)area.Height;
				}
				return false;
			}


			/// <summary>
			/// MouseMove method for PlotDrag interaction
			/// </summary>
			/// <param name="X">mouse X position</param>
			/// <param name="Y"> mouse Y position</param>
			/// <param name="keys"> mouse and keyboard modifiers</param>
			/// <param name="ps">the InteractivePlotSurface2D</param>
			public override bool DoMouseMove(int X, int Y, Modifier keys, InteractivePlotSurface2D ps)
			{
				Rectangle area = ps.PlotAreaBoundingBoxCache;
				
				// Mouse Left-Button gives Plot Drag, Ctrl.Left-Button Zooms
				if (((keys & Modifier.Button1) != 0) && dragInitiated_) {
					ps.CacheAxes();
					
					double dX = X - lastPoint_.X;		// distance mouse has moved
					double dY = Y - lastPoint_.Y;
					lastPoint_ = new Point(X, Y);
	
					if ((keys & Modifier.Control) != 0) {
						// Axis re-ranging required
						double factor = Sensitivity;
						if ((keys & Modifier.Alt) != 0) {
						 factor *= 0.25;	// arbitrary change	
						}
						double xProportion = +dX*factor/area.Width;
						double yProportion = -dY*factor/area.Height;
						
						if (horizontal_) {
                            ps.ZoomAxis(ps.XAxis1, xProportion, focusX);
							ps.ZoomAxis(ps.XAxis2, xProportion, focusX);
						}
						if (vertical_) {
                            ps.ZoomAxis(ps.YAxis1, yProportion, focusY);
                            ps.ZoomAxis(ps.YAxis2, yProportion, focusY);
						}
					}
					else {
						// Axis translation required
						double xShift = -dX / area.Width;
						double yShift = +dY / area.Height;
						
						if (horizontal_) {
                            ps.TranslateAxis(ps.XAxis1, xShift);
							ps.TranslateAxis(ps.XAxis2, xShift);
						}
						if (vertical_) {
                            ps.TranslateAxis(ps.YAxis1, yShift);
							ps.TranslateAxis(ps.YAxis2, yShift);
						}
					}
					return true;
				}
				return false;
			}


			/// <summary>
			/// MouseUp method for PlotDrag interaction
			/// </summary>
			/// <param name="X">mouse X position</param>
			/// <param name="Y"> mouse Y position</param>
			/// <param name="keys"> mouse and keyboard modifiers</param>
			/// <param name="ps">the InteractivePlotSurface2D</param>
			public override bool DoMouseUp(int X, int Y, Modifier keys, InteractivePlotSurface2D ps)
			{
				if (dragInitiated_) {
					lastPoint_ = unset_;
					dragInitiated_ = false;
					ps.plotCursor = CursorType.LeftPointer;
				}
				return false;
			}
			
			/// <summary>
			/// Sensitivity factor for axis scaling
			/// </summary>
			/// <value></value>
			public float Sensitivity
			{
				get { return sensitivity_; }
                set { sensitivity_ = value; }
			}

			
		} // PlotDrag/Zoom
	
		public class AxisDrag : Interaction
		{
			/// <summary>
			/// Dragging the axes increases or decreases the axes scaling factors
			/// with the expansion being about the point where the drag is started
			/// Only the axis in which the mouse is clicked will be modified
			/// </summary>

			private Axis axis_ = null;
			private bool dragging_ = false;
			private PhysicalAxis physicalAxis_ = null;
			private Point lastPoint_  = new Point();
			private Point startPoint_ = new Point();
			private double focusRatio_ = 0.5;
			private float sensitivity_ = 1.0f;

			/// <summary>
			/// MouseDown method for AxisDrag interaction
			/// </summary>
			/// <param name="X">mouse X position</param>
			/// <param name="Y">mouse Y position</param>
			/// <param name="keys">mouse and keyboard modifiers</param>
			/// <param name="ps">the InteractivePlotSurface2D</param>
			public override bool DoMouseDown(int X, int Y, Modifier keys, InteractivePlotSurface2D ps)
			{
				// if the mouse is inside the plot area [the tick marks may be here,
				// and are counted as part of the axis], then don't invoke drag. 
				if (ps.PlotAreaBoundingBoxCache.Contains(X,Y)) {
					return false;
				}
				
				if ((keys & Modifier.Button1) != 0) {
					// see if hit with axis. NB Only one axis object will be returned
					ArrayList objects = ps.HitTest(new Point(X, Y));

					foreach (object o in objects) {
						if (o is NPlot.Axis) {
							dragging_ = true;
							axis_ = (Axis)o;

							if (ps.PhysicalXAxis1Cache.Axis == axis_) {
								physicalAxis_ = ps.PhysicalXAxis1Cache;
								ps.plotCursor = CursorType.LeftRight;
							}
							else if (ps.PhysicalXAxis2Cache.Axis == axis_) {
								physicalAxis_ = ps.PhysicalXAxis2Cache;
								ps.plotCursor = CursorType.LeftRight;
							}
							else if (ps.PhysicalYAxis1Cache.Axis == axis_) {
								physicalAxis_ = ps.PhysicalYAxis1Cache;
								ps.plotCursor = CursorType.UpDown;
							}
							else if (ps.PhysicalYAxis2Cache.Axis == axis_) {
								physicalAxis_ = ps.PhysicalYAxis2Cache;
								ps.plotCursor = CursorType.UpDown;
							}

							startPoint_ = new Point(X, Y);	// don't combine these - Mono
							lastPoint_ = startPoint_;		// bug #475205 prior to 2.4

							// evaluate focusRatio about which axis is expanded
							float  x = startPoint_.X - physicalAxis_.PhysicalMin.X;
							float  y = startPoint_.Y - physicalAxis_.PhysicalMin.Y;
							double r = Math.Sqrt(x*x + y*y);
							focusRatio_ = r/physicalAxis_.PhysicalLength;
	
							return false;
						}
					}
				}
				return false;
			}
			

			/// <summary>
			/// MouseMove method for AxisDrag interaction
			/// </summary>
			/// <param name="X">mouse X position</param>
			/// <param name="Y"> mouse Y position</param>
			/// <param name="keys"> mouse and keyboard modifiers</param>
			/// <param name="ps">the InteractivePlotSurface2D</param>
			public override bool DoMouseMove(int X, int Y, Modifier keys, InteractivePlotSurface2D ps)
			{
				if (((keys & Modifier.Button1) != 0) && dragging_&& physicalAxis_ != null ) {
					ps.CacheAxes();

					float dX = (X - lastPoint_.X);
					float dY = (Y - lastPoint_.Y);
					lastPoint_ = new Point(X, Y);
					
					// In case the physical axis is not horizontal/vertical, combine dX and dY
					// in a way which preserves their sign and intuitive axis zoom sense, ie
					// because the physical origin is top-left, expand with +ve dX, but -ve dY 
					double distance = dX - dY;
					double proportion = distance*sensitivity_ /physicalAxis_.PhysicalLength;
										
					axis_.IncreaseRange(proportion, focusRatio_);
				
					return true;
				}
				return false;
			}


			/// <summary>
			/// MouseUp method for AxisDrag interaction
			/// </summary>
			/// <param name="X">mouse X position</param>
			/// <param name="Y"> mouse Y position</param>
			/// <param name="keys"> mouse and keyboard modifiers</param>
			/// <param name="ps">the InteractivePlotSurface2D</param>
			public override bool DoMouseUp( int X, int Y, Modifier keys, InteractivePlotSurface2D ps )
			{
				if (dragging_) {
					dragging_ = false;
					axis_ = null;
					physicalAxis_ = null;
					lastPoint_ = new Point();
					ps.plotCursor = CursorType.LeftPointer;
				}
				return false;
			}

	
			/// <summary>
			/// Sensitivity factor for axis scaling
			/// </summary>
			/// <value></value>
			public float Sensitivity
			{
				get { return sensitivity_; }
                set { sensitivity_ = value; }
			}

		} // AxisDrag (Zoom)
		
		public class PlotZoom : Interaction
		{
			/// <summary>
			/// Mouse Scroll (wheel) increases or decreases both axes scaling factors
			/// Zoom direction is Up/+ve/ZoomIn or Down/-ve/ZoomOut.  If the mouse
			/// pointer is inside the plot area, its position is used as the focus point
			/// of the zoom, otherwise the centre of the plot is used as the default
			/// </summary>

			private float sensitivity_ = 1.0f;	// default value
   
			/// <summary>
			/// Mouse Scroll (wheel) method for AxisZoom interaction
			/// </summary>
			/// <param name="X">mouse X position</param>
			/// <param name="Y"> mouse Y position</param>
			/// <param name="direction">wheel direction +/-</param>
			/// <param name="keys">mouse and keyboard modifiers</param>
			/// <param name="ps">the InteractivePlotSurface2D</param>
			public override bool DoMouseScroll (int X, int Y, int direction, Modifier keys, Gtk.PlotSurface2D ps)
			{
				double proportion = 0.1*sensitivity_;	// use initial zoom of 10%
				double focusX = 0.5, focusY = 0.5;		// default focus point
				
				// Zoom direction is +1 for Up/ZoomIn, or -1 for Down/ZoomOut
				proportion *= -direction;
				
				Rectangle area = ps.PlotAreaBoundingBoxCache;
				if (area.Contains(X,Y)) {
					focusX = (double)(X - area.Left)/(double)area.Width;
					focusY = (double)(area.Bottom - Y)/(double)area.Height;
				}
				// Zoom in/out for all defined axes
				
				ps.CacheAxes ();
				
                ps.ZoomAxis (ps.XAxis1, proportion,focusX);
                ps.ZoomAxis (ps.XAxis2, proportion,focusX);
                ps.ZoomAxis (ps.YAxis1, proportion,focusY);
				ps.ZoomAxis (ps.YAxis2, proportion,focusY);
				
				return (true);
			}
	
			/// <summary>
			/// Sensitivity factor for axis zoom
			/// </summary>
			/// <value></value>
			public float Sensitivity
			{
				get { return sensitivity_; }
                set { sensitivity_ = value; }
			}

		} // Mouse Wheel Zoom
	
		public class KeyActions : Interaction
		{
			/// <summary>
			/// Links some of the standard keyboard keys to plot scrolling and zooming.
			/// Since all key-interactions are applied to the complete PlotSurface, any
			/// translation or zooming is applied to all axes that have been defined
			/// 
			/// The following key actions are currently implemented :-
			/// Left 	- scrolls the viewport to the left
			/// Right	- scrolls the viewport to the right
			/// Up		- scrolls the viewport up
			/// Down	- scrolls the viewport down
			/// +		- zooms in
			/// -		- zooms out
			/// Alt		- reduces the effect of the above actions
			/// Home	- restores original view and dimensions
			/// More could be added, but these are a start.
			/// </summary>
			/// 
			
			const double right = +0.25, left  = -0.25;
			const double up = +0.25, down = -0.25;
			const double altFactor = 0.4;	// Alt key reduces effect
			const double zoomIn  = -0.5;	// Should give reversible
			const double zoomOut = +1.0;	// ZoomIn / ZoomOut actions
			const double symmetrical = 0.5;
	

			private float sensitivity_ = 1.0f;	// default value
   
			/// <summary>
			/// Sensitivity factor for axis zoom
			/// </summary>
			/// <value></value>
			public float Sensitivity
			{
				get { return sensitivity_; }
                set { sensitivity_ = value; }
			}
	
				
			/// <summary>
			/// Handler for KeyPress events
			/// </summary>
			/// <param name="key">the NPlot key enumeration</param>
			/// <param name="ps">the InteractivePlotSurface2D</param>
			/// <returns></returns>
			public override bool DoKeyPress(Modifier keys, Gtk.PlotSurface2D ps)
			{
				double factor = Sensitivity;
				if (((keys & Modifier.Alt) != 0)) {
					factor *= altFactor;
				}
				if ((keys & Modifier.Home) != 0) {
					ps.SetOriginalDimensions();
					return true;
				}
				if ((keys & Modifier.Left) != 0) {
					ps.CacheAxes ();
                    ps.TranslateXAxes (left*factor);
					return true;
				}
				if ((keys & Modifier.Right) != 0) {
					ps.CacheAxes ();
                    ps.TranslateXAxes (right*factor);
					return true;
				}
				if ((keys & Modifier.Up) != 0) {
					ps.CacheAxes ();
                    ps.TranslateYAxes (up*factor);
					return true;
				}
				if ((keys & Modifier.Down) != 0) {
					ps.CacheAxes ();
                    ps.TranslateYAxes (down*factor);
					return true;
				}
				if ((keys & Modifier.Plus) != 0) {
					ps.CacheAxes ();
                    ps.ZoomXAxes (zoomIn*factor,symmetrical);
					ps.ZoomYAxes (zoomIn*factor,symmetrical);
					return true;
				}
				if ((keys & Modifier.Minus) != 0) {
					ps.CacheAxes ();
                    ps.ZoomXAxes (zoomOut*factor,symmetrical);
					ps.ZoomYAxes (zoomOut*factor,symmetrical);
					return true;
				}
				return false;
			}
			
			
			/// <summary>
			/// Handler for KeyRelease events
			/// </summary>
			/// <param name="key">the NPlot key enumeration</param>
			/// <param name="ps">the InteractivePlotSurface2D</param>
			/// <returns></returns>
			public override bool DoKeyRelease(Modifier keys, InteractivePlotSurface2D ps)
			{
				return false;
			}


		} // Key Actions

		#endregion	Specific Interactions
 	
	} // class InteractivePlotSurface2D
  
} // namespace NPlot
