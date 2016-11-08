using System;
using System.Collections.Generic;
using System.Text;

namespace 屏幕截图2005
{
    /// <summary>
    /// 定时器项
    /// </summary>
    public class TimerItem
    {
        private TimerItemType _type = TimerItemType.Tips;
        /// <summary>
        /// 定时器项类型
        /// </summary>
        public TimerItemType ItemType
        {
            get { return _type; }
            set { _type = value; }
        }

        private Nullable<DateTime> _TickTime = null;
        /// <summary>
        /// 定时器触发时间
        /// </summary>
        public Nullable<DateTime> TickTime
        {
            get { return _TickTime; }
            set { _TickTime = value; }
        }
        private String _command = String.Empty;
        /// <summary>
        /// 要执行的命令文本
        /// </summary>
        public String CommandText
        {
            get { return _command; }
            set { _command = value; }
        }

        private string _tip = String.Empty;
        /// <summary>
        /// 提示信息
        /// </summary>
        public string Tip
        {
            get { return _tip; }
            set { _tip = value; }
        }


        public TimerItem(Nullable<DateTime> tickTime, string tip)
        {
            this._TickTime = tickTime;
            this._tip = tip;
        }

        public TimerItem(Nullable<DateTime> tickTime, string tip, TimerItemType itemType)
        {
            this._TickTime = tickTime;
            this._tip = tip;
            this._type = itemType;
        }

        public TimerItem(Nullable<DateTime> tickTime, string tip, string commandText)
        {
            this._TickTime = tickTime;
            this._tip = tip;
            this._command = commandText;
        }

        public TimerItem(Nullable<DateTime> tickTime, string tip, TimerItemType itemType, string commandText)
        {
            this._TickTime = tickTime;
            this._tip = tip;
            this._type = itemType;
            this._command = commandText;
        }

    }
}
