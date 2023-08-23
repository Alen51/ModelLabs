using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using FTN.Common;

namespace FTN.Services.NetworkModelService.DataModel.Core
{	
	public class BaseVoltage : IdentifiedObject
	{
		private float nominalVoltage;

		private List<long> conductingEquipments = new List<long>();			

		public BaseVoltage(long globalId) : base(globalId) 
		{
		}
		
		public float NominalVoltage
		{
			get
			{
				return nominalVoltage;
			}

			set
			{
				nominalVoltage = value;
			}
		}
		
		public List<long> ConductingEquipments
		{
			get
			{
				return conductingEquipments;
			}

			set
			{
				conductingEquipments = value;
			}
		}
		
		public override bool Equals(object obj)
		{
			if (base.Equals(obj))
			{
				BaseVoltage x = (BaseVoltage)obj;
				return ((x.NominalVoltage == this.NominalVoltage) &&
						(CompareHelper.CompareLists(x.conductingEquipments,this.conductingEquipments)));
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

		public override bool HasProperty(ModelCode t)
		{
			return true;
		}

		public override void GetProperty(Property prop)
		{
			
		}

		public override void SetProperty(Property property)
		{
			
		}
		
		#endregion IAccess implementation	

		#region IReference implementation

		public override bool IsReferenced
		{
			get
			{
				return conductingEquipments.Count > 0 || base.IsReferenced;
			}
		}

		public override void GetReferences(Dictionary<ModelCode, List<long>> references, TypeOfReference refType)
		{
			if (conductingEquipments != null && conductingEquipments.Count > 0 && (refType == TypeOfReference.Target || refType == TypeOfReference.Both))
			{
				//references[ModelCode.LOCATION_PSRS] = conductingEquipments.GetRange(0, conductingEquipments.Count);
			}

			base.GetReferences(references, refType);
		}

		public override void AddReference(ModelCode referenceId, long globalId)
		{
		}

		public override void RemoveReference(ModelCode referenceId, long globalId)
		{
			
		}
	
		#endregion IReference implementation	
	}
}
