using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Samples.Steps
{
    //What if we also allocated a huge bitmap? Sure, this is a managed .NET object, and the garbage collector will free it. But do you really 250MB of memory just sitting there - waiting for the garbage collector to eventually come along and free it?
    // What if there's an open database connection? Surely we don't want that connection sitting open, waiting for the GC to finalize the object.
    public class Step3 : IDisposable
    {
        private readonly String _instanceName;
        private IntPtr _unmanagedResource;
        private Bitmap _bitmap;
        private DbConnection _connection;
        private bool _disposed;

        public Step3(String instanceName)
        {
            _instanceName = instanceName;
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

                //Free managed resources too
                if (_connection != null)
                {
                    _connection.Dispose();
                    _connection = null;
                }
                if (_bitmap != null)
                {
                    _bitmap.Dispose();
                    _bitmap = null;
                }

                _disposed = true;
            }
        }

    }
}
