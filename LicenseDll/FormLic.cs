namespace LicenseDll
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class FormLic : Form
    {
        private Button button1;
        private cAesEx c_aase;
        private string c_reqCode;
        private IContainer components;
        private Label label1;
        private Label label2;
        private string m_licensekey;
        private int m_term;
        private TextBox textBox1;
        private TextBox textBox2;

        public FormLic(string reqcode, cAesEx inst)
        {
            this.InitializeComponent();
            string str = InsertString_string(reqcode.ToLower());
            this.textBox1.Text = str;
            this.c_reqCode = reqcode;
            this.c_aase = inst;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string liccode = this.textBox2.Text.Trim().ToUpper();
            if ((liccode.Length != 0x10) && (liccode.Length != 0x13))
            {
                this.textBox2.Text = "Invalid LicenseKey.";
            }
            else
            {
                liccode = liccode.Replace("-", "");
                int num = this.c_aase.TestLicenseKey(this.c_reqCode, liccode);
                if (num == -1)
                {
                    this.textBox2.Text = "Invalid LicenseKey.";
                }
                else
                {
                    this.Term = num;
                    this.Licensekey = liccode;
                    base.DialogResult = DialogResult.OK;
                    base.Close();
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.button1 = new Button();
            this.label1 = new Label();
            this.label2 = new Label();
            this.textBox1 = new TextBox();
            this.textBox2 = new TextBox();
            base.SuspendLayout();
            this.button1.Location = new Point(350, 130);
            this.button1.Name = "button1";
            this.button1.Size = new Size(0x5e, 0x29);
            this.button1.TabIndex = 0;
            this.button1.Text = "OK";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new EventHandler(this.button1_Click);
            this.label1.AutoSize = true;
            this.label1.BackColor = SystemColors.Control;
            this.label1.Location = new Point(0x1d, 40);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x55, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "Request Code";
            this.label2.AutoSize = true;
            this.label2.Location = new Point(0x1f, 0x55);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x4c, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "License Key";
            this.textBox1.Font = new Font("굴림", 12f, FontStyle.Bold, GraphicsUnit.Point, 0x81);
            this.textBox1.Location = new Point(0x81, 0x25);
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new Size(0x13b, 0x1a);
            this.textBox1.TabIndex = 3;
            this.textBox2.Font = new Font("굴림", 12f, FontStyle.Bold);
            this.textBox2.Location = new Point(0x81, 0x52);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new Size(0x13b, 0x1a);
            this.textBox2.TabIndex = 4;
            base.AutoScaleDimensions = new SizeF(7f, 12f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new Size(0x1d1, 0xbb);
            base.Controls.Add(this.textBox2);
            base.Controls.Add(this.textBox1);
            base.Controls.Add(this.label2);
            base.Controls.Add(this.label1);
            base.Controls.Add(this.button1);
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "FormLic";
            this.Text = "License";
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private static string InsertString_string(string code)
        {
            string str = "";
            int num = 0;
            foreach (char ch in code)
            {
                if ((num > 0) && ((num % 4) == 0))
                {
                    str = str + "-";
                }
                str = str + ch;
                num++;
            }
            return str;
        }

        public string Licensekey
        {
            get
            {
                return this.m_licensekey;
            }
            set
            {
                this.m_licensekey = value;
            }
        }

        public int Term
        {
            get
            {
                return this.m_term;
            }
            set
            {
                this.m_term = value;
            }
        }
    }
}

