using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerButton : MonoBehaviour
{
    [SerializeField]
    private GameObject tower;
    [SerializeField]
    private Sprite dragTower;
    [SerializeField]
    private int towerPrice;

    public GameObject Tower
    {
        get
        {
            return tower;
        }
    }

    public Sprite DragTower
    {
        get
        {
            return dragTower;
        }
    
    }

    public int TowerPrice
    {
        get
        {
            return towerPrice;
        }
    }
   
}
