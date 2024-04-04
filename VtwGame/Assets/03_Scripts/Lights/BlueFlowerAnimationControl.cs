using UnityEngine;

public class BlueFlowerAnimationControl : MonoBehaviour
{
    private Animator animator;
    public float checkDistance = 1.0f;
    private LayerMask layerMask;
    public Material activatedMaterial;
    public Material originalMaterial;
    private SpriteRenderer spriteRenderer; 

    private void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>(); 
        layerMask = (1 << LayerMask.NameToLayer("playableground")) | (1 << LayerMask.NameToLayer("bouncy"));
        originalMaterial = spriteRenderer.material;
    }

    private void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, checkDistance, layerMask);
        bool isBouncy = hit.collider != null && hit.collider.gameObject.layer == LayerMask.NameToLayer("bouncy");
        animator.SetBool("isActivated", isBouncy);
        animator.SetBool("isDeactivated", !isBouncy);
        spriteRenderer.material = isBouncy ? activatedMaterial : originalMaterial;
    }
}
