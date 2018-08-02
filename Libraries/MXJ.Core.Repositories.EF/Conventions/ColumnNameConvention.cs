using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MXJ.Core.Repositories.EF.Conventions
{
    public class ColumnNameConvention: Convention
    {
        public ColumnNameConvention()
        {
            this.Properties().Configure(c => c.HasColumnName(c.ClrPropertyInfo.Name.ToUpper()));
        }
    }
}
