using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace LampaEventSystem
{
    public class EventRule
    {
        public EventRule(int id)
        {
            Dictionary<string, string> ruleDetails;
            EasySQL eventRuleInfoGetter = new EasySQL("select * from adm_EventRules where id=" + id.ToString());
            ruleDetails = eventRuleInfoGetter.RowToDictionary();
            Id = Convert.ToInt32(ruleDetails["id"]);
            eventId = Convert.ToInt32(ruleDetails["event_id"]);
            eventType = Convert.ToInt32(ruleDetails["event_type"]);
            parameters = ruleDetails["event_detail"].Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(pairs => pairs.Split('='))
                .ToDictionary(pair => pair[0].Replace(" ",string.Empty), pair => pair[1].Replace(" ",string.Empty));
        }

        public int Id { get; }
        public int eventId { get; }
        public int eventType { get; }
        public Dictionary<string, string> parameters = new Dictionary<string, string>();
    }
}
