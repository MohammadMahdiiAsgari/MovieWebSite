using Microsoft.EntityFrameworkCore;
using Movie.Domain.Models;
using Movie.Domain.Models.Movies;
using Movie.Domain.Models.TVShows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movie.Data.Context
{
    public class MyMovieContext : DbContext
    {
        public MyMovieContext(DbContextOptions<MyMovieContext> options) : base(options) { }


        #region UserTable

        public DbSet<User> Users { get; set; }

        #endregion

        #region Movie Tabels

        public DbSet<MovieGroup> MovieGroups { get; set; }
        public DbSet<MovieSubGroup> MovieSubGroups { get; set; }

        public DbSet<Movies> Movies { get; set; }

        public DbSet<MovieGallery> movieGalleries { get; set; }

        #endregion


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }

            // پیکربندی رابطه Many-to-Many بین Show و TVShowsSubGroup با استفاده از مدل واسط
            modelBuilder.Entity<ShowListTVShowsSubGroup>()
                .HasKey(ssts => new { ssts.ShowId, ssts.TVShowsSubGroupId }); // تعریف کلید اصلی ترکیبی

            modelBuilder.Entity<ShowListTVShowsSubGroup>()
                .HasOne(ssts => ssts.ShowLists)
                .WithMany(s => s.ShowListTVShowsSubGroups) // نام خصوصیت Navigation در مدل Show
                .HasForeignKey(ssts => ssts.ShowId); // کلید خارجی در مدل واسط به Show

            modelBuilder.Entity<ShowListTVShowsSubGroup>()
                .HasOne(ssts => ssts.TVShowsSubGroup)
                .WithMany(ts => ts.ShowListTVShowsSubGroups) // نام خصوصیت Navigation در مدل TVShowsSubGroup
                .HasForeignKey(ssts => ssts.TVShowsSubGroupId); // کلید خارجی در مدل واسط به TVShowsSubGroup

            // ... پیکربندی سایر روابط (Show -> Season, Season -> Episode) اگر لازم است ...
            // EF Core معمولا روابط یک به چند را به صورت قراردادی تشخیص می دهد.

            // اطمینان از اعمال تنظیمات پایه
            base.OnModelCreating(modelBuilder);
        }




        #region TV Shows

        public DbSet<TVShowsGroup> TVShowsGroups { get; set; }

        public DbSet<TVShowsSubGroup> TVShowsSubGroups { get; set; }

        public DbSet<ShowLists> ShowLists { get; set; }

        public DbSet<Seasons> Seasons { get; set; }

        public DbSet<Episodes> Episodes { get; set; }

        public DbSet<ShowListTVShowsSubGroup> ShowListTVShowsSubGroups { get; set; }


        #endregion
    }
}
