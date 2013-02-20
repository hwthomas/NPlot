using System;

namespace NPlot
{

    public class KeyActions : Interaction
    {
        /// <summary>
        /// Links some of the standard keyboard keys to plot scrolling and zooming.
        /// Since all key-interactions are applied to the complete PlotSurface, any
        /// translation or zooming is applied to all axes that have been defined
        /// 
        /// The following key actions are currently implemented :-
        /// Left    - scrolls the viewport to the left
        /// Right   - scrolls the viewport to the right
        /// Up      - scrolls the viewport up
        /// Down    - scrolls the viewport down
        /// +       - zooms in
        /// -       - zooms out
        /// Alt     - reduces the effect of the above actions
        /// Home    - restores original view and dimensions
        /// More could be added, but these are a start.
        /// </summary>
        /// 

        const double right = +0.25, left  = -0.25;
        const double up = +0.25, down = -0.25;
        const double altFactor = 0.4;   // Alt key reduces effect
        const double zoomIn  = -0.5;    // Should give reversible
        const double zoomOut = +1.0;    // ZoomIn / ZoomOut actions
        const double symmetrical = 0.5;

        private float sensitivity_ = 1.0f;  // default value

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
        public override bool DoKeyPress(Modifier keys, InteractivePlotSurface2D ps)
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
                ps.CacheAxes();
                ps.TranslateXAxes (left*factor);
                return true;
            }
            if ((keys & Modifier.Right) != 0) {
                ps.CacheAxes();
                ps.TranslateXAxes (right*factor);
                return true;
            }
            if ((keys & Modifier.Up) != 0) {
                ps.CacheAxes();
                ps.TranslateYAxes (up*factor);
                return true;
            }
            if ((keys & Modifier.Down) != 0) {
                ps.CacheAxes();
                ps.TranslateYAxes (down*factor);
                return true;
            }
            if ((keys & Modifier.Plus) != 0) {
                ps.CacheAxes();
                ps.ZoomXAxes (zoomIn*factor,symmetrical);
                ps.ZoomYAxes (zoomIn*factor,symmetrical);
                return true;
            }
            if ((keys & Modifier.Minus) != 0) {
                ps.CacheAxes();
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

}