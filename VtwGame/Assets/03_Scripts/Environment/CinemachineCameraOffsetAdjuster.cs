using Cinemachine;
using UnityEngine;

public class CinemachineCameraOffsetAdjuster : MonoBehaviour
{
    public CinemachineVirtualCamera cinemachineCamera;
    public Transform playerTransform;
    public Rigidbody2D playerRigidbody;
    public float smoothTime = 0.5f;
    public float maxFallingOffsetY = -5f;
    public float maxFallSpeed = -10f;
    public float fallDelay = 0.4f;
    public float fixedHorizontalOffset = 1f;

    private Vector3 offsetVelocity = Vector3.zero;
    private float originalOffsetY;
    private float fallingTime = 0;
    private bool isFalling = false;

    private void Start()
    {
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

        Vector3 targetOffset = new Vector3(targetOffsetX, targetOffsetY, currentOffset.z);
        Vector3 newOffset = Vector3.SmoothDamp(currentOffset, targetOffset, ref offsetVelocity, smoothTime);

        cinemachineCamera.GetCinemachineComponent<CinemachineFramingTransposer>().m_TrackedObjectOffset = newOffset;
    }
}
