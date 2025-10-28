using UnityEngine;

namespace Game.CameraSystem
{

    public class PlayerHeadTargetProvider : MonoBehaviour, ICameraTargetProvider
    {
        [SerializeField] private Transform headTransform;
        [SerializeField] private Vector3 cameraOffset = new Vector3(0f, 0.05f, -0.08f);

        public Transform TargetTransform => headTransform;

        /// <summary>
        /// ���������� �������� ������� ������ � ������ ��������.
        /// </summary>
        public Vector3 GetCameraPosition()
        {
            return headTransform.TransformPoint(cameraOffset);
        }

        /// <summary>
        /// ���������� ���������� ������ (���� ������� �����).
        /// </summary>
        public Quaternion GetCameraRotation()
        {
            return headTransform.rotation;
        }
    }
}
