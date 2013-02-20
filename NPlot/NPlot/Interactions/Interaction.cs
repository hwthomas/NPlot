using System;
using System.Drawing;

namespace NPlot
{
    /// <summary>
    /// Encapsulates a number of separate "Interactions". An interaction comprises a number 
    /// of handlers for mouse and keyboard events that work in a specific way, eg rescaling
    /// the axes, scrolling the PlotSurface, etc. 
    /// </summary>
    /// <remarks>
    /// This is a virtual base class, rather than abstract, since particular Interactions will
    /// only require to override a limited number of the possible default handlers. These do
    /// nothing, and return false so that the plot does not require redrawing.
    /// </remarks>
    public class Interaction
    {
        public Interaction ()
        {
        }

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
        /// Draw Overlay content over the cached background plot
        /// </summary>
        public virtual void DoDraw (Graphics g, Rectangle dirtyRect)
        {
        }

    }

}

