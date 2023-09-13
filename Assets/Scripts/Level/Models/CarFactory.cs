using System;
using CarPool.Entities;
using CarPool.Entities.Models;
using CarPool.Entities.Models.Cars;
using CarPool.Entities.ViewModels;
using CarPool.Entities.ViewModels.Cars;
using CarPool.Entities.Views;
using CarPool.Entities.Views.Cars;
using CarPool.Tools;
using Ji2.CommonCore;
using UnityEngine;

namespace CarPool.Level.Models
{
    public class CarFactory
    {
        private readonly PositionedDragInput _positionedDragInput;
        private readonly UpdateService _updateService;

        public CarFactory(
            UpdateService updateService,
            PositionedDragInput positionedDragInput
        )
        {
            _updateService = updateService;
            _positionedDragInput = positionedDragInput;
        }

        public T Build<T>(SpawnPoint spawnPoint) where T: MovableCarModel
        {
            var carSettings = spawnPoint.Settings;
            
            var (car, pointer) = BuildViews(carSettings.CarPrefab, carSettings.PointerPrefab, spawnPoint);
            T carModel = BuildModels<T>(car, pointer, carSettings);
            
            _positionedDragInput.Subscribe(carModel, car.transform);
            _updateService.Add(carModel);

            return carModel;
        }

        public void Clear(MovableCarModel carModel)
        {
            _positionedDragInput.Unsubscribe(carModel);
            _updateService.Remove(carModel);
        }

        private (MovableCar, Pointer) BuildViews(
            MovableCar carPrefab,
            Pointer pointerPrefab,
            SpawnPoint spawnPoint
        )
        {
            MovableCar car = GameObject.Instantiate(
                carPrefab, 
                spawnPoint.transform.position,
                spawnPoint.transform.rotation
            );
            
            Pointer pointer = GameObject.Instantiate(
                pointerPrefab,
                car.transform
            );

            return (car, pointer);
        }

        private T BuildModels<T>(
            MovableCar car,
            Pointer pointer,
            MovableCarSettings carSettings
        ) where T: MovableCarModel
        {
            PointerModel pointerModel = new PointerModel(carSettings);
            PointerVM pointerVM = new PointerVM(pointerModel);
            pointer.Construct(pointerVM);

            if (typeof(T) == typeof(MovableCarModel))
            {
                MovableCarModel carModel = new MovableCarModel(carSettings, pointerModel);
                MovableCarVM carVM = new MovableCarVM(carModel);
                car.Construct(carVM);

                return (T) carModel;
            }
            if (typeof(T) == typeof(PlayerCarModel))
            {
                PlayerCarModel carModel = new PlayerCarModel(carSettings, pointerModel);
                PlayerCarVM carVM = new PlayerCarVM(carModel);
                (car as PlayerCar).Construct(carVM);

                return (T)(MovableCarModel) carModel;
            }
            
            throw new NotImplementedException("No such car type");
        }
    }
}