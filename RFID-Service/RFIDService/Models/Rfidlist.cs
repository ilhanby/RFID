using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RFIDService.Models
{
    public class Rfidlist
    {
        int _Id;
        public int Id
        {
            get { return _Id; }
            set { _Id = value; }
        }

        string _EPC;
        public string EPC
        {
            get { return _EPC; }
            set { _EPC = value; }
        }

        string _Antenna;
        public string Antenna
        {
            get { return _Antenna; }
            set { _Antenna = value; }
        }

        DateTime _CreationTime;
        public DateTime CreationTime
        {
            get { return _CreationTime; }
            set { _CreationTime = value; }
        }

        string _IsDeleted;
        public string IsDeleted
        {
            get { return _IsDeleted; }
            set { _IsDeleted = value; }
        }
    }
}
