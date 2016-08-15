using System.Windows;
using Unicom.Register.Common;

namespace Unicom.Register
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            Operation.RefreashBaseInfo();
            base.OnStartup(e);
        }
    }
}
