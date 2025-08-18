namespace Application.Common
{
    public class TestResultDto
    {
        public Guid Id { get; set; }

        public long Timestamp { get; set; }

        public string TestType { get; set; } = string.Empty;

        public string? TargetHost { get; set; }

        public string ResultValue { get; set; } = string.Empty;

        public bool IsSuccess { get; set; }

        public string? Details { get; set; }

        public Guid? TestId { get; set; }
    }
}
