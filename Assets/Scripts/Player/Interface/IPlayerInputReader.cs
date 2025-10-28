using UnityEngine;

namespace Game.Player
{
    public interface IPlayerInputReader
    {
        Vector2 ReadMovement(); // X = strafe, Y = forward
    }
}
