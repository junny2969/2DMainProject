using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform Transform_Player;
    [SerializeField] private Vector3 _offset = new Vector3(0f, 1f, -10f);
    [SerializeField] private float _smoothSpeed = 5f;

    private void LateUpdate()
    {
        if (transform == null) return;

        Vector3 targetPosition = Transform_Player.position + _offset;
        transform.position = Vector3.Lerp(transform.position, targetPosition, _smoothSpeed);
    }
}
