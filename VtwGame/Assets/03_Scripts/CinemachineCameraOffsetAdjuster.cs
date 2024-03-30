using Cinemachine;
using UnityEngine;

public class CinemachineCameraOffsetAdjuster : MonoBehaviour
{
    public CinemachineVirtualCamera cinemachineCamera;
    public Transform playerTransform;
    public Rigidbody2D playerRigidbody;
    public float smoothTime = 0.5f; // Dauer des Übergangs für den Offset
    public float maxFallingOffsetY = -5f; // Maximale zusätzliche Offset-Anpassung nach unten, wenn der Spieler fällt
    public float maxFallSpeed = -10f; // Geschwindigkeit, bei der der maximale Offset erreicht wird
    public float fallDelay = 0.4f; // Verzögerung, bevor der Offset angepasst wird
    public float fixedHorizontalOffset = 1f; // Fester horizontaler Offset

    private Vector3 offsetVelocity = Vector3.zero; // Wird von SmoothDamp verwendet für Offset-Anpassungen
    private float originalOffsetY; // Ursprünglicher vertikaler Offset
    private float fallingTime = 0; // Wie lange der Spieler schon fällt
    private bool isFalling = false; // Ob der Spieler fällt

    private void Start()
    {
        // Initialisiere den ursprünglichen vertikalen Offset
        originalOffsetY = cinemachineCamera.GetCinemachineComponent<CinemachineFramingTransposer>().m_TrackedObjectOffset.y;
    }

    private void Update()
    {
        CheckFalling();
        AdjustOffsets();
    }

    void CheckFalling()
    {
        if (playerRigidbody.velocity.y < 0)
        {
            if (!isFalling)
            {
                isFalling = true;
                fallingTime = 0;
            }
            fallingTime += Time.deltaTime;
        }
        else
        {
            if (isFalling)
            {
                isFalling = false;
                fallingTime = 0;
            }
        }
    }

    void AdjustOffsets()
    {
        Vector3 currentOffset = cinemachineCamera.GetCinemachineComponent<CinemachineFramingTransposer>().m_TrackedObjectOffset;
        float targetOffsetX = playerTransform.localScale.x > 0 ? fixedHorizontalOffset : -fixedHorizontalOffset;
        float targetOffsetY = originalOffsetY;

        if (isFalling && fallingTime >= fallDelay)
        {
            float speedFactor = Mathf.InverseLerp(0, maxFallSpeed, playerRigidbody.velocity.y);
            targetOffsetY = Mathf.Lerp(originalOffsetY, originalOffsetY + maxFallingOffsetY, speedFactor);
        }

        // Anwenden von SmoothDamp für sanften Übergang zum neuen Offset
        Vector3 targetOffset = new Vector3(targetOffsetX, targetOffsetY, currentOffset.z);
        Vector3 newOffset = Vector3.SmoothDamp(currentOffset, targetOffset, ref offsetVelocity, smoothTime);

        cinemachineCamera.GetCinemachineComponent<CinemachineFramingTransposer>().m_TrackedObjectOffset = newOffset;
    }
}
