using HedgePlatform.BLL.Infr;
using HedgePlatform.BLL.Interfaces;
using HedgePlatform.DAL.Interfaces;
using Microsoft.Extensions.Logging;

namespace HedgePlatform.BLL.Services
{
    public class CheckDBConnectionService : ICheckDBConnectionService
    {
        private readonly ILogger _logger = Log.CreateLogger<PDFService>();
        private IUnitOfWork _db { get; set; }
        public CheckDBConnectionService(IUnitOfWork uow)
        {
            _db = uow;
        }

        public void CheckDBConnection()
        {
            if (!_db.Users.CanConnect())
            {
                _logger.LogError("NO_DB_CONNECTION");
                throw new ValidationException("NO_DB_CONNECTION", "");                
            }
        }
    }
}
