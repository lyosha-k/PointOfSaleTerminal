namespace PointOfSaleTerminal
{
    public interface IDiscountCard
    {
        void AddAmount(decimal amount);
        int GetDiscountPercent();
    }

    public class DiscountCard : IDiscountCard
    {
        decimal _accumulatedAmount;

        public static readonly IDiscountCard None = new NullCard();

        public void AddAmount(decimal amount)
        {
            _accumulatedAmount += amount;
        }

        public int GetDiscountPercent()
        {
            if (_accumulatedAmount < 1000)
                return 0;
            if (_accumulatedAmount < 2000)
                return 1;
            if (_accumulatedAmount < 5000)
                return 3;
            if (_accumulatedAmount < 10000)
                return 5;
            return 7;
        }

        class NullCard : IDiscountCard
        {
            public int GetDiscountPercent() => 0;

            public void AddAmount(decimal amount)
            {
            }
        }
    }
}