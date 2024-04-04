using System.Collections;
using UnityEngine;

public class GreenPinkFlowerAnimationControl : MonoBehaviour
{
    public Transform checkPoint;
    public LayerMask climbableLayer;
    public LayerMask passableLayer;
    public Material climbableMaterial;
    public Material passableMaterial;
    public Material pinkGreenGlowMaterial;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private bool previousClimbable = false;
    private bool previousPassable = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.material = pinkGreenGlowMaterial;
    }

    private void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast(checkPoint.position, transform.right, 0.1f, climbableLayer | passableLayer);

        bool isClimbable = hit.collider != null && ((1 << hit.collider.gameObject.layer) & climbableLayer) != 0;
        bool isPassable = hit.collider != null && ((1 << hit.collider.gameObject.layer) & passableLayer) != 0;

        if ((isClimbable != previousClimbable) || (isPassable != previousPassable))
        {
            if (isClimbable || isPassable)
            {
                animator.SetBool("isDeactivated", false);
                animator.SetBool("isClimbable", isClimbable);
                animator.SetBool("isPassable", isPassable);
                Material targetMaterial = isClimbable ? climbableMaterial : passableMaterial;
                if (spriteRenderer.material != targetMaterial)
                {
                    StartCoroutine(SetMaterialAfterAnimation(targetMaterial));
                }
            }
            else
            {
                StartCoroutine(ChangeToIdleState());
            }
        }

        previousClimbable = isClimbable;
        previousPassable = isPassable;
    }

    IEnumerator SetMaterialAfterAnimation(Material newMaterial)
    {
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.material = newMaterial;
    }

    IEnumerator ChangeToIdleState()
    {
        animator.SetBool("isClimbable", false);
        animator.SetBool("isPassable", false);
        animator.SetBool("isDeactivated", true);

        yield return new WaitForEndOfFrame();
        yield return new WaitForSeconds(0.5f);

        spriteRenderer.material = pinkGreenGlowMaterial;
    }
}
