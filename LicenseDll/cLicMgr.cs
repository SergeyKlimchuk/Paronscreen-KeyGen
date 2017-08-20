namespace LicenseDll
{
    using System;
    using System.Globalization;
    using System.Text.RegularExpressions;
    using System.Windows.Forms;

    public class cLicMgr
    {
        private cAesEx c_aes = new cAesEx();
        private cDB_access c_db = new cDB_access();
        private cLicenseInfo c_licinfo;
        private cSystemInfo c_sysinfo = new cSystemInfo();

        public void Change_key_iv()
        {
            this.c_aes.Change_key_iv();
            this.c_db.ChangeBackupFileName();
        }

        private bool CheckDate(DateTime now, string date)
        {
            string str = date;
            DateTime time = Convert.ToDateTime(str);
            if (now > time)
            {
                return false;
            }
            return true;
        }

        private int CheckGameStart()
        {
            this.c_licinfo = new cLicenseInfo();
            try
            {
                this.c_db.ReadData(this.c_licinfo);
            }
            catch (Exception)
            {
                this.c_db.RecoveryMdb();
                try
                {
                    this.c_db.ReadData(this.c_licinfo);
                }
                catch (Exception exception)
                {
                    cGlobalRef.writefileLine("라이센스정보가 손상됬습니다. 라이센스키를 새로 등록해야 합니다.");
                    cGlobalRef.writefileLine(exception.Message);
                    MessageBox.Show("라이센스정보가 손상됬습니다. 라이센스키를 새로 등록해야 합니다.");
                    throw;
                }
            }
            try
            {
                this.DecodeLicInfo();
            }
            catch (Exception exception2)
            {
                cGlobalRef.writefileLine("err 142549 ");
                cGlobalRef.writefileLine(exception2.Message);
                return 0;
            }
            DateTime now = DateTime.Now;
            string date = this.c_licinfo.D_end.Replace("Z", "");
            if (!this.CheckDate(now, date))
            {
                cGlobalRef.writefileLine("인증기간 만료. 연장 또는 정식 등록이 필요합니다.   ");
                MessageBox.Show("인증기간 만료. 연장 또는 정식 등록이 필요합니다.");
                return 0;
            }
            string str2 = this.c_licinfo.D_current.Replace("Z", "");
            if (this.CheckDate(now, str2))
            {
                cGlobalRef.writefileLine(" err 9457812 ");
                return 0;
            }
            string clientCode = this.c_aes.GetClientCode(this.c_licinfo.D_start);
            if (clientCode != this.c_licinfo.C_reqCode)
            {
                cGlobalRef.writefileLine(" err 140547 ");
                return 0;
            }
            int num = this.c_aes.TestLicenseKey(clientCode, this.c_licinfo.C_licCode);
            int index = num;
            if (num == -1)
            {
                cGlobalRef.writefileLine(" err 2014102 ");
                return 0;
            }
            string str5 = Convert.ToDateTime(this.c_licinfo.D_start.Replace("Z", "")).AddDays((double) cGlobalRef.a_Period[index]).ToString("u");
            if (this.c_licinfo.D_end != str5)
            {
                cGlobalRef.writefileLine(" err710211 ");
                return 0;
            }
            this.c_licinfo.D_current = DateTime.Now.ToString("u");
            this.EncodeLicInfo();
            this.c_db.UpdateData(this.c_licinfo.D_current);
            this.c_db.MakeBackup(this.c_licinfo);
            return 1;
        }

        public void DecodeLicInfo()
        {
            string str = null;
            this.c_licinfo.D_start = this.DecodeString(this.c_licinfo.D_start);
            str = this.c_licinfo.D_start.Replace("=", "").Replace("+", "");
            this.c_licinfo.D_start = str;
            this.c_licinfo.C_licCode = this.DecodeString(this.c_licinfo.C_licCode);
            this.c_licinfo.C_reqCode = this.DecodeString(this.c_licinfo.C_reqCode);
            str = this.c_licinfo.C_reqCode.Replace("=", "").Replace("+", "");
            this.c_licinfo.C_reqCode = str;
            this.c_licinfo.D_current = this.DecodeString(this.c_licinfo.D_current);
            this.c_licinfo.D_end = this.DecodeString(this.c_licinfo.D_end);
            str = this.c_licinfo.D_end.Replace("=", "").Replace("+", "");
            this.c_licinfo.D_end = str;
        }

        public string DecodeString(string str)
        {
            string[] strArray = new Regex(",").Split(str);
            int num = strArray.Length - 1;
            byte[] original = new byte[num];
            int index = 0;
            foreach (string str2 in strArray)
            {
                if (str2.Length > 0)
                {
                    original[index] = (byte) (original[index] + byte.Parse(str2, NumberStyles.Number));
                    index++;
                }
            }
            try
            {
                str = this.c_aes.Decrypt(original);
            }
            catch (Exception exception)
            {
                cGlobalRef.writefileLine("err 95410527112");
                cGlobalRef.writefileLine(exception.Message);
                throw;
            }
            str = str.Replace("\0", "");
            return str;
        }

        public void EncodeLicInfo()
        {
            string str = "==";
            this.c_licinfo.D_start = str + this.c_licinfo.D_start;
            this.c_aes.Encrypt(this.c_licinfo.D_start);
            this.c_licinfo.D_start = this.c_aes.ChangeByte2String();
            this.c_aes.Encrypt(this.c_licinfo.C_licCode);
            this.c_licinfo.C_licCode = this.c_aes.ChangeByte2String();
            str = "==+++===";
            this.c_licinfo.C_reqCode = str + this.c_licinfo.C_reqCode;
            this.c_aes.Encrypt(this.c_licinfo.C_reqCode);
            this.c_licinfo.C_reqCode = this.c_aes.ChangeByte2String();
            this.c_aes.Encrypt(this.c_licinfo.D_current);
            this.c_licinfo.D_current = this.c_aes.ChangeByte2String();
            str = "++";
            this.c_licinfo.D_end = str + this.c_licinfo.D_end;
            this.c_aes.Encrypt(this.c_licinfo.D_end);
            this.c_licinfo.D_end = this.c_aes.ChangeByte2String();
        }

        public int GameStart()
        {
            int num = 0;
            try
            {
                num = this.CheckGameStart();
            }
            catch (Exception exception)
            {
                cGlobalRef.writefileLine("err 8970052");
                cGlobalRef.writefileLine(exception.Message);
            }
            DateTime.Now.ToString("u");
            if (num != 0)
            {
                return num;
            }
            DateTime now = DateTime.Now;
            string date = now.ToString("u");
            string clientCode = this.c_aes.GetClientCode(date);
            FormLic lic = new FormLic(clientCode, this.c_aes);
            if (lic.ShowDialog() == DialogResult.OK)
            {
                this.c_licinfo = new cLicenseInfo();
                this.c_licinfo.D_end = now.AddDays((double) cGlobalRef.a_Period[lic.Term]).ToString("u");
                this.c_licinfo.D_start = date;
                this.c_licinfo.D_current = DateTime.Now.ToString("u");
                this.c_licinfo.C_reqCode = clientCode;
                this.c_licinfo.C_licCode = lic.Licensekey;
                this.EncodeLicInfo();
                this.c_db.MakeOri(this.c_licinfo);
                this.c_db.MakeBackup(this.c_licinfo);
                return 1;
            }
            return 0;
        }
    }
}

