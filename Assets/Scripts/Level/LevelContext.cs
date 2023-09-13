using System;
using System.Collections.Generic;
using CarPool.Entities.Views;
using CarPool.Entities.Views.Cars;
using Ji2.Context;
using UnityEngine;

namespace CarPool.Level
{
    public class LevelContext : SceneInstaller
    {
        [SerializeField] private Finish finish;
        [SerializeField] private Coin[] coins;
        [SerializeField] private StaticCar[] staticCars;
        [SerializeField] private SpawnPoint[] spawnPoints;

        protected override IEnumerable<(Type type, object obj)> GetDependencies() => new List<(Type, object)>
        {
            (typeof(Finish), finish),
            (typeof(Coin[]), coins),
            (typeof(StaticCar[]), staticCars),
            (typeof(SpawnPoint[]), spawnPoints)
        };
    }
}