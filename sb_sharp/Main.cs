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

        private Dictionary<WebView2, CoreWebView2> _webCore;
        private Stack<string> _closedTabs;
        private List<Tab> _tabs;
        private Tab? _selectedTab;
        private Color _themeColour;
        private Point _mouseDownLocation; //For dragging tabs
        private bool _tabsMoved;
        FormWindowState _lastWindowState;
        public Main() {
            InitializeComponent();
            _webCore = new Dictionary<WebView2, CoreWebView2>();
            _closedTabs = new Stack<string>();
            _tabs = new List<Tab>();
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
            if (_selectedTab == null) {
                eURL.Text = "";
                return;
            }
            if (ControlBox)
                Text = _selectedTab.Text;
            eURL.Text = _selectedTab.URL;
        }

        void setTabSelected(Tab tab) {
            if (_selectedTab != null)
                _selectedTab.BackColour = _themeColour;
            _selectedTab = tab;
            _selectedTab.Select();
            setTitleText();
        }

        void tabClicked(object sender, MouseEventArgs e) {
            _mouseDownLocation = e.Location;
            Button? b = sender as Button;
            if (b == null) return;
            //b.BringToFront();
            Tab t = _tabs.FirstOrDefault(t => t.TabButton == b);
            switch (e.Button) {
                case MouseButtons.Left:
                    setTabSelected(t);
                break;
                case MouseButtons.Middle:
                    closeTab(b.Controls[0], e);
                break;
            }
        }

        void tabMoved(object sender, MouseEventArgs e) {
            if (e.Button != MouseButtons.Left) return;
            Button? b = _selectedTab.TabButton;
            if (b == null) return;
            int x = e.X + b.Left - _mouseDownLocation.X;
            int idx = _tabs.IndexOf(_selectedTab);
            if (e.X > _mouseDownLocation.X) {
                _tabsMoved = true;
                b.Left = x + b.Width < pTabBar.Width ? x : pTabBar.Width - b.Width;
                if (idx < _tabs.Count - 1) {
                    Button? bUnder = _tabs[idx + 1].TabButton;
                    if (bUnder != null) {
                        if ((b.Right > bUnder.Left + bUnder.Width / 2) && (bUnder.Left > b.Left)) {
                            x = bUnder.Left - bUnder.Width;
                            bUnder.Left = x > 0 ? x : 0;
                            _tabs[idx] = _tabs[idx + 1];
                            _tabs[idx + 1] = _selectedTab;
                        }
                    }
                }
            }
            else if (e.X < _mouseDownLocation.X) {
                _tabsMoved = true;
                b.Left = x > 0 ? x : 0;
                if (idx > 0) {
                    Button? bUnder = _tabs[idx - 1].TabButton;
                    if (bUnder != null) {
                        if ((b.Left < bUnder.Left + bUnder.Width / 2) && (bUnder.Right < b.Right)) {
                            x = bUnder.Left + bUnder.Width;
                            bUnder.Left = x + bUnder.Width < pTabBar.Width ? x : pTabBar.Width - bUnder.Width;
                            _tabs[idx] = _tabs[idx - 1];
                            _tabs[idx - 1] = _selectedTab;
                        }
                    }
                }
            }
        }
        void tabReleased(object sender, MouseEventArgs e) {
            if (_tabsMoved)
                alignTabs();
            _tabsMoved = false; //Prevents re-sorting the tabs if the user just clicks a tab
        }

        void navigationCompleted(object? sender, object e) {
            CoreWebView2? c = sender as CoreWebView2;
            if (c == null) return;
            WebView2? wv = _webCore.FirstOrDefault(wc => wc.Value == c).Key;
            if (wv == null) return;
            Tab? t = _tabs.FirstOrDefault(t => t.WebView == wv);
            if (t == null) return;
            t.Text = c.DocumentTitle;
            setTitleText();
        }

        void webViewClicked(object sender, EventArgs e) {
            Console.WriteLine("TEST");
        }

        //Need to wait for this part of the webview to initialise after webview creation in order to asign event handlers
        void initCoreWebView(object sender, EventArgs e) {
            WebView2? wv = sender as WebView2;
            if (wv == null) return;
            wv.CoreWebView2.DocumentTitleChanged += navigationCompleted;
            wv.Click += webViewClicked;
            wv.NavigateToString("<html><head></head><body>test</body></html>");
            _webCore.Add(wv, wv.CoreWebView2);
        }

        Tab newTab(bool selectNewTab = true) {
            Tab t = new Tab(_themeColour, pTabBar.Height, _tabs.Count * 150, pWebBrowser.Width, pWebBrowser.Height);
            Button b = t.TabButton;
            b.MouseDown += tabClicked;
            b.MouseMove += tabMoved;
            b.MouseUp += tabReleased;
            Button x = t.CloseButton;
            x.Click += closeTab;
            x.MouseEnter += toogleShowCloseTab;
            x.MouseLeave += toogleShowCloseTab;
            WebView2 wv = t.WebView;
            wv.CoreWebView2InitializationCompleted += initCoreWebView;
            pTabBar.Controls.Add(t.TabButton);
            _tabs.Add(t);
            pWebBrowser.Controls.Add(wv);
            if (selectNewTab)
                setTabSelected(t);
            return t;
        }

        void toogleShowCloseTab(object sender, EventArgs e)
        {
            Button? x = sender as Button;
            if (x == null) return;
            x.Image = x.Image == null ? bClose.Image : null;
        }

        void alignTabs() {
            for (int i = 0; i < _tabs.Count; i++) {
                Button? t = _tabs[i].TabButton;
                if (t == null) continue;
                t.Location = new Point(i * t.Width, 0);
            }
        }

        void closeTab(object sender, EventArgs e) {
            Tab tab = _selectedTab;
            if (sender != null) {
                Button? x = sender as Button;
                if (x == null) return;
                tab = _tabs.FirstOrDefault(t => t.CloseButton == x);
            }
            if (tab == null) return;
            WebView2 wv = tab.WebView;
            pWebBrowser.Controls.Remove(wv);
            int tabIndex = _tabs.IndexOf(tab);
            pTabBar.Controls.Remove(tab.TabButton);
            _tabs.Remove(tab);
            _webCore.Remove(wv);
            if (wv.Source != null)
                _closedTabs.Push(wv.Source.ToString());
            wv.Dispose();
            //If we closed the current tab we're on, ideally switch to the tab to the right, if that does not exist, use the tab to the left
            if (_selectedTab == tab) {
                if (_tabs.Count > 0) {
                    int tabIndexToSet = tabIndex - 1;
                    if (_tabs.Count > tabIndex)
                        tabIndexToSet = tabIndex;
                    Tab tabToSelect = _tabs[tabIndexToSet];
                    setTabSelected(tabToSelect);
                }
                else {
                    _selectedTab = null;
                }
            }
            alignTabs();
            setTitleText();
        }

        private void bNewTab_Click(object sender, EventArgs e) {
            newTab();
        }

        //Handles moving the window via the tabbar rather than the built in bar
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
            Uri uriResult;
            bool result = Uri.TryCreate(url, UriKind.Absolute, out uriResult)
                && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
            if (!result) {
                url = HttpUtility.UrlEncode(url);
                url =$"https://duckduckgo.com/?q={url}";
                result = Uri.TryCreate(url, UriKind.Absolute, out uriResult)
                    && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
            }
            if (result) {
                _selectedTab.URL = uriResult.ToString();
                _selectedTab.Select();
            }
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
            return _selectedTab.WebView;
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

        //Due to the border style, maximising the window has a weird offset around the edges so we change the style when maximised
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

    public class Tab {
        private string? _URL;
        private WebView2? _webView;
        private Button _tab;
        private Button _x;
        public string Text {
            get => _tab.Text;
            set {
                _tab.Text = value;
            }
        }
        public Button TabButton { get => _tab; set => _tab = value; }
        public Button CloseButton { get => _x; set => _x = value; }
        public string? URL { get => _URL; set => _URL = value; }
        public WebView2? WebView { get => _webView; }
        public Color BackColour {
            set {
                _tab.BackColor = value;
            }
        }

        public Tab(Color c, int h, int x, int viewWidth, int viewHeight) {
            const int bWidth = 150;
            int bHeight = h;
            _tab = new Button()
            {
                FlatStyle = FlatStyle.Flat,
                Location = new Point
                {
                    X = x,
                    Y = 0
                },
                Size = new Size(bWidth, bHeight),
                Text = "New Tab",
            };
            _tab.FlatAppearance.BorderSize = 0;
            _tab.BackColor = c;

            //Close tab button
            _x = new Button();
            _x.Visible = false; //Button creation can be slow and cause flickering - make it invisible to hide this until it's fully created
            _x.FlatStyle = FlatStyle.Flat;
            int xSize = (int)(bHeight * 0.75);
            _x.Size = new Size(xSize, xSize);
            _x.Parent = _tab;
            _x.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            _x.Location = new Point((int)(_tab.Width - _x.Width * 1.25), (int)((bHeight / 2) - (_x.Height / 2)));
            _x.FlatAppearance.BorderSize = 0;
            _x.Text = "";
            _x.Font = new Font(_x.Font.FontFamily, 8);
            _x.BackColor = Color.Transparent;
            _x.Image = null;
            _x.Visible = true;
            _webView = new WebView2
            {
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right,
                Size = new Size(viewWidth, viewHeight),
            };
        }
        public void Select() {
            BackColour = Color.White;
            _webView.BringToFront();
            _tab.BringToFront();
            if (_webView.Source == null && URL != null) {
                _webView.Source = new Uri(URL);
            }
        }
    }
}