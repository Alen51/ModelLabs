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


    /// Time point for a schedule where the time between the consecutive points is constant.
    public class RegularTimePoint : IdentifiedObject {
        
        /// Regular interval schedule containing this time point.
        private long intervalSchedule=0;
        
        private int sequenceNumber;
        
        private float value1;
        
        
        
        
        /// The second value at the time. The meaning of the value is defined by the derived type of the associated schedule.
        private float value2;
        
        

        public RegularTimePoint(long globalId) : base(globalId)
        {
        }

       

        public long IntervalSchedule { get => intervalSchedule; set => intervalSchedule = value; }
        public int SequenceNumber { get => sequenceNumber; set => sequenceNumber = value; }
        public float Value1 { get => value1; set => value1 = value; }
        public float Value2 { get => value2; set => value2 = value; }

        public override bool Equals(object obj)
        {
            if (base.Equals(obj))
            {
                RegularTimePoint x = (RegularTimePoint)obj;
                return ((x.value1 == this.value1)             &&
                    (x.value2 == this.value2)                 &&
                    (x.sequenceNumber == this.sequenceNumber) && 
                    (x.intervalSchedule == this.intervalSchedule));
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
                case ModelCode.REGULARTPOINT_REGULARISCHEDULE:
                case ModelCode.REGULARTPOINT_SEQNUM:
                case ModelCode.REGULARTPOINT_VALUE1:
                case ModelCode.REGULARTPOINT_VALUE2:
                    return true;

                default:
                    return base.HasProperty(t);
            }
        }

        public override void GetProperty(Property prop)
        {
            switch (prop.Id)
            {
                case ModelCode.REGULARTPOINT_REGULARISCHEDULE:
                    prop.SetValue(intervalSchedule);
                    break;
                case ModelCode.REGULARTPOINT_SEQNUM:
                    prop.SetValue(sequenceNumber);
                    break;
                case ModelCode.REGULARTPOINT_VALUE1:
                    prop.SetValue(value1);
                    break;
                case ModelCode.REGULARTPOINT_VALUE2:
                    prop.SetValue(value2);
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
                case ModelCode.REGULARTPOINT_REGULARISCHEDULE:
                    intervalSchedule = property.AsLong();
                    break;
                case ModelCode.REGULARTPOINT_SEQNUM:
                    sequenceNumber = property.AsInt();
                    break;
                case ModelCode.REGULARTPOINT_VALUE1:
                    value1 = property.AsFloat();
                    break;
                case ModelCode.REGULARTPOINT_VALUE2:
                    value1 = property.AsFloat();
                    break;

                default:
                    base.SetProperty(property);
                    break;
            }
        }

        public override void GetReferences(Dictionary<ModelCode, List<long>> references, TypeOfReference refType)
        {
            if (intervalSchedule != 0 && (refType == TypeOfReference.Reference || refType == TypeOfReference.Both))
            {
                references[ModelCode.REGULARTPOINT_REGULARISCHEDULE] = new List<long>();
                references[ModelCode.REGULARTPOINT_REGULARISCHEDULE].Add(intervalSchedule);
            }

            base.GetReferences(references, refType);
        }

    }
}
