using System;
using Ji2.CommonCore;
using Ji2Core.Core;
using UnityEngine;

namespace CarPool.Tools
{
    public class TrackingCamera : ILateUpdatable
    {
        private readonly CameraProvider _cameraProvider;
        private readonly UpdateService _updateService;

        private Transform _tracking;
        private TrackSettings _trackSettings;
        private Vector3 _positionOffset;
        private Quaternion _rotationOffset;

        public TrackingCamera(
            CameraProvider cameraProvider,
            UpdateService updateService
        )
        {
            _cameraProvider = cameraProvider;
            _updateService = updateService;
            
            _updateService.Add(this);
        }

        public void SetTrackingObject(Transform tracking, TrackSettings trackSettings)
        {
            _tracking = tracking;
            _trackSettings = trackSettings;

            var cameraTransform = _cameraProvider.MainCamera.transform;
            
            _positionOffset = tracking.position - cameraTransform.position;
            _rotationOffset = cameraTransform.rotation * Quaternion.Inverse(tracking.rotation);
        }

        void ILateUpdatable.OnLateUpdate()
        {
            if (_tracking is null || _trackSettings is TrackSettings.None)
                return;

            var cameraTransform = _cameraProvider.MainCamera.transform;
            
            if ((_trackSettings & TrackSettings.MoveWithTracking) != 0)
            {
                cameraTransform.position = _tracking.position - _positionOffset;
            }

            if ((_trackSettings & TrackSettings.RotateWithTracking) != 0)
            {
                // Should be checked for correctness
                cameraTransform.rotation = _tracking.rotation * _rotationOffset;
            }

            if ((_trackSettings & TrackSettings.LookAtTracking) != 0)
            {
                cameraTransform.LookAt(_tracking);
            }
        }

        public void Clear()
        {
            _tracking = null;
            _trackSettings = TrackSettings.None;
        }
    }

    [Flags]
    public enum TrackSettings
    {
        None = 0,
        MoveWithTracking = 1,
        RotateWithTracking = 2,
        LookAtTracking = 4
    }
}