using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerCombatController : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private Animator animator;
    [SerializeField] private AttackExecutor executor;
    [SerializeField] private List<AttackData> comboSequence = new List<AttackData>();

    [Header("Combo Settings")]
    [SerializeField] private float comboInputWindow = 0.8f; // допустимое время для следующего удара
    [SerializeField] private int maxComboStep = 3;

    private int currentStep = 0;
    private bool canQueueNext = false;
    private float lastAttackEndTime = -10f;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
        {
            OnAttackInput();
        }
    }

    private void OnAttackInput()
    {
        // Если текущее окно позволяет продолжить комбо
        if (canQueueNext)
        {
            canQueueNext = false;
            StartCoroutine(PerformAttackAtIndex(Mathf.Clamp(currentStep + 1, 0, comboSequence.Count - 1)));
        }
        // Если комбо сброшено — начинаем сначала
        else if (Time.time - lastAttackEndTime > comboInputWindow)
        {
            StartCoroutine(PerformAttackAtIndex(0));
        }
    }

    private IEnumerator PerformAttackAtIndex(int index)
    {
        if (index < 0 || index >= comboSequence.Count) yield break;
        var attack = comboSequence[index];

        currentStep = index;
        lastAttackEndTime = Time.time;
        canQueueNext = false;

        // Триггер анимации
        if (!string.IsNullOrEmpty(attack.animationTrigger))
        {
            animator.SetTrigger(attack.animationTrigger);
        }

        // Выполняем атаку
        executor.ExecuteAttack(attack);

        float t = 0f;
        float duration = attack.duration;
        float queueWindowStart = duration * 0.5f; // окно для следующей атаки
        float queueWindowEnd = duration * 0.9f;

        // Время анимации
        while (t < duration)
        {
            t += Time.deltaTime;

            if (t >= queueWindowStart && t <= queueWindowEnd)
            {
                canQueueNext = true;
            }
            yield return null;
        }

        canQueueNext = false;
        lastAttackEndTime = Time.time;
    }

    public void ExecuteAttackByIndex(int idx)
    {
        StartCoroutine(PerformAttackAtIndex(idx));
    }
}
