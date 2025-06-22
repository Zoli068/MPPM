using FTN.Common;
using FTN.ServiceContracts;
using MVVM.Helpers;
using MVVM.ViewModels;
using System;
using System.Collections.Generic;

namespace MVVM.Commands
{
    public class GetExtentValuesCommand
    {
        private ModelResourcesDesc modelResourcesDesc;
        private NetworkModelGDAProxy gdaQueryProxy = null;

        public GetExtentValuesCommand(ModelResourcesDesc modelResourcesDesc)
        {
            this.modelResourcesDesc = modelResourcesDesc;
        }

        public List<List<Tuple<string, string>>> GetExtentValues(DMSType type, List<ModelCode> properties)
        {
            int iteratorID = 0;
            int resultLeft;
            ModelCode code;
            List<ResourceDescription> result;
            List<ResourceDescription> results = new List<ResourceDescription>();
            List<List<Tuple<string, string>>> sendingBackValue = new List<List<Tuple<string, string>>>();

            try
            {
                code = modelResourcesDesc.GetModelCodeFromType(type);
                iteratorID = GdaQueryProxy.GetExtentValues(code, properties);
                resultLeft = GdaQueryProxy.IteratorResourcesLeft(iteratorID);

                if (resultLeft == 0) { return new List<List<Tuple<string, string>>>(); }

                while(resultLeft > 0)
                {
                    result = GdaQueryProxy.IteratorNext(500,iteratorID);
                    results.AddRange(result);
                    resultLeft = GdaQueryProxy.IteratorResourcesLeft(iteratorID);
                }

                foreach (ResourceDescription val in results)
                {
                    UpdateAvailableGIds( val);
                    sendingBackValue.Add(ResourceDescriptionToListOfStrings.ConvertToListOfStrings(val));
                }

                return sendingBackValue;
            }
            catch (Exception) { }

            return new List<List<Tuple<string, string>>>();
        }

        private void UpdateAvailableGIds(ResourceDescription rd)
        {
            DMSType type;
            ModelCode code;
            ushort typeVal;

            if (rd == null) { return; }

            unchecked
            {
                typeVal = (ushort)((rd.Id >> 32) & 0xFFFF);
            }

            type = (DMSType)typeVal;

            code = modelResourcesDesc.GetModelCodeFromType(type);

            if (type != DMSType.MASK_TYPE)
            {
                if (!AvailableGIDViewModel.AvailableGIDs[type].Contains(rd.Id.ToString()))
                {
                    AvailableGIDViewModel.AvailableGIDs[type].Add(rd.Id.ToString());
                }
            }
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