using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace 屏幕截图2005
{
    class EditTextBox : TextBox
    {
        public EditTextBox()
        {
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            this.BackgroundImage = null;
            this.BackColor = Color.Transparent;
        }

        public override System.Drawing.Color BackColor
        {
            get
            {
                return base.BackColor;
            }
            set
            {
                base.BackColor = value;
            }
        }

        public override System.Drawing.Image BackgroundImage
        {
            get
            {
                return base.BackgroundImage;
            }
            set
            {
                base.BackgroundImage = value;
            }
        }

        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
            //base.OnPaintBackground(pevent);
            pevent.Graphics.FillRectangle(new SolidBrush(Color.Blue), pevent.ClipRectangle);
            MessageBox.Show("ok");
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            //base.OnPaint(e);
            e.Graphics.FillRectangle(new SolidBrush(Color.Blue), e.ClipRectangle);
            MessageBox.Show("ok");
        }

        protected override void OnParentBackColorChanged(EventArgs e)
        {
            //base.OnParentBackColorChanged(e);
            MessageBox.Show("ok");
        }

        protected override void OnParentBackgroundImageChanged(EventArgs e)
        {
            //base.OnParentBackgroundImageChanged(e);
            MessageBox.Show("ok");
        }
    }
}
