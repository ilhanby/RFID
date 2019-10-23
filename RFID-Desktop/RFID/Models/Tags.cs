using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RFID.Models
{
    public class Tags
    {
        string _EPC;
        public string EPC
        {
            get { return _EPC; }
            set { _EPC = value; }
        }

        uint _Count;
        public uint Count
        {
            get { return _Count; }
            set { _Count = value; }
        }

        string _RSSI;
        public string RSSI
        {
            get { return _RSSI; }
            set { _RSSI = value; }
        }

        string _Antenna;
        public string Antenna
        {
            get { return _Antenna; }
            set { _Antenna = value; }
        }
    }
}
