namespace LicenseDll
{
    using System;
    using System.Management;

    internal class cSystemInfo
    {
        public void GetSystemInfo(cAesEx aes)
        {
            string aa = null;
            SelectQuery query = new SelectQuery("Win32_DiskDrive");
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(query);
            foreach (ManagementObject obj2 in searcher.Get())
            {
                string str2 = obj2["DeviceID"].ToString();
                string strB = @"\\.\PHYSICALDRIVE0";
                if (str2.CompareTo(strB) == 0)
                {
                    aa = obj2["PNPDeviceID"].ToString();
                    aes.AddString(aa);
                    aa = obj2["Signature"].ToString();
                    aes.AddString(aa);
                    aa = obj2["Size"].ToString();
                    aes.AddString(aa);
                    aa = obj2["SystemName"].ToString();
                    aes.AddString(aa);
                    aa = obj2["TotalSectors"].ToString();
                    aes.AddString(aa);
                    break;
                }
            }
            ManagementObjectSearcher searcher2 = new ManagementObjectSearcher("select * from win32_processor");
            foreach (ManagementObject obj3 in searcher2.Get())
            {
                aa = obj3["Name"].ToString();
                aes.AddString(aa);
                aa = obj3["ProcessorID"].ToString();
                aes.AddString(aa);
                break;
            }
            ObjectQuery query2 = new ObjectQuery("SELECT * FROM Win32_NetworkAdapter");
            ManagementObjectSearcher searcher3 = new ManagementObjectSearcher(query2);
            query2 = new ObjectQuery("SELECT * FROM Win32_OperatingSystem");
            ManagementObjectCollection objects = new ManagementObjectSearcher(query2).Get();
            string str4 = null;
            foreach (ManagementObject obj4 in objects)
            {
                str4 = obj4["csname"].ToString();
                aes.AddString(str4);
                str4 = obj4["SerialNumber"].ToString();
                aes.AddString(str4);
            }
        }
    }
}

