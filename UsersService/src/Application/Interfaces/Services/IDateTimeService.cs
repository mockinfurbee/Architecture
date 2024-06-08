namespace Application.Interfaces.Services
{
    public interface IDateTimeService
    {
        public DateTime NowUtc { get; }
        public string GetTime24ByTS(TimeSpan ts);
    }
}
