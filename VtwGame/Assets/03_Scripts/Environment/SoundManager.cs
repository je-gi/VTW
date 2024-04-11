using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    public AudioClip jumpSound;
    public AudioClip dashSound;
    public AudioClip throwSound;
    public AudioClip deathSound;

    private AudioSource audioSource;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        audioSource = GetComponent<AudioSource>();
    }

    public void PlayJumpSound()
    {
        PlaySound(jumpSound);
    }

    public void PlayDashSound()
    {
        PlaySound(dashSound);
    }

    public void PlayThrowSound()
    {
        PlaySound(throwSound);
    }

    public void PlayDeathSound()
    {
        PlaySound(deathSound);
    }

    private void PlaySound(AudioClip clip)
    {
        if (clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}
