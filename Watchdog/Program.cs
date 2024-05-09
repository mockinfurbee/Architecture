using NLog;

namespace Watchdog
{
    internal class Program
    {
        private readonly static Logger _logger = LogManager.GetCurrentClassLogger();

        static async Task Main(string[] args)
        {
            LogManager.Setup();
            HttpClient client = new HttpClient();
            

            while (true)
            {
                using var response = await client.GetAsync("https://localhost:port/hc");
                var status = await response.Content.ReadAsStringAsync();

                if (status.Contains("Unhealthy"))
                {
                    _logger.Fatal(status);
                }
                else
                {
                    _logger.Info("100% OK.");
                }

                await Task.Delay(600000);
            }
        }
    }
}