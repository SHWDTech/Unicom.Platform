using System;
using System.Linq;
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

        private string _deviceCode;

        private long _deviceId;

        public AddDevice()
        {
            InitializeComponent();
            InitSelections();
        }

        private void InitSelections()
        {
            foreach (var project in _context.Projects)
            {
                CmbProject.Items.Add(new { Key = project.code, Value = project.name });
            }
            foreach (var contextDevice in _context.Devices)
            {
                CmbDeviceList.Items.Add(new {Key = contextDevice.Id, Value = contextDevice.code});
            }
        }

        private emsDevice ConbineDevice()
        {
            var emsDevice = new emsDevice()
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

            return emsDevice;
        }

        private EmsDevice ConbineStoreDevice(emsDevice emsDevice)
        {
            var dev = new EmsDevice
            {
                SystemCode = TxtSystemCode.Text,
                OnTransfer = CbOnlineStatus.IsChecked == true,
                code = emsDevice.code,
                name = emsDevice.name,
                ipAddr = emsDevice.ipAddr,
                macAddr = emsDevice.macAddr,
                port = emsDevice.port,
                version = emsDevice.version,
                projectCode = emsDevice.projectCode,
                longitude = emsDevice.longitude,
                latitude = emsDevice.latitude,
                startDate = emsDevice.startDate,
                startDateSpecified = true,
                endDate = emsDevice.endDate,
                endDateSpecified = true,
                installDate = emsDevice.installDate,
                installDateSpecified = true,
                onlineStatus = true,
                onlineStatusSpecified = true,
                videoUrl = emsDevice.videoUrl
            };

            return dev;
        }

        private void Submit(object sender, RoutedEventArgs e)
        {
            try
            {
                var emsDevice = ConbineDevice();
                var service = new UnicomService();
                var result = service.PushDevices(new[] { emsDevice });
                if (result.result[0].value.ToString().Contains("ERROR"))
                {
                    MessageBox.Show(result.result[0].value.ToString());
                    return;
                }
                emsDevice.code = result.result[0].key.ToString();
                var dev = ConbineStoreDevice(emsDevice);
                _context.AddOrUpdate(dev);
                var range = new EmsAutoDust
                {
                    DevSystemCode = dev.SystemCode,
                    RangeMinValue = long.Parse(TxtRangeMinValue.Text),
                    RangeMaxValue = long.Parse(TxtRangeMaxValue.Text)
                };
                _context.AddOrUpdate(range);
                MessageBox.Show("添加成功。");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void UpdateDevice(object sender, RoutedEventArgs args)
        {
            try
            {
                var device = ConbineDevice();
                device.code = _deviceCode;
                var service = new UnicomService();
                var result = service.PushDeviceStatus(new[] { device });
                if (result.result.Length <= 0)
                {
                    MessageBox.Show("No Result");
                    var dev = ConbineStoreDevice(device);
                    dev.Id = _deviceId;
                    _context.AddOrUpdate(dev);
                    MessageBox.Show("更新成功。");
                    return;
                }
                if (result.result[0].value.ToString().Contains("ERROR"))
                {
                    MessageBox.Show(result.result[0].value.ToString());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void LoadDevice(object sender, RoutedEventArgs args)
        {
            _deviceId = long.Parse(CmbDeviceList.SelectedValue.ToString());
            var device = _context.Devices.FirstOrDefault(dev => dev.Id == _deviceId);
            if (device == null) return;
            _deviceCode = device.code;
            TxtDeviceName.Text = device.name;
            TxtIpAddress.Text = device.ipAddr;
            TxtMacAddress.Text = device.macAddr;
            TxtPort.Text = device.port;
            TxtDeviceVersion.Text = device.version;
            CmbProject.SelectedValue = device.projectCode;
            TxtLongitude.Text = device.longitude;
            TxtLatitude.Text = device.latitude;
            DpStartDate.SelectedDate = device.startDate;
            DpEndDate.SelectedDate = device.endDate;
            DpInstallDate.SelectedDate = device.endDate;
            CbOnlineStatus.IsChecked = true;
            TxtVideoUrl.Text = device.videoUrl;
            TxtSystemCode.Text = device.SystemCode;
        }
    }
}
