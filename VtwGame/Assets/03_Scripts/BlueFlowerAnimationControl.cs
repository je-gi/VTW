using UnityEngine;

public class BlueFlowerAnimationControl : MonoBehaviour
{
    private Animator animator;
    public float checkDistance = 1.0f;
    private LayerMask layerMask;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        layerMask = (1 << LayerMask.NameToLayer("playableground")) | (1 << LayerMask.NameToLayer("bouncy"));
    }

    private void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, checkDistance, layerMask);
        Debug.DrawLine(transform.position, transform.position + Vector3.down * checkDistance, Color.red);

        if (hit.collider != null)
        {
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("bouncy"))
            {
                animator.SetBool("isActivated", true);
                animator.SetBool("isDeactivated", false);
            }
            else if (hit.collider.gameObject.layer == LayerMask.NameToLayer("playableground"))
            {
                animator.SetBool("isActivated", false);
                animator.SetBool("isDeactivated", true);
            }
        }
        else
        {
            animator.SetBool("isActivated", false);
            animator.SetBool("isDeactivated", true);
        }
    }
}
