using System;
using Ji2Core.Core.Tools;

namespace CarPool.UI.Models
{
    public class GameScreenModel
    {
        private readonly TimeScaler _timeScaler;
        
        private bool _settingsEnabled = false;

        public event Action<bool> OnToggleSettings;
        public event Action OnWin;
        public event Action OnLose;
        public event Action OnLoadNextLevel;
        public event Action OnReload;
        public event Action OnReset;
        public event Action<int> OnMoneyAmountChange;
        public event Action<int> OnSwipeCountChange;

        public GameScreenModel(TimeScaler timeScaler)
        {
            _timeScaler = timeScaler;
        }

        public void LoadNextLevel()
        {
            OnLoadNextLevel?.Invoke();
        }

        public void ShowWinMenu()
        {
            OnWin?.Invoke();
        }

        public void ShowLoseMenu()
        {
            OnLose?.Invoke();
        }
        
        public void Reload()
        {
            OnReload?.Invoke();
        }

        public void ToggleSettings()
        {
            _settingsEnabled = !_settingsEnabled;

            if (_settingsEnabled)
            {
                _timeScaler.Pause();
            }
            else
            {
                _timeScaler.Confirm();
            }
            
            OnToggleSettings?.Invoke(_settingsEnabled);
        }

        public void ResetSaves()
        {
            OnReset?.Invoke();
        }

        public void ChangeMoneyAmount(int amount)
        {
            OnMoneyAmountChange?.Invoke(amount);
        }

        public void ChangeSwipeCount(int count)
        {
            OnSwipeCountChange?.Invoke(count);
        }
    }
}