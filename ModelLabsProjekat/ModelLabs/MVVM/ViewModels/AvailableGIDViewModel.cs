using FTN.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MVVM.Helpers;

namespace MVVM.ViewModels
{
    public class AvailableGIDViewModel:BindableBase
    {

        private DMSType selectedDMSType;
        private ObservableCollection<string> gidsToShow;

        public ObservableCollection<DMSType> DMSTypes { get; set; } = new ObservableCollection<DMSType>()
                                                                               {
                                                                                    DMSType.DISCONNECTOR,
                                                                                    DMSType.OUTAGESCHEDULE,
                                                                                    DMSType.REGULARTIMEPOINT,
                                                                                    DMSType.IRREGULARTIMEPOINT,
                                                                                    DMSType.REGULARINTERVALSCHEDULE,
                                                                                    DMSType.SWITCHINGOPERATION
                                                                                };

        public ObservableCollection<string> GIDsToShow
        {
            get => gidsToShow;
            set
            {
                SetProperty(ref gidsToShow, value);
            }
        }


        public AvailableGIDViewModel()
        {
            SelectedDMSType = DMSType.DISCONNECTOR;
            GIDsToShow = AvailableGIDs[SelectedDMSType];
        }

        public DMSType SelectedDMSType
        {
            get => selectedDMSType;
            set
            {
                SetProperty(ref selectedDMSType, value);
                GIDsToShow = AvailableGIDs[value];
            }
        }

        public static Dictionary<DMSType, ObservableCollection<string>> AvailableGIDs { get; set; } = new Dictionary<DMSType, ObservableCollection<string>>
        {
             {DMSType.OUTAGESCHEDULE, new ObservableCollection<string>()},
             {DMSType.SWITCHINGOPERATION, new ObservableCollection<string>()},
             {DMSType.DISCONNECTOR, new ObservableCollection<string>()},
             {DMSType.REGULARINTERVALSCHEDULE, new ObservableCollection<string>()},
             {DMSType.IRREGULARTIMEPOINT, new ObservableCollection<string>()},
             {DMSType.REGULARTIMEPOINT, new ObservableCollection<string>()},
        };
    }
}
