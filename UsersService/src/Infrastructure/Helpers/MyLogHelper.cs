using Application.Interfaces.Services;
using Infrastructure.Services;
using System.Diagnostics;
using System.Text.Json;

namespace Infrastructure.Helpers
{
    public static class MyLogHelper
    {
        private static readonly IDateTimeService _dateTimeService = new DateTimeService();

        public static string StopSwAndGetLogString(string traceId, Stopwatch sw, object request, object response)
        {
            sw.Stop();
            string timeIn24Format = _dateTimeService.GetTime24ByTS(sw.Elapsed);
            string requestAsString = JsonSerializer.Serialize(request);
            string responseAsString = JsonSerializer.Serialize(response);
            string logString = GetStringForLog(traceId, requestAsString, timeIn24Format, responseAsString);
            return logString;
        }
        private static string GetStringForLog(string request, string traceId, string elapsedTime, string response)
        {
            return $"TraceId: {traceId} | Request: {request} | Execution time: {elapsedTime} | Response: {response}";
        }

    }
}
