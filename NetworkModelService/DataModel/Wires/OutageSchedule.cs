//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FTN {
    using System;
    using System.Collections.Generic;
    using FTN;
    using FTN.Common;
    using FTN.Services.NetworkModelService.DataModel.Core;


    /// The period of time that a piece of equipment is out of service, for example, for maintenance or testing; including the equipment's active power rating while under maintenance. The X-axis represents absolute time and the Y-axis represents the equipment's available rating while out of service.
    public class OutageSchedule : IrregularIntervalSchedule {
        
        /// An OutageSchedule may operate many switches.
        

        private List<long> switchingOperations = new List<long>();

        public List<long> SwitchingOperations { get => switchingOperations; set => switchingOperations = value; }

        public OutageSchedule(long globalId) : base(globalId)
        {
        }

        

        public override bool Equals(object obj)
        {
            if (base.Equals(obj))
            {
                OutageSchedule x = (OutageSchedule)obj;
                return ((CompareHelper.CompareLists(x.switchingOperations, this.switchingOperations)));
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

        public override bool HasProperty(ModelCode t)
        {
            switch (t)
            {
                case ModelCode.OUTAGESCHEDULE_SWITCHINGOPS:
                
                    return true;

                default:
                    return base.HasProperty(t);
            }
        }

        public override void GetProperty(Property prop)
        {
            switch (prop.Id)
            {
                
                case ModelCode.OUTAGESCHEDULE_SWITCHINGOPS:
                    prop.SetValue(switchingOperations);
                    break;
                default:
                    base.GetProperty(prop);
                    break;
            }
        }

        public override void SetProperty(Property property)
        {
            switch (property.Id)
            {
                

                default:
                    base.SetProperty(property);
                    break;
            }
        }

        public override bool IsReferenced
        {
            get
            {
                return switchingOperations.Count > 0 || base.IsReferenced;
            }
        }

        public override void GetReferences(Dictionary<ModelCode, List<long>> references, TypeOfReference refType)
        {
            if (switchingOperations != null && switchingOperations.Count > 0 && (refType == TypeOfReference.Target || refType == TypeOfReference.Both))
            {
                references[ModelCode.OUTAGESCHEDULE_SWITCHINGOPS] = switchingOperations.GetRange(0, switchingOperations.Count);
            }

            base.GetReferences(references, refType);
        }

        public override void AddReference(ModelCode referenceId, long globalId)
        {
            switch (referenceId)
            {
                case ModelCode.SWITCHINGOPERATION_OSCHEDULE:
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
                case ModelCode.REGULARTPOINT_REGULARISCHEDULE:

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

    }
}
