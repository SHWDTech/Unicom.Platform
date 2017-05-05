using System;
using System.Globalization;
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

            foreach (var project in _context.Projects)
            {
                CmbProjectList.Items.Add(new { Key = project.name, Value = project.Id });
            }

            TxtShortTitle.Text = AppConfig.ShortTitle;
        }

        private void LoadProject(object sender, RoutedEventArgs e)
        {
            if (!long.TryParse(CmbProjectList.SelectedValue.ToString(), out long prjId))
            {
                MessageBox.Show("获取工程ID失败！");
                return;
            }

            var prj = _context.Projects.FirstOrDefault(p => p.Id == prjId);
            if (prj == null)
            {
                MessageBox.Show("获取工程信息失败！");
                return;
            }
            TxtBjCode.Text = prj.code.Replace(AppConfig.ShortTitle, string.Empty);
            TxtPrjName.Text = prj.name;
            CmbDistrict.Text = prj.district;
            CmbPrjType.SelectedValue = prj.prjType;
            CmbPrjCategory.SelectedValue = prj.prjCategory;
            CmbPrjPeriod.SelectedValue = prj.prjPeriod;
            CmbRegion.SelectedValue = prj.region;
            TxtStreet.Text = prj.street;
            TxtLongitude.Text = prj.longitude;
            TxtLatitude.Text = prj.latitude;
            TxtContractors.Text = prj.contractors;
            TxtSuperintendent.Text = prj.superintendent;
            TxtTelephone.Text = prj.telephone;
            TxtAddress.Text = prj.address;
            TxtSiteArea.Text = prj.siteArea.ToString(CultureInfo.InvariantCulture);
            TxtBuildingArea.Text = prj.buildingArea.ToString(CultureInfo.InvariantCulture);
            DpStartDate.DisplayDate = prj.startDate;
            DpEndDate.DisplayDate = prj.endDate;
            TxtStage.Text = prj.stage;
            CbCompleted.IsChecked = prj.isCompleted;
        }

        private void Submit(object sender, RoutedEventArgs e)
        {
            try
            {
                var emsProject = new emsProject
                {
                    code = $"{TxtShortTitle.Text}{TxtBjCode.Text}",
                    name = $"{TxtPrjName.Text}",
                    district = $"{CmbDistrict.Text}",
                    prjType = int.Parse(CmbPrjType.SelectedValue.ToString()),
                    prjTypeSpecified = true,
                    prjCategory = int.Parse(CmbPrjCategory.SelectedValue.ToString()),
                    prjCategorySpecified = true,
                    prjPeriod = int.Parse(CmbPrjPeriod.SelectedValue.ToString()),
                    prjPeriodSpecified = true,
                    region = int.Parse(CmbRegion.SelectedValue.ToString()),
                    regionSpecified = true,
                    street = TxtStreet.Text,
                    longitude = TxtLongitude.Text,
                    latitude = TxtLatitude.Text,
                    contractors = TxtContractors.Text,
                    superintendent = TxtSuperintendent.Text,
                    telephone = TxtTelephone.Text,
                    address = TxtAddress.Text,
                    siteArea = float.Parse(TxtSiteArea.Text),
                    siteAreaSpecified = true,
                    buildingArea = float.Parse(TxtBuildingArea.Text),
                    buildingAreaSpecified = true,
                    startDate = DpStartDate.DisplayDate,
                    startDateSpecified = true,
                    endDate = DpEndDate.DisplayDate,
                    endDateSpecified = true,
                    stage = TxtStage.Text,
                    status = true,
                    statusSpecified = true,
                    isCompleted = CbCompleted.IsChecked == true,
                    isCompletedSpecified = true
                };

                var service = new UnicomService();
                var result = service.PushProjects(new[] { emsProject });
                if (result.result[0].value.ToString().Contains("ERROR"))
                {
                    MessageBox.Show(result.result[0].value.ToString());
                    return;
                }
                var prj = new EmsProject
                {
                    SystemCode = TxtSystemCode.Text,
                    onTransfer = false,
                    code = emsProject.code,
                    name = emsProject.name,
                    district = emsProject.district,
                    prjType = emsProject.prjType,
                    prjTypeSpecified = true,
                    prjCategory = emsProject.prjCategory,
                    prjCategorySpecified = true,
                    prjPeriod = emsProject.prjPeriod,
                    prjPeriodSpecified = true,
                    region = emsProject.region,
                    regionSpecified = true,
                    street = emsProject.street,
                    longitude = emsProject.longitude,
                    latitude = emsProject.latitude,
                    contractors = emsProject.contractors,
                    superintendent = emsProject.superintendent,
                    telephone = emsProject.telephone,
                    address = emsProject.address,
                    siteArea = emsProject.siteArea,
                    siteAreaSpecified = true,
                    buildingArea = emsProject.buildingArea,
                    buildingAreaSpecified = true,
                    startDate = emsProject.startDate,
                    startDateSpecified = true,
                    endDate = emsProject.endDate,
                    endDateSpecified = true,
                    stage = emsProject.stage,
                    status = emsProject.status,
                    statusSpecified = emsProject.statusSpecified,
                    isCompleted = emsProject.isCompleted,
                    isCompletedSpecified = true
                };
                _context.AddOrUpdate(prj);
                MessageBox.Show("添加成功。");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                var innerException = ex.InnerException;
                while (innerException != null)
                {
                    MessageBox.Show(innerException.Message);
                    innerException = innerException.InnerException;
                }
            }

        }
    }
}
