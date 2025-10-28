using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackExecutor : MonoBehaviour
{
    [SerializeField] private HitboxController[] hitboxes;
    [SerializeField] private Transform hitVfxParent;
    [SerializeField] private AudioSource audioSource;

    private AttackData currentAttack;
    private Coroutine attackRoutine;

    // ����������, �� ����� ������ ��� ������ ����
    private readonly HashSet<IHitReceiver> damagedReceivers = new HashSet<IHitReceiver>();

    public void ExecuteAttack(AttackData attack)
    {
        if (attackRoutine != null)
            StopCoroutine(attackRoutine);

        currentAttack = attack;
        attackRoutine = StartCoroutine(RunAttack());
    }

    private IEnumerator RunAttack()
    {
        if (currentAttack == null) yield break;

        float start = currentAttack.hitWindowStart;
        float end = currentAttack.hitWindowEnd;
        float duration = currentAttack.duration;
        float t = 0f;

        damagedReceivers.Clear();

        // ���� ������ ��������� ����
        while (t < start)
        {
            t += Time.deltaTime;
            yield return null;
        }

        // �������� ��� ��������
        foreach (var hitbox in hitboxes)
        {
            hitbox.OnHit += OnHit;
            hitbox.EnableHitbox();
        }

        // �������� ����
        while (t < end)
        {
            t += Time.deltaTime;
            yield return null;
        }

        // ��������� ��������
        foreach (var hitbox in hitboxes)
        {
            hitbox.OnHit -= OnHit;
            hitbox.DisableHitbox();
        }

        // ��� ����� ��������
        while (t < duration)
        {
            t += Time.deltaTime;
            yield return null;
        }

        attackRoutine = null;
    }

    private void OnHit(Vector3 hitPoint, Vector3 dir, Collider other)
    {
        if (currentAttack == null) return;

        // �������� �� ��������� ���� �� ���� �� ����������
        var hitReceiver = other.GetComponentInParent<IHitReceiver>();
        if (hitReceiver == null || damagedReceivers.Contains(hitReceiver)) return;
        damagedReceivers.Add(hitReceiver);

        // VFX
        if (currentAttack.hitVfxPrefab != null)
        {
            var vfx = Instantiate(currentAttack.hitVfxPrefab, hitPoint, Quaternion.LookRotation(dir));
            var target = other.transform;

            if (target != null)
            {
                // ��������� ���������, ������� �������� VFX ��������� ������ ����
                var rotator = vfx.AddComponent<VfxOrbit>();
                rotator.target = target;
                rotator.rotationSpeed = 120f; // �������� �������� (�������/���)
                rotator.orbitRadius = 1f;   // ������ ��������
            }
            Destroy(vfx, 3f);
        }

        // SFX
        if (currentAttack.hitSfx != null && audioSource != null)
        {
            audioSource.PlayOneShot(currentAttack.hitSfx);
        }

        // ����
        hitReceiver.ReceiveHit(hitPoint, dir, currentAttack.damage, currentAttack.hitForce);
    }
}
