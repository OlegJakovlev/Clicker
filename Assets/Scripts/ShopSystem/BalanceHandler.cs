using System;

namespace ShopSystem
{
    public class BalanceHandler
    {
        public event Action BalanceChanged;

        public int CurrentBalance { get; private set; }

        public void AddBalance(int amount)
        {
            if (amount < 0) return;
            CurrentBalance += amount;
            BalanceChanged?.Invoke();
        }

        public void SubtractBalance(int amount)
        {
            if (amount < 0) return;
            CurrentBalance -= amount;
            BalanceChanged?.Invoke();
        }
    }
}