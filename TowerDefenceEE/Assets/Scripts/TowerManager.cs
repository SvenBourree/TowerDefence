using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TowerManager : Singleton<TowerManager>
{

    private TowerButton towerButtonPressed;


    public void selectedTower(TowerButton towerSelected)
    {
        towerButtonPressed = towerSelected;
        Debug.Log(" you pressed " + towerButtonPressed.gameObject);
    }

    public void placeTower(RaycastHit2D hit)
    {
        //checks if you clic the UI (the buttons) because UI is not a game object
        if (!EventSystem.current.IsPointerOverGameObject() && towerButtonPressed != null)
        {
            GameObject newTower = Instantiate(towerButtonPressed.Tower);
            newTower.transform.position = hit.transform.position;


        }
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // 0 is left mouse button 1 is right mouse button
        if (Input.GetMouseButtonDown(0))
        {
            //this looks for the point on the map where you clicked
            Vector2 mapPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //vector2.zero is in the left bottom corner at position y:0 and x:0
            RaycastHit2D hit = Physics2D.Raycast(mapPoint, Vector2.zero);
            if (hit.collider.tag=="BuildSpot")
            {
                placeTower(hit);

            }

        }

    }
}
