using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseController : MonoBehaviour
{
    Tile TileUnderMouse;
    TileMap tileMap;

    public LayerMask LayerIDForTiles;

    private Vector3 lastMousePosition;

    // Start is called before the first frame update
    void Start()
    {
        tileMap = FindObjectOfType<TileMap>();
    }

    // Update is called once per frame
    void Update()
    {

        TileUnderMouse = MouseToTile();

        if (Input.GetMouseButtonDown(0))
        {
            if (TileUnderMouse != null)
            {
                tileMap.ControlTile = TileUnderMouse;
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            if (tileMap.ControlTile != null)
            {
                tileMap.ControlTile.OnReleased();
                tileMap.ControlTile = null;
            }
        }


        Vector3 currentMousePosition = Input.mousePosition;

        // Calculate the difference in mouse position between frames
        float deltaX = currentMousePosition.x - lastMousePosition.x;
        float deltaY = currentMousePosition.y - lastMousePosition.y;

        // Update the lastMousePosition for the next frame
        lastMousePosition = currentMousePosition;

        // Check if the mouse is moving horizontally
        if (Mathf.Abs(deltaX) > Mathf.Abs(deltaY)) // You can adjust this threshold value to control sensitivity
        {
            if (tileMap.ControlTile != null)
            {
                Vector3 ScreenmousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                tileMap.ControlTile.OnHorizontalDrag(ScreenmousePosition.x);
            }
        }
        else if (Mathf.Abs(deltaX) < Mathf.Abs(deltaY))
        {
            
            if (tileMap.ControlTile != null)
            {
                Vector3 ScreenmousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                tileMap.ControlTile.OnVerticalDrag(ScreenmousePosition.y);
            }
        }
    }

    Tile MouseToTile()
    {
        //Debug.LogError("MouseToTile");

        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero,100f,LayerIDForTiles);

        if (hit && hit.transform.gameObject.GetComponent<Tile>() != null && hit.transform.gameObject.GetComponent<Tile>().Show && !hit.transform.gameObject.GetComponent<Tile>().Locked)
        {

            //Debug.LogError("MouseToTileHit");
            return hit.transform.gameObject.GetComponent<Tile>();
        }
        //Debug.Log("Found nothing.");
        return null;
    }

}
