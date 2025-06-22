using FTN.Common;
using FTN.ESI.SIMES.CIM.CIMAdapter;
using System;

namespace MVVM.Commands
{
    public static class ApplyDeltaCommand
    {
        public static bool ApplyDMSNetworkModelDeltaCommand(Delta nmsDelta,CIMAdapter adapter,out string log)
        {
            log = "Failed";

            if (nmsDelta != null)
            {
                try
                {
                    log = adapter.ApplyUpdates(nmsDelta);
                }
                catch (Exception)
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
            return true;
        }
    }
}