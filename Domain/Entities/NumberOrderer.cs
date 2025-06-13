namespace Domain.Entities
{
    public class NumberOrderer
    {
        public IReadOnlyCollection<int> Values { get; }

        public NumberOrderer(IEnumerable<int> values)
        {
            Values = values.ToList().AsReadOnly();
        }
    }
}
