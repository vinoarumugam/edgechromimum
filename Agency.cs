using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace edgeChromium
{
    public class Agency
    {
        int _agencyID = 1000;

        public int GetAgencyID()
        {
            return _agencyID;
        }

        public void SetAgencyID(int agencyID)
        {
            _agencyID = agencyID;
        }

        public string GetAgencyDetails(int agencyID)
        {
            return $"ID: {agencyID} Name : B2C International.";
        }
    }
}
