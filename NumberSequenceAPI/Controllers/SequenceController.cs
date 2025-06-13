using Application.Interfaces;
using Application.Models.Request;
using Application.Models.Response;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    /// <summary>
    /// Controller responsável pela análise de sequências numéricas.
    /// </summary>
    [ApiController]
    [Route("api/sequence")]
    public class SequenceController : ControllerBase
    {
        private readonly INumberSequenceService _service;
        private readonly IValidator<NumberSequenceRequest> _validator;
        private readonly ILogger<SequenceController> _logger;
        private readonly IValidator<NumberSequenceOrderRequest> _orderValidator;

        /// <summary>
        /// Injeção do serviço de análise de sequência numérica.
        /// </summary>
        /// <param name="service">Serviço de análise de sequência</param>
        /// <param name="validator">Serviço de validação de entrada</param>
        /// <param name="orderValidator">Serviço de validação de entrada</param>
        /// <param name="logger">Logger injetado via Serilog</param>
        public SequenceController(
            INumberSequenceService service,
            IValidator<NumberSequenceRequest> validator,
            IValidator<NumberSequenceOrderRequest> orderValidator,
            ILogger<SequenceController> logger)
        {
            _service = service;
            _validator = validator;
            _orderValidator = orderValidator;
            _logger = logger;
        }

        /// <summary>
        /// Analisa uma sequência de números inteiros:
        /// Ordem crescente/decrescente, Números repetidos,
        /// Alternância e Primalidade.
        /// </summary>
        /// <param name="request">Lista de inteiros a ser analisada</param>
        /// <returns>Resultado da análise da sequência numérica</returns>
        /// <response code="200">Sequência analisada com sucesso</response>
        /// <response code="400">Requisição inválida</response>
        /// <response code="500">Erro interno do servidor</response>
        [HttpPost("analyze")]
        [ProducesResponseType(typeof(NumberSequenceResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<NumberSequenceResponse>> Analyze([FromBody] NumberSequenceRequest request)
        {
            var validationResult = _validator.Validate(request);
            if (!validationResult.IsValid)
            {
                _logger.LogWarning("Validation failed for sequence: {@Errors}", validationResult.Errors);
                return BadRequest(validationResult);
            }

            try
            {
                var result = await _service.AnalyzeSequenceAsync(request);

                _logger.LogInformation("Analysis completed successfully: {@Result}", result);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while analyzing the sequence.");
                return StatusCode(500, "Internal server error while analyzing the sequence.");
            }
        }

        /// <summary>
        /// Ordena uma sequência de números inteiros em ordem crescente e decrescente.
        /// </summary>
        /// <param name="request">Lista de inteiros a ser ordenada</param>
        /// <returns>Sequência ordenada em ordem crescente e decrescente</returns>
        /// <response code="200">Sequência ordenada com sucesso</response>
        /// <response code="400">Requisição inválida</response>
        /// <response code="500">Erro interno do servidor</response>
        [HttpPost("order")]
        [ProducesResponseType(typeof(NumberSequenceOrderResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<NumberSequenceOrderResponse>> Order([FromBody] NumberSequenceOrderRequest request)
        {
            var validationResult = _orderValidator.Validate(request);
            if (!validationResult.IsValid)
            {
                _logger.LogWarning("Validation failed: {@Errors}", validationResult.Errors);
                return BadRequest(validationResult);
            }

            try
            {
                var result = await _service.OrderSequenceAsync(request);

                _logger.LogInformation("Ordering completed. Ascending: {CountAsc}, Descending: {CountDesc}",
                    result.SortedAscending.Count, result.SortedDescending.Count);

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while ordering the sequence.");
                return StatusCode(500, "Internal server error while ordering the sequence.");
            }
        }
    }
}
