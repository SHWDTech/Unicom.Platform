using System.Windows;

namespace Unicom.Register.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void OpenAddProject(object sender, RoutedEventArgs e)
        {
            var addProjectWindow = new AddProject();
            addProjectWindow.ShowDialog();
        }

        private void OpenAddDevice(object sender, RoutedEventArgs e)
        {
            var addDeviceWindow = new AddDevice();
            addDeviceWindow.ShowDialog();
        }

        private void OpenProjectDeviceManage(object sender, RoutedEventArgs e)
        {
            var manageWindow = new ProjectDeviceManage();
            manageWindow.ShowDialog();
        }
    }
}
