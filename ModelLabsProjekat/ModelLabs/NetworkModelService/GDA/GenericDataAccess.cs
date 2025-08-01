﻿using FTN.Common;
using FTN.ServiceContracts;
using System;
using System.Collections.Generic;


namespace FTN.Services.NetworkModelService
{
    public class GenericDataAccess : INetworkModelGDAContract
	{
		private static Dictionary<int, ResourceIterator> resourceItMap = new Dictionary<int, ResourceIterator>();
		private static int resourceItId = 0;
		protected static NetworkModel nm = null;

		public GenericDataAccess()
		{
		}

		public static NetworkModel NetworkModel
		{
			set
			{
				nm = value;
			}
		}

		public UpdateResult ApplyUpdate(Delta delta)
		{
			return nm.ApplyDelta(delta);
		}

		public ResourceDescription GetValues(long resourceId, List<ModelCode> propIds)
		{
			try
			{				
				ResourceDescription retVal = nm.GetValues(resourceId, propIds);				
				return retVal;
			}			
			catch (Exception ex)
			{
				string message = string.Format("Getting values for resource with ID = 0x{0:x16} failed. {1}", resourceId, ex.Message);
				CommonTrace.WriteTrace(CommonTrace.TraceError, message);
				throw new Exception(message);
			}	
		}

		public int GetExtentValues(ModelCode entityType, List<ModelCode> propIds)
		{
			try
			{
				ResourceIterator ri = nm.GetExtentValues(entityType, propIds);
				int retVal = AddIterator(ri);

				return retVal;
			}
			catch (Exception ex)
			{
				string message = string.Format("Getting extent values for ModelCode = {0} failed. ", entityType, ex.Message);
				CommonTrace.WriteTrace(CommonTrace.TraceError, message);
				throw new Exception(message);
			}
		}

		public int GetRelatedValues(long source, List<ModelCode> propIds, Association association)
		{
			try
			{
				ResourceIterator ri = nm.GetRelatedValues(source, propIds, association);
				int retVal = AddIterator(ri);

				return retVal;
			}
			catch (Exception ex)
			{
				string message = string.Format("Getting related values for resource with ID = 0x{0:x16} failed. {1}", source, ex.Message);
				CommonTrace.WriteTrace(CommonTrace.TraceError, message);
				throw new Exception(message);
			}
		}					

		public List<ResourceDescription> IteratorNext(int n, int id)
		{			
			try
			{								
				List<ResourceDescription> retVal = GetIterator(id).Next(n);				

				return retVal;
			}			
			catch (Exception ex)
			{
				string message = string.Format("IteratorNext failed. Iterator ID = {0}. Resources to fetch count = {1}. {2} ", id, n, ex.Message);
				CommonTrace.WriteTrace(CommonTrace.TraceError, message);
				throw new Exception(message);
			}		
		}

		public bool IteratorRewind(int id)
		{			
			try
			{								
				GetIterator(id).Rewind();
				
				return true;
			}						
			catch (Exception ex)
			{
				string message = string.Format("IteratorRewind failed. Iterator ID = {0}. {1}", id, ex.Message);
				CommonTrace.WriteTrace(CommonTrace.TraceError, message);
				throw new Exception(message);
			}				
		}

		public int IteratorResourcesTotal(int id)
		{			
			try
			{				
				int retVal = GetIterator(id).ResourcesTotal();
				return retVal;
			}			
			catch (Exception ex)
			{
				string message = string.Format("IteratorResourcesTotal failed. Iterator ID = {0}. {1}", id, ex.Message);
				CommonTrace.WriteTrace(CommonTrace.TraceError, message);
				throw new Exception(message);
			}				
		}

		public int IteratorResourcesLeft(int id)
		{			
			try
			{								
				int resourcesLeft = GetIterator(id).ResourcesLeft();

				return resourcesLeft;
			}						
			catch (Exception ex)
			{
				string message = string.Format("IteratorResourcesLeft failed. Iterator ID = {0}. {1}", id, ex.Message);
				CommonTrace.WriteTrace(CommonTrace.TraceError, message);
				throw new Exception(message);
			}				
		}

		public bool IteratorClose(int id)
		{			
			try
			{				
				bool retVal = RemoveIterator(id);				

				return retVal;
			}			
			catch (Exception ex)
			{
				string message = string.Format("IteratorClose failed. Iterator ID = {0}. {1}", id, ex.Message);
				CommonTrace.WriteTrace(CommonTrace.TraceError, message);
				throw new Exception(message);
			}			
		}

		private int AddIterator(ResourceIterator iterator)
		{
			lock (resourceItMap)
			{
				int iteratorId = ++resourceItId;
				resourceItMap.Add(iteratorId, iterator);
				return iteratorId;
			}
		}

		private ResourceIterator GetIterator(int iteratorId)
		{
			lock (resourceItMap)
			{
				if (resourceItMap.ContainsKey(iteratorId))
				{
					return resourceItMap[iteratorId];
				}
				else
				{
					throw new Exception(string.Format("Iterator with given ID = {0} doesn't exist.", iteratorId));					
				}
			}
		}

		private bool RemoveIterator(int iteratorId)
		{
			lock (resourceItMap)
			{
				return resourceItMap.Remove(iteratorId);
			}
		}
	}
}