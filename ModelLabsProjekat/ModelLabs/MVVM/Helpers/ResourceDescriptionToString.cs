using FTN.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;
namespace MVVM.Helpers
{
    public static class ResourceDescriptionToListOfStrings
    {

        public static List<Tuple<string, string>> ConvertToListOfStrings(ResourceDescription rd)
        {
            List<Tuple<string, string>> response = new List<Tuple<string, string>>();

            EnumDescs enumDescs = new EnumDescs();
            if (rd != null) 
            {
                foreach (Property prop in  rd.Properties) 
                {

                    Type type = null;

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
                                List<long> references = prop.AsReferences();
                               string res = "";
                                int num = 0;
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
