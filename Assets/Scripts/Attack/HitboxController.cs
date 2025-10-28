using UnityEngine;
using System;
using System.Collections.Generic;

[RequireComponent(typeof(Collider))]
public class HitboxController : MonoBehaviour
{
    public event Action<Vector3, Vector3, Collider> OnHit; // point, dir, target collider
    [SerializeField] private Collider hitCollider;
    [SerializeField] private LayerMask hittableLayers;

    private readonly HashSet<Collider> alreadyHit = new HashSet<Collider>();
    private bool isActive = false;
    private Vector3 lastHitPos;

    private void Reset()
    {
        hitCollider = GetComponent<Collider>();
        hitCollider.isTrigger = true;
    }

    private void Awake()
    {
        if (hitCollider == null) hitCollider = GetComponent<Collider>();
        hitCollider.isTrigger = true;
        DisableHitbox();
    }

    public void EnableHitbox()
    {
        alreadyHit.Clear();
        hitCollider.enabled = true;
        isActive = true;
    }

    public void DisableHitbox()
    {
        hitCollider.enabled = false;
        isActive = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & hittableLayers) == 0) return;
        if (alreadyHit.Contains(other)) return;

        alreadyHit.Add(other);
        lastHitPos = other.ClosestPoint(transform.position);

        Vector3 dir = transform.forward;
        OnHit?.Invoke(lastHitPos, dir, other);
    }

    private void OnDrawGizmos()
    {
        if (!isActive) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 0.15f);
        if (lastHitPos != Vector3.zero)
        {
            Gizmos.DrawLine(transform.position, lastHitPos);
        }
    }
}
