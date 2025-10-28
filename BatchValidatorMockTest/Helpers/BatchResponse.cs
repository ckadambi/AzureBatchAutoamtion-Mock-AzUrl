namespace BatchValidatorMockTest.Helpers
{
    public class BatchResponse
    {
        public int ExitCode { get; set; }
        public string? Output { get; set; }
        public string? Error { get; set; }
        public string? Logs { get; set; }
    }
}
