﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerButton : MonoBehaviour
{
    [SerializeField]
    private GameObject tower;

    public GameObject Tower
    {
        get
        {
            return tower;
        }
    }
   
}
