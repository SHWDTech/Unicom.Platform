using System;
using System.Windows;
using Unicom.Platform;
using Unicom.Platform.Model;
using Unicom.Platform.Model.Service_References.UnicomPlatform;
using Unicom.Platform.SQLite;
using Unicom.Register.Common;

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
                var emsDevice = new emsDevice
                {
                    name = TxtDeviceName.Text,
                    ipAddr = TxtIpAddress.Text,
                    macAddr = TxtMacAddress.Text,
                    port = TxtPort.Text,
                    version = TxtDeviceVersion.Text,
                    projectCode = CmbProject.SelectedValue.ToString(),
                    longitude = TxtLongitude.Text,
                    latitude = TxtLatitude.Text,
                    startDate = DpStartDate.DisplayDate,
                    startDateSpecified = true,
                    endDate = DpEndDate.DisplayDate,
                    endDateSpecified = true,
                    installDate = DpInstallDate.DisplayDate,
                    installDateSpecified = true,
                    onlineStatus = CbOnlineStatus.IsChecked == true,
                    onlineStatusSpecified = true,
                    videoUrl = TxtVideoUrl.Text
                };

                var service = new UnicomService();
                var result = service.PushDevices(new[] {emsDevice});
                if (!result.result[0].value.ToString().Contains("ERROR"))
                {
                    var project = new EmsDevice
                    {
                        SystemCode = TxtSystemCode.Text,
                        UnicomCode = result.result[0].key.ToString(),
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
