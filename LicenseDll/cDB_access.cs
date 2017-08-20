namespace LicenseDll
{
    using Microsoft.Win32;
    using System;
    using System.Windows.Forms;

    internal class cDB_access
    {
        private string m_keyName;
        private string m_keyName_backup;
        private string m_keyName_backup_licServer;
        private string m_keyName_ori;
        private string m_keyName_ori_licServer;

        public cDB_access()
        {
            string name = Registry.LocalMachine.Name;
            this.m_keyName = name + @"\SOFTWARE\Microsoft\ASP.NET2.0";
            this.m_keyName_ori = this.m_keyName;
            this.m_keyName_backup = name + @"\SOFTWARE\Microsoft\XNA";
            this.m_keyName_ori_licServer = name + @"\SOFTWARE\Microsoft\XNA2";
            this.m_keyName_backup_licServer = name + @"\SOFTWARE\Microsoft\XNA3";
        }

        public void BackupMdb(string curdate)
        {
            this.m_keyName = this.m_keyName_backup;
            this.UpdateData(curdate);
            this.m_keyName = this.m_keyName_ori;
        }

        public void ChangeBackupFileName()
        {
            this.m_keyName_backup = this.m_keyName_backup_licServer;
            this.m_keyName_ori = this.m_keyName_ori_licServer;
            this.m_keyName = this.m_keyName_ori;
        }

        private bool CheckData(cLicenseInfo cinfo)
        {
            if (cinfo.D_start == null)
            {
                throw new Exception(" err 5781201 ");
            }
            if (cinfo.D_end == null)
            {
                throw new Exception(" err 5781202 ");
            }
            if (cinfo.C_reqCode == null)
            {
                throw new Exception(" err 5781203 ");
            }
            if (cinfo.C_licCode == null)
            {
                throw new Exception(" err 5781204 ");
            }
            if (cinfo.D_current == null)
            {
                throw new Exception(" err 5781205 ");
            }
            return true;
        }

        public void InsertData(cLicenseInfo data)
        {
            Registry.SetValue(this.m_keyName, "VerInfo1", data.D_start);
            Registry.SetValue(this.m_keyName, "VerInfo2", data.D_end);
            Registry.SetValue(this.m_keyName, "VerInfo3", data.C_reqCode);
            Registry.SetValue(this.m_keyName, "VerInfo4", data.C_licCode);
            Registry.SetValue(this.m_keyName, "VerInfo5", data.D_current);
        }

        public void MakeBackup(cLicenseInfo cinfo)
        {
            this.m_keyName = this.m_keyName_backup;
            this.InsertData(cinfo);
            this.m_keyName = this.m_keyName_ori;
        }

        public void MakeOri(cLicenseInfo cinfo)
        {
            cGlobalRef.writefileLine("MakeOri");
            this.m_keyName = this.m_keyName_ori;
            this.InsertData(cinfo);
        }

        public void ReadData(cLicenseInfo cinfo)
        {
            try
            {
                cinfo.D_start = (string) Registry.GetValue(this.m_keyName, "VerInfo1", null);
                cinfo.D_end = (string) Registry.GetValue(this.m_keyName, "VerInfo2", null);
                cinfo.C_reqCode = (string) Registry.GetValue(this.m_keyName, "VerInfo3", null);
                cinfo.C_licCode = (string) Registry.GetValue(this.m_keyName, "VerInfo4", null);
                cinfo.D_current = (string) Registry.GetValue(this.m_keyName, "VerInfo5", null);
                if (!this.CheckData(cinfo))
                {
                    throw new Exception("errr 1122 4544");
                }
            }
            catch (InvalidOperationException exception)
            {
                cGlobalRef.writefileLine("오류11");
                cGlobalRef.writefileLine(exception.Message);
                MessageBox.Show(exception.Message.ToString(), "오류11", MessageBoxButtons.OK);
                throw;
            }
            catch (Exception exception2)
            {
                cGlobalRef.writefileLine("오류14");
                cGlobalRef.writefileLine(exception2.Message);
                throw;
            }
        }

        public void RecoveryMdb()
        {
            cGlobalRef.writefileLine("RecoveryMdb");
            this.m_keyName = this.m_keyName_backup;
            cLicenseInfo cinfo = new cLicenseInfo();
            try
            {
                this.ReadData(cinfo);
            }
            catch (Exception exception)
            {
                cGlobalRef.writefileLine("err 61609994");
                cGlobalRef.writefileLine(exception.Message);
                throw;
            }
            this.MakeOri(cinfo);
        }

        public void UpdateData(string date)
        {
            Registry.SetValue(this.m_keyName, "VerInfo5", date);
        }
    }
}

