using UnityEngine;

public class PlayerCameraController : MonoBehaviour
{
    [SerializeField] public Transform player;
    [Space]
    [SerializeField] private Vector3 cameraOffsetPosition;
    [SerializeField] private Vector3 cameraOffsetRotation;

    private Transform cameraTransform;

    private void Awake() => cameraTransform = transform;

    private void LateUpdate()
    {
        cameraTransform.position = player.position + cameraOffsetPosition;
        cameraTransform.rotation = Quaternion.Euler(cameraOffsetRotation);
    }
}