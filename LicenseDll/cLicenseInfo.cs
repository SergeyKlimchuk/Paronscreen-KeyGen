namespace LicenseDll
{
    using System;

    internal class cLicenseInfo
    {
        private string c_licCode;
        private string c_reqCode;
        private string d_current;
        private string d_end;
        private string d_start;

        public string C_licCode
        {
            get
            {
                return this.c_licCode;
            }
            set
            {
                this.c_licCode = value;
            }
        }

        public string C_reqCode
        {
            get
            {
                return this.c_reqCode;
            }
            set
            {
                this.c_reqCode = value;
            }
        }

        public string D_current
        {
            get
            {
                return this.d_current;
            }
            set
            {
                this.d_current = value;
            }
        }

        public string D_end
        {
            get
            {
                return this.d_end;
            }
            set
            {
                this.d_end = value;
            }
        }

        public string D_start
        {
            get
            {
                return this.d_start;
            }
            set
            {
                this.d_start = value;
            }
        }
    }
}

