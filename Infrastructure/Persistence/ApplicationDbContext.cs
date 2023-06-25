using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Interfaces;
using Domain.Constants;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Infrastructure.Persistence;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>, IApplicationDbContext
{
    private readonly IIdentityService _identityService;

    public ApplicationDbContext(DbContextOptions options, IIdentityService identityService)
        : base(options)
    {
        _identityService = identityService;
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        #region Tables

        builder.Entity<ApplicationUser>().ToTable("ApplicationUsers");
        builder.Entity<ApplicationRole>().ToTable("ApplicationRoles");
        builder.Entity<IdentityUserClaim<Guid>>().ToTable("ApplicationUserClaims");
        builder.Entity<IdentityUserRole<Guid>>().ToTable("ApplicationUserRoles")
            .HasKey(x => new { x.UserId, x.RoleId });
        builder.Entity<IdentityUserLogin<Guid>>().ToTable("ApplicationUserLogins").HasKey(x => x.UserId);
        builder.Entity<IdentityRoleClaim<Guid>>().ToTable("ApplicationRoleClaims");
        builder.Entity<IdentityUserToken<Guid>>().ToTable("ApplicationUserTokens").HasKey(x => x.UserId);

        builder.Entity<Category>().ToTable("Categories");
        builder.Entity<Order>().ToTable("Orders");
        builder.Entity<OrderItem>().ToTable("OrderItems");
        builder.Entity<Payment>().ToTable("Payments");
        builder.Entity<Product>().ToTable("Products");
        builder.Entity<Table>().ToTable("Tables");
        builder.Entity<TableOrder>().ToTable("TableOrders");
        builder.Entity<Unit>().ToTable("Units");
        builder.Entity<TableOrderRequest>().ToTable("TableOrderRequests");
        builder.Entity<TableBooking>().ToTable("TableBookings");
        
        foreach (var relationship in builder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
        {
            relationship.DeleteBehavior = DeleteBehavior.Restrict;
        }

        #endregion

       
    }


    public DbSet<Category> Categories { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<Payment> Payments { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Table> Tables { get; set; }
    public DbSet<TableOrder> TableOrders { get; set; }
    public DbSet<Unit> Units { get; set; }
    public DbSet<TableOrderRequest> TableOrderRequests { get; set; }
    public DbSet<TableBooking> TableBookings { get; set; }
    public override EntityEntry Entry(object entity) => base.Entry(entity);

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        var userChange = string.IsNullOrEmpty(_identityService.GetCurrentUserLogin().Id)
            ? AppConst.Author
            : _identityService.GetCurrentUserLogin().Id;
        var dateTimeChange = DateTime.Now;

        foreach (var entry in ChangeTracker.Entries<Audit>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedBy = userChange;
                    entry.Entity.CreatedAt = dateTimeChange;
                    break;
                case EntityState.Modified:
                    entry.Entity.UpdatedBy = userChange;
                    entry.Entity.UpdatedAt = dateTimeChange;
                    break;
                case EntityState.Detached:
                    break;
                case EntityState.Unchanged:
                    break;
                case EntityState.Deleted:
                    entry.Entity.UpdatedBy = userChange;
                    entry.Entity.UpdatedAt = dateTimeChange;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        var result = await base.SaveChangesAsync(cancellationToken);

        return result;
    }
}