using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class HeadReaction : MonoBehaviour, IHitReceiver
{
    private Mesh mesh;
    private Vector3[] originalVertices;
    private Vector3[] deformedVertices;
    private bool isRecovering = false;

    private Renderer rend;
    private Color originalColor;
    private Coroutine colorRecoverRoutine;

    [Header("Deformation Settings")]
    public float deformationRadius = 0.15f;
    public float deformationStrength = 0.05f;
    public float recoverySpeed = 3f;

    [Header("Redness Settings")]
    public float maxRed = 1f;             // Насколько красный
    public float colorRecoverySpeed = 2f; // Скорость возврата белого

    void Start()
    {
        // создаем рабочую копию меша
        mesh = Instantiate(GetComponent<MeshFilter>().sharedMesh);
        GetComponent<MeshFilter>().mesh = mesh;

        originalVertices = mesh.vertices;
        deformedVertices = mesh.vertices;

        // Renderer для изменения цвета материала
        rend = GetComponent<MeshRenderer>();
        originalColor = rend.material.color;
    }

    public void ReceiveHit(Vector3 hitPoint, Vector3 direction, float damage, float force)
    {
        Debug.Log($"Head got hit! Damage: {damage}");

        // --- Деформация вершин ---
        Vector3 localHitPoint = transform.worldToLocalMatrix.MultiplyPoint3x4(hitPoint);
        Vector3 localDirection = transform.worldToLocalMatrix.MultiplyVector(direction.normalized);

        int affected = 0;

        // Обновляем вершины частями, чтобы не загружать процесс
        for (int i = 0; i < deformedVertices.Length; i++)
        {
            float dist = Vector3.Distance(deformedVertices[i], localHitPoint);
            if (dist < deformationRadius)
            {
                affected++;
                float falloff = Mathf.Pow(1 - dist / deformationRadius, 2f);
                Vector3 dirFromHit = (deformedVertices[i] - localHitPoint).normalized;
                Vector3 offset = dirFromHit * deformationStrength * falloff;
                deformedVertices[i] += offset;
            }
        }

        Debug.Log($"Vertices affected: {affected}");

        mesh.vertices = deformedVertices;
        mesh.MarkDynamic();  // Маркируем как динамичный для ускорения обновлений (не всегда необходимо, но может помочь).

        // Перерасчет нормалей и границ только после окончания деформации
        if (!isRecovering)
            StartCoroutine(RecoverShape());

        // --- Красим весь меш в красный через материал ---
        rend.material.color = Color.red * maxRed;

        if (colorRecoverRoutine != null)
            StopCoroutine(colorRecoverRoutine);
        colorRecoverRoutine = StartCoroutine(RecoverColor());
    }

    private IEnumerator RecoverShape()
    {
        isRecovering = true;
        float t = 0f;

        while (t < 1f)
        {
            // Обновляем вершины частями для уменьшения нагрузки на систему
            for (int i = 0; i < deformedVertices.Length; i++)
            {
                deformedVertices[i] = Vector3.Lerp(deformedVertices[i], originalVertices[i], Time.deltaTime * recoverySpeed);
            }

            mesh.vertices = deformedVertices;
            mesh.MarkDynamic();  // Снова маркеруем как динамичный после изменений.

            // Перерасчет нормалей и границ только по завершению
            if (t >= 1f)
            {
                mesh.RecalculateNormals();
                mesh.RecalculateBounds();
            }

            t += Time.deltaTime;
            yield return null;
        }

        isRecovering = false;
    }

    private IEnumerator RecoverColor()
    {
        float t = 0f;
        while (t < 1f)
        {
            rend.material.color = Color.Lerp(rend.material.color, originalColor, Time.deltaTime * colorRecoverySpeed);
            t += Time.deltaTime;
            yield return null;
        }
        rend.material.color = originalColor;
    }
}
