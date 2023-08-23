namespace FTN.ESI.SIMES.CIM.CIMAdapter.Importer
{
	using FTN.Common;

	/// <summary>
	/// PowerTransformerConverter has methods for populating
	/// ResourceDescription objects using PowerTransformerCIMProfile_Labs objects.
	/// </summary>
	public static class PowerTransformerConverter
	{

		#region Populate ResourceDescription
		public static void PopulateIdentifiedObjectProperties(FTN.IdentifiedObject cimIdentifiedObject, ResourceDescription rd)
		{
			if ((cimIdentifiedObject != null) && (rd != null))
			{
				if (cimIdentifiedObject.MRIDHasValue)
				{
					rd.AddProperty(new Property(ModelCode.IDOBJ_MRID, cimIdentifiedObject.MRID));
				}
				if (cimIdentifiedObject.NameHasValue)
				{
					rd.AddProperty(new Property(ModelCode.IDOBJ_NAME, cimIdentifiedObject.Name));
				}
                if (cimIdentifiedObject.AliasNameHasValue)
                {
					rd.AddProperty(new Property(ModelCode.IDOBJ_ALIASNAME, cimIdentifiedObject.AliasName));
                }
				
			}
		}

		public static void PopulateBasicIntervalScheduleProperties(FTN.BasicIntervalSchedule cimBasicIntervalSchedule, ResourceDescription rd)
		{
			if ((cimBasicIntervalSchedule != null) && (rd != null))
			{
				PowerTransformerConverter.PopulateIdentifiedObjectProperties(cimBasicIntervalSchedule, rd);

                if (cimBasicIntervalSchedule.StartTimeHasValue)
                {
					rd.AddProperty(new Property(ModelCode.BASICISCEDULE_STARTTIME, cimBasicIntervalSchedule.StartTime));
                }
				if (cimBasicIntervalSchedule.Value1UnitHasValue)
				{
					rd.AddProperty(new Property(ModelCode.BASICISCEDULE_V1U,(short)GetDMSUnitSymbol(cimBasicIntervalSchedule.Value1Unit)));
				}
				if (cimBasicIntervalSchedule.Value1MultiplierHasValue)
				{
					rd.AddProperty(new Property(ModelCode.BASICISCEDULE_V1M,(short)GetDMSUnitMultiplier(cimBasicIntervalSchedule.Value1Multiplier)));
				}
				if (cimBasicIntervalSchedule.Value2UnitHasValue)
				{
					rd.AddProperty(new Property(ModelCode.BASICISCEDULE_STARTTIME,(short)GetDMSUnitSymbol(cimBasicIntervalSchedule.Value2Unit)));
				}
				if (cimBasicIntervalSchedule.Value2MultiplierHasValue)
				{
					rd.AddProperty(new Property(ModelCode.BASICISCEDULE_V2M,(short)GetDMSUnitMultiplier(cimBasicIntervalSchedule.Value2Multiplier)));
				}
			}
		}


		public static void IrregularIntervalScheduleProperties(FTN.IrregularIntervalSchedule cimIrregularIntervalSchedule, ResourceDescription rd)
		{
			if ((cimIrregularIntervalSchedule != null) && (rd != null))
			{
				PowerTransformerConverter.PopulateBasicIntervalScheduleProperties(cimIrregularIntervalSchedule, rd);
			}
		}

		public static void PopulateOutageScheduleProperties(FTN.OutageSchedule cimOutageSchedule, ResourceDescription rd)
		{
			if ((cimOutageSchedule != null) && (rd != null))
			{
				PowerTransformerConverter.IrregularIntervalScheduleProperties(cimOutageSchedule, rd);
			}
		}

		public static void PopulatePowerSystemResourceProperties(FTN.PowerSystemResource cimPowerSystemResource, ResourceDescription rd, ImportHelper importHelper, TransformAndLoadReport report)
		{
			if ((cimPowerSystemResource != null) && (rd != null))
			{
				PowerTransformerConverter.PopulateIdentifiedObjectProperties(cimPowerSystemResource, rd);

				
			}
		}


		public static void PopulateEquipmentProperties(FTN.Equipment cimEquipment, ResourceDescription rd, ImportHelper importHelper, TransformAndLoadReport report)
		{
			if ((cimEquipment != null) && (rd != null))
			{
				PowerTransformerConverter.PopulatePowerSystemResourceProperties(cimEquipment, rd, importHelper, report);
			}
		}

		public static void PopulateConductingEquipmentProperties(FTN.ConductingEquipment cimConductingEquipment, ResourceDescription rd, ImportHelper importHelper, TransformAndLoadReport report)
		{
			if ((cimConductingEquipment != null) && (rd != null))
			{
				PowerTransformerConverter.PopulateEquipmentProperties(cimConductingEquipment, rd, importHelper, report);
			}
		}

		public static void PopulateSwitchProperties(FTN.Switch cimSwitch, ResourceDescription rd, ImportHelper importHelper, TransformAndLoadReport report)
		{
			if ((cimSwitch != null) && (rd != null))
			{
				PowerTransformerConverter.PopulateConductingEquipmentProperties(cimSwitch, rd,importHelper,report);
				
				if (cimSwitch.SwitchingOperationsHasValue)
				{
					long gid = importHelper.GetMappedGID(cimSwitch.SwitchingOperations.ID);
					if (gid < 0)
					{
						report.Report.Append("WARNING: Convert ").Append(cimSwitch.GetType().ToString()).Append(" rdfID = \"").Append(cimSwitch.ID);
						report.Report.Append("\" - Failed to set reference to SwitchingOperation: rdfID \"").Append(cimSwitch.SwitchingOperations.ID).AppendLine(" \" is not mapped to GID!");
					}
					rd.AddProperty(new Property(ModelCode.SWITCH_SWITCHINGOP, gid));
				}
				
			}
		}

		public static void PopulateDisconectorProperties(FTN.Disconnector cimDisconector, ResourceDescription rd, ImportHelper importHelper, TransformAndLoadReport report)
		{
			if ((cimDisconector != null) && (rd != null))
			{
				PowerTransformerConverter.PopulateSwitchProperties(cimDisconector, rd, importHelper, report);
			}
		}

		public static void PopulateSwitchingOperationProperties(FTN.SwitchingOperation cimSwitchingOperation, ResourceDescription rd, ImportHelper importHelper, TransformAndLoadReport report)
		{
			if ((cimSwitchingOperation != null) && (rd != null))
			{
				PowerTransformerConverter.PopulateIdentifiedObjectProperties(cimSwitchingOperation, rd);

                if (cimSwitchingOperation.NewStateHasValue)
                {
					rd.AddProperty(new Property(ModelCode.SWITCHINGOPERATION_NEWSTATE, (short)GetDMSSwitchState(cimSwitchingOperation.NewState)));
                }
				if (cimSwitchingOperation.OperationTimeHasValue)
				{
					rd.AddProperty(new Property(ModelCode.SWITCHINGOPERATION_OPTIME, cimSwitchingOperation.OperationTime));
				}
				if (cimSwitchingOperation.OutageScheduleHasValue)
				{
					long gid = importHelper.GetMappedGID(cimSwitchingOperation.OutageSchedule.ID);
					if (gid < 0)
					{
						report.Report.Append("WARNING: Convert ").Append(cimSwitchingOperation.GetType().ToString()).Append(" rdfID = \"").Append(cimSwitchingOperation.ID);
						report.Report.Append("\" - Failed to set reference to OutageSchedule: rdfID \"").Append(cimSwitchingOperation.OutageSchedule.ID).AppendLine(" \" is not mapped to GID!");
					}
					rd.AddProperty(new Property(ModelCode.SWITCHINGOPERATION_OSCHEDULE, gid));
				}

			}
		}

		public static void PopulateRegularTimePointProperties(FTN.RegularTimePoint cimRegulerTimepoint, ResourceDescription rd, ImportHelper importHelper, TransformAndLoadReport report)
		{
			if ((cimRegulerTimepoint != null) && (rd != null))
			{
				PowerTransformerConverter.PopulateIdentifiedObjectProperties(cimRegulerTimepoint, rd);

				if (cimRegulerTimepoint.SequenceNumberHasValue)
				{
					rd.AddProperty(new Property(ModelCode.REGULARTPOINT_SEQNUM, cimRegulerTimepoint.SequenceNumber));
				}
				if (cimRegulerTimepoint.Value1HasValue)
				{
					rd.AddProperty(new Property(ModelCode.REGULARTPOINT_VALUE1, cimRegulerTimepoint.Value1));
				}
				if (cimRegulerTimepoint.Value2HasValue)
				{
					rd.AddProperty(new Property(ModelCode.REGULARTPOINT_VALUE2, cimRegulerTimepoint.Value2));
				}
				if (cimRegulerTimepoint.IntervalScheduleHasValue)
				{
					long gid = importHelper.GetMappedGID(cimRegulerTimepoint.IntervalSchedule.ID);
					if (gid < 0)
					{
						report.Report.Append("WARNING: Convert ").Append(cimRegulerTimepoint.GetType().ToString()).Append(" rdfID = \"").Append(cimRegulerTimepoint.ID);
						report.Report.Append("\" - Failed to set reference to IntervalSchedule: rdfID \"").Append(cimRegulerTimepoint.IntervalSchedule.ID).AppendLine(" \" is not mapped to GID!");
					}
					rd.AddProperty(new Property(ModelCode.REGULARTPOINT_REGULARISCHEDULE, gid));
				}

			}
		}

		public static void PopulateRegularIntervalScheduleProperties(FTN.RegularIntervalSchedule cimRegularIntervalSchedule, ResourceDescription rd, ImportHelper importHelper, TransformAndLoadReport report)
		{
			if ((cimRegularIntervalSchedule != null) && (rd != null))
			{
				PowerTransformerConverter.PopulateBasicIntervalScheduleProperties(cimRegularIntervalSchedule, rd);

                if (cimRegularIntervalSchedule.EndTimeHasValue)
                {
					rd.AddProperty(new Property(ModelCode.REGULARISCHEDULE_ENDTIME, cimRegularIntervalSchedule.EndTime));
                }
				

			}
		}

		public static void PopulateIregularTimePointProperties(FTN.IrregularTimePoint cimIrregularTimePoint, ResourceDescription rd, ImportHelper importHelper, TransformAndLoadReport report)
		{
			if ((cimIrregularTimePoint != null) && (rd != null))
			{
				PowerTransformerConverter.PopulateIdentifiedObjectProperties(cimIrregularTimePoint, rd);

				if (cimIrregularTimePoint.Value1HasValue)
				{
					rd.AddProperty(new Property(ModelCode.IRREGULARTPOINT_VALUE1, cimIrregularTimePoint.Value1));
				}
				if (cimIrregularTimePoint.Value2HasValue)
				{
					rd.AddProperty(new Property(ModelCode.IRREGULARTPOINT_VALUE2, cimIrregularTimePoint.Value2));
				}
				if (cimIrregularTimePoint.IntervalScheduleHasValue)
				{
					long gid = importHelper.GetMappedGID(cimIrregularTimePoint.IntervalSchedule.ID);
					if (gid < 0)
					{
						report.Report.Append("WARNING: Convert ").Append(cimIrregularTimePoint.GetType().ToString()).Append(" rdfID = \"").Append(cimIrregularTimePoint.ID);
						report.Report.Append("\" - Failed to set reference to IntervalSchedule: rdfID \"").Append(cimIrregularTimePoint.IntervalSchedule.ID).AppendLine(" \" is not mapped to GID!");
					}
					rd.AddProperty(new Property(ModelCode.IRREGULARTPOINT_IREGULARISCHEDULE, gid));
				}


			}
		}
		/*
		public static void PopulatePowerTransformerProperties(FTN.PowerTransformer cimPowerTransformer, ResourceDescription rd, ImportHelper importHelper, TransformAndLoadReport report)
		{
			if ((cimPowerTransformer != null) && (rd != null))
			{
				PowerTransformerConverter.PopulateEquipmentProperties(cimPowerTransformer, rd, importHelper, report);

				if (cimPowerTransformer.FunctionHasValue)
				{
					rd.AddProperty(new Property(ModelCode.POWERTR_FUNC, (short)GetDMSTransformerFunctionKind(cimPowerTransformer.Function)));
				}
				if (cimPowerTransformer.AutotransformerHasValue)
				{
					rd.AddProperty(new Property(ModelCode.POWERTR_AUTO, cimPowerTransformer.Autotransformer));
				}
			}
		}
		
		public static void PopulateTransformerWindingProperties(FTN.TransformerWinding cimTransformerWinding, ResourceDescription rd, ImportHelper importHelper, TransformAndLoadReport report)
		{
			if ((cimTransformerWinding != null) && (rd != null))
			{
				PowerTransformerConverter.PopulateConductingEquipmentProperties(cimTransformerWinding, rd, importHelper, report);

				if (cimTransformerWinding.ConnectionTypeHasValue)
				{
					rd.AddProperty(new Property(ModelCode.POWERTRWINDING_CONNTYPE, (short)GetDMSWindingConnection(cimTransformerWinding.ConnectionType)));
				}
				if (cimTransformerWinding.GroundedHasValue)
				{
					rd.AddProperty(new Property(ModelCode.POWERTRWINDING_GROUNDED, cimTransformerWinding.Grounded));
				}
				if (cimTransformerWinding.RatedSHasValue)
				{
					rd.AddProperty(new Property(ModelCode.POWERTRWINDING_RATEDS, cimTransformerWinding.RatedS));
				}
				if (cimTransformerWinding.RatedUHasValue)
				{
					rd.AddProperty(new Property(ModelCode.POWERTRWINDING_RATEDU, cimTransformerWinding.RatedU));
				}
				if (cimTransformerWinding.WindingTypeHasValue)
				{
					rd.AddProperty(new Property(ModelCode.POWERTRWINDING_WINDTYPE, (short)GetDMSWindingType(cimTransformerWinding.WindingType)));
				}
				if (cimTransformerWinding.PhaseToGroundVoltageHasValue)
				{
					rd.AddProperty(new Property(ModelCode.POWERTRWINDING_PHASETOGRNDVOLTAGE, cimTransformerWinding.PhaseToGroundVoltage));
				}
				if (cimTransformerWinding.PhaseToPhaseVoltageHasValue)
				{
					rd.AddProperty(new Property(ModelCode.POWERTRWINDING_PHASETOPHASEVOLTAGE, cimTransformerWinding.PhaseToPhaseVoltage));
				}
				if (cimTransformerWinding.PowerTransformerHasValue)
				{
					long gid = importHelper.GetMappedGID(cimTransformerWinding.PowerTransformer.ID);
					if (gid < 0)
					{
						report.Report.Append("WARNING: Convert ").Append(cimTransformerWinding.GetType().ToString()).Append(" rdfID = \"").Append(cimTransformerWinding.ID);
						report.Report.Append("\" - Failed to set reference to PowerTransformer: rdfID \"").Append(cimTransformerWinding.PowerTransformer.ID).AppendLine(" \" is not mapped to GID!");
					}
					rd.AddProperty(new Property(ModelCode.POWERTRWINDING_POWERTRW, gid));
				}
			}
		}

		public static void PopulateWindingTestProperties(FTN.WindingTest cimWindingTest, ResourceDescription rd, ImportHelper importHelper, TransformAndLoadReport report)
		{
			if ((cimWindingTest != null) && (rd != null))
			{
				PowerTransformerConverter.PopulateIdentifiedObjectProperties(cimWindingTest, rd);

				if (cimWindingTest.LeakageImpedanceHasValue)
				{
					rd.AddProperty(new Property(ModelCode.WINDINGTEST_LEAKIMPDN, cimWindingTest.LeakageImpedance));
				}
				if (cimWindingTest.LoadLossHasValue)
				{
					rd.AddProperty(new Property(ModelCode.WINDINGTEST_LOADLOSS, cimWindingTest.LoadLoss));
				}
				if (cimWindingTest.NoLoadLossHasValue)
				{
					rd.AddProperty(new Property(ModelCode.WINDINGTEST_NOLOADLOSS, cimWindingTest.NoLoadLoss));
				}
				if (cimWindingTest.PhaseShiftHasValue)
				{
					rd.AddProperty(new Property(ModelCode.WINDINGTEST_PHASESHIFT, cimWindingTest.PhaseShift));
				}
				if (cimWindingTest.LeakageImpedance0PercentHasValue)
				{
					rd.AddProperty(new Property(ModelCode.WINDINGTEST_LEAKIMPDN0PERCENT, cimWindingTest.LeakageImpedance0Percent));
				}
				if (cimWindingTest.LeakageImpedanceMaxPercentHasValue)
				{
					rd.AddProperty(new Property(ModelCode.WINDINGTEST_LEAKIMPDNMAXPERCENT, cimWindingTest.LeakageImpedanceMaxPercent));
				}
				if (cimWindingTest.LeakageImpedanceMinPercentHasValue)
				{
					rd.AddProperty(new Property(ModelCode.WINDINGTEST_LEAKIMPDNMINPERCENT, cimWindingTest.LeakageImpedanceMinPercent));
				}

				if (cimWindingTest.From_TransformerWindingHasValue)
				{
					long gid = importHelper.GetMappedGID(cimWindingTest.From_TransformerWinding.ID);
					if (gid < 0)
					{
						report.Report.Append("WARNING: Convert ").Append(cimWindingTest.GetType().ToString()).Append(" rdfID = \"").Append(cimWindingTest.ID);
						report.Report.Append("\" - Failed to set reference to TransformerWinding: rdfID \"").Append(cimWindingTest.From_TransformerWinding.ID).AppendLine(" \" is not mapped to GID!");
					}
					rd.AddProperty(new Property(ModelCode.WINDINGTEST_POWERTRWINDING, gid));
				}
			}
		}*/
		#endregion Populate ResourceDescription

		#region Enums convert
		public static UnitMultiplier GetDMSUnitMultiplier(FTN.UnitMultiplier multipliers)
		{
			switch (multipliers)
			{
				case FTN.UnitMultiplier.c:
					return UnitMultiplier.c;
				case FTN.UnitMultiplier.d:
					return UnitMultiplier.d;
				case FTN.UnitMultiplier.G:
					return UnitMultiplier.G;
				case FTN.UnitMultiplier.k:
					return UnitMultiplier.k;
				case FTN.UnitMultiplier.m:
					return UnitMultiplier.m;
				case FTN.UnitMultiplier.M:
					return UnitMultiplier.M;
				case FTN.UnitMultiplier.micro:
					return UnitMultiplier.micro;
				case FTN.UnitMultiplier.n:
					return UnitMultiplier.n;
				case FTN.UnitMultiplier.none:
					return UnitMultiplier.none;
				case FTN.UnitMultiplier.p:
					return UnitMultiplier.p;
				case FTN.UnitMultiplier.T:
					return UnitMultiplier.T;


				default: return UnitMultiplier.none;
			}
		}

		public static UnitSymbol GetDMSUnitSymbol(FTN.UnitSymbol symbol)
		{
			switch (symbol)
			{
				case FTN.UnitSymbol.A:
					return UnitSymbol.A;
				case FTN.UnitSymbol.deg:
					return UnitSymbol.deg;
				case FTN.UnitSymbol.degC:
					return UnitSymbol.degC;
				case FTN.UnitSymbol.F:
					return UnitSymbol.F;
				case FTN.UnitSymbol.g:
					return UnitSymbol.g;
				case FTN.UnitSymbol.h:
					return UnitSymbol.h;
				case FTN.UnitSymbol.H:
					return UnitSymbol.H;
				case FTN.UnitSymbol.Hz:
					return UnitSymbol.Hz;
				case FTN.UnitSymbol.J:
					return UnitSymbol.J;
				case FTN.UnitSymbol.m:
					return UnitSymbol.m;
				case FTN.UnitSymbol.m2:
					return UnitSymbol.m2;
				case FTN.UnitSymbol.m3:
					return UnitSymbol.m3;
				case FTN.UnitSymbol.min:
					return UnitSymbol.min;
				case FTN.UnitSymbol.N:
					return UnitSymbol.N;
				case FTN.UnitSymbol.none:
					return UnitSymbol.none;
				case FTN.UnitSymbol.ohm:
					return UnitSymbol.ohm;
				case FTN.UnitSymbol.Pa:
					return UnitSymbol.Pa;
				case FTN.UnitSymbol.rad:
					return UnitSymbol.rad;
				case FTN.UnitSymbol.s:
					return UnitSymbol.s;
				case FTN.UnitSymbol.S:
					return UnitSymbol.S;
				case FTN.UnitSymbol.V:
					return UnitSymbol.V;
				case FTN.UnitSymbol.VA:
					return UnitSymbol.VA;
				case FTN.UnitSymbol.VAh:
					return UnitSymbol.VAh;
				case FTN.UnitSymbol.VAr:
					return UnitSymbol.VAr;
				case FTN.UnitSymbol.VArh:
					return UnitSymbol.VArh;
				case FTN.UnitSymbol.W:
					return UnitSymbol.W;
				case FTN.UnitSymbol.Wh:
					return UnitSymbol.Wh;
				
					
				default:
					return UnitSymbol.none;
			}
		}
		
		public static SwitchState GetDMSSwitchState(FTN.SwitchState state)
		{
			switch (state)
			{
				
				case FTN.SwitchState.close:
					return SwitchState.open;
				case FTN.SwitchState.open:
					return SwitchState.open;
				default:
					return SwitchState.close;
			}
		}
		/*
		public static WindingConnection GetDMSWindingConnection(FTN.WindingConnection windingConnection)
		{
			switch (windingConnection)
			{
				case FTN.WindingConnection.D:
					return WindingConnection.D;
				case FTN.WindingConnection.I:
					return WindingConnection.I;
				case FTN.WindingConnection.Z:
					return WindingConnection.Z;
				case FTN.WindingConnection.Y:
					return WindingConnection.Y;
				default:
					return WindingConnection.Y;
			}
		}*/
		#endregion Enums convert
	}
}
