using Xunit;
using System;

using BatchValidatorMockTest.Helpers;

namespace BatchValidatorMockTest.Tests
{
    public class BatchValidatorTests
    {
        private readonly string _basePath = "ResponseFile";

        /// <summary>
        /// Validate Job1 during success mock log exist in json format
        /// </summary>
        [Fact]
        public void Validate_Success_Job1JsonLogs_Valid_json()
        {
            string relativePath = Path.Combine($"{_basePath}//Job1//", "Job1mockSuccess.json");
            string fullPath = Path.Combine(AppContext.BaseDirectory, relativePath);

            var mockResponse = MockHelper.LoadMockResponse(fullPath);

            var result = BatchValidator.ValidateJsonLog(mockResponse);

            Assert.True(result.IsSuccess);
            Assert.Equal("Job completed successfully", result.Output);
        }

        /// <summary>
        /// Validate Job1 during failure(not found case) mock log in json format
        /// </summary>
        [Fact]
        public void Validate_Failure_Job1JsonLogs_DuringFileNotFound_json()
        {
            string relativePath = Path.Combine($"{_basePath}//Job1//", "Job1mockFailure.json");
            string fullPath = Path.Combine(AppContext.BaseDirectory, relativePath);

            var mockResponse = MockHelper.LoadMockResponse(fullPath);
            var result = BatchValidator.ValidateJsonLog(mockResponse);

            Assert.False(result.IsSuccess);
            Assert.Equal("File not found", result.Message);
        }

        /// <summary>
        /// Validate Job1 during success mock log exist in text format
        /// </summary>
        [Fact]
        public void Validate_Success_Job1Logs_Valid_Text()
        {
            string expectedLogs = "Hello, User. This HTTP triggered function executed successfully.";

            string relativePath = Path.Combine($"{_basePath}//Job1//", "Job1mockSuccess.txt");
            string fullPath = Path.Combine(AppContext.BaseDirectory, relativePath);

            var mockResponse = MockHelper.LoadMockResponse(fullPath);

            var result = BatchValidator.ValidateTextLog(mockResponse);

            Assert.Contains(expectedLogs, result.Output?.ToString() ?? string.Empty);
        }

        /// <summary>
        /// Validate Job1 during fialure mock log exist in text format
        /// </summary>
        [Fact]
        public void Validate_Failure_Job1Logs_InValid_Text()
        {
            string expectedLogs = "Validation failed. Returning 400 Bad Request";

            string relativePath = Path.Combine($"{_basePath}//Job1//", "Job1mockFailure.txt");
            string fullPath = Path.Combine(AppContext.BaseDirectory, relativePath);

            var mockResponse = MockHelper.LoadMockResponse(fullPath);

            var result = BatchValidator.ValidateTextLog(mockResponse);

            Assert.Contains(expectedLogs, result.Output?.ToString() ?? string.Empty);
        }
    }
}
