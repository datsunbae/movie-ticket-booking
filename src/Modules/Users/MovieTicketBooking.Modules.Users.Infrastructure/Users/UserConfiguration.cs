using MovieTicketBooking.Modules.Users.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MovieTicketBooking.Modules.Users.Infrastructure.Users;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(user => user.Id);
        
        builder.Property(user => user.Email).HasMaxLength(300);

        builder.Property(user => user.FirstName).HasMaxLength(200);
        
        builder.Property(user => user.LastName).HasMaxLength(200);
        
        builder.HasIndex(user => user.Email).IsUnique();
        
        builder.HasIndex(user => user.IdentityId).IsUnique();
    }
}