using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using FTN.Common;
using FTN.Services.NetworkModelService.DataModel.Core;

namespace FTN.Services.NetworkModelService.DataModel.Wires
{
    public class Switch : ConductingEquipment
    {
        private long switchingOperations;

        public Switch(long globalId) : base(globalId)
        {
        }

        public long SwitchingOperations {  get => switchingOperations; set => switchingOperations = value; }

        public override bool Equals(object obj)
        {
            if (base.Equals(obj))
            {
                Switch x = (Switch)obj;

                return x.switchingOperations == switchingOperations;
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
                case ModelCode.SWITCH_SWOP:
                    return true;

                default:
                    return base.HasProperty(property);
            }
        }

        public override void GetProperty(Property property)
        {
            switch (property.Id)
            {
                case ModelCode.SWITCH_SWOP:
                    property.SetValue(switchingOperations);
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
                case ModelCode.SWITCH_SWOP:
                    SwitchingOperations = property.AsReference();
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
            if (switchingOperations != 0 && (refType != TypeOfReference.Reference || refType != TypeOfReference.Both))
            {
                references[ModelCode.SWITCH_SWOP] = new List<long> { SwitchingOperations };
            }

            base.GetReferences(references, refType);
        }

        #endregion IReference implementation

    }
}
