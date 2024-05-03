using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace StudentManagement.Service.Services
{
    public class BackgroundWorkerService : BackgroundService
    {
        readonly ILogger<BackgroundWorkerService> _logger;
        public BackgroundWorkerService(ILogger<BackgroundWorkerService> logger)
        {
            _logger = logger;
        }


        protected async override Task ExecuteAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                _logger.LogInformation("New student enrollment ends soon!");
                //await Task.Delay(1000,  cancellationToken);
                await Task.Delay(3600000, cancellationToken); //for one hour delay 
            }



        }



    }
}
