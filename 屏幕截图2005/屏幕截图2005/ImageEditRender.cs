using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace 屏幕截图2005
{
    /// <summary>
    /// 用于绘制当前选中的编辑项
    /// </summary>
    public static class ImageEditRender
    {
        /// <summary>
        /// 绘制截图编辑选中的画笔
        /// </summary>
        /// <param name="g"></param>
        /// <param name="pen"></param>
        /// <param name="brush"></param>
        public static void DrawSelectedBrush(ref Graphics g, ref ToolsRuntimeStyle currentToolStyle, System.Collections.ArrayList pointList)
        {
            if (Program.debugMode)
            {
                DebugTextBox.DebugText("DrawEditSelected_Brush>this.pointList.Count=" + pointList.Count.ToString());
            }
            if (pointList.Count == 0)
            {
                return;
            }

            g.SmoothingMode = SmoothingMode.AntiAlias;
            SolidBrush sbrush = new SolidBrush(currentToolStyle.BorderColor);
            for (int i = 0; i < pointList.Count; i++)
            {
                System.Drawing.Point point = (Point)pointList[i];
                g.FillEllipse(sbrush, point.X, point.Y, currentToolStyle.BorderWidth, currentToolStyle.BorderWidth);
            }
        }
    }
}
