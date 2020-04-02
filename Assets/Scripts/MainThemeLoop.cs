using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainThemeLoop : MonoBehaviour
{
    public AudioSource musicSource;
    public AudioClip musicStart;

    // Start is called before the first frame update
    void Start()
    {
        musicSource.PlayOneShot(musicStart);
        double clipLength = (double)musicStart.samples / musicStart.frequency;

        musicSource.PlayScheduled(AudioSettings.dspTime + clipLength);
    }

}
