using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace HedgePlatform.BLL.Infr
{
    public class PgValidation : IDBValidation
    {
       
        public string GetProperty(DbUpdateException ex)
        {
            PostgresException inner_ex = ex.InnerException as PostgresException;
            return inner_ex.ColumnName;
        }
        public string GetMessage(DbUpdateException ex)
        {
            PostgresException inner_ex = ex.InnerException as PostgresException;
            return inner_ex.Message;
        }
    }
}
