using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace ROI.Pages
{
    public partial class MainPage : Window
    {
        //Variant
        private double windowHeight  = 0;
        private double windowWidth   = 0;
        private ScaleTransform windowScale = new ScaleTransform();
        public DispatcherTimer timer;

        //Function
        public MainPage()
        {
            InitializeComponent();
            UI_Init();
            Loaded += new RoutedEventHandler(UI_Loaded);
        }

        //
        // Window UI Format
        //
        private void UI_Init()
        {
            //
            // Window Base UI Init
            //
            WindowStyle = WindowStyle.None;
            //WindowState = WindowState.Maximized;
            Height = 1080;
            Width = 1920;
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            ResizeMode = ResizeMode.NoResize;

            //
            // ROI UI Init
            //_

            UI_Img_Road.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/load_t_02.jpg"));
            UI_Img_Photo.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/photo_01.png"));
            UI_Label_Message.Content = "No Event";
            UI_Label_Message.FontSize = 80;
            UI_Label_Message.VerticalContentAlignment = VerticalAlignment.Center;
            UI_Label_Message.HorizontalContentAlignment = HorizontalAlignment.Center;
        }

        //
        // Window Loaded Event
        //
        private void UI_Loaded(object sender, RoutedEventArgs e)
        {
            windowHeight = Height;
            windowWidth = Width;

            if (WindowState == WindowState.Maximized)
            {
                UI_ChangeUI(ActualHeight, ActualWidth);
            }

            SizeChanged += new SizeChangedEventHandler(UI_SizeChange);
        }

        //
        // Window UI Size Auto Change Functions
        //
        private void UI_ChangeUI(double Height, double Width)
        {
            windowScale.ScaleX = Width / windowWidth;
            windowScale.ScaleY = Height / windowHeight;

            FrameworkElement rootElement = Content as FrameworkElement;

            rootElement.LayoutTransform = windowScale;
        }

        private void UI_SizeChange(object sender, SizeChangedEventArgs e)
        {
            UI_ChangeUI(e.NewSize.Height, e.NewSize.Width);
        }
        
        //
        // Emergency Event Function
        //
        public void Emergency() //int roadType, BitmapImage photo, string text
        {
            try
            {
                //
                // Check Emergency Task
                //

                if (timer != null)
                {
                    if (timer.IsEnabled)
                    {
                        timer.Stop();
                        UI_EmergencyReset();
                        return;
                    }
                }

                //
                // Set ROI UI
                //

                //UI_Img_Road.Source = new BitmapImage(new Uri($"pack://application:,,,/Resources/Road{roadType}.jpg"));
                //UI_Img_Photo.Source = photo;
                //UI_Label_Message.Content = text;

                timer = new DispatcherTimer();
                timer.Interval = TimeSpan.FromSeconds(1);   //1 Sec
                timer.Tick += new EventHandler(UI_EmergencyMode);
                timer.Start();

            }
            catch
            {
                MessageBox.Show("Error 00 : Can not load Image");
            }
        }

        private void UI_EmergencyMode(object sender, EventArgs e)
        {
            UI_Label_Message.Content = "Warning";
            if (UI_Label_Message.Foreground.ToString() != "#FFFF0000")  //If Label Message Color is Red
            {
                //Road Border Img Enable
                UI_Img_Emergency.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/emergency_02_01.png"));
                UI_Label_Message.Foreground = new SolidColorBrush(Colors.Red);
            }
            else
            {
                //Road Border Img Disable
                UI_Img_Emergency.Source = null;
                UI_Label_Message.Foreground = new SolidColorBrush(Colors.Black);
            }
        }

        private void UI_EmergencyReset()
        {
            UI_Label_Message.Content = "No Event";
            UI_Label_Message.Foreground = new SolidColorBrush(Colors.Black);
            UI_Img_Emergency.Source = null;
        }

        //
        // Test Function
        //
        private void Btn_Clicked(object sender, RoutedEventArgs e)
        {
            Emergency();
        }
    }
}
