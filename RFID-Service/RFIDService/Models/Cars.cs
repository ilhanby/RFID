using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RFIDService.Models
{
    public class Cars
    {
        int _Id;
        public int Id
        {
            get { return _Id; }
            set { _Id = value; }
        }

        string _CarNumber;
        public string CarNumber
        {
            get { return _CarNumber; }
            set { _CarNumber = value; }
        }

        string _EPC1;
        public string EPC1
        {
            get { return _EPC1; }
            set { _EPC1 = value; }
        }

        string _EPC2;
        public string EPC2
        {
            get { return _EPC2; }
            set { _EPC2 = value; }
        }

        string _EPC3;
        public string EPC3
        {
            get { return _EPC3; }
            set { _EPC3 = value; }
        }

        string _EPC4;
        public string EPC4
        {
            get { return _EPC4; }
            set { _EPC4 = value; }
        }

        string _EPC5;
        public string EPC5
        {
            get { return _EPC5; }
            set { _EPC5 = value; }
        }

        string _IsDeleted;
        public string IsDeleted
        {
            get { return _IsDeleted; }
            set { _IsDeleted = value; }
        }

        string _IsActive;
        public string IsActive
        {
            get { return _IsActive; }
            set { _IsActive = value; }
        }

        string _Description;
        public string Description
        {
            get { return _Description; }
            set { _Description = value; }
        }
    }
}
