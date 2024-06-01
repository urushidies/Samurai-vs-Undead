
using UnityEngine;

public class MusicPlayerScript : MonoBehaviour
{
    public AudioClip musicClip;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = musicClip;
        audioSource.volume = 0.4f;  // Set the volume (0.0 to 1.0)
        audioSource.loop = true;    // Enable looping
        audioSource.playOnAwake = true; // Play on awake
        audioSource.Play();
    }
}
