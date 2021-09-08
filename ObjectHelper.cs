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
    public class ObjectHelper
    {
        Customer _customer;

        DynamicPluginRouter _dynamicPluginRouter;
        public ObjectHelper()
        {
            _customer = new Customer();
        }

        public DynamicPluginRouter DynamicPluginRouter   // property
        {
            get { return _dynamicPluginRouter; }   // get method           
        }

        public object GetObjectForScripting()
        {
            _dynamicPluginRouter = new DynamicPluginRouter();
            _dynamicPluginRouter = AddMethod(_dynamicPluginRouter, "GetCustomerID");
            _dynamicPluginRouter = AddMethod(_dynamicPluginRouter, "SetCustomerID");
            return _dynamicPluginRouter;

        }

        private DynamicPluginRouter AddMethod(DynamicPluginRouter dynamicPluginRouter, string methodName)
        {
            Type delegateObj = Getdelegate(this.GetType(), methodName);
            Delegate currentDelegate = Delegate.CreateDelegate(delegateObj, this, methodName);

            if (currentDelegate != null)
            {
                dynamicPluginRouter.SetMethod(methodName, currentDelegate);
            }
            return dynamicPluginRouter;
        }

        private Type Getdelegate(Type type, string methodName)
        {
            return type.GetNestedType(string.Concat(methodName, "Delegate"));
        }       

        public delegate int GetCustomerIDDelegate();
        public int GetCustomerID()
        {
            return _customer.GetCustomerID();
        }

        public delegate void SetCustomerIDDelegate(int customerID);
        public void SetCustomerID(int customerID)
        {
            _customer.SetCustomerID(customerID);
        }
    }
}
