using FTN.Common;
using FTN.ServiceContracts;
using MVVM.Helpers;
using MVVM.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVM.Commands
{
    public class GetExtentValuesCommand
    {
        private ModelResourcesDesc modelResourcesDesc;

        private NetworkModelGDAProxy gdaQueryProxy = null;


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

        public GetExtentValuesCommand(ModelResourcesDesc modelResourcesDesc)
        {
            this.modelResourcesDesc = modelResourcesDesc;
        }


        public List<List<Tuple<string, string>>> GetExtentValues(DMSType type, List<ModelCode> properties)
        {
            int iteratorID = 0;
            try
            {

                ModelCode code = modelResourcesDesc.GetModelCodeFromType(type);

                iteratorID = GdaQueryProxy.GetExtentValues(code, properties);

                List<ResourceDescription> results = new List<ResourceDescription>();

                int resultLeft = GdaQueryProxy.IteratorResourcesLeft(iteratorID);

                if (resultLeft == 0) { return new List<List<Tuple<string, string>>>(); }

                while(resultLeft > 0 )
                {
                    List<ResourceDescription> result = GdaQueryProxy.IteratorNext(500,iteratorID);
                    results.AddRange(result);
                    resultLeft = GdaQueryProxy.IteratorResourcesLeft(iteratorID);
                }


                List<List<Tuple<string, string>>> sendingBackValue =new List<List<Tuple<string, string>>>();

                foreach (ResourceDescription val in results)
                {
                    UpdateAvailableGIds( val);
                    sendingBackValue.Add(ResourceDescriptionToListOfStrings.ConvertToListOfStrings(val));
                }


                return sendingBackValue;
            }
            catch (Exception ex) { }

            return new List<List<Tuple<string, string>>>();
        }


        private void UpdateAvailableGIds(ResourceDescription rd)
        {
            if (rd == null) { return; }

            ushort typeVal;
            unchecked
            {
                typeVal = (ushort)((rd.Id >> 32) & 0xFFFF);
            }


            DMSType type = (DMSType)typeVal;
            ModelCode code = modelResourcesDesc.GetModelCodeFromType(type);

            if (type != DMSType.MASK_TYPE)
            {
                if (!AvailableGIDViewModel.AvailableGIDs[type].Contains(rd.Id.ToString()))
                {
                    AvailableGIDViewModel.AvailableGIDs[type].Add(rd.Id.ToString());
                }
            }
        }
    }
}

