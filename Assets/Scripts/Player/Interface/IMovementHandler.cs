using UnityEngine;

namespace Game.Player
{
    public interface IMovementHandler
    {
        void Move(Vector3 direction, float deltaTime);
        float CurrentSpeed { get; }
    }
}
