using System;
using System.Runtime.InteropServices;
using System.Windows.Interop;
using System.Windows.Media;

namespace ScreenDimmer
{
    public partial class MainWindow
    {
        const byte WARMTHCLR_R_MAX = 32;
        const byte WARMTHCLR_G_MAX = 0;
        const byte WARMTHCLR_B_MAX = 0;

        public double OpacityValue
        {
            set => BgBrush.Opacity = value;
        }

        public double WarmthValue
        {
            set => BgBrush.Color = CalculateColorFromWarmth(value);
        }

        private Color CalculateColorFromWarmth(double w)
        {
            byte r, g, b;
            if (w <= 0)
            {
                w *= -1;
                r = (byte)(WARMTHCLR_R_MAX * w);
                g = (byte)(WARMTHCLR_G_MAX * w);
                b = (byte)(WARMTHCLR_B_MAX * w);
            }
            else
            {
                r = g = 0;
                b = (byte)(32 * w);
            }
            return Color.FromRgb(r, g, b);
        }

        public MainWindow()
        {
            InitializeComponent();
            new Controls(this).Show();
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            var hwnd = new WindowInteropHelper(this).Handle;
            WindowsServices.SetWindowExTransparent(hwnd);
        }
    }

    // Make Window Transparent and ClickThrough: Courtesy https://stackoverflow.com/a/3367137
    public static class WindowsServices
    {
        const int WS_EX_TRANSPARENT = 0x00000020;
        const int GWL_EXSTYLE = (-20);

        [DllImport("user32.dll")]
        static extern int GetWindowLong(IntPtr hwnd, int index);

        [DllImport("user32.dll")]
        static extern int SetWindowLong(IntPtr hwnd, int index, int newStyle);

        public static void SetWindowExTransparent(IntPtr hwnd)
        {
            var extendedStyle = GetWindowLong(hwnd, GWL_EXSTYLE);
            SetWindowLong(hwnd, GWL_EXSTYLE, extendedStyle | WS_EX_TRANSPARENT);
        }
    }
}
