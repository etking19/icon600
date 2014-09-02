using System;
using System.Collections.Generic;
using System.Text;

namespace Utils.Hooks
{
    public abstract class IHooks
    {
        public abstract bool StartHook(int threadId);
        public abstract bool StopHook();

        public abstract bool IsHooking();
    }
}
