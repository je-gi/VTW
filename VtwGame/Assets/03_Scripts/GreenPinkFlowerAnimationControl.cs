using System.Collections;
using UnityEngine;

public class GreenPinkFlowerAnimationControl : MonoBehaviour
{
    public Transform checkPoint;
    public LayerMask climbableLayer;
    public LayerMask passableLayer;
    public Material climbableMaterial;
    public Material passableMaterial;
    public Material pinkGreenGlowMaterial; // Standardmaterial für den Idle-Zustand
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private bool previousClimbable = false;
    private bool previousPassable = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.material = pinkGreenGlowMaterial; // Setze das Startmaterial
    }

    private void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast(checkPoint.position, transform.right, 0.1f, climbableLayer | passableLayer);

        bool isClimbable = hit.collider != null && ((1 << hit.collider.gameObject.layer) & climbableLayer) != 0;
        bool isPassable = hit.collider != null && ((1 << hit.collider.gameObject.layer) & passableLayer) != 0;

        // Zustandsänderungen überprüfen
        if ((isClimbable != previousClimbable) || (isPassable != previousPassable))
        {
            if (isClimbable || isPassable)
            {
                // Setze den entsprechenden Zustand und das Material, wenn sich der Zustand ändert
                animator.SetBool("isDeactivated", false);
                animator.SetBool("isClimbable", isClimbable);
                animator.SetBool("isPassable", isPassable);
                Material targetMaterial = isClimbable ? climbableMaterial : passableMaterial;
                if (spriteRenderer.material != targetMaterial) // Überprüfen, ob das Material gewechselt werden muss
                {
                    StartCoroutine(SetMaterialAfterAnimation(targetMaterial));
                }
            }
            else
            {
                StartCoroutine(ChangeToIdleState());
            }
        }

        // Aktualisiere die vorherigen Zustände für den nächsten Durchlauf
        previousClimbable = isClimbable;
        previousPassable = isPassable;
    }

    IEnumerator SetMaterialAfterAnimation(Material newMaterial)
    {
        yield return new WaitForSeconds(0.1f); // Kurze Verzögerung
        spriteRenderer.material = newMaterial;
    }

    IEnumerator ChangeToIdleState()
    {
        animator.SetBool("isClimbable", false);
        animator.SetBool("isPassable", false);
        animator.SetBool("isDeactivated", true);

        yield return new WaitForEndOfFrame();
        yield return new WaitForSeconds(0.5f); // Wartezeit, bis die Deaktivierungsanimation beginnt

        spriteRenderer.material = pinkGreenGlowMaterial;
    }
}
