using UnityEngine;

public class BouncyLight : MonoBehaviour
{
    private CircleCollider2D auraCollider;

    private void Awake()
    {
        auraCollider = GetComponent<CircleCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("playableground"))
        {
            collision.gameObject.layer = LayerMask.NameToLayer("bouncy");
            Debug.Log($"Made {collision.gameObject.name} bouncy.");
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