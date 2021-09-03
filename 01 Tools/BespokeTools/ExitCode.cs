namespace Protime.Bespoke.Tools
{
    public enum ExitCode
    {
        Success = 0,
        SuccessWithWarning = 1,
        InvalidParams = 10,
        ConnectionFailure = 20,
        UnknownError = 90
    }
}