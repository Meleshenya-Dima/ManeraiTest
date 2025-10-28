using UnityEngine;

namespace Game.Player
{
    public class PlayerMovementController : MonoBehaviour
    {
        [Header("Dependencies")]
        [SerializeField] private MonoBehaviour inputReaderBehaviour;
        [SerializeField] private MonoBehaviour movementHandlerBehaviour;
        [SerializeField] private PlayerAnimatorController animatorController;

        private IPlayerInputReader inputReader;
        private IMovementHandler movementHandler;

        private void Awake()
        {
            if (inputReaderBehaviour is IPlayerInputReader reader)
                inputReader = reader;
            else
                Debug.LogError("InputReader не реализует IPlayerInputReader");

            if (movementHandlerBehaviour is IMovementHandler mover)
                movementHandler = mover;
            else
                Debug.LogError("MovementHandler не реализует IMovementHandler");
        }

        private void Update()
        {
            // 1️⃣ Получаем вход от клавиатуры / джойстика
            Vector2 input = inputReader.ReadMovement();

            // 2️⃣ Формируем локальный вектор: x = поворот (A/D), z = вперед/назад (W/S)
            Vector3 moveDir = new Vector3(input.x, 0f, input.y);

            // 3️⃣ Отправляем в движение
            movementHandler.Move(moveDir, Time.deltaTime);

            // 4️⃣ Обновляем анимацию
            animatorController.UpdateAnimation(movementHandler.CurrentSpeed);

            // 5️⃣ Debug для проверки входа (удалить после теста)
        }
    }
}
