using CarPool.UI.ViewModels;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Ji2Core.UI.Screens;
using TMPro;
using UnityEngine;

namespace CarPool.UI.Views
{
    public class GameScreen : BaseScreen
    {
        private const float FadeAnimationDuration = .8f;
        
        [SerializeField] private TMP_Text moneyAmountText;
        [SerializeField] private TMP_Text swipeCountText;
        [SerializeField] private RectTransform coinImage;
        [SerializeField] private GameObject swipeInfinityImage;
        [SerializeField] private GameObject settingsMenu;
        [SerializeField] private CanvasGroup winMenu;
        [SerializeField] private CanvasGroup loseMenu;
        [SerializeField] private TMP_Text levelNumberText;

        private GameScreenVM _viewModel;

        public void Construct(GameScreenVM viewModel, int levelNumber)
        {
            _viewModel = viewModel;

            _viewModel.OnToggleSettings += OnSettingsToggled;
            _viewModel.OnWin += OnWon;
            _viewModel.OnLose += OnLost;
            _viewModel.OnMoneyAmountChange += OnMoneyAmountChanged;
            _viewModel.OnSwipeCountChange += OnSwipeCountChanged;

            levelNumberText.text = $"Уровень {(levelNumber < 10 ? $"0{levelNumber}" : levelNumber)}";
        }

        public void ClickNextLevelButton()
        {
            _viewModel.ClickNextLevelButton();
        }

        public void ClickReloadButton()
        {
            _viewModel.ClickReloadButton();
        }

        public void ClickSettingsButton()
        {
            _viewModel.ClickSettingsButton();
        }

        public void ClickResetButton()
        {
            _viewModel.ClickResetButton();
        }

        private void OnSettingsToggled(bool enabled)
        {
            settingsMenu.SetActive(enabled);
        }

        private async void OnWon()
        {
            await Fade(winMenu, 0f, 1f, FadeAnimationDuration);
            winMenu.blocksRaycasts = true;
        }

        private async void OnLost()
        {
            await Fade(loseMenu, 0f, 1f, FadeAnimationDuration);
            loseMenu.blocksRaycasts = true;
        }

        private async UniTask Fade(CanvasGroup group, float from, float to, float duration)
        {
            await UniTask.Create(async () =>
            {
                var curve = new AnimationCurve(
                    new Keyframe(0f, from), 
                    new Keyframe(1f, to)
                );
                
                for (float time = 0f; time < duration; time += Time.unscaledDeltaTime)
                {
                    group.alpha = curve.Evaluate(time / duration);
                    await UniTask.NextFrame();
                }

                group.alpha = to;
            });
        }

        private void OnMoneyAmountChanged(int amount)
        {
            moneyAmountText.text = $"{amount}";
        }

        private void OnSwipeCountChanged(int count)
        {
            swipeCountText.gameObject.SetActive(count >= 0);
            swipeInfinityImage.SetActive(count < 0);
            swipeCountText.text = $"{count}";
        }
    }
}