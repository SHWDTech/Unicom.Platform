using System;
using System.Windows;
using Unicom.Platform;
using Unicom.Platform.Model;
using Unicom.Platform.Model.Service_References.UnicomPlatform;
using Unicom.Platform.SQLite;
using Unicom.Register.Common;
using EmsDevice = Unicom.Platform.Model.Service_References.UnicomPlatform.EmsDevice;

namespace Unicom.Register.Views
{
    /// <summary>
    /// Interaction logic for AddDevice.xaml
    /// </summary>
    public partial class AddDevice
    {
        private readonly UnicomContext _context = new UnicomContext(AppConfig.ConnectionString);

        public AddDevice()
        {
            InitializeComponent();
            InitSelections();
        }

        private void InitSelections()
        {
            foreach (var project in _context.Projects)
            {
                CmbProject.Items.Add(new { Key = project.UnicomCode, Value = project.UnicomName });
            }
        }

        private void Submit(object sender, RoutedEventArgs e)
        {
            try
            {
                var emsDevice = new EmsDevice
                {
                    Name = TxtDeviceName.Text,
                    IpAddr = TxtIpAddress.Text,
                    MacAddr = TxtMacAddress.Text,
                    Port = TxtPort.Text,
                    Version = TxtDeviceVersion.Text,
                    ProjectCode = CmbProject.SelectedValue.ToString(),
                    Longitude = TxtLongitude.Text,
                    Latitude = TxtLatitude.Text,
                    StartDate = DpStartDate.DisplayDate,
                    StartDateSpecified = true,
                    EndDate = DpEndDate.DisplayDate,
                    EndDateSpecified = true,
                    InstallDate = DpInstallDate.DisplayDate,
                    InstallDateSpecified = true,
                    OnlineStatus = CbOnlineStatus.IsChecked == true,
                    OnlineStatusSpecified = true,
                    VideoUrl = TxtVideoUrl.Text
                };

                var service = new UnicomService();
                var result = service.PushDevices(new[] {emsDevice});
                if (!result.Result[0].Value.ToString().Contains("ERROR"))
                {
                    var project = new Platform.Model.EmsDevice
                    {
                        SystemCode = TxtSystemCode.Text,
                        UnicomCode = result.Result[0].Key.ToString(),
                        ProjectUnicomCode = CmbProject.SelectedValue.ToString(),
                        UnicomName = TxtDeviceName.Text,
                        OnTransfer = false
                    };
                    _context.AddOrUpdate(project);
                    MessageBox.Show("添加成功。");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
