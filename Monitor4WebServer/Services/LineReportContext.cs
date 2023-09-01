using Microsoft.EntityFrameworkCore;
using Monitor4WebServer.Data;

namespace Monitor4WebServer.Services
{
    public class LineReportContext:DbContext
    {
        public DbSet<LineReport> LineReports { get; set; }

        
        

        public LineReportContext(DbContextOptions<LineReportContext> options) : base(options)
        {

        }
        
        
        
        /*
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(
                @"Data Source=C:\Users\Informatica\Desktop\MONITOR4DBSQLITE\LineReport.db");
        }
        */


    }

}

