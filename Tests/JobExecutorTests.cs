// using System.Net;
// using System.Net.Http;
// using System.Threading.Tasks;
// using AzureJobAutomation.Models;
// using AzureJobAutomation.Services;
// using AzureJobAutomation.Utils;
// using FluentAssertions;
// using System.Threading;
// using System;
// using Xunit;

// namespace AzureJobAutomation.Tests;

// public class JobExecutorTests
// {
//     [Fact]
//     public async Task ExecuteJobAsync_ReturnsSuccess_On2xx()
//     {
//         // Arrange
//         var handler = new StubHandler((req) => new HttpResponseMessage(HttpStatusCode.OK)
//         {
//             Content = new StringContent("{\"ok\":true}")
//         });
//         var client = new HttpClient(handler);
//         var logger = new SimpleLogger();
//         var exec = new HttpJobExecutor(client, logger) { MaxRetries = 0 };

//         var job = new JobDefinition
//         {
//             Name = "TestJob",
//             Url = "https://example.com/trigger",
//             Method = "POST",
//         };

//         // Act
//         var result = await exec.ExecuteJobAsync(job);

//         // Assert
//         result.Success.Should().BeTrue();
//         result.StatusCode.Should().Be(HttpStatusCode.OK);
//         result.Attempts.Should().Be(1);
//     }

//     [Fact]
//     public async Task ExecuteJobAsync_Retries_AndFails_On5xx()
//     {
//         // Arrange: always 500
//         var handler = new StubHandler((req) => new HttpResponseMessage(HttpStatusCode.InternalServerError));
//         var client = new HttpClient(handler) { Timeout = TimeSpan.FromSeconds(3) };
//         var logger = new SimpleLogger();
//         var exec = new HttpJobExecutor(client, logger) { MaxRetries = 2, RetryDelay = TimeSpan.FromMilliseconds(10) };
//         var job = new JobDefinition { Name = "FailJob", Url = "https://example.com/fail" };

//         // Act
//         var result = await exec.ExecuteJobAsync(job);

//         // Assert
//         result.Success.Should().BeFalse();
//         result.Attempts.Should().BeGreaterThan(1);
//         result.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
//     }

//     [Fact]
//     public async Task ExecuteJobAsync_TimesOut_AndRetries()
//     {
//         // Arrange: simulate timeout via TaskCanceledException
//         var handler = new StubHandler(_ => throw new TaskCanceledException("timeout"));
//         var client = new HttpClient(handler) { Timeout = TimeSpan.FromMilliseconds(50) };
//         var logger = new SimpleLogger();
//         var exec = new HttpJobExecutor(client, logger) { MaxRetries = 1, RetryDelay = TimeSpan.FromMilliseconds(5) };
//         var job = new JobDefinition { Name = "TimeoutJob", Url = "https://example.com/slow" };

//         // Act
//         var result = await exec.ExecuteJobAsync(job);

//         // Assert
//         result.Success.Should().BeFalse();
//         result.Attempts.Should().Be(2);
//         result.StatusCode.Should().BeNull();
//     }

//     private sealed class StubHandler : HttpMessageHandler
//     {
//         private readonly Func<HttpRequestMessage, HttpResponseMessage> _responder;
//         public StubHandler(Func<HttpRequestMessage, HttpResponseMessage> responder)
//             => _responder = responder;

//         protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
//             => Task.FromResult(_responder(request));
//     }
// }
