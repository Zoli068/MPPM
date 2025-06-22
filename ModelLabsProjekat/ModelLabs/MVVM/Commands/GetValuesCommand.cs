using FTN.Common;
using FTN.ServiceContracts;
using MVVM.Helpers;
using System;
using System.Collections.Generic;

namespace MVVM.Commands
{
    public class GetValuesCommand
    {
        private ModelResourcesDesc modelResourcesDesc;
        private NetworkModelGDAProxy gdaQueryProxy = null;

        public GetValuesCommand(ModelResourcesDesc modelResourcesDesc )
        {
            this.modelResourcesDesc = modelResourcesDesc;
        }
   
        public List<Tuple<string, string>> GetValues(long gid,List<ModelCode> properties)
        {
            ResourceDescription rd = null;
            try
            {
                rd = GdaQueryProxy.GetValues(gid, properties);
            }
            catch (Exception) { }

            List<Tuple<string, string>> response = ResourceDescriptionToListOfStrings.ConvertToListOfStrings(rd);
            return response;
        }

        public NetworkModelGDAProxy GdaQueryProxy
        {
            get
            {
                if (gdaQueryProxy != null)
                {
                    gdaQueryProxy.Abort();
                    gdaQueryProxy = null;
                }

                gdaQueryProxy = new NetworkModelGDAProxy("NetworkModelGDAEndpoint");
                gdaQueryProxy.Open();

                return gdaQueryProxy;
            }
        }
    }
}
