using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LampaEventSystem
{
    public class RuleDetail
    {
        public RuleDetail(int id)
        {
            EasySQL ruleDetailGetter = new EasySQL("select * from adm_EventRuleDetails where id=" + id.ToString());
            Dictionary<string, string> ruleDetails = new Dictionary<string, string>();
            ruleDetails = ruleDetailGetter.RowToDictionary();
            Id = Convert.ToInt32(ruleDetails["id"]);
            eventRuleId = Convert.ToInt32(ruleDetails["event_rule_id"]);
            subscriberId = Convert.ToInt32(ruleDetails["subscriber_id"]);
            templateId = Convert.ToInt32(ruleDetails["template_id"]);
            eventWorkKindId = Convert.ToInt32(ruleDetails["eventworkkind_id"]);
        }
        public int Id { get; }
        public int eventRuleId { get; }
        public int subscriberId { get; }
        public int templateId { get; }
        public int eventWorkKindId { get; }
    }
}
