using MVVM.Helpers;
using MVVM.Views;
using System.Collections.ObjectModel;
using System.Windows;
using FTN.Common;
using FTN.ESI.SIMES.CIM.CIMAdapter;
using FTN.ServiceContracts;

namespace MVVM.ViewModels
{
    public class MainViewModel:BindableBase
    {
        private CIMAdapter adapter = new CIMAdapter();
        private ModelResourcesDesc modelResourcesDesc = new ModelResourcesDesc();

        private TabItemViewModel selectedView;
        private bool isNMSPopulated;

        public ObservableCollection<TabItemViewModel> Views { get; } = new ObservableCollection<TabItemViewModel>();

        public MainViewModel()
        {
            Views.Add(new TabItemViewModel("Home",new HomeViewModel(adapter)));
            Views.Add(new TabItemViewModel("Get Values",new GetValuesViewModel(adapter, modelResourcesDesc)));
            Views.Add(new TabItemViewModel("Get Extent Values",new GetExtentValuesViewModel(adapter, modelResourcesDesc)));
            Views.Add(new TabItemViewModel("Available GIDs", new AvailableGIDViewModel()));
            IsNMSPopulated = false;
        }



        public TabItemViewModel SelectedTab
        {
            get => selectedView;
            set
            {
                SetProperty(ref selectedView, value);
            }
        }

        public bool IsNMSPopulated
        {
            get => isNMSPopulated;
            set
            {
                SetProperty(ref isNMSPopulated, value);
            }
        }
    }
}