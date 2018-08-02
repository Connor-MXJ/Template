using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MXJ.Projects.Domain.Models;

namespace MXJ.Projects.Domain.Context.Mapping
{
    public class ProvinceMap : EntityTypeConfiguration<Province>
    {
        public ProvinceMap()
        {
            ToTable("Province");
            HasKey(u => u.Code);
        }
    }

    public class CityMap : EntityTypeConfiguration<City>
    {
        public CityMap()
        {
            ToTable("City");
            HasKey(u => u.Code);
        }
    }

    public class AreaMap : EntityTypeConfiguration<Area>
    {
        public AreaMap()
        {
            ToTable("Area");
            HasKey(u => u.Code);
        }
    }
}
