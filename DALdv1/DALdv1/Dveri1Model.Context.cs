﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DALdv1
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class dbdveri1Entities1 : DbContext
    {
        public dbdveri1Entities1()
            : base("name=dbdveri1Entities1")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Adresa> Adresas { get; set; }
        public virtual DbSet<Contact> Contacts { get; set; }
        public virtual DbSet<FotoFyrnitury> FotoFyrnituries { get; set; }
        public virtual DbSet<FotoMegkomnDverey> FotoMegkomnDvereys { get; set; }
        public virtual DbSet<FotoVhodnyhDverey> FotoVhodnyhDvereys { get; set; }
        public virtual DbSet<Furnitura> Furnituras { get; set; }
        public virtual DbSet<GrafikWork> GrafikWorks { get; set; }
        public virtual DbSet<Klient> Klients { get; set; }
        public virtual DbSet<MegkomnatnyeDveri> MegkomnatnyeDveris { get; set; }
        public virtual DbSet<SliderLeftImg> SliderLeftImgs { get; set; }
        public virtual DbSet<SliderMainImg> SliderMainImgs { get; set; }
        public virtual DbSet<VhodnyeDveri> VhodnyeDveris { get; set; }
    }
}
