using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MXJ.Core.Repositories.EF.Conventions
{
    public class PathConvention : Convention
    {
        public PathConvention()
        {
            this.Properties<string>().Where(p => p.Name == "Path")
                .Configure(c => c.HasMaxLength(100).IsRequired());
        }
    }
}
