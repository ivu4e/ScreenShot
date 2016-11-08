using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace 屏幕截图2005
{
    public class EditTextFont
    {
        private string name;
        private float size;
        private bool bold;
        private bool italic;
        private bool underline;
        private bool strikeout;
        private Color color;

        public EditTextFont()
        {
            name = "宋体";
            size = 12f;
            bold = false;
            italic = false;
            underline = false;
            color = Color.Black;
        }

        public Font GetFont()
        {
            Font font = new Font("宋体", 12f, FontStyle.Regular, GraphicsUnit.Pixel);
            try
            {
                FontStyle fStyle = FontStyle.Regular;
                if (this.bold)
                {
                    fStyle |= FontStyle.Bold;
                }
                if (this.italic)
                {
                    fStyle |= FontStyle.Italic;
                }
                if (this.underline)
                {
                    fStyle |= FontStyle.Underline;
                }
                if (this.strikeout)
                {
                    fStyle |= FontStyle.Strikeout;
                }
                font = new Font(this.name, this.size, fStyle, GraphicsUnit.Pixel);
            }
            catch (System.Exception ex)
            {
                System.Console.WriteLine(ex.Message);
            }
            return font;
        }
        /// <summary>
        /// 获取此 System.Drawing.Font 的字体名称。
        /// </summary>
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        /// <summary>
        /// 获取此 System.Drawing.Font 的全身大小，单位采用 System.Drawing.Font.Unit 属性指定的单位。
        /// </summary>
        public float Size
        {
            get { return size; }
            set { size = value; }
        }
        /// <summary>
        /// 获取一个值，该值指示此 System.Drawing.Font 是否为粗体。
        /// 如果此 System.Drawing.Font 为粗体，则为 true；否则为 false。
        /// </summary>
        public bool Bold
        {
            get { return bold; }
            set { bold = value; }
        }
        /// <summary>
        /// 获取一个值，该值指示此 System.Drawing.Font 是否为斜体。
        /// 如果此 System.Drawing.Font 为斜体，则为 true；否则为 false。
        /// </summary>
        public bool Italic
        {
            get { return italic; }
            set { italic = value; }
        }
        /// <summary>
        /// 获取一个值，该值指示此 System.Drawing.Font 是否有下划线。
        /// 如果此  有下划线，则为 true；否则为 false。
        /// </summary>
        public bool Underline
        {
            get { return underline; }
            set { underline = value; }
        }

        /// <summary>
        /// 获取一个值，该值指示此 System.Drawing.Font 是否指定贯穿字体的横线。
        /// 如果有一条横线贯穿此 System.Drawing.Font，则为 true；否则为 false。
        /// </summary>
        public bool Strikeout
        {
            get { return strikeout; }
            set { strikeout = value; }
        }
        /// <summary>
        /// 获取此字体的颜色
        /// </summary>
        public Color Color
        {
            get { return color; }
            set { color = value; }
        }
    }
}
