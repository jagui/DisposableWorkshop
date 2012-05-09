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
    //    If the user calls Dispose on your object, then everything has been cleaned up. Later on, when the Garbage Collector comes along and calls Finalize, it will then call Dispose again.

    //Not only is this wasteful, but if your object has junk references to objects you already disposed of from the last call to dispose, you'll try to dispose them again!

    //You'll notice in my code i was careful to remove references to objects that i've disposed, so i don't try to call Dispose on a junk object reference. But that didn't stop a subtle bug from creeping in.

    //When the user calls Dispose: the handle gdiCursorBitmapStreamFileHandle is destroyed. Later when the garbage collector runs, it will try to destroy the same handle again
    public class Step6 : IDisposable
    {
        private readonly String _instanceName;
        private IntPtr _unmanagedResource;
        private Bitmap _bitmap;
        private DbConnection _connection;
        private bool _disposed;

        public Step6(String instanceName)
        {
            _instanceName = instanceName;
            _unmanagedResource = Marshal.StringToCoTaskMemAuto(instanceName);
        }

        public string InstanceName
        {
            get { return _instanceName; }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
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

        ~Step6()
        {
            //we're being finalized (i.e. destroyed), call Dispose in case the user forgot to
            Dispose(false);
        }
    }
}
