using UnityEngine;

public class ClimbableLight : MonoBehaviour
{
    public bool isClimbable = false;
    public AudioClip LightCollisionSound;
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public bool IsClimbable()
    {
        return isClimbable;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("playablewall"))
        {
            collision.gameObject.layer = LayerMask.NameToLayer("climbable");
            isClimbable = true;
            Debug.Log($"Made {collision.gameObject.name} climbable.");
            audioSource.PlayOneShot(LightCollisionSound);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("climbable"))
        {
            collision.gameObject.layer = LayerMask.NameToLayer("playablewall");
            isClimbable = false;
            Debug.Log($"Made {collision.gameObject.name} not climbable.");
        }
    }
}
