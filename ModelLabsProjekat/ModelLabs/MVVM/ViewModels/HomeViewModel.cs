using FTN.Common;
using FTN.ESI.SIMES.CIM.CIMAdapter;
using MVVM.Commands;
using MVVM.Helpers;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows;

namespace MVVM.ViewModels
{
    public class HomeViewModel:BindableBase
    {
        private string selectedFile=string.Empty;
        private Visibility selectedFileVisibilty=Visibility.Hidden;

        private CIMAdapter adapter;
        private ModelResourcesDesc mrd = new ModelResourcesDesc();

        public MyICommand SelectFileCommand { get; set; }
        public MyICommand ApplyInsertDelta { get; set; }

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
            string log;
            bool success = false;
            Delta nmsDelta = null;
            MatchCollection matches;
            List<string> serverIDs = new List<string>();

            success = CIMXMLToNMSDeltaCommand.ConvertCIMXMLToDMSNetworkModelDelta(selectedFile,out nmsDelta, adapter, FTN.ESI.SIMES.CIM.CIMAdapter.Manager.SupportedProfiles.Projekat9);

            if (!success)
            {
                MessageBox.Show("Can't convert CIMXML file to DMS Network Model Delta", "Error with CIMXML", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            success = ApplyDeltaCommand.ApplyDMSNetworkModelDeltaCommand(nmsDelta, adapter,out log);

            if (!success)
            {
                MessageBox.Show("Can't apply delta to Network Model Service", "Error with applying delta", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            matches = Regex.Matches(log, @"Server globalId:\s+(0x[0-9a-fA-F]+)");

            foreach (Match match in matches)
            {
                ushort typeVal;

                string hex = match.Groups[1].Value;
                hex = hex.Replace("0x", "");
                hex = hex.Replace("\"", "");

                long id = Convert.ToInt64(hex,16);

                unchecked
                {
                    typeVal = (ushort)((id >> 32) & 0xFFFF);
                }

                DMSType type =(DMSType)typeVal;

                if (type != DMSType.MASK_TYPE)
                {
                    if (!AvailableGIDViewModel.AvailableGIDs[type].Contains(id.ToString())) 
                    {
                        AvailableGIDViewModel.AvailableGIDs[type].Add(id.ToString());
                    }
                }
            }

            nmsDelta = null;
            SelectedFile = string.Empty;
            SelectedFileVisibility= Visibility.Hidden;

            MessageBox.Show("Successfully applied delta to Network Model Serivce","Success",MessageBoxButton.OK,MessageBoxImage.Information);
        }

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
    }
}