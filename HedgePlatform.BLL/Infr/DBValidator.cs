using Microsoft.EntityFrameworkCore;
using Npgsql;
using System;

namespace HedgePlatform.BLL.Infr
{
    public static class DBValidator
    {
        private static IDBValidation _validation;
        private static DbUpdateException _ex;

        private static string ExMessage;
        private static string ExProperty; 
        
        public static void SetException (DbUpdateException ex)
        {
            _ex = ex;
            SetValidation();
            SetErrProperty();
            SetErrMessage();
        }
        public static string GetErrProperty() => ExProperty;

        public static string GetErrMessage() => ExMessage;

        private static void SetErrProperty()
        {
            SetValidation();
            ExProperty = _validation.GetProperty(_ex);
        }
        private static void SetErrMessage()
        {
            SetValidation();
            ExMessage = _validation.GetMessage(_ex);
        }

        private static void SetValidation ()
        {
            if (_ex == null) throw new NullReferenceException("Exception type is not set");
            switch (_ex.InnerException)
            {
                case PostgresException _:
                    _validation = new PgValidation();
                    break;
                default:
                    throw new ValidationException("SERVER_DB_ERROR","");
            }
        }
    }
}
