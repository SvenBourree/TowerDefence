using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TowerManager : Singleton<TowerManager>
{

    private TowerButton towerButtonPressed;
    private SpriteRenderer spriteRenderer;



    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
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
            if (hit.collider.tag == "BuildSpot")
            {
                hit.collider.tag = "NoMoreBuildingHere";
                placeTower(hit);

            }


        }
        if (spriteRenderer.enabled)
        {
            followCursor();
        }

    }

    public void selectedTower(TowerButton towerSelected)
    {
        towerButtonPressed = towerSelected;
        enableDragTower(towerButtonPressed.DragTower);
        Debug.Log(" you pressed " + towerButtonPressed.gameObject);
    }

    public void placeTower(RaycastHit2D hit)
    {
        //checks if you clic the UI (the buttons) because UI is not a game object
        if (!EventSystem.current.IsPointerOverGameObject() && towerButtonPressed != null)
        {
            GameObject newTower = Instantiate(towerButtonPressed.Tower);
            newTower.transform.position = hit.transform.position;
            disableDragTower();


        }
    }

    public void followCursor()
    {
        transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector2(transform.position.x, transform.position.y);
    }

    public void enableDragTower(Sprite sprite)
    {
        spriteRenderer.enabled = true;
        spriteRenderer.sprite = sprite;
    }

    public void disableDragTower()
    {
        spriteRenderer.enabled = false;

    }
}
