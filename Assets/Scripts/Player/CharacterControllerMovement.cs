using UnityEngine;

namespace Game.Player
{
    public class CharacterControllerMovement : MonoBehaviour, IMovementHandler
    {
        [Header("Movement Settings")]
        [SerializeField] private float moveSpeed = 3f;
        [SerializeField] private float backwardSpeed = 2f;
        [SerializeField] private float rotationSpeed = 120f;

        private CharacterController controller;
        private Transform playerTransform;

        public float CurrentSpeed { get; private set; }

        private void Awake()
        {
            controller = GetComponent<CharacterController>();
            playerTransform = transform;
        }

        public void Move(Vector3 direction, float deltaTime)
        {
            float turnInput = Mathf.Clamp(direction.x, -1f, 1f);
            float forwardInput = Mathf.Clamp(direction.z, -1f, 1f);

            // 🔁 Поворот
            if (Mathf.Abs(turnInput) > 0.1f)
            {
                float turn = turnInput * rotationSpeed * deltaTime;
                playerTransform.Rotate(Vector3.up, turn);
            }

            // 🚶‍♂️ Движение только при вводе
            if (Mathf.Abs(forwardInput) > 0.01f)
            {
                float currentMoveSpeed = forwardInput > 0 ? moveSpeed : backwardSpeed;
                Vector3 moveDir = playerTransform.forward * forwardInput;
                controller.Move(moveDir * currentMoveSpeed * deltaTime);
                CurrentSpeed = Mathf.Abs(forwardInput) * currentMoveSpeed;
            }
            else
            {
                // ❌ Полная остановка — не вызываем Move вообще
                CurrentSpeed = 0f;
            }

            // 💀 Защита: если контроллер накопил остаточное движение, обнуляем его вручную
            if (!controller.isGrounded)
            {
                controller.Move(Vector3.down * 0.05f); // легкий “прижим” вниз
            }
        }
    }
}
