using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoggingSample
{
    public class MyApplication
    {
        private readonly ILogger _logger;
        public MyApplication(ILogger<MyApplication> logger)
        {
            _logger = logger;
        }
        public void Start()
        {
            _logger.LogInformation($"MyApplication Started at {DateTime.Now}");
            LoadDashboard();
        }

        private void LoadDashboard()
        {
            try
            {
                _logger.LogWarning("MyApplication->LoadDashboard() can throw Exception!");
                int[] a = new int[] { 1, 2, 3, 4, 5 };
                int b = a[5];
                Console.WriteLine($"Value of B: {b}");
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                _logger.LogCritical($"MyApplication->LoadDashboard() Code needs to be fixed");
            }
        }

        public void Stop()
        {
            _logger.LogInformation($"MyApplication Stopped at {DateTime.Now}");
        }

        public void HandleError(Exception ex)
        {
            _logger.LogError($"MyApplication Error Encountered at {DateTime.Now} & Error is: {ex.Message}");
        }
    }
}
