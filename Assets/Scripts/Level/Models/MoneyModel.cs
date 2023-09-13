using System;
using CarPool.Entities.Views;
using CarPool.Tools;

namespace CarPool.Level.Models
{
    public class MoneyModel
    {
        private readonly MoneyDataContainer _moneyDataContainer;

        public event Action<int> OnMoneyAmountChange;

        public int MoneyAmount => _moneyDataContainer.MoneyAmount;

        public MoneyModel(MoneyDataContainer moneyDataContainer, Coin[] coins)
        {
            _moneyDataContainer = moneyDataContainer;
            _moneyDataContainer.Bootstrap();

            foreach (var coin in coins)
            {
                coin.OnCollect += OnCoinCollected;
            }
        }

        public void ResetSaves()
        {
            _moneyDataContainer.Subtract(_moneyDataContainer.MoneyAmount);
            Save();
            OnMoneyAmountChange?.Invoke(MoneyAmount);
        }

        private void OnCoinCollected(int reward)
        {
            _moneyDataContainer.Add(reward);
            
            OnMoneyAmountChange?.Invoke(MoneyAmount);
        }

        public void Save()
        {
            _moneyDataContainer.Save();
        }
    }
}