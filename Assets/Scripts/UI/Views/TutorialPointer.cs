using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace CarPool.UI.Views
{
    public class TutorialPointer : MonoBehaviour
    {
        [SerializeField] private float scaleOnAnimation;
        [SerializeField] private float scaleDuration;
        [SerializeField] private float moveDuration;
        [SerializeField] private float moveMagnitude;
        
        public async UniTask AnimateSwipe(
            Vector3 fromPosition,
            Vector3 direction,
            CancellationToken cancellationToken
        )
        {
            transform.position = fromPosition;
            
            var sequence = DOTween.Sequence()
                .Pause()
                .Append(
                    transform.DOScale(transform.localScale * scaleOnAnimation, scaleDuration)
                )
                .Append(
                    transform.DOMove(fromPosition + direction * moveMagnitude, moveDuration)
                ).SetEase(Ease.OutQuad)
                .Append(
                    transform.DOScale(transform.localScale, scaleDuration)
                );

            Show();
            
            await UniTask.Delay(TimeSpan.FromSeconds(.5d), cancellationToken: cancellationToken)
                .SuppressCancellationThrow();

            sequence.Play();
            await UniTask.WaitWhile(
                    () => sequence.IsActive(),
                    cancellationToken: cancellationToken
                )
                .SuppressCancellationThrow();

            await UniTask.Delay(TimeSpan.FromSeconds(.5d), cancellationToken: cancellationToken)
                .SuppressCancellationThrow();

            Hide();
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }
        
        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}