using UnityEngine;

[CreateAssetMenu(menuName = "Combat/Attack Data")]
public class AttackData : ScriptableObject
{
    [Header("Animation")]
    public string animationTrigger; // ��� �������� ��� state (�������� "Punch_L")

    [Header("Timing (seconds)")]
    public float duration = 1.4f;          // ����� ������������ �������� / �����
    public float hitWindowStart = 0.5f;   // ������ ������ ��������� ���� ����� (���)
    public float hitWindowEnd = 0.9f;     // ����� ���� ����� (���)

    [Header("Combat")]
    public float damage = 20f;
    public float hitForce = 5f;

    [Header("VFX / SFX")]
    public GameObject hitVfxPrefab;
    public AudioClip hitSfx;
}
