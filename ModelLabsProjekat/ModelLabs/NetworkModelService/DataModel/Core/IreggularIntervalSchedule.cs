using FTN.Common;
using System.Collections.Generic;

namespace FTN.Services.NetworkModelService.DataModel.Core
{
    public class IreggularIntervalSchedule : BasicIntervalSchedule
    {

        private List<long> timePoints;

        public IreggularIntervalSchedule(long globalId) : base(globalId)
        {
        }

        public List<long> TimePoints { get => timePoints; set => timePoints = value; }

        public override bool Equals(object obj)
        {
            if (base.Equals(obj))
            {
                IreggularIntervalSchedule x = (IreggularIntervalSchedule)obj;

                return CompareHelper.CompareLists(x.TimePoints, this.TimePoints, true);
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
                case ModelCode.IRISCH_ITP:
                    return true;

                default:
                    return base.HasProperty(property);  
            }
        }

        public override void GetProperty(Property property)
        {
            switch (property.Id)
            {
                case ModelCode.IRISCH_ITP:
                    property.SetValue(TimePoints);
                    break;

                default:
                    base.GetProperty(property);
                    break;
            }
        }

        public override void SetProperty(Property property)
        {
            base.SetProperty(property);
        }

        #endregion IAccess implementation

        #region IReference implementation

        public override bool IsReferenced
        {
            get
            {
                return (TimePoints.Count > 0) || base.IsReferenced;
            }
        }

        public override void GetReferences(Dictionary<ModelCode, List<long>> references, TypeOfReference refType)
        {
            if (TimePoints != null && TimePoints.Count > 0 && (refType == TypeOfReference.Target || refType == TypeOfReference.Both))
            {
                references[ModelCode.IRISCH_ITP] = timePoints.GetRange(0, timePoints.Count);
            }

            base.GetReferences(references, refType);
        }

        public override void AddReference(ModelCode referenceId, long globalId)
        {
            switch (referenceId)
            {
                case ModelCode.ITP_IRISCH:
                    timePoints.Add(globalId);
                    break;

                default:
                    base.AddReference(referenceId, globalId);
                    break;
            }
        }

        public override void RemoveReference(ModelCode referenceId, long globalId)
        {
            switch (referenceId)
            {
                case ModelCode.ITP_IRISCH:

                    if (timePoints.Contains(globalId))
                    {
                        timePoints.Remove(globalId);
                    }
                    else
                    {
                        CommonTrace.WriteTrace(CommonTrace.TraceWarning, "Entity (GID = 0x{0:x16}) doesn't contain reference 0x{1:x16}.", this.GlobalId, globalId);
                    }

                    break;
                default:
                    base.RemoveReference(referenceId, globalId);
                    break;
            }
        }

        #endregion IReference implementation
    }
}
