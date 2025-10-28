using UnityEngine;

namespace Game.Player
{
    [RequireComponent(typeof(Animator))]
    public class PlayerAnimatorController : MonoBehaviour
    {
        [SerializeField] private string speedParam = "Speed";
        private Animator animator;

        private void Awake()
        {
            animator = GetComponent<Animator>();
        }

        public void UpdateAnimation(float speed)
        {
            animator.SetFloat(speedParam, speed, 0f, Time.deltaTime);
        }
    }
}
