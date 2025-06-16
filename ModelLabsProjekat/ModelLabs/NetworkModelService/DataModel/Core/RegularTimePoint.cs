using FTN.Common;
using System.Collections.Generic;

namespace FTN.Services.NetworkModelService.DataModel.Core
{
    public class RegularTimePoint : IdentifiedObject
    {

        private long intervalSchedule=0;
        private int sequenceNumber;
        private float value1;
        private float value2;

        public RegularTimePoint(long globalId) : base(globalId)
        {
        }

        public int SequenceNumber { get => sequenceNumber; set => sequenceNumber = value; }
        public float Value1 { get => value1; set => value1 = value; }
        public float Value2 { get => value2; set => value2 = value; }
        public long IntervalSchedule { get => intervalSchedule; set => intervalSchedule = value; }

        public override bool Equals(object obj)
        {
            if (base.Equals(obj))
            {
                RegularTimePoint x = (RegularTimePoint)obj;

                return (x.sequenceNumber == this.sequenceNumber && x.intervalSchedule == this.intervalSchedule && x.value1 == this.value1 && x.value2 == this.value2);
            }
            else
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        #region IAccess implementation

        public override bool HasProperty(ModelCode property)
        {
            switch (property)
            {
                case ModelCode.RTP_RISCH:
                case ModelCode.RTP_SEQ:
                case ModelCode.RTP_VAL1:
                case ModelCode.RTP_VAL2:
                    return true;

                default:
                    return base.HasProperty(property);
            }
        }

        public override void GetProperty(Property property)
        {
            switch (property.Id)
            {
                case ModelCode.RTP_RISCH:
                    property.SetValue(IntervalSchedule);
                    break;

                case ModelCode.RTP_SEQ:
                    property.SetValue(SequenceNumber);
                    break;

                case ModelCode.RTP_VAL1:
                    property.SetValue(Value1);
                    break;

                case ModelCode.RTP_VAL2:
                    property.SetValue(Value2);
                    break;

                default:
                    base.GetProperty(property);
                    break;
            }
        }

        public override void SetProperty(Property property)
        {
            switch (property.Id)
            {
                case ModelCode.RTP_RISCH:
                    intervalSchedule = property.AsReference();
                    break;

                case ModelCode.RTP_SEQ:
                    sequenceNumber = property.AsInt();
                    break;

                case ModelCode.RTP_VAL1:
                    value1 = property.AsFloat();
                    break;

                case ModelCode.RTP_VAL2:
                    value2 = property.AsFloat();
                    break;

                default:
                    base.SetProperty(property);
                    break;
            }
        }

        #endregion

        #region IReference implementation

        public override void GetReferences(Dictionary<ModelCode, List<long>> references, TypeOfReference refType)
        {
            if (IntervalSchedule != 0 && (refType != TypeOfReference.Reference || refType != TypeOfReference.Both))
            {
                references[ModelCode.RTP_RISCH] = new List<long> { IntervalSchedule };
            }

            base.GetReferences(references, refType);
        }

        #endregion IReference implementation

    }
}