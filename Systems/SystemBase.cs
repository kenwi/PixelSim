using System;
using System.Reflection;

namespace PixelSim.Systems
{
    public abstract class SystemBase
    {
        public SystemBase()
        {
            var c = this.GetType();
        }
    }
}
