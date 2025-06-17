using FTN.Common;
using FTN.ESI.SIMES.CIM.CIMAdapter;
using MVVM.Commands;
using MVVM.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using static System.Net.Mime.MediaTypeNames;

namespace MVVM.ViewModels
{
    public class HomeViewModel:BindableBase
    {
        private CIMAdapter adapter;
        private Visibility selectedFileVisibilty=Visibility.Hidden;
        private string selectedFile=string.Empty;
        private ModelResourcesDesc mrd = new ModelResourcesDesc();

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

            string log;

            success = ApplyDeltaCommand.ApplyDMSNetworkModelDeltaCommand(nmsDelta, adapter,out log);

            List<string> serverIDs = new List<string>();
            MatchCollection matches = Regex.Matches(log, @"Server globalId:\s+(0x[0-9a-fA-F]+)");

            foreach (Match match in matches)
            {
                string hex = match.Groups[1].Value;
                hex = hex.Replace("0x", "");
                hex = hex.Replace("\"", "");
                long id = Convert.ToInt64(hex,16);
                ushort typeVal;

                unchecked
                {
                    typeVal = (ushort)((id >> 32) & 0xFFFF);
                }


                DMSType type =(DMSType)typeVal;
                ModelCode code = mrd.GetModelCodeFromType(type);

                if (type != DMSType.MASK_TYPE)
                {
                    if (!AvailableGIDViewModel.AvailableGIDs[type].Contains(id.ToString())) 
                    {
                        AvailableGIDViewModel.AvailableGIDs[type].Add(id.ToString());
                    }
                }
            }

            
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