using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RFIDService.Models
{
    public class Parameters
    {
        string _Parameter;
        public string Parameter
        {
            get { return _Parameter; }
            set { _Parameter = value; }
        }

        string _Value;
        public string Value
        {
            get { return _Value; }
            set { _Value = value; }
        }
    }
}
