using Tank.Financing.EnterpriseDetails;
using Tank.Financing.Enterprises;
using Tank.Financing.Applies;
using Tank.Financing.FinancialProducts;
using Volo.Abp.EntityFrameworkCore.Modeling;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.AuditLogging.EntityFrameworkCore;
using Volo.Abp.BackgroundJobs.EntityFrameworkCore;
using Volo.Abp.BlobStoring.Database.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.FeatureManagement.EntityFrameworkCore;
using Volo.Abp.Identity;
using Volo.Abp.Identity.EntityFrameworkCore;
using Volo.Abp.IdentityServer.EntityFrameworkCore;
using Volo.Abp.LanguageManagement.EntityFrameworkCore;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using Volo.Abp.SettingManagement.EntityFrameworkCore;
using Volo.Abp.TextTemplateManagement.EntityFrameworkCore;
using Volo.Saas.EntityFrameworkCore;
using Volo.Saas.Editions;
using Volo.Saas.Tenants;
using Volo.Payment.EntityFrameworkCore;
using Volo.CmsKit.EntityFrameworkCore;

namespace Tank.Financing.EntityFrameworkCore;

[ReplaceDbContext(typeof(IIdentityProDbContext))]
[ReplaceDbContext(typeof(ISaasDbContext))]
[ConnectionStringName("Default")]
public class FinancingDbContext :
    AbpDbContext<FinancingDbContext>,
    IIdentityProDbContext,
    ISaasDbContext
{
    public DbSet<EnterpriseDetail> EnterpriseDetails { get; set; }
    public DbSet<Enterprise> Enterprises { get; set; }
    public DbSet<Apply> Applies { get; set; }
    public DbSet<FinancialProduct> FinancialProducts { get; set; }
    /* Add DbSet properties for your Aggregate Roots / Entities here. */

    #region Entities from the modules

    /* Notice: We only implemented IIdentityProDbContext and ISaasDbContext
     * and replaced them for this DbContext. This allows you to perform JOIN
     * queries for the entities of these modules over the repositories easily. You
     * typically don't need that for other modules. But, if you need, you can
     * implement the DbContext interface of the needed module and use ReplaceDbContext
     * attribute just like IIdentityProDbContext and ISaasDbContext.
     *
     * More info: Replacing a DbContext of a module ensures that the related module
     * uses this DbContext on runtime. Otherwise, it will use its own DbContext class.
     */

    // Identity
    public DbSet<IdentityUser> Users { get; set; }
    public DbSet<IdentityRole> Roles { get; set; }
    public DbSet<IdentityClaimType> ClaimTypes { get; set; }
    public DbSet<OrganizationUnit> OrganizationUnits { get; set; }
    public DbSet<IdentitySecurityLog> SecurityLogs { get; set; }
    public DbSet<IdentityLinkUser> LinkUsers { get; set; }

    // SaaS
    public DbSet<Tenant> Tenants { get; set; }
    public DbSet<Edition> Editions { get; set; }
    public DbSet<TenantConnectionString> TenantConnectionStrings { get; set; }

    #endregion

    public FinancingDbContext(DbContextOptions<FinancingDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        /* Include modules to your migration db context */

        builder.ConfigurePermissionManagement();
        builder.ConfigureSettingManagement();
        builder.ConfigureBackgroundJobs();
        builder.ConfigureAuditLogging();
        builder.ConfigureIdentityPro();
        builder.ConfigureIdentityServer();
        builder.ConfigureFeatureManagement();
        builder.ConfigureLanguageManagement();
        builder.ConfigurePayment();
        builder.ConfigureSaas();
        builder.ConfigureTextTemplateManagement();
        builder.ConfigureBlobStoring();
        builder.ConfigureCmsKit();
        builder.ConfigureCmsKitPro();

        /* Configure your own tables/entities inside here */

        //builder.Entity<YourEntity>(b =>
        //{
        //    b.ToTable(FinancingConsts.DbTablePrefix + "YourEntities", FinancingConsts.DbSchema);
        //    b.ConfigureByConvention(); //auto configure for the base class props
        //    //...
        //});
        if (builder.IsHostDatabase())
        {

        }
        if (builder.IsHostDatabase())
        {

        }
        if (builder.IsHostDatabase())
        {

        }
        if (builder.IsHostDatabase())
        {

        }
        if (builder.IsHostDatabase())
        {

        }
        if (builder.IsHostDatabase())
        {

        }
        if (builder.IsHostDatabase())
        {

        }
        if (builder.IsHostDatabase())
        {

        }
        if (builder.IsHostDatabase())
        {

        }
        if (builder.IsHostDatabase())
        {

        }
        if (builder.IsHostDatabase())
        {

        }
        if (builder.IsHostDatabase())
        {

        }
        if (builder.IsHostDatabase())
        {

        }
        if (builder.IsHostDatabase())
        {

        }
        if (builder.IsHostDatabase())
        {
            builder.Entity<EnterpriseDetail>(b =>
{
    b.ToTable(FinancingConsts.DbTablePrefix + "EnterpriseDetails", FinancingConsts.DbSchema);
    b.ConfigureByConvention();
    b.Property(x => x.EnterpriseName).HasColumnName(nameof(EnterpriseDetail.EnterpriseName)).IsRequired();
    b.Property(x => x.TotalAssets).HasColumnName(nameof(EnterpriseDetail.TotalAssets)).IsRequired();
    b.Property(x => x.Income).HasColumnName(nameof(EnterpriseDetail.Income)).IsRequired();
    b.Property(x => x.EnterpriseType).HasColumnName(nameof(EnterpriseDetail.EnterpriseType)).IsRequired();
    b.Property(x => x.StaffNumber).HasColumnName(nameof(EnterpriseDetail.StaffNumber)).IsRequired();
    b.Property(x => x.Industry).HasColumnName(nameof(EnterpriseDetail.Industry)).IsRequired();
    b.Property(x => x.Location).HasColumnName(nameof(EnterpriseDetail.Location)).IsRequired();
    b.Property(x => x.RegisteredAddress).HasColumnName(nameof(EnterpriseDetail.RegisteredAddress)).IsRequired();
    b.Property(x => x.BusinessAddress).HasColumnName(nameof(EnterpriseDetail.BusinessAddress)).IsRequired();
    b.Property(x => x.BusinessScope).HasColumnName(nameof(EnterpriseDetail.BusinessScope)).IsRequired();
    b.Property(x => x.Description).HasColumnName(nameof(EnterpriseDetail.Description)).IsRequired();
});

        }
        if (builder.IsHostDatabase())
        {

        }
        if (builder.IsHostDatabase())
        {

        }
        if (builder.IsHostDatabase())
        {

        }
        if (builder.IsHostDatabase())
        {

        }
        if (builder.IsHostDatabase())
        {
            builder.Entity<FinancialProduct>(b =>
{
    b.ToTable(FinancingConsts.DbTablePrefix + "FinancialProducts", FinancingConsts.DbSchema);
    b.ConfigureByConvention();
    b.Property(x => x.Period).HasColumnName(nameof(FinancialProduct.Period)).IsRequired().HasMaxLength(FinancialProductConsts.PeriodMaxLength);
    b.Property(x => x.GuaranteeMethod).HasColumnName(nameof(FinancialProduct.GuaranteeMethod)).IsRequired();
    b.Property(x => x.CreditCeiling).HasColumnName(nameof(FinancialProduct.CreditCeiling)).IsRequired();
    b.Property(x => x.Organization).HasColumnName(nameof(FinancialProduct.Organization)).IsRequired().HasMaxLength(FinancialProductConsts.OrganizationMaxLength);
    b.Property(x => x.AppliedNumber).HasColumnName(nameof(FinancialProduct.AppliedNumber));
    b.Property(x => x.APR).HasColumnName(nameof(FinancialProduct.APR)).IsRequired();
    b.Property(x => x.Rating).HasColumnName(nameof(FinancialProduct.Rating));
    b.Property(x => x.Name).HasColumnName(nameof(FinancialProduct.Name)).IsRequired();
});

        }
        if (builder.IsHostDatabase())
        {
            builder.Entity<Enterprise>(b =>
{
    b.ToTable(FinancingConsts.DbTablePrefix + "Enterprises", FinancingConsts.DbSchema);
    b.ConfigureByConvention();
    b.Property(x => x.EnterpriseName).HasColumnName(nameof(Enterprise.EnterpriseName)).IsRequired();
    b.Property(x => x.ArtificialPerson).HasColumnName(nameof(Enterprise.ArtificialPerson)).IsRequired();
    b.Property(x => x.EstablishedTime).HasColumnName(nameof(Enterprise.EstablishedTime)).IsRequired();
    b.Property(x => x.DueTime).HasColumnName(nameof(Enterprise.DueTime));
    b.Property(x => x.CreditCode).HasColumnName(nameof(Enterprise.CreditCode)).IsRequired();
    b.Property(x => x.ArtificialPersonId).HasColumnName(nameof(Enterprise.ArtificialPersonId)).IsRequired();
    b.Property(x => x.RegisteredCapital).HasColumnName(nameof(Enterprise.RegisteredCapital)).IsRequired();
    b.Property(x => x.PhoneNumber).HasColumnName(nameof(Enterprise.PhoneNumber)).IsRequired();
    b.Property(x => x.CertPhotoPath).HasColumnName(nameof(Enterprise.CertPhotoPath)).IsRequired();
    b.Property(x => x.IdPhotoPath1).HasColumnName(nameof(Enterprise.IdPhotoPath1)).IsRequired();
    b.Property(x => x.IdPhotoPath2).HasColumnName(nameof(Enterprise.IdPhotoPath2)).IsRequired();
    b.Property(x => x.CertificateStatus).HasColumnName(nameof(Enterprise.CertificateStatus)).IsRequired();
});

        }
        if (builder.IsHostDatabase())
        {
            builder.Entity<Apply>(b =>
{
    b.ToTable(FinancingConsts.DbTablePrefix + "Applies", FinancingConsts.DbSchema);
    b.ConfigureByConvention();
    b.Property(x => x.EnterpriseName).HasColumnName(nameof(Apply.EnterpriseName)).IsRequired();
    b.Property(x => x.Organization).HasColumnName(nameof(Apply.Organization)).IsRequired();
    b.Property(x => x.ProductName).HasColumnName(nameof(Apply.ProductName)).IsRequired();
    b.Property(x => x.Allowance).HasColumnName(nameof(Apply.Allowance));
    b.Property(x => x.APR).HasColumnName(nameof(Apply.APR));
    b.Property(x => x.Period).HasColumnName(nameof(Apply.Period));
    b.Property(x => x.ApplyStatus).HasColumnName(nameof(Apply.ApplyStatus)).IsRequired();
    b.Property(x => x.GuaranteeMethod).HasColumnName(nameof(Apply.GuaranteeMethod));
    b.Property(x => x.ApplyTime).HasColumnName(nameof(Apply.ApplyTime)).IsRequired();
    b.Property(x => x.PassedTime).HasColumnName(nameof(Apply.PassedTime));
});

        }
    }
}