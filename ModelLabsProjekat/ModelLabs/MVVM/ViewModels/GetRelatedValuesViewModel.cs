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
    public class GetRelatedValuesViewModel:BindableBase
    {
        public MyICommand GetRelatedValuesCommand { get; set; } 

        private int selectedResultNumber;
        private ObservableCollection<int> resultNumbers=new ObservableCollection<int>();
        private List<DMSType> dmsTypes;
        private DMSType selectedDMSType;
        private CIMAdapter adapter;
        private ModelResourcesDesc modelResourcesDesc;
        private List<List<Tuple<string, string>>> readedComplexValues;
        private ObservableCollection<Tuple<string, string>> readedValues;
        private ObservableCollection<string> selectedAttributes = new ObservableCollection<string>();
        private ObservableCollection<string> attributes = new ObservableCollection<string>();
        private ObservableCollection<string> selectableRefAttributes=new ObservableCollection<string>();
        private string selectedRefAttribute=string.Empty;
        private string selectedAttributeToAdd = string.Empty;
        private string selectedAttributeToRemove = string.Empty;
        private string gidValue = string.Empty;

        public GetRelatedValuesViewModel(CIMAdapter adapter, ModelResourcesDesc modelResourcesDesc)
        {
            this.adapter = adapter;
            this.modelResourcesDesc = modelResourcesDesc;

            GetRelatedValuesCommand = new MyICommand(getRelatedValues);

            DMSTypes = modelResourcesDesc.AllDMSTypes.ToList();
            DMSTypes.Remove(DMSType.MASK_TYPE);

            if (DMSTypes.Count > 0)
            {
                SelectedDMSType = DMSTypes[0];
            }
        }

        private void getRelatedValues()
        {

            Association association = new Association();

            if (SelectedRefAttribute == string.Empty)
            {
                MessageBox.Show("You must select a ref attribute", "No selected reference", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            association.PropertyId = (ModelCode)Enum.Parse(typeof(ModelCode), SelectedRefAttribute);
            association.Type=GetTargetingModelCode.GetTargetingPropertyOwnersModelCode((ModelCode)Enum.Parse(typeof(ModelCode), SelectedRefAttribute));

            List<ModelCode> properties = new List<ModelCode>(SelectedAttributes.Select(x => (ModelCode)Enum.Parse(typeof(ModelCode), x)).ToList());

            if (selectedAttributes.Count == 0)
            {
                MessageBox.Show("You must select atleast 1 attribute", "No selected attributes", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            long gid;

            if (long.TryParse(gidValue, out gid))
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
            
            GetRelatedValuesCommand getRelatedValuesCommand = new GetRelatedValuesCommand(modelResourcesDesc);

            readedComplexValues=getRelatedValuesCommand.GetRelatedValues(gid,association,properties);

            if (readedComplexValues.Count == 0)
            {
                MessageBox.Show("With the selected options there, NMS sent back no result", "No readed value", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            for (int i = 1; i < readedComplexValues.Count + 1; i++)
            {
                ResultNumbers.Add(i);
            }

            SelectedResultNumber = 1;

        }

        private void getRefAttributes()
        {
            List<ModelCode> res=modelResourcesDesc.GetPropertyIds(selectedDMSType,PropertyType.Reference);
            res.AddRange(modelResourcesDesc.GetPropertyIds(selectedDMSType, PropertyType.ReferenceVector));

            SelectableRefAttributes = new ObservableCollection<string>(res.Select(x => x.ToString()).ToList());


            if (SelectableRefAttributes.Count > 0)
            {
                getAttributesForSelectedRef();
            }
            else
            {
                SelectedAttributes = new ObservableCollection<string>();
                Attributes = new ObservableCollection<string>();
                SelectedAttributeToAdd = string.Empty;
                SelectedAttributeToRemove = string.Empty;
            }
        }

        private void getAttributesForSelectedRef()
        {
            if (SelectedRefAttribute == string.Empty || SelectedRefAttribute == null)        
            {
                SelectedAttributes = new ObservableCollection<string>();
                Attributes = new ObservableCollection<string>();
                SelectedAttributeToAdd = string.Empty;
                SelectedAttributeToRemove = string.Empty;
                return;        
            }

            ModelCode referencedCode = GetTargetingModelCode.GetTargetingPropertyOwnersModelCode((ModelCode)Enum.Parse(typeof(ModelCode), SelectedRefAttribute));
            List<ModelCode> res = modelResourcesDesc.GetAllPropertyIds(referencedCode);

            Attributes= new ObservableCollection<string>(res.Select(x => x.ToString()).ToList());
        }

        private void resetSelectedValues()
        {
            SelectedAttributes.Clear();
            SelectableRefAttributes.Clear();
            SelectedRefAttribute = string.Empty;
            SelectedAttributeToAdd = string.Empty;
            SelectedAttributeToRemove = string.Empty;
            GIDValue= string.Empty;
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
                SetProperty(ref selectedResultNumber, value);
                if (readedComplexValues.Count == 0)
                {
                    return;
                }

                ReadedValues = new ObservableCollection<Tuple<string, string>>(readedComplexValues[SelectedResultNumber - 1]);
            }
        }

        public ObservableCollection<int> ResultNumbers
        {
            get => resultNumbers;
            set
            {
                SetProperty(ref resultNumbers, value);
            }
        }

        public string GIDValue
        {
            get => gidValue;
            set
            {
                SetProperty(ref gidValue, value);
            }
        }

        public string SelectedAttributeToAdd
        {
            get => selectedAttributeToAdd;
            set
            {
                SetProperty(ref selectedAttributeToAdd, value);
                if (value != null)
                {
                    if (value.Trim() != string.Empty)
                    {
                        SelectedAttributes.Add(value);
                        Attributes.Remove(value);
                        selectedAttributeToAdd = string.Empty;
                    }
                }
            }
        }

        public string SelectedAttributeToRemove
        {
            get => selectedAttributeToRemove;
            set
            {
                SetProperty(ref selectedAttributeToRemove, value);
                if (value != null)
                {
                    if (value.Trim() != string.Empty)
                    {
                        SelectedAttributes.Remove(value);
                        Attributes.Add(value);
                        selectedAttributeToRemove = string.Empty;
                    }
                }
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

        public ObservableCollection<string> SelectableRefAttributes
        {
            get => selectableRefAttributes;
            set
            {

                SetProperty(ref selectableRefAttributes, value);
            }
        }

        public string SelectedRefAttribute
        {
            get => selectedRefAttribute;
            set
            {
                SetProperty(ref selectedRefAttribute, value);
                if(value!= string.Empty)
                {
                    SelectedAttributes = new ObservableCollection<string>();
                    Attributes = new ObservableCollection<string>();
                    SelectedAttributeToAdd = string.Empty;
                    SelectedAttributeToRemove = string.Empty;
                    ResultNumbers = new ObservableCollection<int>();
                    ReadedValues = new ObservableCollection<Tuple<string, string>>();
                    getAttributesForSelectedRef();
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

        public DMSType SelectedDMSType
        {
            get => selectedDMSType;
            set
            {
                SetProperty(ref selectedDMSType, value);
                resetSelectedValues();
                getRefAttributes();
                ResultNumbers = new ObservableCollection<int>();
                ReadedValues = new ObservableCollection<Tuple<string, string>>();
            }
        }
    }
}
