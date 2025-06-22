using FTN.Common;
using System;
using System.Collections.Generic;
namespace MVVM.Helpers
{
    public static class ResourceDescriptionToListOfStrings
    {
        public static List<Tuple<string, string>> ConvertToListOfStrings(ResourceDescription rd)
        {
            Type type = null;
            EnumDescs enumDescs = new EnumDescs();
            List<Tuple<string, string>> response = new List<Tuple<string, string>>();

            if (rd != null) 
            {
                foreach (Property prop in  rd.Properties) 
                {            
                    type = enumDescs.GetEnumTypeForPropertyId(prop.Id, false);

                    if(type != null)
                    {
                        response.Add(new Tuple<string,string>(prop.Id.ToString(),Enum.ToObject(type,prop.AsEnum()).ToString()));               
                    }
                    else
                    {
                        if((0x00000000000000ff & (long)prop.Id) == 0x09)
                        {
                             response.Add(new Tuple<string,string>(prop.Id.ToString(),prop.AsReference().ToString()));               

                        }
                        else
                        {
                            if((0x00000000000000ff & (long)prop.Id) == 0x19)
                            {
                                int num = 0;
                                string res = "";
                                List<long> references = prop.AsReferences();

                                foreach(long val in references)
                                {
                                    num++;
                                    res += val.ToString()+"   ";
                                }

                                response.Add(new Tuple<string, string>(prop.Id.ToString(), res));
                            }
                            else
                            {
                                response.Add(new Tuple<string,string>(prop.Id.ToString(),prop.ToString()));               
                            }
                        }
                    }
                }
            }
            return response;
        }
    }
}