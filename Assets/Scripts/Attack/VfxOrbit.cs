using UnityEngine;

public class VfxOrbit : MonoBehaviour
{
    public Transform target;
    public float rotationSpeed = 90f;
    public float orbitRadius = 1f;

    private float angle = 0f;

    private void Update()
    {
        if (target == null) return;

        angle += rotationSpeed * Time.deltaTime;
        if (angle > 360f) angle -= 360f;

        // ������������ ����� ������� �� ����������
        Vector3 offset = new Vector3(
            Mathf.Cos(angle * Mathf.Deg2Rad) * orbitRadius,
            0f,
            Mathf.Sin(angle * Mathf.Deg2Rad) * orbitRadius
        );

        transform.position = target.position + offset;
        transform.LookAt(target.position);
    }
}
