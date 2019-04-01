using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Mes.Core;
using Mes.Core.Domain;
using Mes.Infrastructure.Mappings;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Mes.Infrastructure
{
    public class MesDbContext : IdentityDbContext, IDbContext
    {
        public MesDbContext(DbContextOptions<MesDbContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ApplicationUserMap());
            modelBuilder.ApplyConfiguration(new ApplicationRoleMap());
            ApplyEntityTypeConfigurations(modelBuilder, GetType().Assembly);
            base.OnModelCreating(modelBuilder);
        }
        public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            using (var transaction = await Database.BeginTransactionAsync())
            {
                try
                {
                    await SaveChangesAsync();
                    transaction.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }
        }

        #region private
        private static void ApplyEntityTypeConfigurations(ModelBuilder modelBuilder, Assembly assembly)
        {
            var mappingTypes = EntityTypes(assembly);
            foreach (var type in mappingTypes)
            {
                dynamic configurationInstance = Activator.CreateInstance(type);
                modelBuilder.ApplyConfiguration(configurationInstance);
            }
        }
        private static IEnumerable<Type> EntityTypes(Assembly assembly)
        {
            var typeToApply = assembly.GetTypes().Where(t => t.BaseType != null
            && t.BaseType.IsAbstract
            && t.BaseType.IsGenericType
            && t.BaseType.GetGenericTypeDefinition() == typeof(BaseEntityTypeConfiguration<>));
            return typeToApply;
        }
        #endregion
    }
}
