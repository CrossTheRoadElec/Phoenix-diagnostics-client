namespace CTRE.Phoenix.dotNET
{
    /// <summary>
    /// Essentially a System.Threading.Thread wrapper to make startup/shutdowns easier.
    /// </summary>
    public class Thread
    {
        System.Threading.Thread _thread;
        System.Threading.ManualResetEvent _shuttingDown = new System.Threading.ManualResetEvent(false);


        public Thread(System.Threading.ThreadStart threadStart)
        {
            _thread = new System.Threading.Thread(threadStart);
            
        }
        public Thread(System.Threading.ParameterizedThreadStart parameterizedThreadStart)
        {
            _thread = new System.Threading.Thread(parameterizedThreadStart);
        }

        public void Start()
        {
            _thread.Start();
        }

        /// <summary>
        /// Polling routine the thread implem should periodically to determine when to return from thread routine.
        /// </summary>
        /// <returns></returns>
        public bool ShuttingDown()
        {
            bool retval = _shuttingDown.WaitOne(0);

            return retval;
        }

        /// <summary>
        /// Signal thread to shutdown.  Wait up to timeoutMs for thread to close.
        /// </summary>
        /// <param name="timeoutMs"></param>
        /// <returns></returns>
        public bool Shutdown(int timeoutMs)
        {
            bool retval;
            /* signal shutdown */
            _shuttingDown.Set();
            /* wait for thread to end */
            retval = _thread.Join(timeoutMs);
            return retval;
        }

        /// <summary>
        /// If Shutdown call returns false, application should either
        /// wait longer, modify thread implem for catching shut down request, or call Abort().
        /// Abort should be a last resort as a proper thread implementatoin should not require it.
        /// </summary>
        public void Abort()
        {
            _thread.Abort();
        }
    }
}
