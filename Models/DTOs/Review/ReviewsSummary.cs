namespace YssWebstoreApi.Models.DTOs.Review
{
    public class ReviewsSummary
    {
        private readonly Dictionary<int, int> _rates = [];

        public double Average
        {
            get => (double)_rates.Sum(x => x.Key * x.Value) / _rates.Sum(x => x.Value);
        }

        public int TotalCount
        {
            get => _rates.Sum(x => x.Value);
        }

        public IReadOnlyCollection<RateSummary> Rates
        {
            get
            {
                double totalCount = TotalCount;

                return _rates.Select(x => new RateSummary
                {
                    Rate = x.Key,
                    Count = x.Value,
                    Share = x.Value / totalCount
                }).ToList();
            }
        }

        public void AddRates(int rate, int count)
        {
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
