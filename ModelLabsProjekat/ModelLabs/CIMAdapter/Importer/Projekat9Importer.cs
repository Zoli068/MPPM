using System;
using System.Collections.Generic;
using CIM.Model;
using FTN.Common;
using FTN.ESI.SIMES.CIM.CIMAdapter.Manager;

namespace FTN.ESI.SIMES.CIM.CIMAdapter.Importer
{
	/// <summary>
	/// PowerTransformerImporter
	/// </summary>
	public class Projekat9Importer
	{
		/// <summary> Singleton </summary>
		private static Projekat9Importer ptImporter = null;
		private static object singletoneLock = new object();

		private ConcreteModel concreteModel;
		private Delta delta;
		private ImportHelper importHelper;
		private TransformAndLoadReport report;


		#region Properties
		public static Projekat9Importer Instance
		{
			get
			{
				if (ptImporter == null)
				{
					lock (singletoneLock)
					{
						if (ptImporter == null)
						{
							ptImporter = new Projekat9Importer();
							ptImporter.Reset();
						}
					}
				}
				return ptImporter;
			}
		}

		public Delta NMSDelta
		{
			get 
			{
				return delta;
			}
		}
		#endregion Properties


		public void Reset()
		{
			concreteModel = null;
			delta = new Delta();
			importHelper = new ImportHelper();
			report = null;
		}

		public TransformAndLoadReport CreateNMSDelta(ConcreteModel cimConcreteModel)
		{
			LogManager.Log("Importing PowerTransformer Elements...", LogLevel.Info);
			report = new TransformAndLoadReport();
			concreteModel = cimConcreteModel;
			delta.ClearDeltaOperations();

			if ((concreteModel != null) && (concreteModel.ModelMap != null))
			{
				try
				{
					// convert into DMS elements
					ConvertModelAndPopulateDelta();
				}
				catch (Exception ex)
				{
					string message = string.Format("{0} - ERROR in data import - {1}", DateTime.Now, ex.Message);
					LogManager.Log(message);
					report.Report.AppendLine(ex.Message);
					report.Success = false;
				}
			}
			LogManager.Log("Importing PowerTransformer Elements - END.", LogLevel.Info);
			return report;
		}

		/// <summary>
		/// Method performs conversion of network elements from CIM based concrete model into DMS model.
		/// </summary>
		private void ConvertModelAndPopulateDelta()
		{
			LogManager.Log("Loading elements and creating delta...", LogLevel.Info);

			//// import all concrete model types (DMSType enum)
			ImportOutageSchedule();
            ImportSwitchingOperation();
			ImportDisconnector();
			ImportRegularIntervalSchedule();
			ImportIrregularTimePoint();
			ImportRegularTimePoint();

			LogManager.Log("Loading elements and creating delta completed.", LogLevel.Info);
		}

        #region Import

        #region OutageSchedule Import
        private void ImportOutageSchedule()
		{
			SortedDictionary<string, object> cimOutageSchedules = concreteModel.GetAllObjectsOfType("FTN.OutageSchedule");
			if (cimOutageSchedules != null)
			{
				foreach (KeyValuePair<string, object> cimOutageSchedulePair in cimOutageSchedules)
				{
					FTN.OutageSchedule cimOutageSchedule = cimOutageSchedulePair.Value as FTN.OutageSchedule;

					ResourceDescription rd = CreateOutageScheduleResourceDescription(cimOutageSchedule);
					if (rd != null)
					{
						delta.AddDeltaOperation(DeltaOpType.Insert, rd, true);
						report.Report.Append("OutageSchedule ID = ").Append(cimOutageSchedule.ID).Append(" SUCCESSFULLY converted to GID = ").AppendLine(rd.Id.ToString());
					}
					else
					{
						report.Report.Append("OutageSchedule ID = ").Append(cimOutageSchedule.ID).AppendLine(" FAILED to be converted");
					}
				}
				report.Report.AppendLine();
			}
		}

        private ResourceDescription CreateOutageScheduleResourceDescription(FTN.OutageSchedule cimOutageSchedule)
        {
            ResourceDescription rd = null;
            if (cimOutageSchedule != null)
            {
                long gid = ModelCodeHelper.CreateGlobalId(0, (short)DMSType.OUTAGESCHEDULE, importHelper.CheckOutIndexForDMSType(DMSType.OUTAGESCHEDULE));
                rd = new ResourceDescription(gid);
                importHelper.DefineIDMapping(cimOutageSchedule.ID, gid);

                ////populate ResourceDescription
                Projekat9Converter.PopulateOutageScheduleProperties(cimOutageSchedule, rd, importHelper, report);
            }
            return rd;
        }

        #endregion

        #region RegularIntervalSchedule Import

        private void ImportRegularIntervalSchedule()
        {
            SortedDictionary<string, object> regularIntervalSchedules = concreteModel.GetAllObjectsOfType("FTN.RegularIntervalSchedule");
            if (regularIntervalSchedules != null)
            {
                foreach (KeyValuePair<string, object> cimRegularIntervalSchedulesPair in regularIntervalSchedules)
                {
                    FTN.RegularIntervalSchedule cimRegularIntervalSchedule = cimRegularIntervalSchedulesPair.Value as FTN.RegularIntervalSchedule;

                    ResourceDescription rd = CreateRegularIntervalScheduleResourceDescription(cimRegularIntervalSchedule);
                    if (rd != null)
                    {
                        delta.AddDeltaOperation(DeltaOpType.Insert, rd, true);
                        report.Report.Append("RegularIntervalSchedule ID = ").Append(cimRegularIntervalSchedule.ID).Append(" SUCCESSFULLY converted to GID = ").AppendLine(rd.Id.ToString());
                    }
                    else
                    {
                        report.Report.Append("RegularIntervalSchedule ID = ").Append(cimRegularIntervalSchedule.ID).AppendLine(" FAILED to be converted");
                    }
                }
                report.Report.AppendLine();
            }
        }

        private ResourceDescription CreateRegularIntervalScheduleResourceDescription(FTN.RegularIntervalSchedule cimRegularIntervalSchedule)
        {
            ResourceDescription rd = null;
            if (cimRegularIntervalSchedule != null)
            {
                long gid = ModelCodeHelper.CreateGlobalId(0, (short)DMSType.REGULARINTERVALSCHEDULE, importHelper.CheckOutIndexForDMSType(DMSType.REGULARINTERVALSCHEDULE));
                rd = new ResourceDescription(gid);
                importHelper.DefineIDMapping(cimRegularIntervalSchedule.ID, gid);

                ////populate ResourceDescription
                Projekat9Converter.PopulateRegularIntervalScheduleProperties(cimRegularIntervalSchedule, rd, importHelper, report);
            }
            return rd;
        }

        #endregion

        #region RegularTimePoint Import

        private void ImportRegularTimePoint()
        {
            SortedDictionary<string, object> regularTimePoints = concreteModel.GetAllObjectsOfType("FTN.RegularTimePoint");
            if (regularTimePoints != null)
            {
                foreach (KeyValuePair<string, object> cimRegularTimePointPair in regularTimePoints)
                {
                    FTN.RegularTimePoint cimRegularTimePoint = cimRegularTimePointPair.Value as FTN.RegularTimePoint;

                    ResourceDescription rd = CreateRegularTimePointResourceDescription(cimRegularTimePoint);
                    if (rd != null)
                    {
                        delta.AddDeltaOperation(DeltaOpType.Insert, rd, true);
                        report.Report.Append("RegularTimePoint ID = ").Append(cimRegularTimePoint.ID).Append(" SUCCESSFULLY converted to GID = ").AppendLine(rd.Id.ToString());
                    }
                    else
                    {
                        report.Report.Append("RegularTimePoint ID = ").Append(cimRegularTimePoint.ID).AppendLine(" FAILED to be converted");
                    }
                }
                report.Report.AppendLine();
            }
        }

        private ResourceDescription CreateRegularTimePointResourceDescription(FTN.RegularTimePoint cimRegularTimePoint)
        {
            ResourceDescription rd = null;
            if (cimRegularTimePoint != null)
            {
                long gid = ModelCodeHelper.CreateGlobalId(0, (short)DMSType.REGULARTIMEPOINT, importHelper.CheckOutIndexForDMSType(DMSType.REGULARTIMEPOINT));
                rd = new ResourceDescription(gid);
                importHelper.DefineIDMapping(cimRegularTimePoint.ID, gid);

                ////populate ResourceDescription
                Projekat9Converter.PopulateRegularTimePointProperties(cimRegularTimePoint, rd, importHelper, report);							
            }
            return rd;
        }

        #endregion

        #region IrregularTimePoint Import

        private void ImportIrregularTimePoint()
        {
            SortedDictionary<string, object> irregularTimePoints = concreteModel.GetAllObjectsOfType("FTN.IrregularTimePoint");
            if (irregularTimePoints != null)
            {
                foreach (KeyValuePair<string, object> cimIrregularTimePointPair in irregularTimePoints)
                {
                    FTN.IrregularTimePoint cimIrregularTimePoint = cimIrregularTimePointPair.Value as FTN.IrregularTimePoint;

                    ResourceDescription rd = CreateIrregularTimePointDescription(cimIrregularTimePoint);
                    if (rd != null)
                    {
                        delta.AddDeltaOperation(DeltaOpType.Insert, rd, true);
                        report.Report.Append("IrregularTimePoint ID = ").Append(cimIrregularTimePoint.ID).Append(" SUCCESSFULLY converted to GID = ").AppendLine(rd.Id.ToString());
                    }
                    else
                    {
                        report.Report.Append("IrregularTimePoint ID = ").Append(cimIrregularTimePoint.ID).AppendLine(" FAILED to be converted");
                    }
                }
                report.Report.AppendLine();
            }
        }

        private ResourceDescription CreateIrregularTimePointDescription(FTN.IrregularTimePoint cimIrregularTimePoint)
        {
            ResourceDescription rd = null;
            if (cimIrregularTimePoint != null)
            {
                long gid = ModelCodeHelper.CreateGlobalId(0, (short)DMSType.IRREGULARTIMEPOINT, importHelper.CheckOutIndexForDMSType(DMSType.IRREGULARTIMEPOINT));
                rd = new ResourceDescription(gid);
                importHelper.DefineIDMapping(cimIrregularTimePoint.ID, gid);

                ////populate ResourceDescription
                Projekat9Converter.PopulateIrregularTimePointProperties(cimIrregularTimePoint, rd, importHelper, report);								
            }
            return rd;
        }

        #endregion

        #region Disconnector

        private void ImportDisconnector()
        {
            SortedDictionary<string, object> disconnectors = concreteModel.GetAllObjectsOfType("FTN.Disconnector");
            if (disconnectors != null)
            {
                foreach (KeyValuePair<string, object> cimDisconnectorPair in disconnectors)
                {
                    FTN.Disconnector cimDisconnector = cimDisconnectorPair.Value as FTN.Disconnector;

                    ResourceDescription rd = CreateDisconnectorDescription(cimDisconnector);
                    if (rd != null)
                    {
                        delta.AddDeltaOperation(DeltaOpType.Insert, rd, true);
                        report.Report.Append("Disconnector ID = ").Append(cimDisconnector.ID).Append(" SUCCESSFULLY converted to GID = ").AppendLine(rd.Id.ToString());
                    }
                    else
                    {
                        report.Report.Append("Disconnector ID = ").Append(cimDisconnector.ID).AppendLine(" FAILED to be converted");
                    }
                }
                report.Report.AppendLine();
            }
        }

        private ResourceDescription CreateDisconnectorDescription(FTN.Disconnector cimDisconnector)
        {
            ResourceDescription rd = null;
            if (cimDisconnector != null)
            {
                long gid = ModelCodeHelper.CreateGlobalId(0, (short)DMSType.DISCONNECTOR, importHelper.CheckOutIndexForDMSType(DMSType.DISCONNECTOR));
                rd = new ResourceDescription(gid);
                importHelper.DefineIDMapping(cimDisconnector.ID, gid);

                ////populate ResourceDescription
                Projekat9Converter.PopulateDisconnectorProperties(cimDisconnector, rd, importHelper, report);
            }
            return rd;
        }

        #endregion

        #region SwitchingOperation

        private void ImportSwitchingOperation()
        {
            SortedDictionary<string, object> swtichingOperations = concreteModel.GetAllObjectsOfType("FTN.SwitchingOperation");
            if (swtichingOperations != null)
            {
                foreach (KeyValuePair<string, object> cimSwitchingOperationPair in swtichingOperations)
                {
                    FTN.SwitchingOperation cimSwitchingOperation = cimSwitchingOperationPair.Value as FTN.SwitchingOperation;

                    ResourceDescription rd = CreateSwitchingOperationDescription(cimSwitchingOperation);
                    if (rd != null)
                    {
                        delta.AddDeltaOperation(DeltaOpType.Insert, rd, true);
                        report.Report.Append("SwitchingOperation ID = ").Append(cimSwitchingOperation.ID).Append(" SUCCESSFULLY converted to GID = ").AppendLine(rd.Id.ToString());
                    }
                    else
                    {
                        report.Report.Append("SwitchingOperation ID = ").Append(cimSwitchingOperation.ID).AppendLine(" FAILED to be converted");
                    }
                }
                report.Report.AppendLine();
            }
        }

        private ResourceDescription CreateSwitchingOperationDescription(FTN.SwitchingOperation cimSwitchingOperation)
        {
            ResourceDescription rd = null;
            if (cimSwitchingOperation != null)
            {
                long gid = ModelCodeHelper.CreateGlobalId(0, (short)DMSType.SWITCHINGOPERATION, importHelper.CheckOutIndexForDMSType(DMSType.SWITCHINGOPERATION));
                rd = new ResourceDescription(gid);
                importHelper.DefineIDMapping(cimSwitchingOperation.ID, gid);

                ////populate ResourceDescription
                Projekat9Converter.PopulateSwitchingOperationProperties(cimSwitchingOperation, rd, importHelper, report);
            }
            return rd;
        }

        #endregion

        #endregion Import
    }
}

