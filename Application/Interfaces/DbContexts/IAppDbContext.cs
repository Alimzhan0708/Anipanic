using System.Threading;
using System.Threading.Tasks;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Interfaces.DbContexts
{
    public interface IAppDbContext
    {
        DbSet<Movie> Movies { get; set; }
        DbSet<Episode> Episodes { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}