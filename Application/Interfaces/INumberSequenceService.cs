using Application.Models.Request;
using Application.Models.Response;

namespace Application.Interfaces
{
    public interface INumberSequenceService
    {
        Task<NumberSequenceResponse> AnalyzeSequenceAsync(NumberSequenceRequest request);

        Task<NumberSequenceOrderResponse> OrderSequenceAsync(NumberSequenceOrderRequest request);
    }
}
