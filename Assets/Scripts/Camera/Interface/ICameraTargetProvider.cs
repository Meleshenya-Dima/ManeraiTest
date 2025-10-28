using UnityEngine;

namespace Game.CameraSystem
{
    public interface ICameraTargetProvider
    {
        Transform TargetTransform { get; }
    }
}
