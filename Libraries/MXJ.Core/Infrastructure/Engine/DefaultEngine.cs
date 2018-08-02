using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MXJ.Core.Infrastructure.IOC;
using MXJ.Core.Utility.Helper;

namespace MXJ.Core.Infrastructure.Engine
{
    public class DefaultEngine : IEngine
    {

        private IDependencyContainer _dependencyContainer;

        public IDependencyContainer DependencyContainer
        {
            get { return _dependencyContainer; }
        }

        public void Initialize()
        {
            _dependencyContainer = GetDependencyContainer(new DependencyContainerFactory());
        }

        private IDependencyContainer GetDependencyContainer(DependencyContainerFactory dependencyContainerFactory)
        {
            ArgumentChecker.IsNotNull(dependencyContainerFactory, "resolverFactory");
            return dependencyContainerFactory.CreateInstance();
        }

        public object GetObject()
        {
            return Obj;
        }

        public static object Obj = new object();
    }
}
