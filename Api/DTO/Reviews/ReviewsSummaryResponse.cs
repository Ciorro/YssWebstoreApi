namespace YssWebstoreApi.Api.DTO.Reviews
{
    public class ReviewsSummaryResponse
    {
        private readonly Dictionary<int, int> _rates = [];

        public int MinRate { get; }
        public int MaxRate { get; }

        public ReviewsSummaryResponse(int minRate, int maxRate)
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

        public IEnumerable<RateCount> Rates
        {
            get
            {
                if (TotalCount == 0)
                {
                    yield break;
                }

                for (int i = MinRate; i <= MaxRate; i++)
                {
                    _rates.TryGetValue(i, out int count);

                    yield return new RateCount(
                        rate: i, 
                        count: count, 
                        share: count / (float)TotalCount);
                }
            }
        }

        public void AddRateCounts(int rate, int count)
        {
            if (rate < MinRate || rate > MaxRate)
            {
                throw new ArgumentOutOfRangeException($"{nameof(rate)} must be between {MinRate} and {MaxRate} (inclusive).");
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

        public struct RateCount
        {
            public int Rate { get; set; }
            public int Count { get; set; }
            public float Share { get; set; }

            public RateCount(int rate, int count, float share)
            {
                Rate = rate;
                Count = count;
                Share = share;
            }
        }
    }
}
