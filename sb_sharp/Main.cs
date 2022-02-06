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
        private Button? _selectedTab;
        private Color _themeColour;
        public Main() {
            InitializeComponent();
            pTabBar.HorizontalScroll.Visible = false;
            pTabBar.VerticalScroll.Enabled = false;
            _tabView = new Dictionary<Button, WebView2>();
            _webCore = new Dictionary<WebView2, CoreWebView2>();
            pWebBrowser.Location = new Point {
                X = 0,
                Y = pHeader.Bottom - 3
            };
            _themeColour = Color.LightCoral;
            pTabBar.BackColor = _themeColour;
            bClose.BackColor = _themeColour;
            bMinimise.BackColor = _themeColour;
            bMaximise.BackColor= _themeColour; 
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
            Button? b = sender as Button;
            if (b == null) return;
            switch (e.Button) {
                case MouseButtons.Left:
                    setTabSelected(b);
                break;
                case MouseButtons.Middle:
                    closeTab(b.Controls[0], e);
                break;
            }
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

        void NewTab() {
            const int bWidth = 150;
            int bHeight = pTabBar.Height;
            Button b = new Button() {
                FlatStyle = FlatStyle.Flat,
                Location = new Point {
                    X = pTabBar.Location.X + (pTabBar.Controls.Count) * bWidth,
                    Y = 0
                },
                Size = new Size(bWidth, bHeight),
                Text = "New Tab",
            };
            b.MouseUp += tabClicked;
            b.FlatAppearance.BorderSize = 0;
            b.BackColor = _themeColour;
            
            //Close tab button
            Button x = new Button();
            x.FlatStyle = FlatStyle.Flat;
            x.Size = new Size(bHeight, bHeight);
            x.Parent = b;
            x.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            x.Location = new Point(b.Width - x.Width, 0);
            x.FlatAppearance.BorderSize = 0;
            x.Text = "X";
            x.Click += closeTab;

            WebView2 wv = new WebView2 {
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right,
                Size = new Size(Width, pWebBrowser.Height),
            };
            wv.CoreWebView2InitializationCompleted += initCoreWebView;
            pTabBar.Controls.Add(b);
            _tabView.Add(b, wv);
            pWebBrowser.Controls.Add(wv);
            setTabSelected(b);
        }

        void closeTab(object sender, EventArgs e) {
            Button? x = sender as Button;
            if (x == null) return;
            Button? tab = x.Parent as Button;
            if (tab == null) return;
            WebView2 wv = _tabView[tab];
            pWebBrowser.Controls.Remove(wv);
            int tabIndex = pTabBar.Controls.IndexOf(tab);
            pTabBar.Controls.Remove(tab);
            _tabView.Remove(tab);
            _webCore.Remove(wv);
            if (_selectedTab == tab) {
                if (pTabBar.Controls.Count > 0) {
                    int tabIndexToSet = tabIndex - 1;
                    if (pTabBar.Controls.Count > tabIndex)
                        tabIndexToSet = tabIndex;
                    Button? tabToSelect = pTabBar.Controls[tabIndexToSet] as Button;
                    if (tabToSelect != null)
                        setTabSelected(tabToSelect);
                }
                else {
                    _selectedTab = null;
                }
            }
            for (int i = 0; i < pTabBar.Controls.Count; i++) {
                Button? t = pTabBar.Controls[i] as Button;
                if (t == null) continue;
                t.Location = new Point(i * t.Width, 0);
            }
        }

        private void bNewTab_Click(object sender, EventArgs e) {
            NewTab();
        }

        private void pTabBar_MouseMove(object sender, MouseEventArgs e) {
            if (e.Button == MouseButtons.Left) {
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
                    WindowState = FormWindowState.Normal;
                break;
                case FormWindowState.Normal:
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

        void Search() {
            String url = eURL.Text; //NewTab blanks out the URL input so we need to capture it here
            if (_selectedTab == null) 
                NewTab();
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
            Search();
        }

        private void eURL_KeyDown(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.Enter) {
                Search();
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
    }
}