using UnityEngine;

public class ClimbableLight : MonoBehaviour
{
    private CircleCollider2D auraCollider;
    public bool isClimbable = false;

    private void Awake()
    {
        auraCollider = GetComponent<CircleCollider2D>();
        if (auraCollider == null)
        {
            Debug.LogError("Circle Collider 2D not found on ClimbableLight.");
        }
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
