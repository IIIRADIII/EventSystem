using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LampaEventSystem
{
    class MailTemplate
    {
        public MailTemplate(int id,int docid, string doctable)
        {
            Id = id;
            parameters.Add("docid",docid.ToString());
            parameters.Add("doctable", doctable);
        }
        public int Id;
        public Dictionary<string, string> parameters = new Dictionary<string, string>();
        public string getSubject()
        {
            Additional ad = new Additional();
            string subject = "";
            EasySQL subjectGetter = new EasySQL("select subject from adm_EmailTemplate where id=" + Id.ToString());
            subject = ad.workSqlsInString(subjectGetter.ValueToString(),parameters);
            return subject;
        }

        public string getBody()
        {
            Additional ad = new Additional();
            string body = "";
            EasySQL bodyGetter = new EasySQL("select body from adm_EmailTemplate where id=" + Id.ToString());
            body = ad.workSqlsInString(bodyGetter.ValueToString(),parameters);
            return body;
        }
    }
}
