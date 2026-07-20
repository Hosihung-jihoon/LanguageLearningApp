using System;
using System.Collections.Concurrent;

namespace LanguageLearningApp.Services
{
    public class GeminiRateLimitException : Exception
    {
        public GeminiRateLimitException(string message) : base(message) { }
    }

    public static class GeminiRateLimiter
    {
        private static readonly ConcurrentQueue<DateTime> MinuteRequests = new();
        private static readonly ConcurrentQueue<DateTime> DailyRequests = new();

        public const int MaxRpm = 15;
        public const int MaxRpd = 1500;

        public static int CurrentRpm
        {
            get
            {
                CleanOldRequests();
                return MinuteRequests.Count;
            }
        }

        public static int CurrentRpd
        {
            get
            {
                CleanOldRequests();
                return DailyRequests.Count;
            }
        }

        public static bool CheckAndIncrement(out string? errorMessage)
        {
            CleanOldRequests();
            errorMessage = null;

            if (MinuteRequests.Count >= MaxRpm)
            {
                errorMessage = "Bạn đã vượt quá giới hạn gọi API Gemini (Tối đa 15 yêu cầu/phút). Vui lòng chờ vài giây và thử lại.";
                return false;
            }

            if (DailyRequests.Count >= MaxRpd)
            {
                errorMessage = "Bạn đã vượt quá giới hạn gọi API Gemini hôm nay (Tối đa 1500 yêu cầu/ngày). Vui lòng quay lại sau 14:00 ngày mai.";
                return false;
            }

            var now = DateTime.UtcNow;
            MinuteRequests.Enqueue(now);
            DailyRequests.Enqueue(now);
            return true;
        }

        private static void CleanOldRequests()
        {
            var now = DateTime.UtcNow;
            
            // Dọn các request cũ hơn 1 phút
            var oneMinuteAgo = now.AddMinutes(-1);
            while (MinuteRequests.TryPeek(out var time) && time < oneMinuteAgo)
            {
                MinuteRequests.TryDequeue(out _);
            }

            // Dọn các request trước mốc 14:00 GMT+7 (07:00 UTC) hôm nay
            var lastReset = now.Date.AddHours(7);
            if (now.Hour < 7)
            {
                lastReset = lastReset.AddDays(-1);
            }

            while (DailyRequests.TryPeek(out var time) && time < lastReset)
            {
                DailyRequests.TryDequeue(out _);
            }
        }
    }
}
