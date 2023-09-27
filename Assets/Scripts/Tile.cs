using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using TMPro;

public class Tile : MonoBehaviour
{
    public GameObject GO;

    public int X;
    public int Y;

    [Range(0, 1)] public float Top, Bottom, Right, Left;

    public int FTopr, FBottomr, FRightc, FLeftc;

    public int requiredTileCount;

    public HashSet<GameObject> MyCharacterObjects;

    public GameObject[] MyCharacterObjectsToArray
    {
        get { return MyCharacterObjects.ToArray(); }
    }

    public TileMap tilemap;

    public enum Direction { None, Hor, Ver };

    public Direction ControlDir;

    private int _topRaw;
    public int TopRaw
    {
        get
        {
            return _topRaw;
        }
        set
        {
            _topRaw = value;

            for (int i = 0; i < tilemap.Rows; i++)
            {
                if (value == i)
                {
                    Top = (float)i / tilemap.Rows;
                }
            }

            FillCell();
        }
    }

    private int _bottomRaw;
    public int BottomRaw
    {
        get
        {
            return _bottomRaw;
        }
        set
        {
            _bottomRaw = value;

            for (int i = 0; i < tilemap.Rows; i++)
            {
                if (value == i)
                {
                    Bottom = (tilemap.Rows - (float)i - 1) / tilemap.Rows;
                }
            }

            FillCell();
        }
    }

    private int _rightCol;
    public int RightCol
    {
        get
        {
            return _rightCol;
        }
        set
        {
            _rightCol = value;

            for (int i = 0; i < tilemap.Colomns; i++)
            {
                if (value == i)
                {
                    Right = (tilemap.Colomns - (float)i - 1) / tilemap.Colomns;
                }
            }

            FillCell();
        }
    }

    private int _leftCol;
    public int LeftCol
    {
        get
        {
            return _leftCol;
        }
        set
        {
            _leftCol = value;

            for (int i = 0; i < tilemap.Colomns; i++)
            {
                if (value == i)
                {
                    Left = (float)(i) / tilemap.Colomns;
                }
            }

            FillCell();
        }
    }

    private bool _show;
    public bool Show
    {
        get
        {
            return _show;
        }
        set
        {
            _show = value;
            this.gameObject.transform.GetComponent<BoxCollider2D>().enabled = value;

            if (value)
            {
                SpriteRenderer image = GetComponent<SpriteRenderer>();
                var tempColor = image.color;
                tempColor.a = 1f;
                image.color = tempColor;
                this.transform.GetChild(0).gameObject.SetActive(true);
                this.transform.GetChild(0).transform.GetChild(5).GetComponent<TextMeshProUGUI>().text = requiredTileCount.ToString();
            }
            else
            {
                SpriteRenderer image = GetComponent<SpriteRenderer>();
                var tempColor = image.color;
                tempColor.a = 0;
                image.color = tempColor;
                this.transform.GetChild(0).gameObject.SetActive(false);
            }
        }
    }

    private int _fillTilesCount;

    public int FillTilesCount
    {
        get
        {
            return _fillTilesCount;
        }
        set
        {
            _fillTilesCount = value;
            FillCELLS = value;
            if (value == requiredTileCount && Show)
            {
               // In your other script
           if (HapticFeedbackManager.instance != null)
                {
                   HapticFeedbackManager.instance.HeavyVibration();
               }
                // Handheld.Vibrate();
                PlaceChas();
                Locked = true;
            }
        }
    }


    public int FillCELLS = 0;
    public bool Locked;


    private void FixedUpdate()
    {
        SetTileBounds();
    }


    public void SetTileBounds()
    {
        Vector2 p = new Vector2(0, 0);

        p.x = (Left - Right) * transform.parent.localScale.x / 2;
        p.y = (Bottom - Top) * transform.parent.localScale.y / 2;
        this.transform.position = p;

        Vector2 s = new Vector2(1, 1);

        s.x = 1 - Right - Left;
        s.y = 1 - Top - Bottom;
        this.transform.localScale = s;
    }

    public void FillCell()
    {
        ControlDir = Direction.None;

        foreach (TileMap.Cell c in tilemap.Cells)
        {
            c.Filled = false;
        }


        foreach (Tile t in tilemap.Tiles)
        {
            if (t != null)
            {
                if (t.Show)
                {
                    for (int i = 0; i < t.RightCol - t.LeftCol + 1; i++)
                    {
                        for (int j = 0; j < t.BottomRaw - t.TopRaw + 1; j++)
                        {
                            TileMap.Cell c = tilemap.GetCellAt(t.LeftCol + i, t.TopRaw + j);
                            c.Filled = true;
                        }
                    }
                }

            }
        }

        int countoffillestiles = 0;

        for (int i = 0; i < this.RightCol - this.LeftCol + 1; i++)
        {
            for (int j = 0; j < this.BottomRaw - this.TopRaw + 1; j++)
            {
                countoffillestiles += 1;
            }
        }

        FillTilesCount = countoffillestiles;

        tilemap.CheckLevel();


        /*TileMap.Cell c =  tilemap.GetCellAt(TopRaw, LeftCol);
        c.Filled = true;*/
    }

    public void PlaceChas()
    {
        int countoffillestiles = 0;

        for (int i = 0; i < this.RightCol - this.LeftCol + 1; i++)
        {
            for (int j = 0; j < this.BottomRaw - this.TopRaw + 1; j++)
            {
                TileMap.Cell c = tilemap.GetCellAt(LeftCol + i, TopRaw + j);
                Vector3 newPosition = c.GO.transform.position;
                MyCharacterObjectsToArray[countoffillestiles].GetComponent<characterView>().newPosition = newPosition;
                countoffillestiles += 1;
            }
        }

    }


    public void OnPressed()
    {
        tilemap.TilePresed();
        Debug.Log("onPresed");

    }

    public void OnReleased()
    {


        for (int i = 0; i < tilemap.Colomns; i++)
        {
            float f = (float)i / tilemap.Colomns;

            if (Right >= f - 1f / (2 * tilemap.Colomns) && Right < f + 1f / (2 * tilemap.Colomns))
            {
                RightCol = tilemap.Colomns - 1 - i;
            }
        }

        for (int i = 0; i < tilemap.Colomns; i++)
        {
            float f = (float)i / tilemap.Colomns;

            if (Left >= f - 1f / (2 * tilemap.Colomns) && Left < f + 1f / (2 * tilemap.Colomns))
            {
                LeftCol = i;
            }
        }


        for (int i = 0; i < tilemap.Rows; i++)
        {
            float f = (float)i / tilemap.Rows;

            if (Bottom >= f - 1f / (2 * tilemap.Rows) && Bottom < f + 1f / (2 * tilemap.Rows))
            {
                BottomRaw = tilemap.Rows - 1 - i;
            }

            //Debug.LogError("Set bottum   " + X.ToString() + ", " + Y.ToString() + " : " + BottomRaw.ToString());
        }

        for (int i = 0; i < tilemap.Rows; i++)
        {
            float f = (float)i / tilemap.Rows;

            if (Top >= f - 1f / (2 * tilemap.Rows) && Top < f + 1f / (2 * tilemap.Rows))
            {
                TopRaw = i;
            }
        }
        Debug.Log("press released");

    }

    public void OnHorizontalDrag(float f)
    {
        if (ControlDir == Direction.None)
        {
            ControlDir = Direction.Hor;
        }

        if (ControlDir == Direction.Hor)
        {

            if (f >= this.transform.position.x)
            {

                int nearestTile = tilemap.Colomns;
                for (int i = 0; i < tilemap.Colomns - RightCol; i++)
                {
                    for (int j = 0; j < BottomRaw + 1 - TopRaw; j++)
                    {
                        if (tilemap.GetCellAt(RightCol + 1 + i, TopRaw + j).Filled)
                        {
                            nearestTile = Mathf.Min(nearestTile, RightCol + 1 + i);
                        }
                    }
                }

                int endTile = nearestTile - 1;
                float rightEnd = ((tilemap.Colomns - (float)endTile - 1) / tilemap.Colomns);

                float val = ((this.transform.parent.position.x + (this.transform.parent.localScale.x / 2) - f) / this.transform.parent.localScale.x);
                Right = Mathf.Clamp(val, rightEnd, ((tilemap.Colomns - (float)FRightc - 1) / tilemap.Colomns));
            }
            else
            {
                int nearestTile = -1;
                for (int i = 0; i < LeftCol; i++)
                {
                    for (int j = 0; j < BottomRaw + 1 - TopRaw; j++)
                    {
                        if (tilemap.GetCellAt(i, TopRaw + j).Filled)
                        {
                            nearestTile = Mathf.Max(nearestTile, i);
                        }
                    }
                }

                int endTile = nearestTile + 1;
                float leftEnd = ((float)(endTile) / tilemap.Colomns);


                float val = -1 * ((this.transform.parent.position.x - (this.transform.parent.localScale.x / 2) - f) / this.transform.parent.localScale.x);
                Left = Mathf.Clamp(val, leftEnd, (float)(FLeftc) / tilemap.Colomns);
            }
        }
    }

    public void OnVerticalDrag(float f)
    {
        if (ControlDir == Direction.None)
        {
            ControlDir = Direction.Ver;
        }

        if (ControlDir == Direction.Ver)
        {

            if (f >= this.transform.position.y)
            {
                int nearestTile = -1;
                for (int i = 0; i < RightCol + 1 - LeftCol; i++)
                {
                    for (int j = 0; j < TopRaw; j++)
                    {
                        if (tilemap.GetCellAt(LeftCol + i, j).Filled)
                        {
                            nearestTile = Mathf.Max(nearestTile, j);
                        }
                    }
                }

                int endTile = nearestTile + 1;
                float topEnd = ((float)(endTile) / tilemap.Rows);


                float val = ((this.transform.parent.position.y + (this.transform.parent.localScale.y / 2) - f) / this.transform.parent.localScale.y);
                //Top = Mathf.Clamp(val, 0, (float)(TopRaw) / tilemap.Rows);
                Top = Mathf.Clamp(val, topEnd, (float)(FTopr) / tilemap.Rows);

            }
            else
            {

                int nearestTile = tilemap.Rows;
                for (int i = 0; i < RightCol + 1 - LeftCol; i++)
                {
                    for (int j = 0; j < tilemap.Rows - BottomRaw; j++)
                    {
                        if (tilemap.GetCellAt(LeftCol + i, BottomRaw + 1 + j).Filled)
                        {
                            nearestTile = Mathf.Min(nearestTile, BottomRaw + 1 + j);
                        }
                    }
                }

                int endTile = nearestTile - 1;
                float bottomEnd = ((tilemap.Rows - (float)endTile - 1) / tilemap.Rows);



                float val = -1 * ((this.transform.parent.position.y - (this.transform.parent.localScale.y / 2) - f) / this.transform.parent.localScale.y);

                //Bottom = Mathf.Clamp(val, 0, (tilemap.Rows - (float)BottomRaw - 1) / tilemap.Rows);
                Bottom = Mathf.Clamp(val, bottomEnd, (tilemap.Rows - (float)FBottomr - 1) / tilemap.Rows);


            }
        }
    }
}
