using FTN.Common;
using FTN.ServiceContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace WPF
{
    public class TestGda
    {
        private ModelResourcesDesc modelResourcesDesc = new ModelResourcesDesc();

        private NetworkModelGDAProxy gdaQueryProxy = null;
        private NetworkModelGDAProxy GdaQueryProxy
        {
            get
            {
                if (gdaQueryProxy != null)
                {
                    gdaQueryProxy.Abort();
                    gdaQueryProxy = null;
                }

                gdaQueryProxy = new NetworkModelGDAProxy("NetworkModelGDAEndpoint");
                gdaQueryProxy.Open();

                return gdaQueryProxy;
            }
        }

        public TestGda()
        {
        }

        public List<string> GetGids()
        {
            List<string> convertedGids = null;

            try
            {
                convertedGids = new List<string>();
                List<long> gids = GdaQueryProxy.GetGids();
                foreach (var gid in gids)
                {
                    convertedGids.Add(string.Format("0x{0:x16}", gid));
                }
            }
            catch (Exception e)
            {
                string message = string.Format("Get all gids method failed.\n\t{0}", e.Message);
                Console.WriteLine(message);
                CommonTrace.WriteTrace(CommonTrace.TraceError, message);
            }
            return convertedGids;
        }
        public List<ModelCode> GetProperties(string gid)
        {
            long convertedGid = Convert.ToInt64(Int64.Parse(gid.Remove(0, 2), System.Globalization.NumberStyles.HexNumber));
            return modelResourcesDesc.GetAllPropertyIdsForEntityId(convertedGid);
        }

		public List<ModelCode> GetProperties(ModelCode modelCode)
		{
			return modelResourcesDesc.GetAllPropertyIds(modelCode);
		}

		public List<ModelCode> GetModelCodes()
		{
			return modelResourcesDesc.NonAbstractClassIds;
		}

		public string GetValues(long globalId, List<ModelCode> properties)
        {
            string message = "Getting values method started.";
            Console.WriteLine(message);
            CommonTrace.WriteTrace(CommonTrace.TraceInfo, message);

            XmlTextWriter xmlWriter = null;
            ResourceDescription rd = null;
            string result = "";

            try
            {
                rd = GdaQueryProxy.GetValues(globalId, properties);
                result = PrintResource(rd);

                xmlWriter = new XmlTextWriter(Config.Instance.ResultDirecotry + "\\GetValues_Results.xml", Encoding.Unicode);
                xmlWriter.Formatting = Formatting.Indented;
                rd.ExportToXml(xmlWriter);
                xmlWriter.Flush();

                message = "Getting values method successfully finished.";
                Console.WriteLine(message);
                CommonTrace.WriteTrace(CommonTrace.TraceInfo, message);
            }
            catch (Exception e)
            {
                message = string.Format("Getting values method for entered id = {0} failed.\n\t{1}", globalId, e.Message);
                Console.WriteLine(message);
                CommonTrace.WriteTrace(CommonTrace.TraceError, message);
            }
            finally
            {
                if (xmlWriter != null)
                {
                    xmlWriter.Close();
                }
            }

            return result;
        }

		public string GetExtentValues(ModelCode entityType, List<ModelCode> properties)
		{
			string message = "Getting extent values method started.";
			Console.WriteLine(message);
			CommonTrace.WriteTrace(CommonTrace.TraceInfo, message);

			XmlTextWriter xmlWriter = null;
			string result = "";
			int iteratorId = 0;

			try
			{
				int numberOfResources = 2;
				int resourcesLeft = 0;

				iteratorId = GdaQueryProxy.GetExtentValues(entityType, properties);
				resourcesLeft = GdaQueryProxy.IteratorResourcesLeft(iteratorId);

				xmlWriter = new XmlTextWriter(Config.Instance.ResultDirecotry + "\\GetExtentValues_Results.xml", Encoding.Unicode);
				xmlWriter.Formatting = Formatting.Indented;

				while (resourcesLeft > 0)
				{
					List<ResourceDescription> rds = GdaQueryProxy.IteratorNext(numberOfResources, iteratorId);

					for (int i = 0; i < rds.Count; i++)
					{
						rds[i].ExportToXml(xmlWriter);
						xmlWriter.Flush();
						result += PrintResource(rds[i]);
					}

					resourcesLeft = GdaQueryProxy.IteratorResourcesLeft(iteratorId);
				}

				GdaQueryProxy.IteratorClose(iteratorId);

				message = "Getting extent values method successfully finished.";
				Console.WriteLine(message);
				CommonTrace.WriteTrace(CommonTrace.TraceInfo, message);
			}
			catch (Exception e)
			{
				message = string.Format("Getting extent values method for ModelCode {0} failed.\n\t{1}", entityType, e.Message);
				Console.WriteLine(message);
				CommonTrace.WriteTrace(CommonTrace.TraceError, message);
			}
			finally
			{
				if (xmlWriter != null)
				{
					xmlWriter.Close();
				}
			}

			return result;
		}

		public string GetRelatedValues(long sourceGID, List<ModelCode> properties, Association association)
		{
			string message = "Getting related values method started.";
			Console.WriteLine(message);
			CommonTrace.WriteTrace(CommonTrace.TraceInfo, message);

			XmlTextWriter xmlWriter = null;

			int numberOfResources = 2;
			string result = "";

			try
			{
				int iteratorId = GdaQueryProxy.GetRelatedValues(sourceGID, properties, association);
				int resourcesLeft = GdaQueryProxy.IteratorResourcesLeft(iteratorId);

				xmlWriter = new XmlTextWriter(Config.Instance.ResultDirecotry + "\\GetRelatedValues_Results.xml", Encoding.Unicode);
				xmlWriter.Formatting = Formatting.Indented;

				while (resourcesLeft > 0)
				{
					List<ResourceDescription> rds = GdaQueryProxy.IteratorNext(numberOfResources, iteratorId);

					for (int i = 0; i < rds.Count; i++)
					{
						rds[i].ExportToXml(xmlWriter);
						xmlWriter.Flush();
						result += PrintResource(rds[i]);
					}

					resourcesLeft = GdaQueryProxy.IteratorResourcesLeft(iteratorId);
				}

				GdaQueryProxy.IteratorClose(iteratorId);

				message = "Getting related values method successfully finished.";
				Console.WriteLine(message);
				CommonTrace.WriteTrace(CommonTrace.TraceError, message);
			}
			catch (Exception e)
			{
				message = string.Format("Getting related values method failed for sourceGlobalId = {0} and association (propertyId = {1}, type = {2}). Reason: {3}", sourceGID, association.PropertyId, association.Type, e.Message);
				Console.WriteLine(message);
				CommonTrace.WriteTrace(CommonTrace.TraceError, message);
			}
			finally
			{
				if (xmlWriter != null)
				{
					xmlWriter.Close();
				}
			}
			return result;
		}

		public List<ModelCode> GetReferencePropertyIds(DMSType type)
		{
			List<ModelCode> references = modelResourcesDesc.GetPropertyIds(type, PropertyType.Reference);
			List<ModelCode> referenceVectors = modelResourcesDesc.GetPropertyIds(type, PropertyType.ReferenceVector);

			List<ModelCode> ids = new List<ModelCode>();
			if (references.Count != 0)
			{
				ids.AddRange(references);
			}
			if (referenceVectors.Count != 0)
			{
				ids.AddRange(referenceVectors);
			}
			return ids;
		}

		public List<ModelCode> GetReferencedEntities(long globalId, ModelCode association)
		{
			List<long> relatedGids = GdaQueryProxy.GetReferencedEntities(globalId, association);

			List<ModelCode> modelCodes = new List<ModelCode>();
			foreach (var gid in relatedGids)
			{
				modelCodes.Add(modelResourcesDesc.GetModelCodeFromId(gid));
			}
			return modelCodes;
		}

		private string PrintResource(ResourceDescription rd)
		{
			StringBuilder sb = new StringBuilder();
			StringBuilder sb2;

			sb.AppendLine("ResourceDescription: ");
			sb.AppendLine($"\tGID: {String.Format("0x{0:x16}", rd.Id)}");
			sb.AppendLine($"\tType: {(DMSType)ModelCodeHelper.ExtractTypeFromGlobalId(rd.Id)}");
			sb.AppendLine("\tProperties:");
			for (int i = 0; i < rd.Properties.Count; i++)
			{
				sb.AppendLine($"\t   Property {i + 1}: ");
				sb.AppendLine($"\t      Id: {rd.Properties[i].Id}");
				sb.Append("\t      Value: ");
				switch (rd.Properties[i].Type)
				{
					case PropertyType.Float:
						sb.AppendLine(rd.Properties[i].AsFloat().ToString());
						break;
					case PropertyType.Bool:
						sb.AppendLine(rd.Properties[i].AsBool().ToString());
						break;
					case PropertyType.Byte:
					case PropertyType.Int32:
					case PropertyType.Int64:
					case PropertyType.TimeSpan:
					case PropertyType.DateTime:
						if (rd.Properties[i].Id == ModelCode.IDOBJ_GID)
						{
							sb.AppendLine(String.Format("0x{0:x16}", rd.Properties[i].AsLong()));
						}
						else
						{
							sb.AppendLine(rd.Properties[i].AsLong().ToString());
						}

						break;
					case PropertyType.Enum:
						try
						{
							EnumDescs enumDescs = new EnumDescs();
							sb.AppendLine(enumDescs.GetStringFromEnum(rd.Properties[i].Id, rd.Properties[i].AsEnum()));
						}
						catch (Exception)
						{
							sb.AppendLine(rd.Properties[i].AsEnum().ToString());
						}

						break;
					case PropertyType.Reference:
						sb.AppendLine(String.Format("0x{0:x16}", rd.Properties[i].AsReference()));
						break;
					case PropertyType.String:
						if (rd.Properties[i].PropertyValue.StringValue == null)
						{
							rd.Properties[i].PropertyValue.StringValue = String.Empty;
						}
						sb.AppendLine(rd.Properties[i].AsString());
						break;

					case PropertyType.Int64Vector:
					case PropertyType.ReferenceVector:
						if (rd.Properties[i].AsLongs().Count > 0)
						{
							sb2 = new StringBuilder(100);
							for (int j = 0; j < rd.Properties[i].AsLongs().Count; j++)
							{
								sb2.Append(String.Format("0x{0:x16}", rd.Properties[i].AsLongs()[j])).Append(", ");
							}

							sb.AppendLine(sb2.ToString(0, sb2.Length - 2));
						}
						else
						{
							sb.AppendLine("empty long/reference vector");
						}

						break;
					case PropertyType.TimeSpanVector:
						if (rd.Properties[i].AsLongs().Count > 0)
						{
							sb2 = new StringBuilder(100);
							for (int j = 0; j < rd.Properties[i].AsLongs().Count; j++)
							{
								sb2.Append(String.Format("0x{0:x16}", rd.Properties[i].AsTimeSpans()[j])).Append(", ");
							}

							sb.AppendLine(sb2.ToString(0, sb2.Length - 2));
						}
						else
						{
							sb.AppendLine("empty long/reference vector");
						}

						break;
					case PropertyType.Int32Vector:
						if (rd.Properties[i].AsInts().Count > 0)
						{
							sb2 = new StringBuilder(100);
							for (int j = 0; j < rd.Properties[i].AsInts().Count; j++)
							{
								sb2.Append(String.Format("{0}", rd.Properties[i].AsInts()[j])).Append(", ");
							}

							sb.AppendLine(sb2.ToString(0, sb2.Length - 2));
						}
						else
						{
							sb.AppendLine("empty int vector");
						}

						break;

					case PropertyType.DateTimeVector:
						if (rd.Properties[i].AsDateTimes().Count > 0)
						{
							sb2 = new StringBuilder(100);
							for (int j = 0; j < rd.Properties[i].AsDateTimes().Count; j++)
							{
								sb2.Append(String.Format("{0}", rd.Properties[i].AsDateTimes()[j])).Append(", ");
							}

							sb.AppendLine(sb2.ToString(0, sb2.Length - 2));
						}
						else
						{
							sb.AppendLine("empty DateTime vector");
						}

						break;

					case PropertyType.BoolVector:
						if (rd.Properties[i].AsBools().Count > 0)
						{
							sb2 = new StringBuilder(100);
							for (int j = 0; j < rd.Properties[i].AsBools().Count; j++)
							{
								sb2.Append(String.Format("{0}", rd.Properties[i].AsBools()[j])).Append(", ");
							}

							sb.AppendLine(sb2.ToString(0, sb2.Length - 2));
						}
						else
						{
							sb.AppendLine("empty int vector");
						}

						break;
					case PropertyType.FloatVector:
						if (rd.Properties[i].AsFloats().Count > 0)
						{
							sb2 = new StringBuilder(100);
							for (int j = 0; j < rd.Properties[i].AsFloats().Count; j++)
							{
								sb2.Append(rd.Properties[i].AsFloats()[j]).Append(", ");
							}

							sb.AppendLine(sb2.ToString(0, sb2.Length - 2));
						}
						else
						{
							sb.AppendLine("empty float vector");
						}

						break;
					case PropertyType.StringVector:
						if (rd.Properties[i].AsStrings().Count > 0)
						{
							sb2 = new StringBuilder(100);
							for (int j = 0; j < rd.Properties[i].AsStrings().Count; j++)
							{
								sb2.Append(rd.Properties[i].AsStrings()[j]).Append(", ");
							}

							sb.AppendLine(sb2.ToString(0, sb2.Length - 2));
						}
						else
						{
							sb.AppendLine("empty string vector");
						}

						break;
					case PropertyType.EnumVector:
						if (rd.Properties[i].AsEnums().Count > 0)
						{
							sb2 = new StringBuilder(100);
							EnumDescs enumDescs = new EnumDescs();

							for (int j = 0; j < rd.Properties[i].AsEnums().Count; j++)
							{
								try
								{
									sb2.Append(String.Format("{0}", enumDescs.GetStringFromEnum(rd.Properties[i].Id, rd.Properties[i].AsEnums()[j]))).Append(", ");
								}
								catch (Exception)
								{
									sb2.Append(String.Format("{0}", rd.Properties[i].AsEnums()[j])).Append(", ");
								}
							}

							sb.AppendLine(sb2.ToString(0, sb2.Length - 2));
						}
						else
						{
							sb.AppendLine("empty enum vector");
						}

						break;

					default:
						throw new Exception("Failed to print Resource Description. Invalid property type.");
				}
			}
			return sb.ToString();
		}
		public void Dispose()
		{
			GC.SuppressFinalize(this);
		}
	}
}
