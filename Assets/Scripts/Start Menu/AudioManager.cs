using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // Start is called before the first frame update

    public MySound[] sounds;

    public static AudioManager instance;
    void Awake()
    {
        //if (instance == null)
        //{
        //    instance = this;
        //} else
        //{
        //    Destroy(gameObject);
        //    return;
        //}

        //DontDestroyOnLoad(gameObject);
        foreach (MySound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    public void Play (string name)
    {
        // find sound in sounds that sound.name is name
        MySound ms = Array.Find(sounds, sound =>  sound.name == name);
        if (ms == null)
        {
            Debug.Log("Sound: " + name + " not found!");
            return;
        }
        if (!ms.source.isPlaying) 
        {
            ms.source.Play();
        }
    }
}
