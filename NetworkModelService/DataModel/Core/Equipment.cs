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
	public class Equipment : PowerSystemResource
	{		
		//private bool isUnderground;
		//private bool isPrivate;
						
		public Equipment(long globalId) : base(globalId) 
		{
		}
	
		/*
		public bool IsUnderground
		{
			get
			{
				return isUnderground;
			}

			set
			{
				isUnderground = value;
			}
		}

		public bool IsPrivate
		{
			get 
			{
				return isPrivate; 
			}
			
			set
			{ 
				isPrivate = value; 
			}
		}*/

		public override bool Equals(object obj)
		{
			if (base.Equals(obj))
			{
				return true;
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
			
				
			return base.HasProperty(property);
			
		}

		public override void GetProperty(Property property)
		{
			
			base.GetProperty(property);
			
		}

		public override void SetProperty(Property property)
		{
			
			base.SetProperty(property);
					
			
		}		

		#endregion IAccess implementation
	}
}
