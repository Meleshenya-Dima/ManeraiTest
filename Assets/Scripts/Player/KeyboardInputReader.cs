using UnityEngine;

namespace Game.Player
{
    public class KeyboardInputReader : MonoBehaviour, IPlayerInputReader
    {
        public Vector2 ReadMovement()
        {
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");
            return new Vector2(horizontal, vertical);
        }
    }
}
