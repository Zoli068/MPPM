using FTN.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVM.Helpers
{
    public static class ResourceDescriptionToListOfStrings
    {

        public static List<Tuple<string, string>> ConvertToListOfStrings(ResourceDescription rd)
        {
            List<Tuple<string, string>> response = new List<Tuple<string, string>>();

            if (rd != null) 
            {

                response.Add(new Tuple<string, string>(ModelCode.IDOBJ_GID.ToString(), rd.Id.ToString()));

                foreach (Property prop in  rd.Properties) 
                {   
                    response.Add(new Tuple<string,string>(prop.Id.ToString(),prop.ToString()));               
                }
            }
            return response;
        }
    }
}
