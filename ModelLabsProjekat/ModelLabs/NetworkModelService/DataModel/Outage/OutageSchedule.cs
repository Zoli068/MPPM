using FTN.Common;
using FTN.Services.NetworkModelService.DataModel.Core;
using System.Collections.Generic;

namespace FTN.Services.NetworkModelService.DataModel.Outage
{
    public class OutageSchedule : IreggularIntervalSchedule
    {
        
        private List<long> switchingOperations = new List<long>();
        
        public OutageSchedule(long globalId) : base(globalId)
        {
        }

        public List<long> SwitchingOperations { get => switchingOperations; set => switchingOperations = value; }

        public override bool Equals(object obj)
        {
            if (base.Equals(obj))
            {
                OutageSchedule x = (OutageSchedule)obj;

                return CompareHelper.CompareLists(x.SwitchingOperations, this.SwitchingOperations, true);
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
                case ModelCode.OUTSCH_SWOP:
                    return true;

                default:
                    return base.HasProperty(property);
            }
        }

        public override void GetProperty(Property property)
        {
            switch (property.Id)
            {
                case ModelCode.OUTSCH_SWOP:
                    property.SetValue(SwitchingOperations);
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
                return (SwitchingOperations.Count > 0) || base.IsReferenced;
            }
        }

        public override void GetReferences(Dictionary<ModelCode, List<long>> references, TypeOfReference refType)
        {
            if (SwitchingOperations != null && SwitchingOperations.Count > 0 && (refType == TypeOfReference.Target || refType == TypeOfReference.Both))
            {
                references[ModelCode.OUTSCH_SWOP] = switchingOperations.GetRange(0, switchingOperations.Count);
            }

            base.GetReferences(references, refType);
        }

        public override void AddReference(ModelCode referenceId, long globalId)
        {
            switch (referenceId)
            {
                case ModelCode.SWOP_OUTSCH:
                    switchingOperations.Add(globalId);
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
                case ModelCode.SWOP_OUTSCH:

                    if (switchingOperations.Contains(globalId))
                    {
                        switchingOperations.Remove(globalId);
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