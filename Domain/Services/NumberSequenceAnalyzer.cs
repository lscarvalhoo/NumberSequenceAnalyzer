using Domain.Common.YourProject.Domain.Common;
using Domain.Entities;
using Domain.Interfaces;

namespace Domain.Services
{
    public class NumberSequenceAnalyzer : INumberSequenceAnalyzer
    {
        public bool IsAscending(NumberSequence sequence)
        {
            for (int i = 1; i < sequence.Values.Count; i++)
            {
                if (sequence.Values[i] <= sequence.Values[i - 1])
                    return false;
            }

            return true;
        }

        public bool IsDescending(NumberSequence sequence)
        {
            for (int i = 1; i < sequence.Values.Count; i++)
            {
                if (sequence.Values[i] >= sequence.Values[i - 1])
                    return false;
            }

            return true;
        }

        public bool HasDuplicates(NumberSequence sequence)
        {
            var seen = new HashSet<int>();
            foreach (var value in sequence.Values)
            {
                if (!seen.Add(value))
                    return true;
            }

            return false;
        }

        public bool IsAlternating(NumberSequence sequence)
        {
            if (sequence.Values.Count < SequenceAnalysisConstants.MIN_SEQUENCE_LENGTH)
                return false;

            for (int index = SequenceAnalysisConstants.FIRST_INDEX;
                     index < sequence.Values.Count - SequenceAnalysisConstants.NEXT_OFFSET;
                     index++)
            {
                int previousValue = sequence.Values[index - SequenceAnalysisConstants.PREVIOUS_OFFSET];
                int currentValue = sequence.Values[index];
                int nextValue = sequence.Values[index + SequenceAnalysisConstants.NEXT_OFFSET];

                bool isPeak = currentValue > previousValue && currentValue > nextValue;
                bool isValley = currentValue < previousValue && currentValue < nextValue;

                bool isAlternatingAtCurrentIndex = isPeak || isValley;

                if (!isAlternatingAtCurrentIndex)
                    return false;
            }

            return true;
        }

        public bool AllPrimes(NumberSequence sequence)
        {
            return sequence.Values.AsParallel().All(value => IsPrime(value));
        }

        private bool IsPrime(int number)
        {
            if (number < SequenceAnalysisConstants.SMALLEST_PRIME)
                return false;

            if (number == SequenceAnalysisConstants.SMALLEST_PRIME)
                return true;

            if (number % SequenceAnalysisConstants.EVEN_NUMBER_DIVISOR == SequenceAnalysisConstants.ZERO)
                return false;

            int boundary = (int)Math.Sqrt(number);

            for (int divisor = SequenceAnalysisConstants.FIRST_ODD_PRIME;
                     divisor <= boundary;
                     divisor += SequenceAnalysisConstants.INCREMENT_BY_TWO)
            {
                if (number % divisor == SequenceAnalysisConstants.ZERO)
                    return false;
            }

            return true;
        }
    }
}