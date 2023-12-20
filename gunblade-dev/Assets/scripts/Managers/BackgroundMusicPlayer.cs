using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusicPlayer : MonoBehaviour
{
    public AudioClip[] clips;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        StartCoroutine(PlayMusic());
    }

    IEnumerator PlayMusic()
    {
        int musicIndex = 0;

        while (true)
        {
            audioSource.clip = clips[musicIndex];
            audioSource.Play();

            yield return new WaitForSeconds(audioSource.clip.length);

            musicIndex = (musicIndex + 1) % clips.Length;
        }
    }
}
