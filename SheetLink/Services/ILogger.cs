using System;

namespace PNCA_SheetLink.SheetLink.Services
{
    public interface ILogger
    {
        void LogException(Exception ex, string task);
        void LogTaskCompleted(string task);
    }
}
