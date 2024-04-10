using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public BoxCollider2D trigger;
    private Animator animator;
    public AudioClip CheckpointActivationSound;
    private AudioSource audioSource;

    public static Checkpoint LastActivatedCheckpoint;
    private bool isActivated = false; 

    private void Awake()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            RespawnController.Instance.respawnPoint = transform;

            if (LastActivatedCheckpoint != null && LastActivatedCheckpoint != this)
            {
                LastActivatedCheckpoint.DeactivateCheckpoint();
            }
            LastActivatedCheckpoint = this;
            ActivateCheckpoint();
        }
    }

    
    public void ActivateCheckpoint()
    {
        animator.SetBool("isActivated", true);
        animator.SetBool("isDeactivated", false);
        if (!isActivated) 
        {
            audioSource.PlayOneShot(CheckpointActivationSound);
            isActivated = true;
        }
        
    }

    public void DeactivateCheckpoint()
    {
        animator.SetBool("isActivated", false);
        animator.SetBool("isDeactivated", true);
        isActivated = false;
    }
}
