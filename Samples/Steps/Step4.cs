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
    //What if the person forgot to call Dispose() on your object? Then they would leak some unmanaged resources!
    //Enter the finalizer which is called by the 

    //Note: They won't leak managed resources, because eventually the garbage collector is going to run, on a background thread, and free the memory associated with any unused objects. This will include your object, and any managed objects you use (e.g. the Bitmap and the DbConnection).

    //The garbage collector will eventually free all managed objects. When it does, it calls the Finalize method on the object. The GC doesn't know, or care, about your Dispose method. That was just a name we chose for a method we call when we want to get rid of unmanaged stuff.

    public class Step4 : IDisposable
    {
        private readonly String _instanceName;
        private IntPtr _unmanagedResource;
        private Bitmap _bitmap;
        private DbConnection _connection;
        private bool _disposed;

        public Step4(String instanceName)
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
                    _connection.Dispose(); //We don't know the order on which objects are Finalized, hence this object could be already finalized
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

        ~Step4()
        {
            Dispose(); //BUG, see Dispose above
        }
    }
}
