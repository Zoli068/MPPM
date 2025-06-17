using FTN.Common;
using FTN.ESI.SIMES.CIM.CIMAdapter;
using MVVM.Commands;
using MVVM.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MVVM.ViewModels
{
    public class HomeViewModel:BindableBase
    {
        private CIMAdapter adapter;
        private Visibility selectedFileVisibilty=Visibility.Hidden;
        private string selectedFile=string.Empty;

        public MyICommand SelectFileCommand { get; set; }
        public MyICommand ApplyInsertDelta { get; set; }

        public Visibility SelectedFileVisibility
        {
            get => selectedFileVisibilty;
            set
            {
                SetProperty(ref selectedFileVisibilty, value);
            }
        }

        public string SelectedFile
        {
            get => selectedFile;
            set
            {
                if (value != string.Empty)
                {
                    SelectedFileVisibility = Visibility.Visible;
                }
                SetProperty(ref selectedFile, value);
            }
        }

        public HomeViewModel(CIMAdapter adapter)
        {
            this.adapter = adapter;
            SelectFileCommand = new MyICommand(OpenFile);
            ApplyInsertDelta = new MyICommand(InsertDelta);
        }


        private void OpenFile()
        {
            SelectedFile = FileCommand.SelectFile("Select CIM-XML file", "CIM-XML Files|*.xml;*.txt;*.rdf;*.xmi|All Files|*.*)");
        }

        private void InsertDelta()
        {
            bool success = false;
            Delta nmsDelta = null;

            success = CIMXMLToNMSDeltaCommand.ConvertCIMXMLToDMSNetworkModelDelta(selectedFile,out nmsDelta, adapter, FTN.ESI.SIMES.CIM.CIMAdapter.Manager.SupportedProfiles.Projekat9);

            if (!success)
            {
                MessageBox.Show("Can't convert CIMXML file to DMS Network Model Delta", "Error with CIMXML", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            success = false;

            success = ApplyDeltaCommand.ApplyDMSNetworkModelDeltaCommand(nmsDelta, adapter);

            if (!success)
            {
                MessageBox.Show("Can't apply delta to Network Model Service", "Error with applying delta", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            nmsDelta = null;
            SelectedFile = string.Empty;
            SelectedFileVisibility= Visibility.Hidden;

            MessageBox.Show("Successfully applied delta to Network Model Serivce","Success",MessageBoxButton.OK,MessageBoxImage.Information);
        }
    }
}
