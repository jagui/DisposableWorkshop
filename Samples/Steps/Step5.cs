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
    //The standard pattern to do this is to have Finalize and Dispose both call a third(!) method; where you pass a Boolean saying if you're calling it from Dispose (as opposed to Finalize), meaning it's safe to free managed resources.
   public class Step5 : IDisposable
    {
        private readonly String _instanceName;
        private IntPtr _unmanagedResource;
        private Bitmap _bitmap;
        private DbConnection _connection;
        private bool _disposed;


        public Step5(String instanceName)
        {
            _instanceName = instanceName;
            _unmanagedResource = Marshal.StringToCoTaskMemAuto(instanceName);
        }

        public void Dispose()
        {
            Dispose(true);
        }

        private void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    //Free managed resources
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
                }

                //Free unmanaged resources
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

        ~Step5()
        {
            //we're being finalized (i.e. destroyed), call Dispose in case the user forgot to
            Dispose(false);
        }
    }
}
