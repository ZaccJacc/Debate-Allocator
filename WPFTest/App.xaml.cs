using System.Windows;

namespace WPFTest
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public MainWindow? window;

        private void ApplicationStartup(object sender, StartupEventArgs e)
        {
            window = new MainWindow();
            window.Show();
        }
    }
}
