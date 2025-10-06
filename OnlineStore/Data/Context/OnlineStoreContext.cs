using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Extensions;

namespace OnlineStore.Data.Context;

public class OnlineStoreContext : DbContext
{
    
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        string connectionString = "Server=127.0.0.1;Port=3306;User ID=root;Password=my-secret-pw;Database=online_store";
        var serverVersion = ServerVersion.AutoDetect(connectionString);
        
        optionsBuilder.UseMySql(connectionString, serverVersion);
        
    }
    
}