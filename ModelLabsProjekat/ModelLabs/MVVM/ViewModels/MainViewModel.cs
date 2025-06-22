using FTN.Common;
using FTN.ESI.SIMES.CIM.CIMAdapter;
using MVVM.Helpers;
using System.Collections.ObjectModel;

namespace MVVM.ViewModels
{
    public class MainViewModel:BindableBase
    {
        private bool isNMSPopulated;

        private TabItemViewModel selectedView;
        private CIMAdapter adapter = new CIMAdapter();
        private ModelResourcesDesc modelResourcesDesc = new ModelResourcesDesc();

        public ObservableCollection<TabItemViewModel> Views { get; } = new ObservableCollection<TabItemViewModel>();

        public MainViewModel()
        {
            Views.Add(new TabItemViewModel("Home",new HomeViewModel(adapter)));
            Views.Add(new TabItemViewModel("Get Values",new GetValuesViewModel(adapter, modelResourcesDesc)));
            Views.Add(new TabItemViewModel("Get Extent Values",new GetExtentValuesViewModel(adapter, modelResourcesDesc)));
            Views.Add(new TabItemViewModel("Get Related Values", new GetRelatedValuesViewModel(adapter,modelResourcesDesc)));
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