using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using TopEndLibraryHub.Data;
using Microsoft.EntityFrameworkCore.Metadata;

namespace TopEndLibraryHub.Tests;

public class MigrationModelTests
{
    [Fact]
    public void SqlServerMigrationChain_HasOneCreationPerSharedTable()
    {
        using var db = new ApplicationDbContext(new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseSqlServer("Server=unused-for-script-generation;Database=Unused;Integrated Security=true;TrustServerCertificate=true").Options);
        var migrations = db.Database.GetMigrations().ToArray();
        Assert.Equal(7, migrations.Length);
        var snapshot = db.GetService<IMigrationsAssembly>().ModelSnapshot!.Model;
        snapshot = db.GetService<IModelRuntimeInitializer>().Initialize(snapshot, designTime: true);
        var changes = db.GetService<IMigrationsModelDiffer>().GetDifferences(
            snapshot.GetRelationalModel(), db.GetService<IDesignTimeModel>().Model.GetRelationalModel());
        var details = string.Join("\n", changes.Select(change => change.GetType().Name + ": " +
            string.Join(", ", change.GetType().GetProperties().Where(property =>
                new[] { "Name", "Table", "ColumnType", "IsNullable", "IsUnique", "Filter", "MaxLength" }.Contains(property.Name))
                .Select(property => property.Name + "=" + property.GetValue(change)))));
        Assert.True(changes.Count == 0, details);
        Assert.False(db.Database.HasPendingModelChanges());
        var script = db.GetService<IMigrator>().GenerateScript();
        foreach (var name in new[] { "Holds", "NotificationLogs", "Branches", "ReceptionDesks", "ItemTransfers" })
            Assert.Single(Regex.Matches(script, @"CREATE TABLE \[" + name + @"\]").Cast<Match>());
    }
}
