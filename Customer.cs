using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace edgeChromium
{
    [ClassInterface(ClassInterfaceType.AutoDual)]
    [ComVisible(true)]
    public class Customer
    {
        int _customerID = 500;
       
        public int GetCustomerID()
        {
            return _customerID;
        }
      
        public void SetCustomerID(int customerID)
        {
            _customerID = customerID;
        }

    }
}
