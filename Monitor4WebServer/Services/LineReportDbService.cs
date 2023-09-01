using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Monitor4WebServer.Data;

namespace Monitor4WebServer.Services
{
    public class LineReportDbService
    {
        /*
        // private readonly DbContextOptions<LineReportContext> _dbContextOptions;
        //DbContextOptions<LineReportContext> dbContextOptions

        private readonly LineReportContext context;

        public LineReportDbService(LineReportContext _context)
        {
            context = _context;

        }

        //get data
        public async Task<List<LineReport>> GetAllLineReports()
        {

                if (!EnsureDatabaseCreated(context))
                {
                    return await context.LineReports.ToListAsync();
                }

                else
                {
                    return new List<LineReport>();
                }

            
        }

        public bool EnsureDatabaseCreated(LineReportContext context)
        {
            bool databaseExists = context.Database.EnsureCreated();

            if (databaseExists)
            {
                // La base de datos se creó correctamente
                // Realiza alguna acción adicional si es necesario
            }
            else
            {
                // La base de datos ya existe
                // Realiza alguna acción adicional si es necesario
            }

            return databaseExists;
        }


        //update data
        public async Task UpdateLineReport(LineReport lineReport)
        {

                if (!EnsureDatabaseCreated(context))
                {
                    var existingLineReport = await context.LineReports.FindAsync(lineReport.Id);

                    if (existingLineReport != null)
                    {

                        context.Update(lineReport);
                        
                        // Guardar los cambios en la base de datos
                        await context.SaveChangesAsync();
                    
                    }
                }
            

        }


        //add data
        public async Task AddLineReport(LineReport lineReport)
        {
        
                if (!EnsureDatabaseCreated(context))
                {
                    context.LineReports.Add(lineReport);
                    await SaveChangesAsync(context);
                }
            
        }


        //save data
        public async Task SaveChangesAsync(LineReportContext context)
        {

            await context.SaveChangesAsync();

        }
        */
    }
}
