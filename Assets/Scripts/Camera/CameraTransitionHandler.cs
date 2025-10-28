using Game.Player;
using UnityEngine;
using UnityEngine.UI;

namespace Game.CameraSystem
{
    /// <summary>
    /// Отвечает за запуск перехода камеры к голове по нажатию кнопки.
    /// </summary>
    public class CameraTransitionHandler : MonoBehaviour
    {
        [SerializeField] private GameObject followButton;
        [SerializeField] private CameraFollowController cameraController;

        [SerializeField] private GameObject Player;
        private void Awake()
        {
            if (followButton != null)
            {
                followButton.GetComponent<Button>().onClick.AddListener(OnFollowButtonPressed);
            }
            else
                Debug.LogWarning("Follow Button не назначена!");
        }

        private void OnFollowButtonPressed()
        {
            cameraController.StartFollowing();
            Player.GetComponent<PlayerCombatController>().enabled = true;
            Player.GetComponent<PlayerMovementController>().enabled = true;
            followButton.SetActive(false);
        }
    }
}
