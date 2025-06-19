using FTN.Common;
using FTN.ESI.SIMES.CIM.CIMAdapter;
using MVVM.Commands;
using MVVM.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MVVM.ViewModels
{
    public class GetExtentValuesViewModel:BindableBase
    {
        public MyICommand GetExtentValuesCommand { get; set; }

        private List<List<Tuple<string, string>>> readedComplexValues;
        private ObservableCollection<Tuple<string, string>> readedValues;
        private int selectedResultNumber;
        private ObservableCollection<int> selectedResultNumbers=new ObservableCollection<int>();
        private List<DMSType> dmsTypes;
        private DMSType selectedDMSType;
        private string selectedAttributeToAdd;
        private string selectedAttributeToRemove;
        private ObservableCollection<string> selectedAttributes = new ObservableCollection<string>();
        private ObservableCollection<string> attributes =new ObservableCollection<string>();
        private CIMAdapter adapter;
        private ModelResourcesDesc modelResourcesDesc;

        public GetExtentValuesViewModel(CIMAdapter adapter, ModelResourcesDesc modelResourcesDesc)
        {
            this.adapter = adapter;
            this.modelResourcesDesc = modelResourcesDesc;
            GetExtentValuesCommand = new MyICommand(getExentValues);

            DMSTypes = modelResourcesDesc.AllDMSTypes.ToList();
            DMSTypes.Remove(DMSType.MASK_TYPE);

            if (DMSTypes.Count > 0)
            {
                SelectedDMSType = DMSTypes[1];
            }
        }

        private void getExentValues()
        {
            List<ModelCode> properties = new List<ModelCode>(SelectedAttributes.Select(x => (ModelCode)Enum.Parse(typeof(ModelCode), x)).ToList());


            if (selectedAttributes.Count == 0)
            {
                MessageBox.Show("You must select atleast 1 attribute", "No selected attributes", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            SelectedResultNumbers = new ObservableCollection<int>();

            GetExtentValuesCommand gvc = new GetExtentValuesCommand(modelResourcesDesc);

            readedComplexValues = gvc.GetExtentValues(selectedDMSType, properties);


            if (readedComplexValues.Count == 0)
            {
               MessageBox.Show("With the selected options there, NMS sent back no result", "No readed value", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            for(int i = 1; i < readedComplexValues.Count + 1; i++)
            {
                SelectedResultNumbers.Add(i);
            }
                SelectedResultNumber = 1;


        }


        private void updateAttributes()
        {
            if (SelectedDMSType != 0)
            {
                Attributes = new ObservableCollection<string>(modelResourcesDesc.GetAllPropertyIds(SelectedDMSType).Select((x) => x.ToString()).ToList());
                SelectedAttributes = new ObservableCollection<string>();
            }
        }

        public ObservableCollection<Tuple<string, string>> ReadedValues
        {
            get => readedValues;
            set
            {
                SetProperty(ref readedValues, value);
            }
        }

        public int SelectedResultNumber
        {
            get => selectedResultNumber;
            set
            {
                SetProperty(ref  selectedResultNumber, value);
                if (readedComplexValues.Count == 0)
                {
                    return;
                }

                ReadedValues = new ObservableCollection<Tuple<string, string>>(readedComplexValues[SelectedResultNumber-1]);
            }
        }

        public ObservableCollection<int> SelectedResultNumbers
        {
            get => selectedResultNumbers;
            set
            {
                SetProperty(ref selectedResultNumbers, value);
            }
        }


        public string SelectedAttributeToRemove
        {
            get => selectedAttributeToRemove;
            set
            {
                if (value != null)
                {
                    if (value != string.Empty)
                    {
                        SelectedAttributes.Remove(value);
                        Attributes.Add(value);
                        selectedAttributeToRemove = string.Empty;
                    }
                }
            }
        }

        public string SelectedAttributeToAdd
        {
            get => selectedAttributeToAdd;
            set
            {
                if (value != null)
                {
                    if (value != string.Empty)
                    {
                        Attributes.Remove(value);
                        SelectedAttributes.Add(value);
                        selectedAttributeToAdd = string.Empty;
                    }
                }
            }
        }

        public List<DMSType> DMSTypes
        {
            get => dmsTypes;
            set
            {
                SetProperty(ref dmsTypes, value);
            }
        }

        public ObservableCollection<string> SelectedAttributes
        {
            get => selectedAttributes;
            set
            {
                SetProperty(ref selectedAttributes, value);
            }
        }

        public ObservableCollection<string> Attributes
        {
            get => attributes;
            set
            {
                SetProperty(ref attributes, value);
            }
        }



        public DMSType SelectedDMSType
        {
            get => selectedDMSType;
            set
            {
                SetProperty(ref selectedDMSType, value);
                updateAttributes();
                SelectedResultNumbers = new ObservableCollection<int>();
                ReadedValues = new ObservableCollection<Tuple<string, string>>();
            }
        }
    }
}
