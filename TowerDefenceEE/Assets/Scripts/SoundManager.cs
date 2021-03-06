﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
    [SerializeField]
    private AudioClip fire;
    [SerializeField]
    private AudioClip water;
    [SerializeField]
    private AudioClip venom;
    [SerializeField]
    private AudioClip death;

    public AudioClip Fire
    {
        get
        {
            return fire;
        }
    }

    public AudioClip Water
    {
        get
        {
            return water;
        }
    }

    public AudioClip Venom
    {
        get
        {
            return venom;
        }
    }

    public AudioClip Death
    {
        get
        {
            return death;
        }
    }
}
