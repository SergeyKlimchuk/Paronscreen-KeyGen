namespace LicenseDll
{
    using System;
    using System.IO;
    using System.Text;

    public class cGlobalRef
    {
        public static readonly int[] a_Period = new int[] { 0xbb8, 5, 10, 15, 30, 60, 90, 120, 150, 180 };
        private static string m_logfilepath = "./loggolf.txt";
        public const int MAX_ARRAY_TERM = 9;

        public static void writefile(string data)
        {
            try
            {
                FileStream stream = new FileStream(m_logfilepath, FileMode.Append);
                StreamWriter writer = new StreamWriter(stream, Encoding.Default);
                writer.BaseStream.Seek(0L, SeekOrigin.End);
                writer.WriteLine(DateTime.Now);
                writer.Write(data);
                writer.Flush();
                writer.Close();
            }
            catch (Exception)
            {
            }
        }

        public static void writefileLine(string data)
        {
            try
            {
                FileStream stream = new FileStream(m_logfilepath, FileMode.Append);
                StreamWriter writer = new StreamWriter(stream, Encoding.Default);
                writer.BaseStream.Seek(0L, SeekOrigin.End);
                writer.WriteLine(DateTime.Now);
                writer.WriteLine(data);
                writer.Flush();
                writer.Close();
            }
            catch (Exception)
            {
            }
        }
    }
}

