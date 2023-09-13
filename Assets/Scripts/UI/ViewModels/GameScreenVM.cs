using System;
using CarPool.UI.Models;

namespace CarPool.UI.ViewModels
{
    public class GameScreenVM
    {
        private readonly GameScreenModel _model;

        public event Action<bool> OnToggleSettings;
        public event Action<int> OnMoneyAmountChange;
        public event Action<int> OnSwipeCountChange;
        public event Action OnWin;
        public event Action OnLose;
        
        public GameScreenVM(GameScreenModel model)
        {
            _model = model;

            _model.OnToggleSettings += (enabled) => OnToggleSettings?.Invoke(enabled);
            _model.OnWin += () => OnWin?.Invoke();
            _model.OnLose += () => OnLose?.Invoke();
            _model.OnMoneyAmountChange += (amount) => OnMoneyAmountChange?.Invoke(amount);
            _model.OnSwipeCountChange += (count) => OnSwipeCountChange?.Invoke(count);
        }

        public void ClickNextLevelButton()
        {
            _model.LoadNextLevel();
        }

        public void ClickReloadButton()
        {
            _model.Reload();
        }

        public void ClickSettingsButton()
        {
            _model.ToggleSettings();
        }

        public void ClickResetButton()
        {
            _model.ResetSaves();
        }
    }
}