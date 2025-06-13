using API.Controllers;
using Application.Interfaces;
using Application.Models.Request;
using Application.Models.Response;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace Tests.API.Controllers
{
    public class SequenceControllerTests
    {
        private readonly INumberSequenceService _service = Substitute.For<INumberSequenceService>();
        private readonly IValidator<NumberSequenceRequest> _validator = Substitute.For<IValidator<NumberSequenceRequest>>();
        private readonly IValidator<NumberSequenceOrderRequest> _orderValidator = Substitute.For<IValidator<NumberSequenceOrderRequest>>();
        private readonly ILogger<SequenceController> _logger = Substitute.For<ILogger<SequenceController>>();

        private SequenceController CreateController()
        {
            return new SequenceController(
                service: _service,
                validator: _validator,
                orderValidator: _orderValidator,
                logger: _logger
            );
        }

        [Fact]
        public async Task Analyze_ShouldReturnBadRequest_WhenValidationFails()
        {
            var request = new NumberSequenceRequest
            {
                Values = new List<int> { 1, 2, 3 }.ToAsyncEnumerable()
            };

            _validator.Validate(request).Returns(new ValidationResult(new[]
            {
            new ValidationFailure("Values", "Invalid sequence")
            }));

            var controller = CreateController();

            var result = await controller.Analyze(request);

            result.Result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task Analyze_ShouldReturnOk_WhenServiceSucceeds()
        {
            var request = new NumberSequenceRequest
            {
                Values = new List<int> { 5, 4, 3 }.ToAsyncEnumerable()
            };

            _validator.Validate(request).Returns(new ValidationResult());

            var response = new NumberSequenceResponse
            {
                IsDescending = true
            };

            _service.AnalyzeSequenceAsync(request).Returns(response);

            var controller = CreateController();

            var result = await controller.Analyze(request);

            result.Result.Should().BeOfType<OkObjectResult>();
            ((OkObjectResult)result.Result!).Value.Should().Be(response);
        }

        [Fact]
        public async Task Analyze_ShouldReturnServerError_WhenExceptionThrown()
        {
            var request = new NumberSequenceRequest
            {
                Values = new List<int> { 9, 8, 7 }.ToAsyncEnumerable()
            };

            _validator.Validate(request).Returns(new ValidationResult());

            _service.AnalyzeSequenceAsync(request).Throws(new Exception("Service failure"));

            var controller = CreateController();

            var result = await controller.Analyze(request);

            result.Result.Should().BeOfType<ObjectResult>()
                .Which.StatusCode.Should().Be(500);
        }

        [Fact]
        public async Task Order_ShouldReturnBadRequest_WhenValidationFails()
        {
            var request = new NumberSequenceOrderRequest
            {
                Values = new List<int> { 3, 2, 1 }.ToAsyncEnumerable()
            };

            _orderValidator.Validate(request).Returns(new ValidationResult(new[]
            {
            new ValidationFailure("Values", "Invalid input")
            }));

            var controller = CreateController();

            var result = await controller.Order(request);

            result.Result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task Order_ShouldReturnOk_WhenServiceSucceeds()
        {
            var request = new NumberSequenceOrderRequest
            {
                Values = new List<int> { 3, 1, 2 }.ToAsyncEnumerable()
            };

            _orderValidator.Validate(request).Returns(new ValidationResult());

            var response = new NumberSequenceOrderResponse
            {
                SortedAscending = new List<int> { 1, 2, 3 },
                SortedDescending = new List<int> { 3, 2, 1 }
            };

            _service.OrderSequenceAsync(request).Returns(response);

            var controller = CreateController();

            var result = await controller.Order(request);

            result.Result.Should().BeOfType<OkObjectResult>();
            ((OkObjectResult)result.Result!).Value.Should().Be(response);
        }

        [Fact]
        public async Task Order_ShouldReturnServerError_WhenExceptionThrown()
        {
            var request = new NumberSequenceOrderRequest
            {
                Values = new List<int> { 1, 2, 3 }.ToAsyncEnumerable()
            };

            _orderValidator.Validate(request).Returns(new ValidationResult());

            _service.OrderSequenceAsync(request).Throws(new Exception("Unexpected error"));

            var controller = CreateController();

            var result = await controller.Order(request);

            result.Result.Should().BeOfType<ObjectResult>()
                .Which.StatusCode.Should().Be(500);
        }
    }
}
