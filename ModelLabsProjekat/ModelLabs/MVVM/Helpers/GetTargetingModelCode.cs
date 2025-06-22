using FTN.Common;
using System;

namespace MVVM.Helpers
{
    public static class GetTargetingModelCode
    {

        public static ModelCode GetTargetingPropertyOwnersModelCode(ModelCode referencingProperty)
        {
            switch (referencingProperty)
            {
                case ModelCode.OUTSCH_SWOP : return ModelCode.SWOP;
                case ModelCode.SWOP_OUTSCH : return ModelCode.OUTSCH;

                case ModelCode.SWITCH_SWOP : return ModelCode.SWOP;
                case ModelCode.SWOP_SWITCH : return ModelCode.SWITCH;

                case ModelCode.IRISCH_ITP  : return ModelCode.ITP;
                case ModelCode.ITP_IRISCH  : return ModelCode.IRISCH;

                case ModelCode.RISCH_RTP   : return ModelCode.RTP;
                case ModelCode.RTP_RISCH   : return ModelCode.RISCH;
            }

            throw new Exception();
        }
    }
}