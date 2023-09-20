using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseTile : MonoBehaviour
{
    public int X;
    public int Y;
    
    [Range(0, 1)] public float Top, Bottom, Right, Left;

    public int FTopr, FBottomr, FRightc, FLeftc;

    public TileMap tilemap;

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

            if (value)
            {
                SpriteRenderer image = GetComponent<SpriteRenderer>();
                var tempColor = image.color;
                tempColor.a = 0.8f;
                image.color = tempColor;
            }
            else
            {
                SpriteRenderer image = GetComponent<SpriteRenderer>();
                var tempColor = image.color;
                tempColor.a = 0;
                image.color = tempColor;
            }
        }
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

   
}
