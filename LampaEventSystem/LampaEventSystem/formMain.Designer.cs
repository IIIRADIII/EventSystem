namespace LampaEventSystem
{
    partial class formMain
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnStart = new System.Windows.Forms.Button();
            this.txtboxLog = new System.Windows.Forms.TextBox();
            this.lblStatus = new System.Windows.Forms.Label();
            this.btnSendDateNotice = new System.Windows.Forms.Button();
            this.btnWorkNewEvents = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtNoticePeriod = new System.Windows.Forms.TextBox();
            this.txtDatePeriod = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(12, 12);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(117, 56);
            this.btnStart.TabIndex = 0;
            this.btnStart.Text = "Старт";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // txtboxLog
            // 
            this.txtboxLog.Location = new System.Drawing.Point(12, 74);
            this.txtboxLog.Multiline = true;
            this.txtboxLog.Name = "txtboxLog";
            this.txtboxLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtboxLog.Size = new System.Drawing.Size(651, 279);
            this.txtboxLog.TabIndex = 1;
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(159, 27);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(0, 13);
            this.lblStatus.TabIndex = 2;
            // 
            // btnSendDateNotice
            // 
            this.btnSendDateNotice.Location = new System.Drawing.Point(534, 12);
            this.btnSendDateNotice.Name = "btnSendDateNotice";
            this.btnSendDateNotice.Size = new System.Drawing.Size(129, 56);
            this.btnSendDateNotice.TabIndex = 4;
            this.btnSendDateNotice.Text = "Отправить уведомления по сроками";
            this.btnSendDateNotice.UseVisualStyleBackColor = true;
            this.btnSendDateNotice.Click += new System.EventHandler(this.btnSendDateNotice_Click);
            // 
            // btnWorkNewEvents
            // 
            this.btnWorkNewEvents.Location = new System.Drawing.Point(411, 12);
            this.btnWorkNewEvents.Name = "btnWorkNewEvents";
            this.btnWorkNewEvents.Size = new System.Drawing.Size(117, 56);
            this.btnWorkNewEvents.TabIndex = 5;
            this.btnWorkNewEvents.Text = "Обработать новые события";
            this.btnWorkNewEvents.UseVisualStyleBackColor = true;
            this.btnWorkNewEvents.Click += new System.EventHandler(this.btnWorkNewEvents_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(135, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(217, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Периодичность проверки событий в мин.";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(135, 40);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(202, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Периодичность проверки сроков в дн.";
            // 
            // txtNoticePeriod
            // 
            this.txtNoticePeriod.Location = new System.Drawing.Point(358, 11);
            this.txtNoticePeriod.Name = "txtNoticePeriod";
            this.txtNoticePeriod.Size = new System.Drawing.Size(33, 20);
            this.txtNoticePeriod.TabIndex = 8;
            this.txtNoticePeriod.Text = "1";
            // 
            // txtDatePeriod
            // 
            this.txtDatePeriod.Location = new System.Drawing.Point(358, 40);
            this.txtDatePeriod.Name = "txtDatePeriod";
            this.txtDatePeriod.Size = new System.Drawing.Size(33, 20);
            this.txtDatePeriod.TabIndex = 9;
            this.txtDatePeriod.Text = "1";
            // 
            // formMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(675, 365);
            this.Controls.Add(this.txtDatePeriod);
            this.Controls.Add(this.txtNoticePeriod);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnWorkNewEvents);
            this.Controls.Add(this.btnSendDateNotice);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.txtboxLog);
            this.Controls.Add(this.btnStart);
            this.Name = "formMain";
            this.Text = "Система уведомлений Lampa";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.TextBox txtboxLog;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Button btnSendDateNotice;
        private System.Windows.Forms.Button btnWorkNewEvents;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtNoticePeriod;
        private System.Windows.Forms.TextBox txtDatePeriod;
    }
}

