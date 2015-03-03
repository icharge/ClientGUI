/// @author Rampastring
/// http://www.moddb.com/members/rampastring

using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Runtime.InteropServices;
using ClientCore;

namespace ClientGUI
{
    /// <summary>
    /// A ComboBox which can prevent you from opening the drop-down list and
    /// which comes with greater graphical capabilities than the default ComboBox.
    /// </summary>
    public class LimitedComboBox : ComboBox
    {
        // Import the GetScrollInfo function from user32.dll
        [DllImport("user32.dll", SetLastError = true)]
        private static extern int GetScrollInfo(IntPtr hWnd, int n,
                                  ref ScrollInfoStruct lpScrollInfo);

        // Win32 constants
        private const int SB_VERT = 1;
        private const int SIF_TRACKPOS = 0x10;
        private const int SIF_RANGE = 0x1;
        private const int SIF_POS = 0x4;
        private const int SIF_PAGE = 0x2;
        private const int SIF_ALL = SIF_RANGE | SIF_PAGE |
                                    SIF_POS | SIF_TRACKPOS;

        private const int SCROLLBAR_WIDTH = 17;
        private const int LISTBOX_YOFFSET = 21;

        // Return structure for the GetScrollInfo method
        [StructLayout(LayoutKind.Sequential)]
        private struct ScrollInfoStruct
        {
            public int cbSize;
            public int fMask;
            public int nMin;
            public int nMax;
            public int nPage;
            public int nPos;
            public int nTrackPos;
        }

        bool _canDropDown = true;
        bool _useCustomDrawing = false;
        bool isDroppedDown = false;

        Color outlineColor;

        Image comboBoxArrow;
        Image openedComboBoxArrow;

        public List<Color> ItemColors = new List<Color>();

        int yPos = -1;
        int xPos = -1;
        int onScreenIndex = -1;
        int hoveredIndex = -1;

        public LimitedComboBox()
        {
            this.SetStyle(ControlStyles.UserPaint, true);
            UseCustomDrawingCode = true;

            if (System.IO.File.Exists(ProgramConstants.gamepath + ProgramConstants.RESOURCES_DIR + "comboBoxArrow.png"))
            {
                comboBoxArrow = Image.FromFile(ProgramConstants.gamepath + ProgramConstants.RESOURCES_DIR + "comboBoxArrow.png");
                openedComboBoxArrow = Image.FromFile(ProgramConstants.gamepath + ProgramConstants.RESOURCES_DIR + "openedComboBoxArrow.png");
            }

            this.DropDown += LimitedComboBox_DropDown;
            this.DropDownClosed += LimitedComboBox_DropDownClosed;

            string[] comboBoxOutlineColor = DomainController.Instance().getComboBoxOutlineColor().Split(',');
            int r = Convert.ToInt32(comboBoxOutlineColor[0]);
            int g = Convert.ToInt32(comboBoxOutlineColor[1]);
            int b = Convert.ToInt32(comboBoxOutlineColor[2]);
            outlineColor = Color.FromArgb(255, r, g, b);
        }

        void LimitedComboBox_DropDown(object sender, EventArgs e)
        {
            isDroppedDown = true;
        }

        void LimitedComboBox_DropDownClosed(object sender, EventArgs e)
        {
            isDroppedDown = false;
            this.Refresh();
        }

        public bool CanDropDown
        {
            get { return _canDropDown; }
            set { _canDropDown = value; }
        }

        public bool UseCustomDrawingCode
        {
            get { return _useCustomDrawing; }
            set
            { _useCustomDrawing = value; }
        }

        public int HoveredIndex
        {
            get { return hoveredIndex; }
            set { hoveredIndex = value; }
        }

        public void AddItem(string text, Color color)
        {
            this.Items.Add(text);
            this.ItemColors.Add(color);
        }

        /// <summary>
        /// Prevent the combobox dropdown from working
        /// http://stackoverflow.com/questions/5337834/prevent-dropdown-area-from-opening-of-combobox-control-in-windows-forms
        /// And make it possible to determine which index the user is currently hovering on
        /// http://www.codeproject.com/Articles/14255/ComboBox-firing-events-when-hovering-on-the-dropdo
        /// </summary>
        protected override void WndProc(ref  Message m)
        {
            if (!CanDropDown &&
               (m.Msg == 0x201 || // WM_LBUTTONDOWN
                m.Msg == 0x203)) // WM_LBUTTONDBLCLK
                return;

            if (m.Msg == 308)
            {
                Point LocalMousePosition = this.PointToClient(Cursor.Position);
                xPos = LocalMousePosition.X;
                yPos = LocalMousePosition.Y - this.Size.Height - 1;
                onScreenIndex = 0;

                int oldYPos = yPos;

                // get the 0-based index of where the cursor is on screen
                // as if it were inside the listbox
                while (yPos >= this.ItemHeight)
                {
                    yPos -= this.ItemHeight;
                    onScreenIndex++;
                }

                //if (yPos < 0) { onScreenIndex = -1; }
                ScrollInfoStruct si = new ScrollInfoStruct();
                si.fMask = SIF_ALL;
                si.cbSize = Marshal.SizeOf(si);
                // m.LParam holds the hWnd to the drop down list that appears
                int getScrollInfoResult = 0;
                getScrollInfoResult = GetScrollInfo(m.LParam, SB_VERT, ref si);

                // k returns 0 on error, so if there is no error add the current
                // track position of the scrollbar to our index
                if (getScrollInfoResult > 0)
                {
                    onScreenIndex += si.nTrackPos;
                }

                // Check we're actually inside the drop down window that appears and 
                // not just over its scrollbar before we actually try to update anything
                // then if we are raise the Hover event for this comboBox
                if (!(xPos > this.Width || xPos < 1 ||
                      oldYPos < 0 || (oldYPos > this.ItemHeight *
                      this.MaxDropDownItems)))
                {
                    if (onScreenIndex > this.Items.Count - 1)
                        hoveredIndex = this.Items.Count - 1;
                    else
                        hoveredIndex = onScreenIndex;

                    //hoveredIndex = (onScreenIndex > this.Items.Count - 1) ?
                    //               this.Items.Count - 1 : onScreenIndex;
                }
                else
                    hoveredIndex = -1;
            }

            base.WndProc(ref m);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (UseCustomDrawingCode)
            {
                Graphics graphics = e.Graphics;

                SolidBrush solidBrush = new SolidBrush(outlineColor);
                graphics.FillRectangle(solidBrush, ClientRectangle);

                graphics.FillRectangle(new SolidBrush(this.BackColor),
                    new Rectangle(ClientRectangle.X + 1, ClientRectangle.Y + 1,
                        ClientRectangle.Width - 2, ClientRectangle.Height - 2));

                if (ItemColors.Count == 0)
                {
                    if (this.Enabled)
                    {
                        graphics.DrawString(this.Text, this.Font, new SolidBrush(this.ForeColor),
                            new PointF(this.ClientRectangle.X + 3, this.ClientRectangle.Y + 3));
                    }
                    else
                    {
                        graphics.DrawString(this.Text, this.Font, new SolidBrush(Color.DarkGray),
                            new PointF(this.ClientRectangle.X + 3, this.ClientRectangle.Y + 3));
                    }
                }
                else if (this.SelectedIndex > -1)
                {
                    graphics.DrawString(this.Text, this.Font, new SolidBrush(ItemColors[this.SelectedIndex]),
                        new PointF(this.ClientRectangle.X + 3, this.ClientRectangle.Y + 3));
                }

                if (comboBoxArrow != null && Enabled)
                {
                    if (isDroppedDown)
                    {
                        graphics.DrawImage(openedComboBoxArrow, this.ClientRectangle.X + this.ClientRectangle.Width - 18,
                            this.ClientRectangle.Y, 18, 21);
                    }
                    else
                    {
                        graphics.DrawImage(comboBoxArrow, this.ClientRectangle.X + this.ClientRectangle.Width - 18,
                            this.ClientRectangle.Y, 18, 21);
                    }
                }
            }
            else
                base.OnPaint(e);
        }
    }

    public class HoverEventArgs : EventArgs
    {
        private int _itemIndex = 0;
        public int itemIndex
        {
            get
            {
                return _itemIndex;
            }
            set
            {
                _itemIndex = value;
            }
        }
    }
}
