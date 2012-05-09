using System;
using System.Runtime.InteropServices;

namespace Samples.Steps
{
    //    The object that you've created needs to expose some method, that the outside world can call, in order to clean up unmanaged resources. There is even a standardized name for this method:
    //There was even an interface created, IDisposable, that has just that one method:
    //So you make your object expose the IDisposable interface, and that way you promise that you've written that single method to clean up your unmanaged resources:
    public class Step2 : IDisposable
    {
        private readonly String _instanceName;
        private IntPtr _unmanagedResource;
        private bool _disposed;

        public Step2(String instanceName)
        {
            _instanceName = instanceName;
            //The unmanaged memory allocated here can't be reclaimed by the Garbage Collector --> LEAK      
            _unmanagedResource = Marshal.StringToCoTaskMemAuto(instanceName);
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                // Release unmanaged resources.
                if (_unmanagedResource != IntPtr.Zero)
                {
                    Marshal.FreeCoTaskMem(_unmanagedResource);
                    Console.WriteLine("[{0}] Unmanaged memory freed at {1:x16}",
                                      _instanceName, _unmanagedResource.ToInt64());
                    _unmanagedResource = IntPtr.Zero;
                }
                _disposed = true;
            }
        }
    }
}
