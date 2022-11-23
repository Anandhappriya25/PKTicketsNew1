using PKTickets.Models.DTO;

namespace PKTickets.Models
{
    public class TimedHostedService : IHostedService, IDisposable
    {
        private readonly ILogger _logger;
        private Timer _timer;
        private readonly PKTicketsDbContext db;

        public TimedHostedService(ILogger<TimedHostedService> logger, PKTicketsDbContext db)
        {
            _logger = logger;
            this.db = db;
        }


        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Timed Background Service is starting.");

            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(3));

            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {
            _logger.LogInformation("Timed Background Service is working.");
            var schedules=db.Schedules.Where(x=>x.IsActive==true).ToList();
            DateTime date = DateTime.Now;
            var schedulesDate = schedules.Where(x => x.Date <= date.Date).ToList();
            TimeSpan time = new TimeSpan(date.Hour, date.Minute, 0);
            var timeValue = TimingConvert.ConvertToInt(Convert.ToString(time));
            foreach (Schedule schedule in schedulesDate)
            {
                var times = db.ShowTimes.FirstOrDefault(x => x.ShowTimeId == schedule.ShowTimeId);
                if (times.ShowTiming < timeValue)
                {
                    schedule.IsActive = false;
                    db.SaveChanges();
                }
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Timed Background Service is stopping.");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
