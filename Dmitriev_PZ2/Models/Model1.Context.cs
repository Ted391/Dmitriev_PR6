﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Dmitriev_PZ2.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class ProgMod_PZ4Entities : DbContext
    {
        public ProgMod_PZ4Entities()
            : base("name=ProgMod_PZ4Entities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Act> Act { get; set; }
        public virtual DbSet<Agreement> Agreement { get; set; }
        public virtual DbSet<Client> Client { get; set; }
        public virtual DbSet<Employee> Employee { get; set; }
        public virtual DbSet<EmployeePost> EmployeePost { get; set; }
        public virtual DbSet<Gender> Gender { get; set; }
        public virtual DbSet<Object> Object { get; set; }
        public virtual DbSet<ObjectInspection> ObjectInspection { get; set; }
        public virtual DbSet<PoliceLeadership> PoliceLeadership { get; set; }
        public virtual DbSet<Report> Report { get; set; }
        public virtual DbSet<ResponsiblePerson> ResponsiblePerson { get; set; }
        public virtual DbSet<ResponsiblePersonType> ResponsiblePersonType { get; set; }
        public virtual DbSet<sysdiagrams> sysdiagrams { get; set; }
        public virtual DbSet<TriggeringAlarm> TriggeringAlarm { get; set; }
        public virtual DbSet<TriggeringAlarmType> TriggeringAlarmType { get; set; }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<UserRole> UserRole { get; set; }
    }
}
