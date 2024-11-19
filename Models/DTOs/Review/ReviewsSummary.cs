namespace YssWebstoreApi.Models.DTOs.Review
{
    public class ReviewsSummary
    {
        private readonly Dictionary<int, int> _rates = [];

        public int MinRate { get; }
        public int MaxRate { get; }

        public ReviewsSummary(int minRate, int maxRate)
        {
            if (maxRate < minRate)
            {
                throw new ArgumentException($"{nameof(minRate)} cannot be greater than {nameof(maxRate)}");
            }

            MinRate = minRate;
            MaxRate = maxRate;
        }

        public double Average
        {
            get => (double)_rates.Sum(x => x.Key * x.Value) / Math.Max(TotalCount, 1);
        }

        public int TotalCount
        {
            get => _rates.Sum(x => x.Value);
        }

        public IReadOnlyCollection<RateSummary> Rates
        {
            get
            {
                double totalCount = Math.Max(TotalCount, 1);
                var rates = new List<RateSummary>();

                for (int i = MinRate; i <= MaxRate; i++)
                {
                    _rates.TryGetValue(i, out var rate);

                    rates.Add(new RateSummary
                    {
                        Rate = i,
                        Count = rate,
                        Share = rate / totalCount
                    });
                }

                return rates;
            }
        }

        public void AddRates(int rate, int count)
        {
            if (rate < MinRate || rate > MaxRate)
            {
                throw new ArgumentOutOfRangeException($"{nameof(rate)} must be between {MinRate} and {MaxRate}");
            }

            if (rate != 0)
            {
                if (!_rates.ContainsKey(rate))
                {
                    _rates.Add(rate, 0);
                }

                _rates[rate] += count;
            }
        }
    }
}
