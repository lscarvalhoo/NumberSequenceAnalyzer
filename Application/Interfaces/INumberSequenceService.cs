using Application.Models.Request;
using Application.Models.Response;

namespace Application.Interfaces
{
    public interface INumberSequenceService
    {
        NumberSequenceResponse AnalyzeSequence(NumberSequenceRequest request);

        NumberSequenceOrderResponse OrderSequence(NumberSequenceOrderRequest request);
    }
}
