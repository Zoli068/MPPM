using FTN.Common;
using System;
using System.Collections.Generic;

namespace FTN.Services.NetworkModelService.DataModel.Core
{
    public class RegularIntervalSchedule : BasicIntervalSchedule
    {

        private DateTime endTime;
        private float timeStep = 0;
        private List<long> timePoints;

        public RegularIntervalSchedule(long globalId) : base(globalId)
        {
        }

        public DateTime EndTime { get => endTime; set => endTime = value; }
        public float TimeStep { get => timeStep; set => timeStep = value; }
        public List<long> TimePoints { get => timePoints; set => timePoints = value; }

        public override bool Equals(object obj)
        {
            if (base.Equals(obj))
            {
                RegularIntervalSchedule x = (RegularIntervalSchedule)obj;

                return (x.endTime == this.endTime && x.timeStep == this.timeStep && CompareHelper.CompareLists(x.TimePoints, this.TimePoints, true));
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
                case ModelCode.RISCH_ENDTIME:
                case ModelCode.RISCH_RTP:
                case ModelCode.RISCH_TIMESTEP:
                    return true;

                default:
                    return base.HasProperty(property);
            }
        }

        public override void GetProperty(Property property)
        {
            switch (property.Id)
            {
                case ModelCode.RISCH_ENDTIME:
                    property.SetValue(endTime);
                    break;

                case ModelCode.RISCH_RTP:
                    property.SetValue(TimePoints);
                    break;

                case ModelCode.RISCH_TIMESTEP:
                    property.SetValue(timeStep);
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
                case ModelCode.RISCH_ENDTIME:
                    endTime = property.AsDateTime();
                    break;

                case ModelCode.RISCH_TIMESTEP:
                    timeStep = property.AsFloat();
                    break;

                default:
                    base.SetProperty(property);
                    break;
            }
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
                references[ModelCode.RISCH_RTP] = timePoints.GetRange(0, timePoints.Count);
            }

            base.GetReferences(references, refType);
        }

        public override void AddReference(ModelCode referenceId, long globalId)
        {
            switch (referenceId)
            {
                case ModelCode.RTP_RISCH:
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
                case ModelCode.RTP_RISCH:

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