using Microsoft.EntityFrameworkCore;

namespace MusicRater.Models
{
    public class MusicRaterDbContext : DbContext
    {
        public MusicRaterDbContext(DbContextOptions<MusicRaterDbContext> options) : base(options)
        {

        }
        public DbSet<Artist> Artists { get; set; }
        public DbSet<Release> Releases { get; set; }    
    }
}
