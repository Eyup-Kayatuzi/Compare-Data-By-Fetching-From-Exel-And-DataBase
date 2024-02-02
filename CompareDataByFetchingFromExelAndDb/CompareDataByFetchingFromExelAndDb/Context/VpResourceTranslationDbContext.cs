using CompareDataByFetchingFromExelAndDb.Entity;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Text.Json;


namespace CompareDataByFetchingFromExelAndDb.Context
{

	class VpResourceTranslationDbContext : DbContext
	{
		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			string _connectionString = (JsonSerializer.Deserialize<ConnectionSettings>(File.ReadAllText("../../../appSettings.json"))).connectionString;
			if (!optionsBuilder.IsConfigured)
			{
				optionsBuilder.UseSqlServer(_connectionString);
			}
		}
		public DbSet<VpResourceTranslationEntitiy> VpResourceTranslation { get; set; }
	}
}
