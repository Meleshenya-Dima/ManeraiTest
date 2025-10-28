using UnityEngine;

namespace Game.CameraSystem
{
    [System.Serializable]
    public class SmoothLerpMover : ISmoothMover
    {
        [Range(0.01f, 20f)]
        [SerializeField] private float positionLerpSpeed = 10f;

        [Range(0.01f, 20f)]
        [SerializeField] private float rotationLerpSpeed = 10f;

        public Vector3 Move(Vector3 current, Vector3 target, float deltaTime)
        {
            return Vector3.Lerp(current, target, deltaTime * positionLerpSpeed);
        }

        public Quaternion Rotate(Quaternion current, Quaternion target, float deltaTime)
        {
            return Quaternion.Slerp(current, target, deltaTime * rotationLerpSpeed);
        }
    }
}
