using System;
using System.Runtime.InteropServices;

namespace Samples.Steps
{
    //The unmanaged memory allocated here can't be reclaimed by the Garbage Collector --> LEAK      
    public class Step1
    {
        private readonly String _instanceName;
        private IntPtr _unmanagedResource;
        
        public Step1(String instanceName)
        {
            _instanceName = instanceName;
            _unmanagedResource = Marshal.StringToCoTaskMemAuto(instanceName);
        }
    }
}
