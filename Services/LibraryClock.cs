namespace TopEndLibraryHub.Services
{
    public class LibraryClock
    {
        private readonly TimeProvider _timeProvider;
        private static readonly TimeSpan DarwinOffset = TimeSpan.FromMinutes(570);

        public LibraryClock(TimeProvider timeProvider)
        {
            _timeProvider = timeProvider;
        }

        public DateTime UtcNow => _timeProvider.GetUtcNow().UtcDateTime;
        public DateTime Today => ToDarwinTime(UtcNow).Date;

        public DateTime ToDarwinTime(DateTime utc)
        {
            return new DateTimeOffset(DateTime.SpecifyKind(utc, DateTimeKind.Utc))
                .ToOffset(DarwinOffset).DateTime;
        }
    }
}
