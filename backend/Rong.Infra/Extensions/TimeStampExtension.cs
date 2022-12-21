using Rong.Infra.Helper;

namespace Rong.Infra.Extensions;

public static class TimeStampExtension
{
    public static string ToLocalTimeString(this long timeStamp)
    {
        var time = TimeHelper.TimeStampToDateTime(timeStamp);
        return time.ToString("yyyy-MM-dd HH:mm:ss");
    }
}