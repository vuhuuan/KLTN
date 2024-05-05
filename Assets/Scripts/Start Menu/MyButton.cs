using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyButton : MonoBehaviour
{
    enum ButtonSounds
    {
        Hover,
        Click,
    }

    [SerializeField] private List<AudioClip> audioClips;

    private AudioSource audioSource;
    private void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
    }
    public void PlayClickSound()
    {
        int click_index = (int) ButtonSounds.Click;
        audioSource.PlayOneShot(audioClips[click_index]);
    }
    public void PlayHoverSound()
    {
        int hover_index = (int) ButtonSounds.Hover;
        audioSource.PlayOneShot(audioClips[hover_index]);
    }
}
