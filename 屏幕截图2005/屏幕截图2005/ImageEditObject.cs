using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace 屏幕截图2005
{
    /// <summary>
    /// 图片编辑对象，用于记录单次编辑的信息
    /// </summary>
    public class ImageEditObject
    {
        public ImageEditMode EditMode;
        public ImageSubEditMode SubEditMode;
        public Rectangle EditRect;
        public Point BeginPoint;
        public Point EndPoint;
        public Pen EditPen;
        public int PenSize;
        public Font TextFont;
        public String Text;
        public bool Selected;
        public SolidBrush BackBrush;

        /// <summary>
        /// 用于记录画笔的轨迹
        /// </summary>
        public System.Drawing.Point[] Points;

        public ImageEditObject(ImageEditMode editMode, Rectangle editRect)
        {
            this.EditMode = editMode;
            this.EditRect = editRect;
            this.Selected = true;
            this.Points = new Point[0];
            this.BackBrush = new SolidBrush(Color.White);
        }

        public void SetBrushPoints(System.Collections.ArrayList pointList)
        {
            if (pointList.Count > 0)
            {
                this.Points = new Point[pointList.Count];
                pointList.CopyTo(this.Points);
            }
        }
    }
}
