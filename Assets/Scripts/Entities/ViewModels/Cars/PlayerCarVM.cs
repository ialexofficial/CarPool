using CarPool.Entities.Models.Cars;

namespace CarPool.Entities.ViewModels.Cars
{
    public class PlayerCarVM : MovableCarVM
    {
        private readonly PlayerCarModel _model;

        public PlayerCarVM(PlayerCarModel model)
            : base(model)
        {
            _model = model;
        }
        
        public void Destroy()
        {
            _model.Destroy();
        }
    }
}