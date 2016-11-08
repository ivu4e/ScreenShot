using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace 屏幕截图2005
{
    /// <summary>
    /// 用于记录工具栏运行时样式
    /// </summary>
    public class ToolsRuntimeStyle
    {
        private Color cFillColor;
        private string sFillType;
        private Color cBorderColor;
        private int nBorderWidth;
        private string sBorderStyle;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="fillColor">填充颜色</param>
        /// <param name="fillType">填充类型</param>
        /// <param name="borderColor">边框颜色</param>
        /// <param name="borderWidth">边框宽度</param>
        /// <param name="borderStyle">边框样式</param>
        public ToolsRuntimeStyle(Color fillColor, string fillType, Color borderColor, int borderWidth, string borderStyle)
        {
            this.cFillColor = fillColor;
            this.sFillType = fillType;
            this.cBorderColor = borderColor;
            this.nBorderWidth = borderWidth;
            this.sBorderStyle = borderStyle;
        }

        /// <summary>
        /// 填充颜色
        /// </summary>
        public Color FillColor
        {
            get { return cFillColor; }
            set { cFillColor = value; }
        }

        /// <summary>
        /// 填充类型
        /// </summary>
        public string FillType
        {
            get { return sFillType; }
            set { sFillType = value; }
        }

        /// <summary>
        /// 边框颜色
        /// </summary>
        public Color BorderColor
        {
            get { return cBorderColor; }
            set { cBorderColor = value; }
        }

        /// <summary>
        /// 边框宽度
        /// </summary>
        public int BorderWidth
        {
            get { return nBorderWidth; }
            set { nBorderWidth = value; }
        }

        /// <summary>
        /// 边框样式
        /// </summary>
        public string BorderStyle
        {
            get { return sBorderStyle; }
            set { sBorderStyle = value; }
        }
    }
}
