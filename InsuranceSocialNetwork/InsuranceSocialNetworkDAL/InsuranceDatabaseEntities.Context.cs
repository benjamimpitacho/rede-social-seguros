﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace InsuranceSocialNetworkDAL
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class InsurancePortal_dbEntities : DbContext
    {
        public InsurancePortal_dbEntities()
            : base("name=InsurancePortal_dbEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<AspNetRoles> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUserLogins> AspNetUserLogins { get; set; }
        public virtual DbSet<NotificationType> NotificationType { get; set; }
        public virtual DbSet<PostComment> PostComment { get; set; }
        public virtual DbSet<PostLike> PostLike { get; set; }
        public virtual DbSet<PostSubject> PostSubject { get; set; }
        public virtual DbSet<PostType> PostType { get; set; }
        public virtual DbSet<AspNetUsers> AspNetUsers { get; set; }
        public virtual DbSet<PostCommentLike> PostCommentLike { get; set; }
        public virtual DbSet<AspNetRolesFunctionalities> AspNetRolesFunctionalities { get; set; }
        public virtual DbSet<FriendStatus> FriendStatus { get; set; }
        public virtual DbSet<Friend> Friend { get; set; }
        public virtual DbSet<Chat> Chat { get; set; }
        public virtual DbSet<ChatMember> ChatMember { get; set; }
        public virtual DbSet<BannerType> BannerType { get; set; }
        public virtual DbSet<County> County { get; set; }
        public virtual DbSet<District> District { get; set; }
        public virtual DbSet<Parish> Parish { get; set; }
        public virtual DbSet<ConstructionCompanyFavorite> ConstructionCompanyFavorite { get; set; }
        public virtual DbSet<GarageFavorite> GarageFavorite { get; set; }
        public virtual DbSet<HomeApplianceRepairFavorite> HomeApplianceRepairFavorite { get; set; }
        public virtual DbSet<InsuranceCompanyContactFavorite> InsuranceCompanyContactFavorite { get; set; }
        public virtual DbSet<MedicalClinicFavorite> MedicalClinicFavorite { get; set; }
        public virtual DbSet<Banner> Banner { get; set; }
        public virtual DbSet<CompanyType> CompanyType { get; set; }
        public virtual DbSet<CompanyService> CompanyService { get; set; }
        public virtual DbSet<AuthorizedEmail> AuthorizedEmail { get; set; }
        public virtual DbSet<PostImage> PostImage { get; set; }
        public virtual DbSet<ChatMessage> ChatMessage { get; set; }
        public virtual DbSet<Notification> Notification { get; set; }
        public virtual DbSet<PostHidden> PostHidden { get; set; }
        public virtual DbSet<ProfileSettings> ProfileSettings { get; set; }
        public virtual DbSet<PaymentStatus> PaymentStatus { get; set; }
        public virtual DbSet<PaymentType> PaymentType { get; set; }
        public virtual DbSet<SystemSettings> SystemSettings { get; set; }
        public virtual DbSet<PaymentNotification> PaymentNotification { get; set; }
        public virtual DbSet<Payment> Payment { get; set; }
        public virtual DbSet<SystemLog> SystemLog { get; set; }
        public virtual DbSet<ChatDelete> ChatDelete { get; set; }
        public virtual DbSet<ChatNote> ChatNote { get; set; }
        public virtual DbSet<ChatNotification> ChatNotification { get; set; }
        public virtual DbSet<ConstructionCompany> ConstructionCompany { get; set; }
        public virtual DbSet<Garage> Garage { get; set; }
        public virtual DbSet<HomeApplianceRepair> HomeApplianceRepair { get; set; }
        public virtual DbSet<InsuranceCompanyContact> InsuranceCompanyContact { get; set; }
        public virtual DbSet<MedicalClinic> MedicalClinic { get; set; }
        public virtual DbSet<Profile> Profile { get; set; }
        public virtual DbSet<CompanyWorkingWith> CompanyWorkingWith { get; set; }
        public virtual DbSet<Post> Post { get; set; }
    }
}
