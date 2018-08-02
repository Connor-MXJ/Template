using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MXJ.Core.Repositories.EF.Conventions
{
    public class UrlConvention: Convention
    {
        public UrlConvention()
        {
            this.Properties<string>().Where(p => p.Name.ToUpper().EndsWith("URL"))
                .Configure(c => c.HasMaxLength(200));
        }
    }
}
