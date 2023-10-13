using AzureFunctions.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureFunctions.Contexts
{
    public class CosmosContext : DbContext
    {
        public CosmosContext(DbContextOptions<CosmosContext> options) : base(options)
        {
        }

        public CosmosContext()
        {
        }

        public DbSet<DeviceItemMessage> Messages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DeviceItemMessage>(entity =>
            {
                entity.HasKey(k => k.DeviceId);
                entity.ToContainer("Messages");
                entity.HasPartitionKey(pk => pk.PartitionKey);
            });
        }
    }
}
