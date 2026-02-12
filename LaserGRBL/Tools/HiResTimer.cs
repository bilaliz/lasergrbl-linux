using System;
using System.Diagnostics;

namespace Tools
{
    public static class HiResTimer
    {
        private static Stopwatch _stopwatch;
        private static long _startTicks;
        private static double _nanoPerTick;

        static HiResTimer()
        {
            _stopwatch = new Stopwatch();
            _nanoPerTick = 1_000_000_000.0 / Stopwatch.Frequency;
            _stopwatch.Start();
            _startTicks = _stopwatch.ElapsedTicks;
        }

        public static long TotalNano
        {
            get
            {
                long elapsed = _stopwatch.ElapsedTicks - _startTicks;
                return (long)(elapsed * _nanoPerTick);
            }
        }

        public static long TotalMilliseconds => _stopwatch.ElapsedMilliseconds;

        public static long CurrentFrequency => Stopwatch.Frequency;
        public static long OriginalFrequency => Stopwatch.Frequency;

        // Compatibility for TestMultiplier (unused in production but kept for API surface)
        public static double TestMultiplier { get; set; } = 1.0;
    }

    public class Utils
    {
        public static string TimeSpanToString(TimeSpan Span, TimePrecision Precision, TimePrecision MaxFallBackPrecision, string Separator, bool WriteSuffix)
        {
            if (Span >= TimeSpan.MaxValue) return "+∞";
            if (Span <= TimeSpan.MinValue) return "-∞";
            
            DateTime N = DateTime.Now;
            return HumanReadableDateDiff(N, N.Add(Span), Precision, MaxFallBackPrecision, Separator, WriteSuffix);
        }

        public static string HumanReadableDateDiff(DateTime MainDate, DateTime OtherDate, TimePrecision Precision, TimePrecision MaxFallBackPrecision, string Separator, bool WriteSuffix)
        {
            string functionReturnValue = "";
            int years = 0; int months = 0; int days = 0;
            int hours = 0; int minutes = 0; int seconds = 0; int milliseconds = 0;

            string S_year = "year"; string S_month = "month"; string S_day = "day";
            string S_hour = "hour"; string S_minute = "min"; string S_second = "sec";
            string S_millisecond = "ms";
            string S_years = "years"; string S_months = "months"; string S_days = "days";
            string S_hours = "hours"; string S_minutes = "min"; string S_seconds = "sec";
            string S_milliseconds = "ms";
            string S_now = "now"; string S_ago = "ago";

            DateTime BiggerDate = MainDate > OtherDate ? MainDate : OtherDate;
            DateTime SmallestDate = MainDate > OtherDate ? OtherDate : MainDate;

            while (BiggerDate.AddYears(-1) >= SmallestDate) { years++; BiggerDate = BiggerDate.AddYears(-1); }
            while (BiggerDate.AddMonths(-1) >= SmallestDate) { months++; BiggerDate = BiggerDate.AddMonths(-1); }
            while (BiggerDate.AddDays(-1) >= SmallestDate) { days++; BiggerDate = BiggerDate.AddDays(-1); }

            TimeSpan diff = BiggerDate - SmallestDate;
            hours = diff.Hours; minutes = diff.Minutes; seconds = diff.Seconds; milliseconds = diff.Milliseconds;

            // Precision fallback logic
            if (Precision == TimePrecision.Years && MaxFallBackPrecision > TimePrecision.Years && years == 0) Precision = TimePrecision.Month;
            if (Precision == TimePrecision.Month && MaxFallBackPrecision > TimePrecision.Month && years == 0 && months == 0) Precision = TimePrecision.Day;
            if (Precision == TimePrecision.Day && MaxFallBackPrecision > TimePrecision.Day && years == 0 && months == 0 && days == 0) Precision = TimePrecision.Hour;
            if (Precision == TimePrecision.Hour && MaxFallBackPrecision > TimePrecision.Hour && years == 0 && months == 0 && days == 0 && hours == 0) Precision = TimePrecision.Minute;
            if (Precision == TimePrecision.Minute && MaxFallBackPrecision > TimePrecision.Minute && years == 0 && months == 0 && days == 0 && hours == 0 && minutes == 0) Precision = TimePrecision.Second;
            if (Precision == TimePrecision.Second && MaxFallBackPrecision > TimePrecision.Second && years == 0 && months == 0 && days == 0 && hours == 0 && minutes == 0 && seconds == 0) Precision = TimePrecision.Millisecond;

            if (years > 0) functionReturnValue += string.Format("{0} {1}|", years, (years == 1 ? S_year : S_years));
            if (Precision > TimePrecision.Years && months > 0) functionReturnValue += string.Format("{0} {1}|", months, (months == 1 ? S_month : S_months));
            if (Precision > TimePrecision.Month && days > 0) functionReturnValue += string.Format("{0} {1}|", days, (days == 1 ? S_day : S_days));
            if (Precision > TimePrecision.Day && hours > 0) functionReturnValue += string.Format("{0} {1}|", hours, (hours == 1 ? S_hour : S_hours));
            if (Precision > TimePrecision.Hour && minutes > 0) functionReturnValue += string.Format("{0} {1}|", minutes, (minutes == 1 ? S_minute : S_minutes));
            if (Precision > TimePrecision.Minute && seconds > 0) functionReturnValue += string.Format("{0} {1}|", seconds, (seconds == 1 ? S_second : S_seconds));
            if (Precision > TimePrecision.Second && milliseconds > 0) functionReturnValue += string.Format("{0} {1}|", milliseconds, (milliseconds == 1 ? S_millisecond : S_milliseconds));

            if (string.IsNullOrEmpty(functionReturnValue))
            {
                if (WriteSuffix) functionReturnValue = S_now;
                else
                {
                    switch(Precision) {
                        case TimePrecision.Years: functionReturnValue = $"0 {S_years}"; break;
                        case TimePrecision.Month: functionReturnValue = $"0 {S_months}"; break;
                        case TimePrecision.Day: functionReturnValue = $"0 {S_days}"; break;
                        case TimePrecision.Hour: functionReturnValue = $"0 {S_hours}"; break;
                        case TimePrecision.Minute: functionReturnValue = $"0 {S_minutes}"; break;
                        case TimePrecision.Second: functionReturnValue = $"0 {S_seconds}"; break;
                        case TimePrecision.Millisecond: functionReturnValue = $"0 {S_milliseconds}"; break;
                    }
                }
            }
            else
            {
                functionReturnValue = functionReturnValue.Trim('|').Replace("|", Separator);
                if (WriteSuffix && MainDate > OtherDate) functionReturnValue += " " + S_ago;
            }

            return functionReturnValue;
        }

        internal static void OpenLink(string url)
        {
            string[] list = url.Split('|');
            foreach (string s in list)
            {
                if (s.ToLower().StartsWith("http://") || s.ToLower().StartsWith("https://"))
                {
                    // Use xdg-open for Linux compatibility if Process.Start fails, but usually Process.Start works on Mono
                    try { Process.Start(string.Format(s, LaserGRBL.UsageStats.GetID())); }
                    catch { /* Ignore */ }
                }
            }
        }

        public enum TimePrecision
        {
            Years = 0, Month = 1, Day = 2, Hour = 3, Minute = 4, Second = 5, Millisecond = 6
        }
    }
}