using FTN.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTN.Services.NetworkModelService.DataModel.Core
{
    public class IrregularTimePoint : IdentifiedObject
    {

        private long intervalSchedule;
        private float time;
        private float value1;
        private float value2;

        public IrregularTimePoint(long globalId) : base(globalId)
        {
        }

        public float Time { get => time; set => time = value; }
        public float Value1 { get => value1; set => value1 = value; }
        public float Value2 { get => value2; set => value2 = value; }
        public long IntervalSchedule { get => intervalSchedule; set => intervalSchedule = value; }

        public override bool Equals(object obj)
        {
            if (base.Equals(obj))
            {
                IrregularTimePoint x = (IrregularTimePoint)obj;

                return (x.time == this.time && x.intervalSchedule == this.intervalSchedule && x.value1==this.value1 && x.value2 == this.value2);
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
                case ModelCode.ITP_IRISCH:
                case ModelCode.ITP_TIME:
                case ModelCode.ITP_VAL1:
                case ModelCode.ITP_VAL2:
                    return true;

                default:
                    return base.HasProperty(property);
            }
        }

        public override void GetProperty(Property property)
        {
            switch (property.Id)
            {
                case ModelCode.ITP_IRISCH:
                    property.SetValue(IntervalSchedule);
                    break;

                case ModelCode.ITP_TIME:
                    property.SetValue(Time);
                    break;

                case ModelCode.ITP_VAL1:
                    property.SetValue(Value1);
                    break;

                case ModelCode.ITP_VAL2:
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
                case ModelCode.ITP_IRISCH:
                    IntervalSchedule = property.AsReference();
                    break;

                case ModelCode.ITP_TIME:
                    time = property.AsLong();
                    break;

                case ModelCode.ITP_VAL1:
                    value1 = property.AsLong();
                    break;

                case ModelCode.ITP_VAL2:
                    value2 = property.AsLong();
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
                references[ModelCode.SWITCH_SWOP] = new List<long> { IntervalSchedule };
            }

            base.GetReferences(references, refType);
        }

        #endregion IReference implementation

    }
}
