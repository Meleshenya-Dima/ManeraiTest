using UnityEngine;

public interface IHitReceiver
{
    void ReceiveHit(Vector3 hitPoint, Vector3 hitDirection, float damage, float force);
}
