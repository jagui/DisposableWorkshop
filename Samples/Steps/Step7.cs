using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Samples.Steps
{
    // Inheritance
    // The derived class does not have a Finalize method
    // or a Dispose method without parameters because it inherits
    // them from the base class.
    public class Derived : Step6
    {
        private IntPtr _unmanagedResource;
        private bool _disposed;

        public Derived(string instanceName)
            : base(instanceName)
        {
            var reversed = instanceName.Reverse().ToString();
            _unmanagedResource = Marshal.StringToCoTaskMemAuto(reversed);
        }

        protected override void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                // Release unmanaged resources.
                if (_unmanagedResource != IntPtr.Zero)
                {
                    Marshal.FreeCoTaskMem(_unmanagedResource);
                    Console.WriteLine("[{0}] Unmanaged memory freed at {1:x16}",
                                      InstanceName, _unmanagedResource.ToInt64());
                    _unmanagedResource = IntPtr.Zero;
                }
                _disposed = true;
            }
            base.Dispose(disposing);

        }
    }
}
