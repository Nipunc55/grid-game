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
    void FixedUpdate()
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
            lastMousePosition = Vector3.zero;

            if (tileMap.ControlTile != null)
            {
                tileMap.ControlTile.OnReleased();                
                tileMap.ControlTile = null;
            }

            
        }

        //lastMousePosition = Vector3.zero;
        Vector3 currentMousePosition = Vector3.zero;
        currentMousePosition = Input.mousePosition;

        // Calculate the difference in mouse position between frames
        float deltaX = 0;
        deltaX = currentMousePosition.x - lastMousePosition.x;
        float deltaY = 0;
        deltaY = currentMousePosition.y - lastMousePosition.y;

        // Update the lastMousePosition for the next frame
        lastMousePosition = currentMousePosition;

        if (Mathf.Abs(deltaY) + Mathf.Abs(deltaX) > 10f)
        {

            // Check if the mouse is moving horizontally
            if (Mathf.Abs(deltaX) > Mathf.Abs(deltaY) ) // You can adjust this threshold value to control sensitivity
            {
                if (tileMap.ControlTile != null)
                {
                    Vector3 ScreenmousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    tileMap.ControlTile.OnHorizontalDrag(ScreenmousePosition.x);
                }


                //Debug.LogError("Hor");
            }
            else if (Mathf.Abs(deltaY) > 1.5f * Mathf.Abs(deltaX) + 5f)
            {

                if (tileMap.ControlTile != null)
                {
                    Vector3 ScreenmousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    tileMap.ControlTile.OnVerticalDrag(ScreenmousePosition.y);
                }

                //Debug.LogError("Ver");
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

            // Debug.LogError("MouseToTileHit");
            return hit.transform.gameObject.GetComponent<Tile>();
        }
        //Debug.Log("Found nothing.");
        return null;
    }

}
