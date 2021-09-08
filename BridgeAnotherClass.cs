using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace edgeChromium
{
    // Bridge and BridgeAnotherClass are C# classes that implement IDispatch and works with AddHostObjectToScript.
    [ClassInterface(ClassInterfaceType.AutoDual)]
    [ComVisible(true)]
    public class BridgeAnotherClass
    {
        // Sample property.
        public string Prop { get; set; } = "Example";
    }

    [ClassInterface(ClassInterfaceType.AutoDual)]
    [ComVisible(true)]
    public class Bridge
    {
        public string Func(string param)
        {
            return "Example: " + param;
        }

        public int GetCustomerID()
        {
            return 500+5;
        }

        public bool IsSecurityEnabled()
        {
            return false;
        }        

        public ObjectHelper objectHelper { get; set; } = new ObjectHelper();

        // Sample indexed property.
        [System.Runtime.CompilerServices.IndexerName("Items")]
        public string this[int index]
        {
            get { return m_dictionary[index]; }
            set { m_dictionary[index] = value; }
        }
        private Dictionary<int, string> m_dictionary = new Dictionary<int, string>();
    }

}
