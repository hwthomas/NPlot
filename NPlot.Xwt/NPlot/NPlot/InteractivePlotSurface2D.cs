//
// XwPlot - A cross-platform charting library using the Xwt toolkit
// 
// InteractivePlotSurface2D.cs
//
// Copyright (C) Hywel Thomas and others.
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
using System.Collections;
using System.ComponentModel;
using System.Data;
using Xwt;
using Xwt.Drawing;

namespace NPlot {
 
	/// <summary>
	/// Interactive PlotSurface2D
	/// </summary> <summary>
	/// Extends PlotSurface2D with Interactions which allow the user
	/// to change the plot using mouse and keyboard inputs.
	/// </summary>
	public class InteractivePlotSurface2D : NPlot.PlotSurface2D
	{
	/// <summary>
		/// Default constructor.
		/// </summary>
		public InteractivePlotSurface2D() : base()
		{
			// Create empty InteractionOccurred and PreRefresh Event handlers
			InteractionOccurred += new InteractionHandler (OnInteractionOccurred);
			PreRefresh += new PreRefreshHandler( OnPreRefresh );
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
			PreRefresh (this);
			Refresh();
		}

		private CursorType plotcursor;	
	
		public CursorType plotCursor
		{
			get { return plotcursor; }
			set { plotcursor = value; }
		}

		#region Axis Cache and Range utilities

		private Axis xAxis1ZoomCache_;		// copies of current axes,
		private Axis yAxis1ZoomCache_;		// saved for restoring the
		private Axis xAxis2ZoomCache_;		// original dimensions after
		private Axis yAxis2ZoomCache_;		// zooming, etc

		/// <summary>
		/// Caches the current axes
		/// </summary>
		public void CacheAxes()
		{
			if (xAxis1ZoomCache_ == null && xAxis2ZoomCache_ == null &&
				yAxis1ZoomCache_ == null && yAxis2ZoomCache_ == null)
			{
				if (XAxis1 != null) {
					xAxis1ZoomCache_ = (Axis)XAxis1.Clone();
				}
				if (XAxis2 != null) {
					xAxis2ZoomCache_ = (Axis)XAxis2.Clone();
				}
				if (YAxis1 != null) {
					yAxis1ZoomCache_ = (Axis)YAxis1.Clone();
				}
				if (YAxis2 != null) {
					yAxis2ZoomCache_ = (Axis)YAxis2.Clone();
				}
			}
		}

		/// <summary>
		/// Sets axes to be those saved in the cache.
		/// </summary>
		public void SetOriginalDimensions()
		{
			if (xAxis1ZoomCache_ != null) {
				XAxis1 = xAxis1ZoomCache_;
				XAxis2 = xAxis2ZoomCache_;
				YAxis1 = yAxis1ZoomCache_;
				YAxis2 = yAxis2ZoomCache_;

				xAxis1ZoomCache_ = null;
				xAxis2ZoomCache_ = null;
				yAxis1ZoomCache_ = null;
				yAxis2ZoomCache_ = null;
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

		#region InteractivePlotSurface Events

		/// An Event is raised to notify clients that an Interaction has modified
		/// the PlotSurface, and a separate Event is also raised prior to a call
		/// to refresh the PlotSurface.	 Currently, the conditions for raising
		/// both Events are the same (ie the PlotSurface has been modified)

		/// <summary>
		/// InteractionOccurred event signature
		/// TODO: expand this to include information about the event. 
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
		}
		
		#endregion

		#region PlotSurface Interaction handlers

		// The methods which are called by the Canvas event handlers and which in turn call
		// the individual Interaction handlers for those events. Note that a reference to the
		// PlotSurface is passed as well as the event details, so that Interactions can call
		// PlotSurface public methods if required (eg to redraw an area of the plotSurface)

		/// <summary>
		/// Handle Draw event for all interactions. Called by platform-specific OnDraw/Paint
		/// </summary>
		protected void DoDraw (Context ctx, Rectangle clip)
		{
			foreach (Interaction i in interactions) {
				i.DoDraw (ctx, clip);
			}
		}

		/// <summary>
		/// Handle MouseEnter for all PlotSurface interactions
		/// </summary>
		/// <returns>true if plot has been modified</returns>
		protected bool DoMouseEnter (EventArgs args)
		{
			bool modified = false;
			foreach (Interaction i in interactions) {
				modified |= i.DoMouseEnter (this);
			}
			ShowCursor (plotCursor);	//set by each Interaction
			if (modified) {
				InteractionOccurred (this);
				ReDraw ();
			}
			return (modified);
		}

		/// <summary>
		/// Handle MouseLeave for all PlotSurface interactions
		/// </summary>
		/// <returns>true if plot has been modified</returns>
		protected bool DoMouseLeave (EventArgs args)
		{
			bool modified = false;
			foreach (Interaction i in interactions) {
				modified |= i.DoMouseLeave (this);
			}
			ShowCursor(plotCursor);
			if (modified) {
				InteractionOccurred (this);
				ReDraw ();
			}
			return (modified);
		}

		/// <summary>
		/// Handle MouseDown for all PlotSurface interactions
		/// </summary>
		/// <param name="X">mouse X position</param>
		/// <param name="Y"> mouse Y position</param>
		/// <param name="keys"> mouse and keyboard modifiers</param>
		/// <returns>true if plot has been modified</returns>
		protected bool DoMouseDown (double X, double Y, Modifier keys)
		{
			bool modified = false;
			foreach (Interaction i in interactions) {
				modified |= i.DoMouseDown (X, Y, keys, this);
			}
			ShowCursor(plotCursor);
			if (modified) {
				InteractionOccurred (this);
				ReDraw ();
			}
			return (modified) ;
		}

		/// <summary>
		/// // Handle MouseUp for all PlotSurface interactions
		/// </summary>
		/// <param name="X">mouse X position</param>
		/// <param name="Y"> mouse Y position</param>
		/// <param name="keys"> mouse and keyboard modifiers</param>
		/// <returns>true if plot has been modified</returns>
		protected bool DoMouseUp (double X, double Y, Modifier keys)
		{
			bool modified = false;
			foreach (Interaction i in interactions) {
				modified |= i.DoMouseUp (X, Y, keys, this);
			}
			ShowCursor (plotCursor);
			if (modified){
				InteractionOccurred (this);
				ReDraw ();
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
		protected bool DoMouseMove (double X, double Y, Modifier keys)
		{
			bool modified = false;
			foreach (Interaction i in interactions) {
				modified |= i.DoMouseMove (X, Y, keys, this);
			}
			ShowCursor (plotCursor);
			if (modified) {
				InteractionOccurred (this);
				ReDraw ();
			}
			return (modified);
		}

		/// <summary>
		/// Handle Mouse Scroll (wheel) for all PlotSurface interactions
		/// </summary>
		/// <param name="X">mouse X position</param>
		/// <param name="Y"> mouse Y position</param>
		/// <param name="direction"> scroll direction</param>
		/// <param name="keys"> mouse and keyboard modifiers</param>
		/// <returns>true if plot has been modified</returns>
		protected bool DoMouseScroll (double X, double Y, int direction, Modifier keys)
		{
			bool modified = false;
			foreach (Interaction i in interactions) {
				modified |= i.DoMouseScroll (X, Y, direction, keys, this);
			}
			ShowCursor(plotCursor);
			if (modified) {
				InteractionOccurred (this);
				ReDraw ();
			}
			return (modified);
		}
	
		/// <summary>
		/// Handle KeyPressed for all PlotSurface interactions
		/// </summary>
		protected bool DoKeyPress (Modifier keys, InteractivePlotSurface2D ps)
		{
			bool modified = false;
			foreach (Interaction i in interactions) {
				modified |= i.DoKeyPress (keys,this);
			}
			if (modified) {
				InteractionOccurred (this);
				ReDraw ();
			}
			return(modified);
		}

		
		/// <summary>
		/// Handle KeyReleased for all PlotSurface interactions
		/// </summary>
		protected bool DoKeyRelease (Modifier keys, InteractivePlotSurface2D ps)
		{
			bool modified = false;
			foreach (Interaction i in interactions) {
				modified |= i.DoKeyRelease (keys,this);
			}
			if (modified) {
				InteractionOccurred (this);
				ReDraw ();
			}
			return (modified);
		}
		#endregion

		#region PlotSurface virtual methods

		/// <summary>
		/// Displays the current plotCursor, set in each interaction
		/// This must be overridden by each implementation so that
		/// the appropriate platform cursor type can be displayed
		/// </summary>
		public virtual void ShowCursor (CursorType plotCursor )
		{
		}

		/// <summary>
		/// Update the entire plot area on the platform-specific output
		/// Override this method for each implementation (Swf, Gtk)
		/// </summary>
		public virtual void Refresh ()
		{
		}

		/// <summary>
		/// Invalidate rectangle specified. The Paint/OnDraw handler will then to redraw the area
		/// </summary>
		public virtual void QueueDraw (Rectangle dirtyRect)
		{
		}

		/// <summary>
		/// Process window updates immediately
		/// </summary>
		public virtual void ProcessUpdates (bool updateChildren)
		{
		}

		#endregion

	} // class InteractivePlotSurface2D
  
} // namespace NPlot
