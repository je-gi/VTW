using UnityEngine;

public class BouncyLight : MonoBehaviour
{
    public AudioClip LightCollisionSound;
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("playableground"))
        {
            collision.gameObject.layer = LayerMask.NameToLayer("bouncy");
            Debug.Log($"Made {collision.gameObject.name} bouncy.");
            audioSource.PlayOneShot(LightCollisionSound);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("bouncy"))
        {
            collision.gameObject.layer = LayerMask.NameToLayer("playableground");
            Debug.Log($"Made {collision.gameObject.name} not bouncy.");
        }
    }
}