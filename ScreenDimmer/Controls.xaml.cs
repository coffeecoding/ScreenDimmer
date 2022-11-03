using System;
using System.Windows;
using System.Windows.Controls;

namespace ScreenDimmer
{
    public partial class Controls : Window
    {
        public Window Main { get; set; }

        public Controls()
        {
            InitializeComponent();
        }

        public Controls(Window main)
        {
            DataContext = this;
            Main = main;
            InitializeComponent();
            // MouseWheel scrolling, courtesy https://stackoverflow.com/a/42294703
            OpacitySlider.PreviewMouseWheel += (s, e) => OpacitySlider.Value += e.Delta / 120 * OpacitySlider.SmallChange;
            WarmthSlider.PreviewMouseWheel += (s, e) => WarmthSlider.Value += e.Delta / 120 * WarmthSlider.SmallChange;
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            (Main as MainWindow).OpacityValue = OpacitySlider.Value;
        }

        private void ShutdownButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.ShutdownMode = ShutdownMode.OnExplicitShutdown;
            Application.Current.Shutdown();
        }

        private void WarmthSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            (Main as MainWindow).WarmthValue = WarmthSlider.Value;
        }

        private void TextOpacityValue_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                bool isValid = double.TryParse(TextOpacityValue.Text, out double input);
                if (!isValid || isValid && (input < OpacitySlider.Minimum || input > OpacitySlider.Maximum)) { 
                    MessageBox.Show($"Value must be in range [{OpacitySlider.Minimum}; {OpacitySlider.Maximum}].");
                    return;
                }
                OpacitySlider.Value = input;
            } catch (Exception) { }
        }

        private void TextWarmthValue_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                bool isValid = double.TryParse(TextWarmthValue.Text, out double input);
                if (!isValid || isValid && (input < WarmthSlider.Minimum || input > WarmthSlider.Maximum))
                {
                    MessageBox.Show($"Value must be in range [{WarmthSlider.Minimum}; {WarmthSlider.Maximum}].");
                    return;
                }
                WarmthSlider.Value = input;
            } catch (Exception) { }
        }
    }
}
