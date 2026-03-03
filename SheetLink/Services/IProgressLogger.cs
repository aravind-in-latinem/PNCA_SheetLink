using System;

namespace PNCA_SheetLink.SheetLink.Services
{
    public interface IProgressLogger
    {
        void LogException(Exception ex, string task);
        void LogTaskCompleted(string task);
    }
}
