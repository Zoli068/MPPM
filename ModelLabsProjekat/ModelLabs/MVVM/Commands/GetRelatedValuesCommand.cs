﻿using FTN.Common;
using FTN.ServiceContracts;
using MVVM.Helpers;
using MVVM.ViewModels;
using System;
using System.Collections.Generic;

namespace MVVM.Commands
{
    public class GetRelatedValuesCommand
    {
        private ModelResourcesDesc modelResourcesDesc;
        private NetworkModelGDAProxy gdaQueryProxy = null;

        public GetRelatedValuesCommand(ModelResourcesDesc modelResourcesDesc)
        {
            this.modelResourcesDesc = modelResourcesDesc;
        }

        public List<List<Tuple<string, string>>> GetRelatedValues(long source,Association association, List<ModelCode> properties)
        {
            int resultLeft;
            int iteratorID = 0;
            List<ResourceDescription> result;
            List<ResourceDescription> results = new List<ResourceDescription>();
            List<List<Tuple<string, string>>> sendingBackValue = new List<List<Tuple<string, string>>>();

            try
            {
                iteratorID = GdaQueryProxy.GetRelatedValues(source, properties, association);
                resultLeft = GdaQueryProxy.IteratorResourcesLeft(iteratorID);

                if (resultLeft == 0) { return new List<List<Tuple<string, string>>>(); }

                while (resultLeft > 0)
                {
                    result = GdaQueryProxy.IteratorNext(500, iteratorID);
                    results.AddRange(result);
                    resultLeft = GdaQueryProxy.IteratorResourcesLeft(iteratorID);
                }     

                foreach (ResourceDescription val in results)
                {
                    UpdateAvailableGIds(val);
                    sendingBackValue.Add(ResourceDescriptionToListOfStrings.ConvertToListOfStrings(val));
                }

                return sendingBackValue;
            }
            catch (Exception) { }

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