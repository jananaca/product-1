﻿//------------------------------------------------------------------------------
// <auto-generated>
//    此代码是根据模板生成的。
//
//    手动更改此文件可能会导致应用程序中发生异常行为。
//    如果重新生成代码，则将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebApplication1.EF
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class EFEntities : DbContext
    {
        public EFEntities()
            : base("name=EFEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<category> category { get; set; }
        public DbSet<groups> groups { get; set; }
        public DbSet<material> material { get; set; }
        public DbSet<privilege> privilege { get; set; }
        public DbSet<product> product { get; set; }
        public DbSet<users> users { get; set; }
        public DbSet<upjd> upjd { get; set; }
    }
}