using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MXJ.Core.Infrastructure.IOC;

namespace MXJ.Core.Infrastructure.Engine
{
    public interface IEngine
    {
        IDependencyContainer DependencyContainer { get; }
        void Initialize();
        object GetObject();
    }
}

