namespace Chatnik.ClientApplication.Forms
{
    partial class LoginForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.buttonLogin = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.textBoxUsername = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxSubscriberPort = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxPublisherPort = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxRemoteAddress = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // buttonLogin
            // 
            this.buttonLogin.Location = new System.Drawing.Point(12, 286);
            this.buttonLogin.Name = "buttonLogin";
            this.buttonLogin.Size = new System.Drawing.Size(214, 23);
            this.buttonLogin.TabIndex = 17;
            this.buttonLogin.Text = "Log in";
            this.buttonLogin.UseVisualStyleBackColor = true;
            this.buttonLogin.Click += new System.EventHandler(this.buttonLogin_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(15, 195);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(60, 15);
            this.label4.TabIndex = 16;
            this.label4.Text = "Username";
            // 
            // textBoxUsername
            // 
            this.textBoxUsername.Location = new System.Drawing.Point(12, 217);
            this.textBoxUsername.Name = "textBoxUsername";
            this.textBoxUsername.Size = new System.Drawing.Size(352, 23);
            this.textBoxUsername.TabIndex = 15;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(202, 129);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(87, 15);
            this.label3.TabIndex = 14;
            this.label3.Text = "Subscriber port";
            // 
            // textBoxSubscriberPort
            // 
            this.textBoxSubscriberPort.Location = new System.Drawing.Point(197, 151);
            this.textBoxSubscriberPort.Name = "textBoxSubscriberPort";
            this.textBoxSubscriberPort.Size = new System.Drawing.Size(167, 23);
            this.textBoxSubscriberPort.TabIndex = 13;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 129);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(81, 15);
            this.label2.TabIndex = 12;
            this.label2.Text = "Publisher port";
            // 
            // textBoxPublisherPort
            // 
            this.textBoxPublisherPort.Location = new System.Drawing.Point(12, 151);
            this.textBoxPublisherPort.Name = "textBoxPublisherPort";
            this.textBoxPublisherPort.Size = new System.Drawing.Size(179, 23);
            this.textBoxPublisherPort.TabIndex = 11;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 64);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(91, 15);
            this.label1.TabIndex = 10;
            this.label1.Text = "Remote address";
            // 
            // textBoxRemoteAddress
            // 
            this.textBoxRemoteAddress.Location = new System.Drawing.Point(12, 86);
            this.textBoxRemoteAddress.Name = "textBoxRemoteAddress";
            this.textBoxRemoteAddress.Size = new System.Drawing.Size(352, 23);
            this.textBoxRemoteAddress.TabIndex = 9;
            // 
            // LoginForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(393, 387);
            this.Controls.Add(this.buttonLogin);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.textBoxUsername);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBoxSubscriberPort);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBoxPublisherPort);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBoxRemoteAddress);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "LoginForm";
            this.Text = "LoginForm";
            this.Load += new System.EventHandler(this.LoginForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonLogin;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBoxUsername;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxSubscriberPort;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxPublisherPort;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxRemoteAddress;
    }
}