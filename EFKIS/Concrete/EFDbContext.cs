
using EFKIS.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFKIS.Concrete
{
    public class EFDbContext : DbContext
    {
        public EFDbContext()
            : base("name=KIS")
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("KOMETA");
        }

        public DbSet<KometaStrana> KometaStrana { get; set; }
        public DbSet<KometaStan> KometaStan { get; set; }
        // Реальное состояние вагонов
        public DbSet<KometaParkState> KometaParkState { get; set; }

        // Информация по станции Промышленная
        public DbSet<PromSostav> PromSostav { get; set; } // Применил составные ключи
        public DbSet<PromNatHist> PromNatHist { get; set;}
        public DbSet<PromVagon> PromVagon { get; set; }
        public DbSet<PromGruzSP> PromGruzSP { get; set; } // грузы на приходе
        public DbSet<PromCex> PromCex { get; set; } // перечень цехов

        // Информация по вагонам
        public DbSet<NumVagStan> NumVagStan { get; set; }
        public DbSet<NumVagStpr1Gr> NumVagStpr1Gr { get; set; } // груз на станции прибытия?
        public DbSet<KometaVagonSob> KometaVagonSob { get; set; } // аренда вагонов
        public DbSet<KometaSobstvForNakl> KometaSobstvForNakl { get; set; } // собственник по накладным       
        public DbSet<NumVagStpr1InStDoc> NumVagStpr1InStDoc { get; set; } // Составы по внутреним станциям по прибытию  
        public DbSet<NumVagStpr1InStVag> NumVagStpr1InStVag { get; set; } // Вагоны по внутреним станциям по прибытию 
        public DbSet<NumVagStpr1OutStDoc> NumVagStpr1OutStDoc { get; set; } // Составы по внутреним станциям по отправке  
        public DbSet<NumVagStpr1OutStVag> NumVagStpr1OutStVag { get; set; } // Вагоны по внутреним станциям по прибытию 
        public DbSet<NumVagStpr1Tupik> NumVagStpr1Tupik { get; set; } // Список тупиков по цехам 
        public DbSet<NumVagStran> NumVagStran { get; set; } // Список стран для отправки 
        public DbSet<NumVagStrana> NumVagStrana { get; set; } // Список стран прибытия
        public DbSet<NumVagGodn> NumVagGodn { get; set; } // Список годностей
        public DbSet<NumVagGDStait> NumVagGDStait { get; set; } // Список станций для отправки
    }
}
