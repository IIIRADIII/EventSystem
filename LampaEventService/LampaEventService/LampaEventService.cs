using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace LampaEventService
{
    public partial class LampaEventService : ServiceBase
    {
        private System.Timers.Timer _eventTimer = new System.Timers.Timer();
        private System.Timers.Timer _dateTimer = new System.Timers.Timer();
        private List<EventRule> _eventRules = new List<EventRule>();
        private List<EventRule> _dateRules = new List<EventRule>();
        private bool _eventTimerTaskSuccess;
        private bool _dateTimerTaskSuccess;
        Additional _ad = new Additional();

        public LampaEventService()
        {
            InitializeComponent();
        }

        private bool isTimeToCheckDates()
        {
            TimeSpan hoursStart = new TimeSpan(07, 0, 0);
            TimeSpan hoursEnd = new TimeSpan(08, 0, 0);
            TimeSpan now = DateTime.Now.TimeOfDay;
            if (hoursStart <= now && now < hoursEnd) { return true; }
            else { return false; };
        }
        private bool isTheSameParameters(Dictionary<string, string> evparams, Dictionary<string, string> ruleparams)
        {
            if (ruleparams.Keys.Contains("NoParams"))
            {
                return true;
            }
            foreach (string key in evparams.Keys)
            {
                if (key != "docid" && key != "doctable")
                {
                    if (!ruleparams.Keys.Contains(key))
                    {
                        return false;
                    }
                    else
                    {
                        if (!(evparams[key] == ruleparams[key]))
                        {
                            return false;
                        }
                    }
                }
                
            }
            return true;
        }
        private List<EventRule> GetEventRules()
        {
            List<EventRule> eventRules = new List<EventRule>();
            List<int> ruleIds = new List<int>();
            EasySQL ruleIdsGetter = new EasySQL("select id from adm_EventRules where event_type = 1 and active=1");
            NewStatusText(ruleIdsGetter.queryString);
            foreach (int id in ruleIdsGetter.IntColToList())
            {
                eventRules.Add(new EventRule(id));
            }
            return eventRules;
        }

        private List<EventRule> GetDateRules()
        {
            List<EventRule> eventRules = new List<EventRule>();
            List<int> ruleIds = new List<int>();
            EasySQL ruleIdsGetter = new EasySQL("select id from adm_EventRules where event_type = 2 and active=1");
            NewStatusText(ruleIdsGetter.queryString);
            foreach (int id in ruleIdsGetter.IntColToList())
            {
                eventRules.Add(new EventRule(id));
            }
            return eventRules;
        }

        private List<RuleDetail> GetEventRuleDetails(int id)
        {
            List<RuleDetail> ruleDetails = new List<RuleDetail>();
            List<int> ruleDetailsIds = new List<int>();
            EasySQL ruleDetailsIdsGetter = new EasySQL("select id from adm_EventRuleDetails where event_rule_id=" + id.ToString());
            NewStatusText(ruleDetailsIdsGetter.queryString);
            ruleDetailsIds = ruleDetailsIdsGetter.IntColToList();
            foreach (int ruleDetailId in ruleDetailsIds)
            {
                ruleDetails.Add(new RuleDetail(ruleDetailId));
            }
            return ruleDetails;
        }

        private List<Event> getNewEvents()
        {
            List<int> newEventsIds = new List<int>();
            List<Event> result = new List<Event>();
            NewStatusText("Проверяю наличие новых событий...");
            NewStatusText("select count(id) from Events where is_checked=0 or is_checked is null");
            EasySQL eventWorker = new EasySQL("select count(id) from Events where is_checked=0 or is_checked is null");
            var count = eventWorker.ValueToInt();
            if (count != 0)
            {
                NewStatusText("Кол-во новых событий -" + count.ToString() + ". Начинаю обработку событий...");
                NewStatusText("Вытаскиваю новые события...");
                NewStatusText("select id from Events where is_checked=0 or is_checked is null");
                eventWorker.queryString = "select id from Events where is_checked=0 or is_checked is null";
                newEventsIds.AddRange(eventWorker.IntColToList());
                foreach (int id in newEventsIds)
                {
                    result.Add(new Event(id));
                }
                return result;
            }
            else
            {
                NewStatusText("Новых событий не найдено");
                return result;
            }
        }

        private void workEvents()
        {
            List<Event> events = new List<Event>();
            events = getNewEvents();
            if (!events.Any())
            {
                NewStatusText("Нет новых событий. Ждем...");
            }
            else
            {
                int evCount = 0;
                foreach (Event ev in events)
                {
                    evCount++;
                    foreach (EventRule rule in _eventRules)
                    {
                        if ((ev.eventId == rule.eventId && isTheSameParameters(ev.parameters, rule.parameters))
                        || (ev.eventId == rule.eventId && !rule.parameters.Any()))
                        {
                            NewStatusText("Для события " + ev.Id.ToString() + " найдено правило " + rule.Id.ToString());
                            NewStatusText("Ищем детали для правила обработки " + rule.Id.ToString());
                            List<RuleDetail> ruleDetails = new List<RuleDetail>();
                            ruleDetails.AddRange(GetEventRuleDetails(rule.Id));
                            if (ruleDetails.Count != 0)
                            {
                                int rdCounter = 0;
                                NewStatusText("Для правила id " + rule.Id + " найдено "
                                    + ruleDetails.Count.ToString() + " деталей обработки. Начинаем обработку...");
                                foreach (RuleDetail rd in ruleDetails)
                                {
                                    string emType = "Уведомления";
                                    List<string> emlist = new List<string>();
                                    string subject;
                                    string body;


                                    NewStatusText("Вытаскиваем тему и текст сообщения для детали правила id" + rd.Id.ToString());
                                    MailTemplate mt = new MailTemplate(rd.templateId, Convert.ToInt32(ev.parameters["docid"]), ev.parameters["doctable"]);
                                    subject = mt.getSubject();
                                    body = mt.getBody();
                                    if (body != "")
                                    {
                                        body = body + " (" + ev.eventDate + ")";
                                    }
                                    NewStatusText("Тема: " + subject);
                                    NewStatusText("Тело: " + body);

                                    NewStatusText("Вытаскиваем список email для отправки для детали правила id " + rd.Id.ToString());
                                    MailSubscriberDetails msd = new MailSubscriberDetails(rd.subscriberId, Convert.ToInt32(ev.parameters["docid"]), ev.parameters["doctable"]);
                                    emlist = msd.GetEmailList();
                                    NewStatusText("Список email для отправки уведомлении : " + string.Join(",", emlist.ToArray()));

                                    if (emlist.Any())
                                    {
                                        MailSender ms = new MailSender(emType, emlist, subject, body);
                                        ms.Send();
                                        rdCounter++;
                                        NewStatusText(String.Format
                                            ("По событию {0} и правилу {1} были отправлены уведомления на ящики: {2} ",
                                            ev.Id.ToString(),
                                            rule.Id.ToString(), 
                                            string.Join(",", emlist.ToArray())
                                            )
                                            );
                                    }
                                    else
                                        NewStatusText("Список email для отправки уведомлений пустой, ничего не делаем");
                                    if (rdCounter == ruleDetails.Count)
                                    {
                                        ev.SetDone();
                                        rdCounter = 0;
                                    }
                                }

                            }
                            else
                            {
                                NewStatusText("Для правила обработки id=" + rule.Id.ToString() +
                                    " по событию " + ev.Id.ToString() + "  не заданы детали обработки. Переходим к следующему событию");
                                continue;
                            }
                            NewStatusText(System.Environment.NewLine);
                        }
                        else
                        {
                            NewStatusText("Для события " + ev.Id.ToString() + "  нет правил обработки. Переходим к следующему событию");
                            ev.setChecked();
                        }
                    }
                    if (events.Count == evCount)
                    {
                        NewStatusText("События обработаны");
                        evCount = 0;
                    }
                }

            }
        }

        public void workDates()
        {
            foreach (EventRule rule in _dateRules)
            {
                string[] rightParams = new string[] { "datevalue", "datecolumn", "doctable" };
                if (rightParams.All(k => rule.parameters.ContainsKey(k)))
                {
                    List<int> eventIds = new List<int>();
                    string eventIdsQuery = "select id from @doctable where @datecolumn is not null and (DATEDIFF(DAY,  DATEADD(day, -1, GETDATE()), @datecolumn)) <=@datevalue and (DATEDIFF(DAY,  DATEADD(day, -1, GETDATE()), @datecolumn)) > 0";
                    EasySQL dateEventDocsGetter = new EasySQL(_ad.replaceSqlParameters(eventIdsQuery, rule.parameters));
                    eventIds = dateEventDocsGetter.IntColToList();
                    foreach (int id in eventIds)
                    {
                        NewStatusText("Ищем детали для правила обработки " + rule.Id.ToString());
                        List<RuleDetail> ruleDetails = new List<RuleDetail>();
                        ruleDetails.AddRange(GetEventRuleDetails(rule.Id));
                        if (ruleDetails.Count != 0)
                        {
                            NewStatusText("Для правила id " + rule.Id + " найдено "
                                + ruleDetails.Count.ToString() + "правил. Начинаем обработку...");
                            foreach (RuleDetail rd in ruleDetails)
                            {
                                string emType = "Уведомления";
                                List<string> emlist = new List<string>();
                                string subject;
                                string body;


                                NewStatusText("Вытаскиваем тему и текст сообщения для детали правила id" + rd.Id.ToString());
                                MailTemplate mt = new MailTemplate(rd.templateId, id, rule.parameters["doctable"]);
                                subject = mt.getSubject();
                                body = mt.getBody();
                                NewStatusText("Тема: " + subject);
                                NewStatusText("Тело: " + body);

                                NewStatusText("Вытаскиваем список email для отправки для детали правила id " + rd.Id.ToString());
                                MailSubscriberDetails msd = new MailSubscriberDetails(rd.subscriberId, id, rule.parameters["doctable"]);
                                emlist = msd.GetEmailList();
                                NewStatusText("Список email для отправки уведомлении : " + string.Join(",", emlist.ToArray()));


                                if (emlist.Any())
                                {
                                    MailSender ms = new MailSender(emType, emlist, subject, body);
                                    ms.Send();
                                }
                                else
                                    NewStatusText("Список email для отправки уведомлений пустой, ничего не делаем");
                            }

                        }
                        else
                        {
                            NewStatusText("Для правила обработки дат id=" + rule.Id.ToString()
                                + "не заданы детали обработки. Переходим к следующему событию");
                            continue;
                        }
                    }
                }
                else
                {
                    NewStatusText("Для правила обработки дат id=" + rule.Id.ToString()
                        + " указаны неправильные параметры");
                    continue;
                }
            }
        }
        private void NewStatusText(string text)
        {
            string str = DateTime.Now.ToString() + " " + text;
            EasySQL logger = new EasySQL("Insert into EventSystemLog(message) values ('"
                + str + "');");
            logger.Update();
            try
            {
                if (!EventLog.SourceExists("LampaEventService"))
                {
                    EventLog.CreateEventSource("LampaEventService", "LampaEventService");
                }
                eventLog.Source = "MyExampleService";
                eventLog.WriteEntry(text);
            }
            catch { }
        }
        protected override void OnStart(string[] args)
        {
            _eventRules = GetEventRules();
            _dateRules = GetDateRules();

            NewStatusText("Start service");
            _eventTimer.AutoReset = true;
            _eventTimer.Interval = 60000;
            _eventTimer.Elapsed += _eventTimer_Elapsed;
            _eventTimer.Start();

            _dateTimer.AutoReset = true;
            _dateTimer.Interval = 3600000;
            _dateTimer.Elapsed += _dateTimer_Elapsed;
            _dateTimer.Start();
            NewStatusText("Service started");
        }

        protected override void OnStop()
        {
            NewStatusText("Stop service");
            _eventTimer.Stop();
            _eventTimer.Dispose();
            _eventTimer = null;

            _dateTimer.Stop();
            _dateTimer.Dispose();
            _dateTimer = null;
            NewStatusText("Service stopped");
        }

        private void _eventTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                workEvents();
                _eventTimerTaskSuccess = true;
            }
            catch (Exception ex)
            {
                _eventTimerTaskSuccess = false;
            }
            finally
            {
                if (_eventTimerTaskSuccess)
                {
                   _eventTimer.Start();
                }
            }
        }

        private void _dateTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                if (isTimeToCheckDates()) { workDates(); }
                _dateTimerTaskSuccess = true;
            }
            catch (Exception ex)
            {
                _dateTimerTaskSuccess = false;
            }
            finally
            {
                if (_dateTimerTaskSuccess)
                {
                    _dateTimer.Start();
                }
            }
            
        }
    }
}
