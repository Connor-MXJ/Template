using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MXJ.Core.Domain.Models;

namespace MXJ.Core.Domain.Repositories
{
    public interface IReadOnlyRepository//<TEntity> where TEntity : BaseEntity
    {
    }
}
