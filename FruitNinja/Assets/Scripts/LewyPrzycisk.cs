using UnityEngine;

public class PlaySoundOnMouseClick : MonoBehaviour
{
    public AudioClip soundClip; // Przypisz dŸwiêk do tego pola w inspektorze
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = soundClip;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            audioSource.Stop();
            audioSource.PlayOneShot(soundClip);
        }
    }
}
