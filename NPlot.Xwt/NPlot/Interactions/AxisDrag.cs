using System;
using System.Collections;
using System.Drawing;

namespace NPlot
{

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
            private double sensitivity_ = 1.0;

            /// <summary>
            /// MouseDown method for AxisDrag interaction
            /// </summary>
            /// <param name="X">mouse X position</param>
            /// <param name="Y">mouse Y position</param>
            /// <param name="keys">mouse and keyboard modifiers</param>
            /// <param name="ps">the InteractivePlotSurface2D</param>
            public override bool DoMouseDown (int X, int Y, Modifier keys, InteractivePlotSurface2D ps)
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

                            startPoint_ = new Point(X, Y);  // don't combine these - Mono
                            lastPoint_ = startPoint_;       // bug #475205 prior to 2.4

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
            public override bool DoMouseMove (int X, int Y, Modifier keys, InteractivePlotSurface2D ps)
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
            public override bool DoMouseUp ( int X, int Y, Modifier keys, InteractivePlotSurface2D ps )
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
            public double Sensitivity
            {
                get { return sensitivity_; }
                set { sensitivity_ = value; }
            }

        } // AxisDrag (Zoom)
        



}
