using FTN.Common;
using FTN.ESI.SIMES.CIM.CIMAdapter;
using FTN.ESI.SIMES.CIM.CIMAdapter.Manager;
using System;
using System.IO;
using System.Text;
using System.Xml;

namespace MVVM.Commands
{
    public static class CIMXMLToNMSDeltaCommand
    {
        public static bool ConvertCIMXMLToDMSNetworkModelDelta(string filePath,out Delta nmsDelta,CIMAdapter adapter,SupportedProfiles profile)
        {
            try
            {
                string log;
                nmsDelta = null;
                using (FileStream fs = File.Open(filePath, FileMode.Open))
                {
                    nmsDelta = adapter.CreateDelta(fs, profile, out log);
                }

                if (nmsDelta != null)
                {
                    using (XmlTextWriter xmlWriter = new XmlTextWriter(".\\deltaExport.xml", Encoding.UTF8))
                    {
                        xmlWriter.Formatting = Formatting.Indented;
                        nmsDelta.ExportToXml(xmlWriter);
                        xmlWriter.Flush();
                    }
                }
            }
            catch (Exception e)
            {
                nmsDelta = null;
                return false;
            }

            return true;
        }
    }
}