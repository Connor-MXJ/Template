using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MXJ.Core.Repositories.EF.Conventions
{

    public class DateTimeRowVersionConvention : Convention
    {
        public DateTimeRowVersionConvention()
        {
            this.Properties<DateTime?>().Where(p => p.Name.Equals("UpdatedTime"))
                .Configure(c => c.IsRowVersion());
            this.Properties<DateTime?>().Where(p => p.Name.Equals("UpdatedTime"))
                .Configure(c => c.HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity));
        }
    }
}
