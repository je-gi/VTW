using UnityEngine;

public class PassableLight : MonoBehaviour

{
    public AudioClip LightCollisionSound;
    private AudioSource audioSource;

    private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
        }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("playablewall"))
        {
            collision.gameObject.layer = LayerMask.NameToLayer("passable");
            Debug.Log($"Made {collision.gameObject.name} passable.");
            audioSource.PlayOneShot(LightCollisionSound);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("passable"))
        {
            collision.gameObject.layer = LayerMask.NameToLayer("playablewall");
            Debug.Log($"Made {collision.gameObject.name} not passable.");
        }
    }
}
