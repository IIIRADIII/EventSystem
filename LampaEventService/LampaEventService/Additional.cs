using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LampaEventService
{
    public class Additional
    {
        public string replaceSqlParameters(string s,  Dictionary<string,string> parameters)
        {
            string query = s;
            foreach(KeyValuePair<string,string> par in parameters)
            {
                query = query.Replace("@" + par.Key.ToString(), par.Value);
            }

            return query;
        }
        public string workSqlsInString(string s, Dictionary<string, string> parameters)
        {
            if (s.Contains("<sql>"))
            {
                var startTag = "<sql>";
                int startIndex = s.IndexOf(startTag) + startTag.Length;
                int insertValuePoint = s.IndexOf(startTag);
                int endIndex = s.IndexOf("</sql>", startIndex);
                string query = s.Substring(startIndex, endIndex - startIndex);
                foreach (KeyValuePair<string, string> par in parameters)
                {
                    query = query.Replace("@" + par.Key.ToString(), par.Value);
                }
                EasySQL getValue = new EasySQL(query);
                s = s.Remove(startIndex-5, endIndex - startIndex + 11);
                s = s.Insert(insertValuePoint, getValue.ValueToString());
                s = workSqlsInString(s,parameters);
                return s;
            }
            else
                return s;
        }
    }
}
