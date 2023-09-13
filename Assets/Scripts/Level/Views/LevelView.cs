using CarPool.Level.Presenters;
using CarPool.UI.Models;
using CarPool.UI.ViewModels;
using CarPool.UI.Views;
using Ji2Core.Core.Tools;

namespace CarPool.Level.Views
{
    public class LevelView
    {
        private readonly GameScreen _gameScreen;
        private readonly GameScreenModel _gameScreenModel;
        private readonly GameScreenVM _gameScreenVM;

        public LevelView(
            LevelPresenter levelPresenter,
            GameScreen gameScreen,
            TimeScaler timeScaler,
            int levelNumber
        )
        {
            _gameScreen = gameScreen;

            _gameScreenModel = new GameScreenModel(timeScaler);
            _gameScreenVM = new GameScreenVM(_gameScreenModel);

            _gameScreenModel.OnReload += levelPresenter.ReloadLevel;
            _gameScreenModel.OnReset += levelPresenter.ResetSaves;
            _gameScreenModel.OnLoadNextLevel += levelPresenter.LoadNextLevel;

            _gameScreen.Construct(_gameScreenVM, levelNumber);
        }

        public void OnMoneyAmountChanged(int amount)
        {
            _gameScreenModel.ChangeMoneyAmount(amount);
        }

        public void OnWon()
        {
            _gameScreenModel.ShowWinMenu();
        }

        public void OnLost()
        {
            _gameScreenModel.ShowLoseMenu();
        }

        public void OnSwipeCountChanged(int swipeCount)
        {
            _gameScreenModel.ChangeSwipeCount(swipeCount);
        }
    }
}