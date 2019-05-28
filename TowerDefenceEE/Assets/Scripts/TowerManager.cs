using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TowerManager : Singleton<TowerManager>
{

    public TowerButton towerButtonPressed { get; set; }
    private SpriteRenderer spriteRenderer;

    private List<Tower> Towerlist = new List<Tower>();
    private List<Collider2D> Buildlist = new List<Collider2D>();
    private Collider2D buildTile;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        buildTile = GetComponent<Collider2D>();
        spriteRenderer.enabled = false;

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
                buildTile = hit.collider;
                buildTile.tag = "NoMoreBuildingHere";
                registerBuildsite(buildTile);
                placeTower(hit);

            }


        }
        if (spriteRenderer.enabled)
        {
            followCursor();
        }

    }

    public void registerBuildsite(Collider2D tag)
    {
        Buildlist.Add(tag);
    }

    public void registerTower(Tower tower)
    {
        Towerlist.Add(tower);
    }

    public void renameBuildsiteTags()
    {
        foreach (Collider2D tag in Buildlist)
        {
            tag.tag = "Buildspot";
        }
        Buildlist.Clear();
    }

    public void destroyAllTowers()
    {
        foreach (Tower tower in Towerlist)
        {
            Destroy(tower.gameObject);
        }
        Towerlist.Clear();
    }


    public void selectedTower(TowerButton towerSelected)
    {
        if (towerSelected.TowerPrice <= GameManager.Instance.TotalMoney)
        {
            towerButtonPressed = towerSelected;
            enableDragTower(towerButtonPressed.DragTower);
            // Debug.Log(" you pressed " + towerButtonPressed.gameObject);

        }
    }

    public void placeTower(RaycastHit2D hit)
    {
        //checks if you clic the UI (the buttons) because UI is not a game object
        if (!EventSystem.current.IsPointerOverGameObject() && towerButtonPressed != null)
        {
            Tower newTower = Instantiate(towerButtonPressed.Tower);
            newTower.transform.position = hit.transform.position;
            buyTower(towerButtonPressed.TowerPrice);
            registerTower(newTower);
            disableDragTower();


        }
    }

    public void buyTower(int price)
    {
        GameManager.Instance.subtractMoney(price);
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
