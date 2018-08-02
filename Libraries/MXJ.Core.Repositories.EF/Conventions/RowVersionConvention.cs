using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MXJ.Core.Repositories.EF.Conventions
{
    public class RowVersionConvention : Convention
    {
        public RowVersionConvention()
        {
            this.Properties<byte[]>().Where(p => p.Name.Equals("RowVersion"))
                .Configure(c => c.IsRowVersion());
        }
    }
}
