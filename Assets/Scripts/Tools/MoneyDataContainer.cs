using Ji2.CommonCore.SaveDataContainer;
using Ji2Core.Core;

namespace CarPool.Tools
{
    public class MoneyDataContainer : IBootstrapable
    {
        private const string MoneySaveKey = "MoneySaveKey";
        
        private readonly ISaveDataContainer _saveDataContainer;

        public int MoneyAmount { get; private set; }
        
        public MoneyDataContainer(ISaveDataContainer saveDataContainer)
        {
            _saveDataContainer = saveDataContainer;
        }

        public void Bootstrap()
        {
            MoneyAmount = _saveDataContainer.GetValue<int>(MoneySaveKey);
        }

        public void Add(int amount)
        {
            MoneyAmount += amount;
        }

        public bool Subtract(int amount)
        {
            if (MoneyAmount < amount)
                return false;

            MoneyAmount -= amount;
            return true;
        }

        public void Save()
        {
            _saveDataContainer.SaveValue(MoneySaveKey, MoneyAmount);
        }
    }
}