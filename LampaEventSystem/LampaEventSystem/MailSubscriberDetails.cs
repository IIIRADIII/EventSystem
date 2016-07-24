using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace LampaEventSystem
{
    class MailSubscriberDetails
    {
        public MailSubscriberDetails(int subscriberid, int docid, string doctable)
        {
            subscriberId = subscriberid;
            parameters.Add("docid", docid.ToString());
            parameters.Add("doctable", doctable);
        }
        public Dictionary<string, string> parameters = new Dictionary<string, string>();
        public int subscriberId;
        private Dictionary<int,string> GetSubscriberDetails(int id)
        {
            EasySQL subsDetailsGetter = new EasySQL("select id,type from adm_subscriberDetails where subscriber_id=" + id.ToString());
            return subsDetailsGetter.IntStrColsToDictionary();        
        }
        public List<string> GetEmailList()
        {
            Dictionary<int, string> Ids = GetSubscriberDetails(subscriberId);
            List<string> emails = new List<string>();
            List<int> concret_ids = new List<int>();
            List<int> sql_ids = new List<int>();
            List<int> role_ids = new List<int>();
            foreach (KeyValuePair<int, string> item in Ids)
            {
                
                switch (item.Value)
                {
                    case "3":
                        concret_ids.Add(item.Key);
                        break;
                    case "4":
                        sql_ids.Add(item.Key);
                        break;
                    case "5":
                        role_ids.Add(item.Key);
                        break;
                }
            }
            if (concret_ids.Any())
            {
                EasySQL emailGetter = new EasySQL();
                emailGetter.queryString = "select value from adm_SubscriberDetails where id in ("
                                          + string.Join(",", concret_ids) + ")";
                emails.AddRange(emailGetter.StrColToList());
            }
            if (sql_ids.Any())
            {
                foreach (int id in sql_ids)
                {
                    Additional ad = new Additional();
                    EasySQL sqlInfoGetter = new EasySQL("select value from adm_SubscriberDetails where id=" + id.ToString());
                    string mailQuery = ad.replaceSqlParameters(sqlInfoGetter.ValueToString(), parameters);
                    EasySQL sqlEmailsGetter = new EasySQL(mailQuery);
                    emails.AddRange(sqlEmailsGetter.StrColToList());
                    foreach (string email in emails)
                    {
                        email.Replace(" ", "");
                    }
                }
            }
            return emails;
        }
    }
}
