using Microsoft.Web.WebView2.WinForms;
using Microsoft.Web.WebView2.Core;
using System.Runtime.InteropServices;
using System.Web;

namespace sb_sharp {
    public partial class Main : Form {

        #region Movement Events for custom title bar
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();
        #endregion

        private Dictionary<Button, WebView2> _tabView;
        private Dictionary<WebView2, CoreWebView2> _webCore;
        private Stack<string> _closedTabs;
        private List<Button> _tabs;
        private Button? _selectedTab;
        private Color _themeColour;
        private Point _mouseDownLocation; //For dragging tabs
        private bool _tabsMoved;
        FormWindowState _lastWindowState;
        public Main() {
            InitializeComponent();
            _tabView = new Dictionary<Button, WebView2>();
            _webCore = new Dictionary<WebView2, CoreWebView2>();
            _closedTabs = new Stack<string>();
            _tabs = new List<Button>();
            _lastWindowState = FormWindowState.Normal;
            pTabBar.HorizontalScroll.Visible = false;
            pTabBar.VerticalScroll.Enabled = false;

            //Position header bar, tabbar and controls at the top of the screen
            //Everything derives from the height of the main header panel
            pHeader.Top = 0;
            pHeader.Left = 0;
            pTabBar.Top = 0;
            pTabBar.Left = 0;
            pTabBar.Width = bNewTab.Left;
            pHeader.Height = 66;
            pTabBar.Height = pHeader.Height / 2;
            pHeader.Width = Width - 16;

            bClose.Top = 0;
            bMinimise.Top = 0;
            bMaximise.Top = 0;
            bNewTab.Top = 0;

            bClose.Height = pTabBar.Height;
            bMinimise.Height = pTabBar.Height;
            bMaximise.Height = pTabBar.Height;
            bNewTab.Height = pTabBar.Height;

            bBack.Left = 0;
            bForward.Left = bBack.Right;
            bReload.Left = bForward.Right;

            pControls.Height = pTabBar.Height;
            pControls.Top = pTabBar.Bottom;
            pControls.Left = 0;
            pWebBrowser.Left = 0;
            pWebBrowser.Top = pHeader.Bottom - 3;
            _themeColour = Color.LightCoral;
            pTabBar.BackColor = _themeColour;
            bClose.BackColor = _themeColour;
            bMinimise.BackColor = _themeColour;
            bMaximise.BackColor= _themeColour;
            MaximizedBounds = Screen.FromHandle(this.Handle).WorkingArea;
        }

        void setTitleText() {
            if (_selectedTab == null) return;
            if (ControlBox)
                Text = _selectedTab.Text;
            WebView2 wv = _tabView[_selectedTab];
            eURL.Text = wv.Source == null ? "" : wv.Source.ToString();
        }

        void setTabSelected(Button tab) {
            _selectedTab = tab;
            foreach (Button b in _tabView.Keys)
                b.BackColor = _themeColour;
            _selectedTab.BackColor = Color.White;
            setTitleText();
            WebView2 wv = _tabView[tab];
            wv.BringToFront();
        }

        void tabClicked(object sender, MouseEventArgs e) {
            _mouseDownLocation = e.Location;
            Button? b = sender as Button;
            if (b == null) return;
            b.BringToFront();
            switch (e.Button) {
                case MouseButtons.Left:
                    setTabSelected(b);
                break;
                case MouseButtons.Middle:
                    closeTab(b.Controls[0], e);
                break;
            }
        }

        void tabMoved(object sender, MouseEventArgs e) {
            if (e.Button != MouseButtons.Left) return;
            Button? b = _selectedTab;
            if (b == null) return;
            int x = e.X + b.Left - _mouseDownLocation.X;
            int idx = _tabs.IndexOf(b);
            if (e.X > _mouseDownLocation.X) {
                _tabsMoved = true;
                b.Left = x + b.Width < pTabBar.Width ? x : pTabBar.Width - b.Width;
                if (idx < _tabs.Count - 1) {
                    Button? bUnder = _tabs[idx + 1] as Button;
                    if (bUnder != null) {
                        if ((b.Right > bUnder.Left + bUnder.Width / 2) && (bUnder.Left > b.Left)) {
                            x = bUnder.Left - bUnder.Width;
                            bUnder.Left = x > 0 ? x : 0;
                            _tabs[idx] = bUnder;
                            _tabs[idx + 1] = b;
                        }
                    }
                }
            }
            else if (e.X < _mouseDownLocation.X) {
                _tabsMoved = true;
                b.Left = x > 0 ? x : 0;
                if (idx > 0) {
                    Button? bUnder = _tabs[idx - 1] as Button;
                    if (bUnder != null) {
                        if ((b.Left < bUnder.Left + bUnder.Width / 2) && (bUnder.Right < b.Right)) {
                            x = bUnder.Left + bUnder.Width;
                            bUnder.Left = x + bUnder.Width < pTabBar.Width ? x : pTabBar.Width - bUnder.Width;
                            _tabs[idx] = bUnder;
                            _tabs[idx - 1] = b;
                        }
                    }
                }
            }
        }
        void tabReleased(object sender, MouseEventArgs e) {
            if (_tabsMoved)
                alignTabs();
            _tabsMoved = false;
        }

        void navigationCompleted(object? sender, object e) {
            CoreWebView2? c = sender as CoreWebView2;
            if (c == null) return;
            WebView2? wv = _webCore.FirstOrDefault(wc => wc.Value == c).Key;
            if (wv == null) return;
            Button? b = _tabView.FirstOrDefault(tv => tv.Value == wv).Key;
            if (b == null) return;
            b.Text = c.DocumentTitle;
            setTitleText();
        }

        void webViewClicked(object sender, EventArgs e) {
            Console.WriteLine("TEST");
        }

        void initCoreWebView(object sender, EventArgs e) {
            WebView2? wv = sender as WebView2;
            if (wv == null) return;
            wv.CoreWebView2.DocumentTitleChanged += navigationCompleted;
            wv.Click += webViewClicked;
            wv.NavigateToString("<html><head></head><body>test</body></html>");
            _webCore.Add(wv, wv.CoreWebView2);
        }

        void newTab() {
            const int bWidth = 150;
            int bHeight = pTabBar.Height;
            Button b = new Button() {
                FlatStyle = FlatStyle.Flat,
                Location = new Point {
                    X = _tabs.Count * bWidth,
                    Y = 0
                },
                Size = new Size(bWidth, bHeight),
                Text = "New Tab",
            };
            b.MouseDown += tabClicked;
            b.MouseMove += tabMoved;
            b.MouseUp += tabReleased;
            b.FlatAppearance.BorderSize = 0;
            b.BackColor = _themeColour;
            
            //Close tab button
            Button x = new Button();
            x.FlatStyle = FlatStyle.Flat;
            int xSize = (int)(bHeight * 0.75);
            x.Size = new Size(xSize, xSize);
            x.Parent = b;
            x.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            x.Location = new Point((int)(b.Width - x.Width * 1.25), (int)((bHeight / 2) - (x.Height / 2)));
            x.FlatAppearance.BorderSize = 0;
            x.Text = "X";
            x.Font = new Font(x.Font.FontFamily, 8);
            x.BackColor = Color.Transparent;
            x.Click += closeTab;

            WebView2 wv = new WebView2 {
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right,
                Size = new Size(Width, pWebBrowser.Height),
            };
            wv.CoreWebView2InitializationCompleted += initCoreWebView;
            pTabBar.Controls.Add(b);
            _tabs.Add(b);
            _tabView.Add(b, wv);
            pWebBrowser.Controls.Add(wv);
            setTabSelected(b);
        }

        void alignTabs() {
            for (int i = 0; i < _tabs.Count; i++) {
                Button? t = _tabs[i] as Button;
                if (t == null) continue;
                t.Location = new Point(i * t.Width, 0);
            }
        }

        void closeTab(object sender, EventArgs e) {
            Button tab = _selectedTab;
            if (sender != null) {
                Button? x = sender as Button;
                if (x == null) return;
                tab = x.Parent as Button;
            }
            if (tab == null) return;
            WebView2 wv = _tabView[tab];
            pWebBrowser.Controls.Remove(wv);
            int tabIndex = _tabs.IndexOf(tab);
            pTabBar.Controls.Remove(tab);
            _tabView.Remove(tab);
            _tabs.Remove(tab);
            _webCore.Remove(wv);
            if (wv.Source != null)
                _closedTabs.Push(wv.Source.ToString());
            wv.Dispose();
            if (_selectedTab == tab) {
                if (_tabs.Count > 0) {
                    int tabIndexToSet = tabIndex - 1;
                    if (_tabs.Count > tabIndex)
                        tabIndexToSet = tabIndex;
                    Button? tabToSelect = _tabs[tabIndexToSet] as Button;
                    if (tabToSelect != null)
                        setTabSelected(tabToSelect);
                }
                else {
                    _selectedTab = null;
                }
            }
            alignTabs();
        }

        private void bNewTab_Click(object sender, EventArgs e) {
            newTab();
        }

        private void pTabBar_MouseMove(object sender, MouseEventArgs e) {
            if (e.Button == MouseButtons.Left) {
                FormBorderStyle = FormBorderStyle.Sizable;
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void bClose_Click(object sender, EventArgs e) {
            Close();
        }

        private void bMinimise_Click(object sender, EventArgs e) {
            WindowState = FormWindowState.Minimized;
        }

        void toggleMaximise() {
            switch (WindowState) {
                case FormWindowState.Maximized: 
                    FormBorderStyle = FormBorderStyle.Sizable;
                    WindowState = FormWindowState.Normal;
                break;
                case FormWindowState.Normal:
                    FormBorderStyle = FormBorderStyle.None;
                    WindowState = FormWindowState.Maximized;
                break;
            }
        }

        private void bMaximise_Click(object sender, EventArgs e) {
            toggleMaximise();
        }

        private void pTabBar_DoubleClick(object sender, EventArgs e) {
            toggleMaximise();
        }

        void search() {
            string url = eURL.Text; //NewTab blanks out the URL input so we need to capture it here
            if (_selectedTab == null) 
                newTab();
            WebView2 wv = _tabView[_selectedTab];
            Uri uriResult;
            bool result = Uri.TryCreate(url, UriKind.Absolute, out uriResult)
                && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
            if (!result) {
                url = HttpUtility.UrlEncode(url);
                url =$"https://duckduckgo.com/?q={url}";
                result = Uri.TryCreate(url, UriKind.Absolute, out uriResult)
                    && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
            }
            if (result) 
                wv.Source = uriResult;
        }
        private void bGo_Click(object sender, EventArgs e) {
            search();
        }

        private void eURL_KeyDown(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.Enter) {
                search();
                e.SuppressKeyPress = true;
            }
        }

        private WebView2 currentView() {
            if (_selectedTab == null)
                return null;
            return _tabView[_selectedTab];
        }

        private void bBack_Click(object sender, EventArgs e) {
            WebView2 wv = currentView();
            if (wv != null) 
                wv.GoBack();
        }

        private void bForward_Click(object sender, EventArgs e) {
            WebView2 wv = currentView();
            if (wv != null)
                wv.GoForward();
        }

        private void bReload_Click(object sender, EventArgs e) {
            WebView2 wv = currentView();
            if (wv != null)
                wv.Reload();
        }

        private void Main_KeyDown(object sender, KeyEventArgs e) {
            if (e.Control) {
                switch (e.KeyCode) {
                    case Keys.W:
                        closeTab(null, e);
                    break;
                    case Keys.T: {
                        if (e.Shift) {
                            if (_closedTabs.Count > 0) {
                                newTab();
                                eURL.Text = _closedTabs.Pop();
                                search();
                            }
                        }
                        else {
                            newTab();
                        }
                        break;
                    }
                }
            }
        }

        private void Main_Resize(object sender, EventArgs e) {
            if (WindowState != _lastWindowState) {
                _lastWindowState = WindowState;
                if (WindowState == FormWindowState.Maximized) 
                    FormBorderStyle = FormBorderStyle.None;
                else if (WindowState == FormWindowState.Normal) 
                    FormBorderStyle = FormBorderStyle.Sizable;
            }
        }
    }
}