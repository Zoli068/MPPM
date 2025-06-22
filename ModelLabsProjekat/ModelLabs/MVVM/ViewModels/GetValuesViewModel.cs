using FTN.Common;
using FTN.ESI.SIMES.CIM.CIMAdapter;
using MVVM.Commands;
using MVVM.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace MVVM.ViewModels
{
    class GetValuesViewModel : BindableBase
    {
        private string gidValue;
        private DMSType selectedDMSType;
        private string selectedAttributeToAdd;
        private string selectedAttributeToRemove;

        private List<DMSType> dmsTypes;

        private ObservableCollection<string> attributes;
        private ObservableCollection<Tuple<string, string>> readedValues;
        private ObservableCollection<string> selectedAttributes =new ObservableCollection<string>();
        private CIMAdapter adapter;
        
        private ModelResourcesDesc modelResourcesDesc;

        public MyICommand GetValuesCommand { get; set; }

        public GetValuesViewModel(CIMAdapter adapter, ModelResourcesDesc modelResourcesDesc)
        {
            this.adapter = adapter;
            this.modelResourcesDesc = modelResourcesDesc;

            GetValuesCommand = new MyICommand(getValues);

            DMSTypes = modelResourcesDesc.AllDMSTypes.ToList();   
            DMSTypes.Remove(DMSType.MASK_TYPE);

            if(DMSTypes.Count>0 )
            {
                SelectedDMSType = DMSTypes[1];
            }
        }
        
        private void getValues()
        {
            long gid=0;
            List<ModelCode> properties;
            GetValuesCommand gvc = new GetValuesCommand(modelResourcesDesc);

            if (selectedAttributes.Count == 0)
            {
                MessageBox.Show("You must select atleast 1 attribute", "No selected attributes", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if(long.TryParse(gidValue,out gid))
            {
                if (gid <= 0)
                {
                    MessageBox.Show("GID must be greater than zero", "Invalid GID Value", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
            }
            else
            {
                MessageBox.Show("GID must be number", "Invalid GID Value", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            properties = new List<ModelCode>(SelectedAttributes.Select(x => (ModelCode)Enum.Parse(typeof(ModelCode), x)).ToList());

            ReadedValues=new ObservableCollection<Tuple<string,string>>(gvc.GetValues(gid, properties));

            if (ReadedValues.Count() == 0)
            {
                MessageBox.Show("With the selected options there, NMS sent back no result", "No readed value", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
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
                SetProperty(ref readedValues,value);
            }
        }

        public string SelectedAttributeToRemove
        {
            get => selectedAttributeToRemove;
            set
            {
                if (value != null && value != string.Empty)
                {
                    SelectedAttributes.Remove(value);
                    Attributes.Add(value);
                }
            }
        }

        public string SelectedAttributeToAdd
        {
            get => selectedAttributeToAdd;
            set
            {
                if (value != null && value != string.Empty)
                {

                    Attributes.Remove(value);
                    SelectedAttributes.Add(value);
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

        public string GIDValue
        {
            get => gidValue;
            set
            {
                SetProperty(ref gidValue,value);
            }
        }

        public DMSType SelectedDMSType
        {
            get => selectedDMSType;
            set
            {
                SetProperty(ref selectedDMSType, value);

                updateAttributes();
                GIDValue = string.Empty;
                ReadedValues = new ObservableCollection<Tuple<string, string>>();
            }
        }
    }
}