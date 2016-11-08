using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace 屏幕截图2005
{
    /// <summary>
    /// 用于计算箭头的Width和Height
    /// </summary>
    class ArrowheadHelper
    {
        public static float PenWidth = 2.0F;

        public static float HypotenuseLength = 1.0F;

        public static float ArrowCapWidth = 4.0F;

        public static float ArrowCapHeight = 5.0F;
        /// <summary>
        /// 计算箭头的Width和Height
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="penWidth"></param>
        public static void ComputeHypotenuse(Point start, Point end, float penWidth)
        {
            int absX = Math.Abs(start.X - end.X);
            int absY = Math.Abs(start.Y - end.Y);

            float len = (float)Math.Sqrt(Math.Pow(absX, 2) + Math.Pow(absY, 2)) / 2;

            if (len > 50)
            {
                PenWidth = penWidth;
            }
            else
            {
                PenWidth = penWidth * (len * 2 / 100);
            }
            HypotenuseLength = len - PenWidth * 2 - 1;
            ArrowCapWidth = PenWidth * 2;
            ArrowCapHeight = ArrowCapWidth + 1;
        }
    }
}
