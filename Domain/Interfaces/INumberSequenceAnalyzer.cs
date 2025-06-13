using Domain.Entities;

namespace Domain.Interfaces
{

    public interface INumberSequenceAnalyzer
    {
        bool IsAscending(NumberSequence sequence);
        bool IsDescending(NumberSequence sequence);
        bool HasDuplicates(NumberSequence sequence);
        bool IsAlternating(NumberSequence sequence);
        bool AllPrimes(NumberSequence sequence);
    }
}