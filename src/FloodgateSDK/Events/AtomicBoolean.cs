using System.Threading;

namespace FloodGate.SDK.Events
{
    internal class AtomicBoolean
    {
        internal volatile int _value; // 0 = false, 1 = true

        internal AtomicBoolean()
        {
            _value = 0;
        }

        internal bool Get()
        {
            return _value != 0;
        }

        internal void Set(bool value)
        {
            Interlocked.Exchange(ref _value, value ? 1 : 0);
        }

        internal bool GetAndSet(bool value)
        {
            int current = Interlocked.Exchange(ref _value, value ? 1 : 0);
            return current != 0;
        }
    }
}
