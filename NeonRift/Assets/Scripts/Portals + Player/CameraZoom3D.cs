using UnityEngine;

public class CameraZoom3D : MonoBehaviour
{
    private Camera mainCamera;

    [Header("Zoom Settings")]
    [SerializeField] private float minFOV = 15f;
    [SerializeField] private float maxFOV = 60f;
    [SerializeField] private float zoomSensitivity = 10f;
    [SerializeField] private float smoothSpeed = 10f;

    private float targetFOV;

    void Start()
    {
        mainCamera = Camera.main;
        targetFOV = mainCamera.fieldOfView;
    }

    void Update()
    {
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");

        // Calculate new target FOV (subtracting zooms in when scrolling up)
        targetFOV -= scrollInput * zoomSensitivity;
        targetFOV = Mathf.Clamp(targetFOV, minFOV, maxFOV);

        // Smoothly interpolate to the target FOV
        mainCamera.fieldOfView = Mathf.Lerp(mainCamera.fieldOfView, targetFOV, Time.deltaTime * smoothSpeed);
    }
}