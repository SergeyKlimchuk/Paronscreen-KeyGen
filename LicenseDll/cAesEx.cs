namespace LicenseDll
{
    using System;
    using System.IO;
    using System.Security.Cryptography;
    using System.Text;

    public class cAesEx
    {
        private byte[] a_encrypted;
        private byte[] a_fromEncrypt;
        private byte[] a_IV = new byte[] { 0x5f, 0x42, 0x16, 0xdd, 0x74, 0xe0, 13, 0x3b, 12, 230, 0xa7, 0x75, 0xcd, 0x94, 0xc7, 0x3d };
        private byte[] a_IV2 = new byte[] { 230, 0xa7, 0x11, 0xcd, 0x94, 0x13, 0x3d, 0x5f, 0x42, 0x16, 0xdd, 0x10, 0xe0, 13, 0x3b, 12 };
        private byte[] a_key = new byte[] { 
            0x7c, 0x59, 0x37, 0xb8, 110, 0x62, 0x9f, 0x70, 0xf7, 0x21, 0xab, 15, 0xb7, 0x37, 0x2e, 0x3b,
            0x75, 40, 0x93, 0xd6, 14, 0x69, 0x1c, 0x2a, 0xa1, 0xf5, 0x98, 0xc5, 0x4a, 0x7c, 0xd5, 0x43
        };
        private byte[] a_key2 = new byte[] { 
            0x61, 0x2e, 210, 0x7c, 0x59, 0x37, 0xb8, 110, 0x62, 0x9f, 0x70, 0xf7, 0x21, 0xab, 15, 0xb7,
            0x3b, 0x75, 40, 0x93, 0xd6, 14, 0x69, 0x1c, 0x2a, 0xa1, 0xf5, 0x98, 0x4a, 0x7c, 0xd5, 0x43
        };
        private byte[] a_toEncrypt;
        private byte[] a_tot = new byte[200];
        private char[] arr_change = new char[100];
        private string c_license;
        private string c_machineCodeLicense;
        private string c_machineCodeLicense_time;
        private string c_machineCodeOnly;
        private RijndaelManaged c_myRijndael = new RijndaelManaged();
        private ASCIIEncoding c_textConverter = new ASCIIEncoding();
        private const int m_LEN_A_TOT = 200;
        private int m_len_max_data;
        private int m_tot = 0x24;

        public cAesEx()
        {
            this.MakeString();
        }

        public void AddString(byte[] arr1)
        {
            int length = arr1.Length;
            if (length > 200)
            {
                length = 200;
            }
            if (this.m_len_max_data < length)
            {
                this.m_len_max_data = length;
            }
            for (int i = 0; i < length; i++)
            {
                this.a_tot[i] = (byte) (this.a_tot[i] + arr1[i]);
            }
        }

        public void AddString(string aa)
        {
            byte[] bytes = this.c_textConverter.GetBytes(aa);
            this.AddString(bytes);
        }

        public void Change_key_iv()
        {
            this.a_key = this.a_key2;
            this.a_IV = this.a_IV2;
        }

        public string ChangeByte2String()
        {
            string str = "";
            foreach (byte num in this.a_encrypted)
            {
                str = str + (object)num + ",";
            }
            return str;
        }

        private string ChangeString(byte[] data, int max)
        {
            data = this.a_encrypted;
            this.DataAddEncrypt(data, max);
            string str = "";
            int num = 0;
            foreach (byte num2 in data)
            {
                int index = num2 % this.m_tot;
                str = str + this.arr_change[index];
                num++;
                if (num >= max)
                {
                    return str;
                }
            }
            return str;
        }

        private void DataAddEncrypt(byte[] data, int no)
        {
            if (data.Length > no)
            {
                for (int i = no; i < data.Length; i++)
                {
                    int index = i % no;
                    data[index] = (byte) (data[index] + data[i]);
                }
            }
        }

        public string Decrypt(byte[] original)
        {
            this.a_encrypted = original;
            ICryptoTransform transform = this.c_myRijndael.CreateDecryptor(this.a_key, this.a_IV);
            MemoryStream stream = new MemoryStream(this.a_encrypted);
            CryptoStream stream2 = new CryptoStream(stream, transform, CryptoStreamMode.Read);
            this.a_fromEncrypt = new byte[this.a_encrypted.Length];
            stream2.Read(this.a_fromEncrypt, 0, this.a_encrypted.Length);
            return this.c_textConverter.GetString(this.a_fromEncrypt);
        }

        public void Decrypt(string original)
        {
            this.a_encrypted = this.c_textConverter.GetBytes(original);
            ICryptoTransform transform = this.c_myRijndael.CreateDecryptor(this.a_key, this.a_IV);
            MemoryStream stream = new MemoryStream(this.a_encrypted);
            CryptoStream stream2 = new CryptoStream(stream, transform, CryptoStreamMode.Read);
            this.a_fromEncrypt = new byte[this.a_encrypted.Length];
            stream2.Read(this.a_fromEncrypt, 0, this.a_encrypted.Length);
            this.c_textConverter.GetString(this.a_fromEncrypt);
        }

        public void Encrypt(string original)
        {
            ICryptoTransform transform = this.c_myRijndael.CreateEncryptor(this.a_key, this.a_IV);
            MemoryStream stream = new MemoryStream();
            CryptoStream stream2 = new CryptoStream(stream, transform, CryptoStreamMode.Write);
            this.a_toEncrypt = this.c_textConverter.GetBytes(original);
            stream2.Write(this.a_toEncrypt, 0, this.a_toEncrypt.Length);
            stream2.FlushFinalBlock();
            this.a_encrypted = stream.ToArray();
        }

        public void Encrypt(byte[] original, int size)
        {
            ICryptoTransform transform = this.c_myRijndael.CreateEncryptor(this.a_key, this.a_IV);
            MemoryStream stream = new MemoryStream();
            CryptoStream stream2 = new CryptoStream(stream, transform, CryptoStreamMode.Write);
            stream2.Write(original, 0, size);
            stream2.FlushFinalBlock();
            this.a_encrypted = stream.ToArray();
        }

        public void EncryptUsingTot()
        {
            this.Encrypt(this.a_tot, this.m_len_max_data);
        }

        public string GetClientCode(string date)
        {
            this.GetMachineCode();
            this.c_machineCodeLicense_time = date;
            this.AddString(this.c_machineCodeLicense_time);
            this.EncryptUsingTot();
            string str = this.ChangeString(this.a_encrypted, 4);
            this.c_machineCodeLicense = this.c_machineCodeOnly + str;
            this.TestLicenseKey(this.c_machineCodeLicense, "ddddd");
            return this.c_machineCodeLicense;
        }

        public string GetLicenseKey(string machine, string term)
        {
            this.Encrypt(machine + term);
            this.c_license = this.ChangeString(this.a_encrypted, 160);
            return this.c_license;
        }

        public string GetMachineCode()
        {
            this.InitData();
            new cSystemInfo().GetSystemInfo(this);
            this.EncryptUsingTot();
            this.c_machineCodeOnly = this.ChangeString(this.a_encrypted, 8);
            return this.c_machineCodeOnly;
        }

        private void InitData()
        {
            this.a_tot = new byte[200];
            this.m_len_max_data = 0;
        }

        private void MakeString()
        {
            int index = 0;
            byte num2 = 0x30;
            for (int i = 0; i < 2; i++)
            {
                for (byte k = 0; k < 10; k = (byte) (k + 1))
                {
                    if (1 == k)
                    {
                        this.arr_change[index] = 'B';
                    }
                    else
                    {
                        this.arr_change[index] = (char) (num2 + k);
                    }
                    index++;
                }
            }
            num2 = 0x41;
            for (byte j = 0; j < 0x1a; j = (byte) (j + 1))
            {
                this.arr_change[index] = (char) (num2 + j);
                index++;
            }
            this.m_tot = index;
        }

        public int TestLicenseKey(string reqcode, string liccode)
        {
            string strB = null;
            for (int i = 0; i < 11; i++)
            {
                this.Encrypt(reqcode + i);
                strB = this.ChangeString(this.a_encrypted, 0x10);
                
                if (liccode.CompareTo(strB) == 0)
                {
                    return i;
                }
            }
            return -1;
        }
        public string Ty(string reqcode)
        {
            string strB = null;
            for (int i = 0; i < 1; i++)
            {
                this.Encrypt(reqcode + i);
                strB = this.ChangeString(this.a_encrypted, 0x10);

            }
            return strB;
        }
    }
}

