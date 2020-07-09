using Microsoft.EntityFrameworkCore;

namespace HedgePlatform.BLL.Infr
{
    public interface IDBValidation
    {
        string GetProperty(DbUpdateException ex);
        string GetMessage(DbUpdateException ex);
    }   
}
