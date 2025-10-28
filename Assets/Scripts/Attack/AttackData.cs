using UnityEngine;

[CreateAssetMenu(menuName = "Combat/Attack Data")]
public class AttackData : ScriptableObject
{
    [Header("Animation")]
    public string animationTrigger; // имя триггера или state (например "Punch_L")

    [Header("Timing (seconds)")]
    public float duration = 1.4f;          // общая длительность анимации / атаки
    public float hitWindowStart = 0.5f;   // момент начала активного окна удара (сек)
    public float hitWindowEnd = 0.9f;     // конец окна удара (сек)

    [Header("Combat")]
    public float damage = 20f;
    public float hitForce = 5f;

    [Header("VFX / SFX")]
    public GameObject hitVfxPrefab;
    public AudioClip hitSfx;
}
