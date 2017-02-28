namespace DronePilot
{
    partial class MainScreen
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
            this.btn_connect = new System.Windows.Forms.Button();
            this.txt_ipaddr = new System.Windows.Forms.TextBox();
            this.rtxt_log = new System.Windows.Forms.RichTextBox();
            this.txt_ipport = new System.Windows.Forms.MaskedTextBox();
            this.SuspendLayout();
            // 
            // btn_connect
            // 
            this.btn_connect.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btn_connect.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_connect.Location = new System.Drawing.Point(27, 48);
            this.btn_connect.Name = "btn_connect";
            this.btn_connect.Size = new System.Drawing.Size(117, 58);
            this.btn_connect.TabIndex = 0;
            this.btn_connect.Text = "Connect to drone";
            this.btn_connect.UseVisualStyleBackColor = true;
            this.btn_connect.Click += new System.EventHandler(this.btn_connect_Click);
            // 
            // txt_ipaddr
            // 
            this.txt_ipaddr.Location = new System.Drawing.Point(27, 129);
            this.txt_ipaddr.Name = "txt_ipaddr";
            this.txt_ipaddr.Size = new System.Drawing.Size(139, 30);
            this.txt_ipaddr.TabIndex = 1;
            this.txt_ipaddr.Text = "Drone IP";
            this.txt_ipaddr.Enter += new System.EventHandler(this.txt_ipaddr_Enter);
            this.txt_ipaddr.Leave += new System.EventHandler(this.txt_ipaddr_Leave);
            // 
            // rtxt_log
            // 
            this.rtxt_log.Location = new System.Drawing.Point(204, 12);
            this.rtxt_log.Name = "rtxt_log";
            this.rtxt_log.Size = new System.Drawing.Size(289, 455);
            this.rtxt_log.TabIndex = 2;
            this.rtxt_log.Text = "";
            // 
            // txt_ipport
            // 
            this.txt_ipport.Location = new System.Drawing.Point(27, 165);
            this.txt_ipport.Mask = "00000";
            this.txt_ipport.Name = "txt_ipport";
            this.txt_ipport.Size = new System.Drawing.Size(139, 30);
            this.txt_ipport.TabIndex = 3;
            this.txt_ipport.ValidatingType = typeof(int);
            // 
            // MainScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 23F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(29)))), ((int)(((byte)(29)))), ((int)(((byte)(29)))));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(508, 499);
            this.Controls.Add(this.txt_ipport);
            this.Controls.Add(this.rtxt_log);
            this.Controls.Add(this.txt_ipaddr);
            this.Controls.Add(this.btn_connect);
            this.Font = new System.Drawing.Font("Segoe UI Symbol", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "MainScreen";
            this.Text = "DronePilot";
            this.Load += new System.EventHandler(this.MainScreen_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_connect;
        private System.Windows.Forms.TextBox txt_ipaddr;
        private System.Windows.Forms.RichTextBox rtxt_log;
        private System.Windows.Forms.MaskedTextBox txt_ipport;
    }
}

