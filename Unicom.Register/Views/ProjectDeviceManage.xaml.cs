using System.Collections.Generic;
using System.Windows;
using Unicom.Platform.Model;
using Unicom.Platform.SQLite;
using Unicom.Register.Common;

namespace Unicom.Register.Views
{
    /// <summary>
    /// Interaction logic for ProjectDeviceManage.xaml
    /// </summary>
    public partial class ProjectDeviceManage
    {
        private readonly UnicomContext _context = new UnicomContext(AppConfig.ConnectionString);

        private readonly Dictionary<long, bool> _projectOnTransfer = new Dictionary<long, bool>();

        private readonly Dictionary<long, bool> _deviceOnTransfer = new Dictionary<long, bool>();

        public ProjectDeviceManage()
        {
            InitializeComponent();
            LoadProjectsAndDevices();
        }

        private void LoadProjectsAndDevices()
        {
            foreach (var project in _context.Projects)
            {
                _projectOnTransfer.Add(project.Id, project.onTransfer);
                CmbProjects.Items.Add(new { Key = project.Id, Value = project.name });
            }
            CmbProjects.SelectedIndex = 0;
            foreach (var device in _context.Devices)
            {
                _deviceOnTransfer.Add(device.Id, device.OnTransfer);
                CmbDevices.Items.Add(new { Key = device.Id, Value = device.name });
            }
            CmbDevices.SelectedIndex = 0;
        }

        private void SwitchProjectsTransfer(object sender, RoutedEventArgs e)
        {
            var projectId = (long) CmbProjects.SelectedValue;
            var project = _context.FirstOrDefault<EmsProject>($"Id = {projectId}");
            project.onTransfer = !project.onTransfer;
            var result = _context.AddOrUpdate(project);
            if (result > 0)
            {
                LblProjectOnTransfer.Content = project.onTransfer;
            }
        }

        private void ProjectOnSelect(object sender, RoutedEventArgs e)
        {
            var projectId = (long) CmbProjects.SelectedValue;
            LblProjectOnTransfer.Content = _projectOnTransfer[projectId];
        }

        private void SwitchDevicesTransfer(object sender, RoutedEventArgs e)
        {
            var deviceId = (long)CmbDevices.SelectedValue;
            var project = _context.FirstOrDefault<EmsDevice>($"Id = {deviceId}");
            project.OnTransfer = !project.OnTransfer;
            var result = _context.AddOrUpdate(project);
            if (result > 0)
            {
                LblDeviceOnTransfer.Content = project.OnTransfer;
            }
        }

        private void DeviceOnSelect(object sender, RoutedEventArgs e)
        {
            var deviceId = (long)CmbDevices.SelectedValue;
            LblDeviceOnTransfer.Content = _deviceOnTransfer[deviceId];
        }
    }
}
