using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LampaEventService
{
    internal class Event
    {
        public Event(int id)
        {
            Dictionary<string, string> evDetails;
            EasySQL eventInfoGetter = new EasySQL("select * from Events where id=" + id.ToString());
            evDetails = eventInfoGetter.RowToDictionary();
            Id = Convert.ToInt32(evDetails["ID"]);
            eventId = Convert.ToInt32(evDetails["Event_id"]);
            parameters = evDetails["Event_params"].Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Select( pairs => pairs.Split('='))
                .ToDictionary(pair => pair[0].Replace(" ",string.Empty), pair => pair[1].Replace(" ",string.Empty));
            parameters.Add("docid", evDetails["Doc_id"]);
            parameters.Add("doctable", evDetails["Doc_table"]);
            eventDate = evDetails["Event_date"];
        }

        public int Id { get; }
        public int eventId { get; }
        int docId { get; }
        string docTable { get; }
        public Dictionary<string, string> parameters { get; }
        public string eventDate { get; }
        public void SetDone()
        {
            EasySQL EventDoneMaker = new EasySQL("update Events set is_processed=1, Date_processed=SYSDATETIME(), is_checked=1 where id=" + this.Id.ToString());
            EventDoneMaker.Update();
        }

        public void setChecked()
        {
            EasySQL EventCheckedMaker = new EasySQL("update Events set is_checked=1 where id=" + this.Id.ToString());
            EventCheckedMaker.Update();
        }



    }
}
