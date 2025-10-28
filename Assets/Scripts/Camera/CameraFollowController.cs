using UnityEngine;

namespace Game.CameraSystem
{
    [RequireComponent(typeof(Camera))]
    public class CameraFollowController : MonoBehaviour
    {
        [SerializeField] private MonoBehaviour targetProviderBehaviour;
        [SerializeField] private SmoothLerpMover mover = new SmoothLerpMover();

        private ICameraTargetProvider targetProvider;
        private Transform camTransform;
        private bool isFollowing;

        private void Awake()
        {
            camTransform = transform;

            if (targetProviderBehaviour is ICameraTargetProvider provider)
                targetProvider = provider;
            else
                Debug.LogError($"{targetProviderBehaviour.name} не реализует ICameraTargetProvider!");
        }

        private void LateUpdate()
        {
            if (!isFollowing || targetProvider == null) return;
            if (!(targetProviderBehaviour is PlayerHeadTargetProvider playerHead)) return;

            Vector3 targetPos = playerHead.GetCameraPosition();
            Quaternion targetRot = playerHead.GetCameraRotation();

            camTransform.position = mover.Move(camTransform.position, targetPos, Time.deltaTime);
            camTransform.rotation = mover.Rotate(camTransform.rotation, targetRot, Time.deltaTime);
        }

        public void StartFollowing()
        {
            isFollowing = true;
        }
    }
}
