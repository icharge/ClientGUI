using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ClientGUI
{
    /// <summary>
    /// A listbox without a scroll bar.
    /// http://stackoverflow.com/questions/13169900/hide-vertical-scroll-bar-in-listbox-control
    /// </summary>
    public partial class ScrollbarlessListBox : ListBox
    {
        public ScrollbarlessListBox()
        {
            InitializeComponent();
        }

        private bool mShowScroll = false;
        protected override System.Windows.Forms.CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                if (!mShowScroll)
                    cp.Style = cp.Style & ~0x200000;
                return cp;
            }
        }

        public bool ShowScrollbar
        {
            get { return mShowScroll; }
            set
            {
                if (value != mShowScroll)
                {
                    mShowScroll = value;
                    if (IsHandleCreated)
                        RecreateHandle();
                }
            }
        }
    }
}
