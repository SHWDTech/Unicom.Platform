using System;
using System.Windows;
using Unicom.Platform;
using Unicom.Platform.Model;
using Unicom.Platform.Model.Service_References.UnicomPlatform;
using Unicom.Platform.SQLite;
using Unicom.Register.Common;
using EmsProject = Unicom.Platform.Model.Service_References.UnicomPlatform.EmsProject;

namespace Unicom.Register.Views
{
    /// <summary>
    /// Interaction logic for AddProject.xaml
    /// </summary>
    public partial class AddProject
    {
        private readonly UnicomContext _context = new UnicomContext(AppConfig.ConnectionString);

        public AddProject()
        {
            InitializeComponent();
            InitSelections();
        }

        private void InitSelections()
        {
            foreach (var emsPrjType in _context.PrjTypes)
            {
                CmbPrjType.Items.Add(new { Key = emsPrjType.Name, Value = emsPrjType.Code });
            }

            foreach (var emsPrjCategory in _context.PrjCategories)
            {
                CmbPrjCategory.Items.Add(new { Key = emsPrjCategory.Name, Value = emsPrjCategory.Code });
            }

            foreach (var emsPrjPeriod in _context.PrjPeriods)
            {
                CmbPrjPeriod.Items.Add(new { Key = emsPrjPeriod.Name, Value = emsPrjPeriod.Code });
            }

            foreach (var region in _context.Regions)
            {
                CmbRegion.Items.Add(new { Key = region.Name, Value = region.Code });
            }

            foreach (var district in _context.Districts)
            {
                CmbDistrict.Items.Add(new { Key = district.Name, Value = district.Code });
            }

            TxtShortTitle.Text = AppConfig.ShortTitle;
        }

        private void Submit(object sender, RoutedEventArgs e)
        {
            try
            {
                var emsProject = new EmsProject
                {
                    Code = $"{TxtShortTitle.Text}{TxtBjCode.Text}",
                    Name = $"{TxtPrjName.Text}",
                    District = $"{CmbDistrict.Text}",
                    PrjType = int.Parse(CmbPrjType.SelectedValue.ToString()),
                    PrjTypeSpecified = true,
                    PrjCategory = int.Parse(CmbPrjCategory.SelectedValue.ToString()),
                    PrjCategorySpecified = true,
                    PrjPeriod = int.Parse(CmbPrjPeriod.SelectedValue.ToString()),
                    PrjPeriodSpecified = true,
                    Region = int.Parse(CmbRegion.SelectedValue.ToString()),
                    RegionSpecified = true,
                    Street = TxtStreet.Text,
                    Longitude = TxtLongitude.Text,
                    Latitude = TxtLatitude.Text,
                    Contractors = TxtContractors.Text,
                    Superintendent = TxtSuperintendent.Text,
                    Telephone = TxtTelephone.Text,
                    Address = TxtAddress.Text,
                    SiteArea = float.Parse(TxtSiteArea.Text),
                    SiteAreaSpecified = true,
                    BuildingArea = float.Parse(TxtBuildingArea.Text),
                    BuildingAreaSpecified = true,
                    StartDate = DpStartDate.DisplayDate,
                    StartDateSpecified = true,
                    EndDate = DpEndDate.DisplayDate,
                    EndDateSpecified = true,
                    Stage = TxtStage.Text,
                    IsCompleted = CbCompleted.IsChecked == true,
                    IsCompletedSpecified = true
                };

                var service = new UnicomService();
                var result = service.PushProjects(new[] {emsProject});
                if (result.Result[0].Value.ToString().Contains("ERROR")) return;
                var project = new Platform.Model.EmsProject
                {
                    UnicomCode = result.Result[0].Value.ToString(),
                    SystemCode = TxtSystemCode.Text,
                    OnTransfer = false,
                    UnicomName = result.Result[0].Key.ToString(),
                    PrjType = int.Parse(CmbPrjType.SelectedValue.ToString())
                };
                _context.AddOrUpdate(project);
                MessageBox.Show("添加成功。");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }
    }
}
