namespace Rong.Infra.Helper;

public static class TimeHelper
{
    public static long GetTimeStamp()
    {
        return DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
    }
    
    /// <summary>
    /// 时间戳转时间
    /// </summary>
    /// <param name="timeStamp">时间戳</param>
    /// <returns>时间</returns>
    public static DateTime TimeStampToDateTime(long timeStamp)
    {
        return DateTimeOffset.FromUnixTimeMilliseconds(timeStamp).DateTime.ToLocalTime();
    }

    public static DateTime TimeStampToDateTime(long? timeStamp)
    {
        return TimeStampToDateTime(Convert.ToInt64(timeStamp));
    }
}