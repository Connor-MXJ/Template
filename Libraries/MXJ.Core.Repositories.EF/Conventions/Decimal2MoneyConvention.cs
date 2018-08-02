using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MXJ.Core.Repositories.EF.Conventions
{
    public class Decimal2MoneyConvention : Convention
    {
        public Decimal2MoneyConvention()
        {
            this.Properties<decimal>().Where(p => p.Name.EndsWith("Amt"))
                .Configure(c => c.HasColumnType("Money"));
        }
    }
}
