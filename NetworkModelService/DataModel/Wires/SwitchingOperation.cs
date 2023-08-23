//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//<cim:Switch.switchingOperations rdf:resource="#1_SW_OP"/>
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FTN {
    using System;
    using FTN;
    using System.Collections.Generic;
    using FTN.Services.NetworkModelService.DataModel.Core;
    using FTN.Common;


    /// A SwitchingOperation is used to define individual switch operations for an OutageSchedule. This OutageSchedule may be associated with another item of Substation such as a Transformer, Line, or Generator; or with the Switch itself as a PowerSystemResource. A Switch may be referenced by many OutageSchedules.
    public class SwitchingOperation : IdentifiedObject {
        
        /// The switch position that shall result from this SwitchingOperation.
        private SwitchState newState;
        
        private DateTime operationTime;
        
       private long outageSchedule=0;
        
       private List<long> switches = new List<long>();
        
       
        public SwitchingOperation(long globalId) : base(globalId)
        {
        }

        public SwitchState NewState { get => newState; set => newState = value; }
        public DateTime OperationTime { get => operationTime; set => operationTime = value; }
        public long OutageSchedule { get => outageSchedule; set => outageSchedule = value; }
        public List<long> Switches { get => switches; set => switches = value; }

        public override bool Equals(object obj)
        {
            if (base.Equals(obj))
            {
                SwitchingOperation x = (SwitchingOperation)obj;
                return ((x.OperationTime == this.OperationTime) &&
                    (x.OutageSchedule == this.OutageSchedule) &&
                    (x.NewState == this.NewState) &&
                        (CompareHelper.CompareLists(x.switches, this.switches)));
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
                case ModelCode.SWITCHINGOPERATION_NEWSTATE:
                case ModelCode.SWITCHINGOPERATION_OPTIME:
                case ModelCode.SWITCHINGOPERATION_OSCHEDULE:
                case ModelCode.SWITCHINGOPERATION_SWITCHES:
                    return true;

                default:
                    return base.HasProperty(t);
            }
        }

        public override void GetProperty(Property prop)
        {
            switch (prop.Id)
            {
                case ModelCode.SWITCHINGOPERATION_NEWSTATE:
                    prop.SetValue((short)newState);
                    break;
                case ModelCode.SWITCHINGOPERATION_OPTIME:
                    prop.SetValue(operationTime);
                    break;
                case ModelCode.SWITCHINGOPERATION_OSCHEDULE:
                    prop.SetValue(outageSchedule);
                    break;
                case ModelCode.SWITCHINGOPERATION_SWITCHES:
                    prop.SetValue(switches);
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
                case ModelCode.SWITCHINGOPERATION_NEWSTATE:
                    newState =(SwitchState) property.AsEnum();
                    break;
                case ModelCode.SWITCHINGOPERATION_OPTIME:
                    operationTime = property.AsDateTime();
                    break;
                case ModelCode.SWITCHINGOPERATION_OSCHEDULE:
                    outageSchedule = property.AsReference();
                    break;

                default:
                    base.SetProperty(property);
                    break;
            }
        }

        public override bool IsReferenced
        {
            get
            {
                return switches.Count != 0 || base.IsReferenced;
            }
        }

        public override void GetReferences(Dictionary<ModelCode, List<long>> references, TypeOfReference refType)
        {
            if (outageSchedule != 0 && (refType == TypeOfReference.Reference || refType == TypeOfReference.Both))
            {
                references[ModelCode.SWITCHINGOPERATION_OSCHEDULE] = new List<long>();
                references[ModelCode.SWITCHINGOPERATION_OSCHEDULE].Add(outageSchedule);
            }

            if (switches != null && switches.Count != 0 && (refType == TypeOfReference.Target || refType == TypeOfReference.Both))
            {
                references[ModelCode.SWITCHINGOPERATION_SWITCHES] = switches.GetRange(0, switches.Count);
            }

            base.GetReferences(references, refType);
        }

        public override void AddReference(ModelCode referenceId, long globalId)
        {
            switch (referenceId)
            {
                case ModelCode.SWITCH_SWITCHINGOP:
                    switches.Add(globalId);
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
                case ModelCode.SWITCH_SWITCHINGOP:

                    if (switches.Contains(globalId))
                    {
                        switches.Remove(globalId);
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
