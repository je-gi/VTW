using UnityEngine;
using System.Collections;

public class FlowerAnimationControl : MonoBehaviour
{
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bouncy"))
        {
            animator.SetBool("isActivated", true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Bouncy"))
        {
            StartCoroutine(WaitBeforeDeactivating());
        }
    }

    IEnumerator WaitBeforeDeactivating()
    {
        // Wartet für 0.2 Sekunden, um die Glowing Animation noch kurz weiterlaufen zu lassen
        yield return new WaitForSeconds(0.5f);
        animator.SetBool("isActivated", false);
        animator.Play("Deactivated");
    }
}
