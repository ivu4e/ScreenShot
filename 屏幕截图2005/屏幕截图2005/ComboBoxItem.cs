using System;
using System.Collections.Generic;
using System.Text;

namespace 屏幕截图2005
{
    public class ComboBoxItem
    {
        private string itemText;
        private string itemValue;

        public ComboBoxItem(string itemText, string itemValue)
        {
            this.itemText = itemText;
            this.itemValue = itemValue;
        }

        public string Value
        {
            get
            {
                return this.itemValue;
            }
        }

        public string Text
        {
            get
            {
                return this.itemText;
            }
        }

        public override string ToString()
        {
            return this.itemText;
        }
    }
}
