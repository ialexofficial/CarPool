using System;

namespace CarPool.Entities.Models.Cars
{
    public class PlayerCarModel : MovableCarModel
    {
        public event Action OnDestroy;

        public PlayerCarModel(MovableCarSettings settings, PointerModel pointerModel)
            : base(settings, pointerModel)
        {}

        public void Destroy()
        {
            OnDestroy?.Invoke();
        }
    }
}