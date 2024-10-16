using UnityEngine;

public class Footsteps : MonoBehaviour
{
    public AudioSource audioSource;

    public void PlayFootstepSound()
    {
        audioSource.pitch = Random.Range(0.8f, 1.2f);
        audioSource.Play();
    }
}
