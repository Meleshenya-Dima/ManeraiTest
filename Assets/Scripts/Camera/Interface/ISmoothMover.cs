using UnityEngine;

namespace Game.CameraSystem
{
    public interface ISmoothMover
    {
        Vector3 Move(Vector3 current, Vector3 target, float deltaTime);
        Quaternion Rotate(Quaternion current, Quaternion target, float deltaTime);
    }
}
