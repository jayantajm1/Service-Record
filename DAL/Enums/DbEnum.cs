// File: DAL/Enums/DbEnums.cs

namespace Service_Record.DAL.Enums
{
    public enum UserRole
    {
        Admin,
        OfficeStaff,
        FieldStaff
    }

    public enum LogAction
    {
        Login,
        Logout
    }
    public enum PartAction
    {
        Added,
        Removed,
        Replaced
    }
    public enum QuotationStatus
    {
        Draft,
        Sent,
        Approved,
        Rejected,
        Cancelled
    }
    public enum ServiceStatus
    {
        Pending,
        InProgress,
        Completed,
        Cancelled
    }

    public enum APIResponseStatus
    {
        Success = 1,
        Warning = 2,
        Error = 3
    }
    public enum OTPType
    {
        ForgotPassword = 1,
        Login = 2,
    }
}