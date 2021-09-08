using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace edgeChromium
{
    [ClassInterface(ClassInterfaceType.AutoDual)]
    [ComVisible(true)]
    public class DynamicPluginRouter : IReflect
    {
        public string LogPath = @"C:\temp\edgeChrome\";
        readonly Dictionary<string, Delegate> _methods = new Dictionary<string, Delegate>();
        public Dictionary<string, Delegate> Methods
        {
            get
            {
                return _methods;
            }
        }

        public void SetMethod(string name, Delegate value)
        {
            _methods[name] = value;
        }

        static Exception NotImplemented()
        {
            var method = new StackTrace(true).GetFrame(1).GetMethod().Name;
            Debug.Assert(false, method);
            return new NotImplementedException(method);
        }

        #region IReflect
        // IReflect

        public FieldInfo GetField(string name, BindingFlags bindingAttr)
        {
            throw NotImplemented();
        }

        public FieldInfo[] GetFields(BindingFlags bindingAttr)
        {
            return new FieldInfo[0];
        }

        public MemberInfo[] GetMember(string name, BindingFlags bindingAttr)
        {
            throw NotImplemented();
        }

        public MemberInfo[] GetMembers(BindingFlags bindingAttr)
        {
            return new MemberInfo[0];
        }

        public MethodInfo GetMethod(string name, BindingFlags bindingAttr)
        {
            throw NotImplemented();
        }

        public MethodInfo GetMethod(string name, BindingFlags bindingAttr, Binder binder, Type[] types, ParameterModifier[] modifiers)
        {
            throw NotImplemented();
        }

        public MethodInfo[] GetMethods(BindingFlags bindingAttr)
        {
            MethodInfo[] methodInfo = _methods.Keys.Select(name => new DynamicMethodInfo(name, _methods[name].Method)).ToArray();
            return methodInfo;
        }

        public PropertyInfo[] GetProperties(BindingFlags bindingAttr)
        {
            return new PropertyInfo[0];
        }

        public PropertyInfo GetProperty(string name, BindingFlags bindingAttr, Binder binder, Type returnType, Type[] types, ParameterModifier[] modifiers)
        {
            throw NotImplemented();
        }

        public PropertyInfo GetProperty(string name, BindingFlags bindingAttr)
        {
            throw NotImplemented();
        }


        public object InvokeMember(string name, BindingFlags invokeAttr, Binder binder, object target, object[] args, ParameterModifier[] modifiers, CultureInfo culture, string[] namedParameters)
        {            
            try
            {             

                if (target == this)
                {
                    Delegate method;
                    if (!_methods.TryGetValue(name, out method))
                        throw new MissingMethodException();
                    object[] newArgs;
                    if (args.Length != method.Method.GetParameters().Length)
                    {
                        newArgs = new object[method.Method.GetParameters().Length];
                        for (int i = 0; i < method.Method.GetParameters().Length; i++)
                        {
                            if (i < args.Length && args.GetValue(i) != null)
                            {
                                newArgs[i] = args.GetValue(i);
                            }
                            else
                            {
                                newArgs[i] = method.Method.GetParameters()[i].DefaultValue;
                            }
                        }
                    }
                    else
                    {
                        newArgs = args;
                    }
                    return method.DynamicInvoke(newArgs);
                }
            }
            catch (Exception ex)
            {
                string message = string.Empty;
                if (ex.InnerException != null)
                {
                    log(string.Concat("Invoke Member (inner exception): ", name, " ", ex.InnerException.Message, Environment.NewLine));
                }
                log(string.Concat("Invoke Member: ", name, " ", ex.Message, Environment.NewLine, "stackTrace: ", ex.StackTrace));

            }
            throw new ArgumentException();
        }



        private void log(string msg)
        {
            try
            {
                if (Directory.Exists(LogPath))
                {
                    File.AppendAllText(string.Concat(LogPath, "log.log"), string.Concat(DateTime.UtcNow.ToString("MM/dd/yyyy hh:mm:ss.ff"), "\t", msg, Environment.NewLine));
                }
            }
            catch
            {
            }
        }

        public Type UnderlyingSystemType
        {
            get { throw NotImplemented(); }
        }
        #endregion

        #region DynamicMethodInfo
        // DynamicPropertyInfo

        class DynamicMethodInfo : System.Reflection.MethodInfo
        {
            string _name;
            MethodInfo _mi;

            public DynamicMethodInfo(string name, MethodInfo mi)
                : base()
            {
                //_name = name;
                _name = name.Replace('.', '_');
                _mi = mi;
            }

            public override MethodInfo GetBaseDefinition()
            {
                return _mi.GetBaseDefinition();
            }

            public override ICustomAttributeProvider ReturnTypeCustomAttributes
            {
                get { return _mi.ReturnTypeCustomAttributes; }
            }

            public override MethodAttributes Attributes
            {
                get { return _mi.Attributes; }
            }

            public override MethodImplAttributes GetMethodImplementationFlags()
            {
                return _mi.GetMethodImplementationFlags();
            }

            public override ParameterInfo[] GetParameters()
            {
                return _mi.GetParameters();
            }

            public override object Invoke(object obj, BindingFlags invokeAttr, Binder binder, object[] parameters, System.Globalization.CultureInfo culture)
            {
                return _mi.Invoke(obj, invokeAttr, binder, parameters, culture);
            }

            public override RuntimeMethodHandle MethodHandle
            {
                get { return _mi.MethodHandle; }
            }

            public override Type DeclaringType
            {
                get { return _mi.DeclaringType; }
            }

            public override object[] GetCustomAttributes(Type attributeType, bool inherit)
            {
                return _mi.GetCustomAttributes(attributeType, inherit);
            }

            public override object[] GetCustomAttributes(bool inherit)
            {
                return _mi.GetCustomAttributes(inherit);
            }

            public override bool IsDefined(Type attributeType, bool inherit)
            {
                return _mi.IsDefined(attributeType, inherit);
            }

            public override string Name
            {
                get { return _name; }
            }

            public override Type ReflectedType
            {
                get { return _mi.ReflectedType; }
            }
        }
        #endregion      

    }
}
