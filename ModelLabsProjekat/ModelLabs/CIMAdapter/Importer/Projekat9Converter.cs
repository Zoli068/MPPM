namespace FTN.ESI.SIMES.CIM.CIMAdapter.Importer
{
    using FTN;
    using FTN.Common;
    using System;

    /// <summary>
    /// PowerTransformerConverter has methods for populating
    /// ResourceDescription objects using PowerTransformerCIMProfile_Labs objects.
    /// </summary>
    public static class Projekat9Converter
	{

        #region Populate Properties

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

        public static void PopulatePowerSystemResourceProperties(FTN.PowerSystemResource cimPowerSystemResource, ResourceDescription rd, ImportHelper importHelper, TransformAndLoadReport report)
        {
            if ((cimPowerSystemResource != null) && (rd != null))
            {
                Projekat9Converter.PopulateIdentifiedObjectProperties(cimPowerSystemResource, rd);
            }
        }

        public static void PopulateEquipmentProperties(FTN.Equipment cimEquipment, ResourceDescription rd, ImportHelper importHelper, TransformAndLoadReport report)
        {
            if ((cimEquipment != null) && (rd != null))
            {
                Projekat9Converter.PopulatePowerSystemResourceProperties(cimEquipment, rd, importHelper, report);
            }
        }

        public static void PopulateConductingEquipmentProperties(FTN.ConductingEquipment cimConductingEquipment, ResourceDescription rd, ImportHelper importHelper, TransformAndLoadReport report)
        {
            if ((cimConductingEquipment != null) && (rd != null))
            {
                Projekat9Converter.PopulateEquipmentProperties(cimConductingEquipment, rd, importHelper, report);
            }
        }

        public static void PopulateSwitchProperties(FTN.Switch cimSwitch, ResourceDescription rd, ImportHelper importHelper, TransformAndLoadReport report)
        {
            if ((cimSwitch != null) && (rd != null))
            {
                Projekat9Converter.PopulateConductingEquipmentProperties(cimSwitch, rd, importHelper, report);


                if (cimSwitch.SwitchingOperationsHasValue)
                {
                    long gid = importHelper.GetMappedGID(cimSwitch.SwitchingOperations.ID);
                    if (gid < 0)
                    {
                        report.Report.Append("WARNING: Convert ").Append(cimSwitch.GetType().ToString()).Append(" rdfID = \"").Append(cimSwitch.ID);
                        report.Report.Append("\" - Failed to set reference to SwitchingOperation: rdfID \"").Append(cimSwitch.SwitchingOperations.ID).AppendLine(" \" is not mapped to GID!");
                    }
                    rd.AddProperty(new Property(ModelCode.SWITCH_SWOP, gid));
                }      
            }
        }

        public static void PopulateDisconnectorProperties(FTN.Disconnector cimDisconnector, ResourceDescription rd, ImportHelper importHelper, TransformAndLoadReport report)
        {
            if ((cimDisconnector != null) && (rd != null))
            {
                Projekat9Converter.PopulateSwitchProperties(cimDisconnector, rd, importHelper, report);
            }
        }

        public static void PopulateBasicIntervalScheduleProperties(FTN.BasicIntervalSchedule cimBasicIntervalSchedule, ResourceDescription rd, ImportHelper importHelper, TransformAndLoadReport report)
        {
            if ((cimBasicIntervalSchedule != null) && (rd != null))
            {
                Projekat9Converter.PopulateIdentifiedObjectProperties(cimBasicIntervalSchedule, rd);

                if (cimBasicIntervalSchedule.StartTimeHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.BISCH_STIME, cimBasicIntervalSchedule.StartTime));
                }

                if (cimBasicIntervalSchedule.Value1MultiplierHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.BISCH_VAL1MUL, (short)GetDMSUnitMultiplier(cimBasicIntervalSchedule.Value1Multiplier)));
                }

                if (cimBasicIntervalSchedule.Value1UnitHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.BISCH_VAL1UN, (short)GetDMSUnitSymbol(cimBasicIntervalSchedule.Value1Unit)));
                }

                if (cimBasicIntervalSchedule.Value2MultiplierHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.BISCH_VAL2MUL, (short)GetDMSUnitMultiplier(cimBasicIntervalSchedule.Value2Multiplier)));
                }

                if (cimBasicIntervalSchedule.Value2UnitHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.BISCH_VAL2UN, (short)GetDMSUnitSymbol(cimBasicIntervalSchedule.Value2Unit)));
                }
            }
        }

        public static void PopulateRegularTimePointProperties(RegularTimePoint cimRegularTimePoint, ResourceDescription rd, ImportHelper importHelper, TransformAndLoadReport report)
        {
            if ((cimRegularTimePoint != null) && (rd != null))
            {
                Projekat9Converter.PopulateIdentifiedObjectProperties(cimRegularTimePoint, rd);

                if (cimRegularTimePoint.SequenceNumberHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.RTP_SEQ, cimRegularTimePoint.SequenceNumber));
                }

                if (cimRegularTimePoint.Value1HasValue)
                {
                    rd.AddProperty(new Property(ModelCode.RTP_VAL1, cimRegularTimePoint.Value1));
                }

                if (cimRegularTimePoint.Value2HasValue)
                {
                    rd.AddProperty(new Property(ModelCode.RTP_VAL2, cimRegularTimePoint.Value2));
                }

                if (cimRegularTimePoint.IntervalScheduleHasValue)
                {
                    long gid = importHelper.GetMappedGID(cimRegularTimePoint.IntervalSchedule.ID);
                    if (gid < 0)
                    {
                        report.Report.Append("WARNING: Convert ").Append(cimRegularTimePoint.GetType().ToString()).Append(" rdfID = \"").Append(cimRegularTimePoint.ID);
                        report.Report.Append("\" - Failed to set reference to RegularIntervalSchedule: rdfID \"").Append(cimRegularTimePoint.IntervalSchedule.ID).AppendLine(" \" is not mapped to GID!");
                    }
                    rd.AddProperty(new Property(ModelCode.RTP_RISCH, gid));
                }
            }
        }

        public static void PopulateIrregularTimePointProperties(IrregularTimePoint cimIrregularTimePoint, ResourceDescription rd, ImportHelper importHelper, TransformAndLoadReport report)
        {
            if ((cimIrregularTimePoint != null) && (rd != null))
            {
                Projekat9Converter.PopulateIdentifiedObjectProperties(cimIrregularTimePoint, rd);

                if (cimIrregularTimePoint.TimeHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.ITP_TIME, cimIrregularTimePoint.Time));
                }
                
                if (cimIrregularTimePoint.Value1HasValue)
                {
                    rd.AddProperty(new Property(ModelCode.ITP_VAL1, cimIrregularTimePoint.Value1));
                }                

                if (cimIrregularTimePoint.Value2HasValue)
                {
                    rd.AddProperty(new Property(ModelCode.ITP_VAL2, cimIrregularTimePoint.Value2));
                }

                if (cimIrregularTimePoint.IntervalScheduleHasValue)
                {
                    long gid = importHelper.GetMappedGID(cimIrregularTimePoint.IntervalSchedule.ID);
                    if (gid < 0)
                    {
                        report.Report.Append("WARNING: Convert ").Append(cimIrregularTimePoint.GetType().ToString()).Append(" rdfID = \"").Append(cimIrregularTimePoint.ID);
                        report.Report.Append("\" - Failed to set reference to IrregularIntervalSchedule: rdfID \"").Append(cimIrregularTimePoint.IntervalSchedule.ID).AppendLine(" \" is not mapped to GID!");
                    }
                    rd.AddProperty(new Property(ModelCode.ITP_IRISCH, gid));
                }
            }
        }


        public static void PopulateSwitchingOperationProperties(SwitchingOperation cimSwitchingOperation, ResourceDescription rd, ImportHelper importHelper, TransformAndLoadReport report)
        {
            if ((cimSwitchingOperation != null) && (rd != null))
            {
                Projekat9Converter.PopulateIdentifiedObjectProperties(cimSwitchingOperation, rd);

                if (cimSwitchingOperation.NewStateHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.SWOP_NSTATE,(short)GetDMSSwitchState(cimSwitchingOperation.NewState)));
                }

                if (cimSwitchingOperation.OperationTimeHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.SWOP_OPTIME, cimSwitchingOperation.OperationTime));
                }

                if (cimSwitchingOperation.OutageScheduleHasValue)
                {
                    long gid = importHelper.GetMappedGID(cimSwitchingOperation.OutageSchedule.ID);
                    if (gid < 0)
                    {
                        report.Report.Append("WARNING: Convert ").Append(cimSwitchingOperation.GetType().ToString()).Append(" rdfID = \"").Append(cimSwitchingOperation.ID);
                        report.Report.Append("\" - Failed to set reference to OutageSchedule: rdfID \"").Append(cimSwitchingOperation.OutageSchedule.ID).AppendLine(" \" is not mapped to GID!");
                    }
                    rd.AddProperty(new Property(ModelCode.SWOP_OUTSCH, gid));
                }
            }
        }

        public static void PopulateIrregularIntervalScheduleProperties(FTN.IrregularIntervalSchedule cimIrregularIntervalSchedule, ResourceDescription rd, ImportHelper importHelper, TransformAndLoadReport report)
        {
            if ((cimIrregularIntervalSchedule != null) && (rd != null))
            {
                Projekat9Converter.PopulateBasicIntervalScheduleProperties(cimIrregularIntervalSchedule, rd, importHelper, report);
            }
        }

        public static void PopulateOutageScheduleProperties(OutageSchedule cimOutageSchedule, ResourceDescription rd, ImportHelper importHelper, TransformAndLoadReport report)
        {
            if ((cimOutageSchedule != null) && (rd != null))
            {
                Projekat9Converter.PopulateIrregularIntervalScheduleProperties(cimOutageSchedule, rd,importHelper,report);
            }
        }

        public static void PopulateRegularIntervalScheduleProperties(RegularIntervalSchedule cimRegularIntervalSchedule, ResourceDescription rd, ImportHelper importHelper, TransformAndLoadReport report)
        {
            if ((cimRegularIntervalSchedule != null) && (rd != null))
            {
                Projekat9Converter.PopulateBasicIntervalScheduleProperties(cimRegularIntervalSchedule, rd,importHelper, report);

                if (cimRegularIntervalSchedule.EndTimeHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.RISCH_ENDTIME, cimRegularIntervalSchedule.EndTime));
                }

                if (cimRegularIntervalSchedule.TimeStepHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.RISCH_TIMESTEP, cimRegularIntervalSchedule.TimeStep));
                }
            }
        }

        #endregion

        #region Enums convert
        public static Common.UnitMultiplier GetDMSUnitMultiplier(FTN.UnitMultiplier multiplier)
		{
			switch (multiplier)
			{
				case FTN.UnitMultiplier.c:
					return Common.UnitMultiplier.c;				
				case FTN.UnitMultiplier.d:
					return Common.UnitMultiplier.d;
                case FTN.UnitMultiplier.G:
                    return Common.UnitMultiplier.G;
                case FTN.UnitMultiplier.k:
                    return Common.UnitMultiplier.k;
                case FTN.UnitMultiplier.m:
                    return Common.UnitMultiplier.m;
                case FTN.UnitMultiplier.M:
                    return Common.UnitMultiplier.M;
                case FTN.UnitMultiplier.micro:
                    return Common.UnitMultiplier.micro;
                case FTN.UnitMultiplier.n:
                    return Common.UnitMultiplier.n;
                case FTN.UnitMultiplier.none:
                    return Common.UnitMultiplier.none;
                case FTN.UnitMultiplier.p:
                    return Common.UnitMultiplier.p;
                default: return Common.UnitMultiplier.T;
			}
		}

		public static Common.UnitSymbol GetDMSUnitSymbol(FTN.UnitSymbol unitSymbol)
		{
			switch (unitSymbol)
			{
                case FTN.UnitSymbol.A:
                    return Common.UnitSymbol.A;
                case FTN.UnitSymbol.deg:
                    return Common.UnitSymbol.deg;
                case FTN.UnitSymbol.degC:
                    return Common.UnitSymbol.degC;
                case FTN.UnitSymbol.F:
                    return Common.UnitSymbol.F;
                case FTN.UnitSymbol.g:
                    return Common.UnitSymbol.g;
                case FTN.UnitSymbol.h:
                    return Common.UnitSymbol.h;
                case FTN.UnitSymbol.H:
                    return Common.UnitSymbol.H;
                case FTN.UnitSymbol.Hz:
                    return Common.UnitSymbol.Hz;
                case FTN.UnitSymbol.J:
                    return Common.UnitSymbol.J;
                case FTN.UnitSymbol.m:
                    return Common.UnitSymbol.m;
                case FTN.UnitSymbol.m2:
                    return Common.UnitSymbol.m2;
                case FTN.UnitSymbol.m3:
                    return Common.UnitSymbol.m3;
                case FTN.UnitSymbol.min:
                    return Common.UnitSymbol.min;
                case FTN.UnitSymbol.N:
                    return Common.UnitSymbol.N;
                case FTN.UnitSymbol.none:
                    return Common.UnitSymbol.none;
                case FTN.UnitSymbol.ohm:
                    return Common.UnitSymbol.ohm;
                case FTN.UnitSymbol.Pa:
                    return Common.UnitSymbol.Pa;
                case FTN.UnitSymbol.rad:
                    return Common.UnitSymbol.rad;
                case FTN.UnitSymbol.s:
                    return Common.UnitSymbol.s;
                case FTN.UnitSymbol.S:
                    return Common.UnitSymbol.S;
                case FTN.UnitSymbol.V:
                    return Common.UnitSymbol.V;
                case FTN.UnitSymbol.VA:
                    return Common.UnitSymbol.VA;
                case FTN.UnitSymbol.VAh:
                    return Common.UnitSymbol.VAh;
                case FTN.UnitSymbol.VAr:
                    return Common.UnitSymbol.VAr;
                case FTN.UnitSymbol.VArh:
                    return Common.UnitSymbol.VArh;
                case FTN.UnitSymbol.W:
                    return Common.UnitSymbol.W;
                default:
					return Common.UnitSymbol.Wh;
			}
		}

		public static Common.SwitchState GetDMSSwitchState(FTN.SwitchState switchState)
		{
			switch (switchState)
			{
				case FTN.SwitchState.close:
					return Common.SwitchState.close;
				default:
					return Common.SwitchState.open;
			}
		}
        #endregion Enums convert
    }
}
