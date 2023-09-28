using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TileMap : MonoBehaviour
{
    public LevelManager lvlmanager;
    public AudioManager audioManager;
    public GameManager gameManager;
    public int Colomns;
    public int Rows;
    public GameObject TilePrefab;
    public GameObject CellPrefab;
    public GameObject ChaPrefab;
    public Cell[,] Cells;
    public Tile[,] Tiles;

    public bool Moving;

    public GameObject LevelPassObject;
    public GameObject GameOverPanel;


    public TextMeshProUGUI CountDownText;

    public System.DateTime StartTime;
    public System.TimeSpan GameDuration;
    public System.DateTime EndTime;
    public int gameTimeInSeconds;
    // TimeSpan timeSpanGameTime;

    private bool _timerOn;
    public bool TimerOn
    {
        get
        {
            return _timerOn;
        }
        set
        {
            if (value == _timerOn)
            {
                return;
            }
            _timerOn = value;

            if (!value)
            {
                GameOn = false;
            }

        }
    }

    public bool GameOn;
    public bool isPaused;


    [System.Serializable]
    public class Cell
    {
        public GameObject GO;
        public int X;
        public int Y;
        private bool __filled;
        public bool Filled
        {
            get
            {
                return __filled;
            }
            set
            {
                __filled = value;

                if (value)
                {
                    SpriteRenderer image = GO.GetComponent<SpriteRenderer>();
                    var tempColor = image.color;
                    tempColor.a = 1f;
                    image.color = tempColor;
                }
                else
                {
                    SpriteRenderer image = GO.GetComponent<SpriteRenderer>();
                    var tempColor = image.color;
                    tempColor.a = 1f;
                    image.color = tempColor;
                }
            }

        }
    }


    private Tile _controlTile;
    public Tile ControlTile
    {
        get
        {
            return _controlTile;
        }
        set
        {
            if (_controlTile != null && value != _controlTile)
            {
                _controlTile.Show = false;
                _controlTile.Show = true;

            }

            _controlTile = value;
            if (value != null)
            {
                SpriteRenderer image = value.transform.GetComponent<SpriteRenderer>();
                var tempColor = image.color;
                tempColor.a = 1f;
                image.color = tempColor;
            }


        }
    }
    public void StartGame()
    {
        ClearChildren();

        setLevelData();
        GenerateMap();
        Resume();


    }
    public void setLevelData()
    {
        Colomns = lvlmanager.currentLevel.gridSizeX;
        Rows = lvlmanager.currentLevel.gridSizeY;
        gameTimeInSeconds = Colomns * Rows * 5;

        setTime(gameTimeInSeconds);

    }
    void setTime(int _time)

    {
        int _minute = _time / 60;
        int _seconds = _time % 60;
        GameDuration = new System.TimeSpan(0, _minute, _seconds);

        StartTime = System.DateTime.UtcNow;
        EndTime = StartTime + GameDuration;

    }
    void AddTimeBy(int _time)
    {
        GameDuration = GameDuration + new System.TimeSpan(0, 0, _time);
        StartTime = System.DateTime.UtcNow;
        EndTime = StartTime + GameDuration;
    }
    public void ClearChildren()
    {
        int childCount = transform.childCount;


        for (int i = childCount - 1; i >= 0; i--)
        {
            Transform child = transform.GetChild(i);
            Destroy(child.gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //StartGame();

    }



    public void GenerateMap()
    {
        //levelCompleted = false;

        if (Cells != null)
        {
            foreach (Cell c in Cells)
            {
                Destroy(c.GO);
            }
        }

        if (Tiles != null)
        {
            foreach (Tile t in Tiles)
            {
                Destroy(t.GO);
            }
        }

        TimerOn = true;




        Vector3 CanvasScal = this.transform.localScale;

        float scalemultiplier = 5;

        if (Colomns <= 2)
        {
            scalemultiplier = 3;
        }
        else if (Colomns <= 4)
        {
            scalemultiplier = 4;
        }
        else 
        {
            scalemultiplier = 4;
        }

        CanvasScal.x = scalemultiplier;
        CanvasScal.y = CanvasScal.x / Colomns * Rows * 0.86f;

        this.transform.localScale = CanvasScal;

        Cells = new Cell[Colomns, Rows];
        for (int i = 0; i < Colomns; i++)
        {
            for (int j = 0; j < Rows; j++)
            {
                Cell c = new Cell();
                c.X = i;
                c.Y = j;
                Cells[i, j] = c;

                GameObject TileGo = Instantiate(CellPrefab, this.transform.position, Quaternion.identity, this.transform);

                c.GO = TileGo;
                BaseTile t = TileGo.GetComponent<BaseTile>();

                t.tilemap = this;

                t.RightCol = i;
                t.LeftCol = i;
                t.TopRaw = j;
                t.BottomRaw = j;

                t.SetTileBounds();

            }
        }
        int p_1 = 0;
        int p_2 = 1;
        int _tileIndex = 0;

        Tiles = new Tile[Colomns, Rows];
        for (int i = 0; i < Colomns; i++)
        {
            for (int j = 0; j < Rows; j++)
            {
                GameObject TileGo = Instantiate(TilePrefab, this.transform.position, Quaternion.identity, this.transform);
                Tile t = TileGo.GetComponent<Tile>();
                Tiles[i, j] = t;
                t.GO = TileGo;

                t.X = i;
                t.Y = j;
                _tileIndex++;
                // Debug.Log(_tileIndex + " " + i + "" + j);
                // if(lvlmanager.currentLevel.cellIndex==_tileIndex){

                // }

                // float r = Random.Range(0, 1f);
                // t.Show = (r <= 0.5);

                t.Show = false;
                foreach (var cell in lvlmanager.currentLevel.resultCells)
                {
                    if (cell.cellIndex.x == i && cell.cellIndex.y == j)
                    {

                        t.requiredTileCount = cell.cellResultCount;
                        t.Show = true;
                        break;
                    }


                }


                // T.show=lvlmanager.currentLevel.cellIndex==_tileIndex;

                // t.requiredTileCount = 0;

                // if (t.Show)
                // {
                //     t.requiredTileCount = lvlmanager.currentLevel.cellResultCount;
                // }

                Cell c = GetCellAt(i, j);



                for (int p = 0; p < t.requiredTileCount; p++)
                {
                    Vector3 cpos = c.GO.transform.position;

                    if (t.requiredTileCount == 2 || t.requiredTileCount == 3 || t.requiredTileCount == 4)
                    {
                        int q = p % 2;
                        int r = p / 2;
                        float s = 0.2f / t.requiredTileCount;
                        float cons = Mathf.Pow(-1f, q);
                        float cons1 = Mathf.Round((q + 1) / 2);


                        cpos.x += ((0.8f * (2 * cons1) * cons + 0.8f) * 2 / Colomns) * scalemultiplier / 5;
                        cpos.y -= ((r * 1.4f - 1.24f) * 2 / Colomns) * scalemultiplier / 5;
                    }
                    else if (t.requiredTileCount == 5 || t.requiredTileCount == 6)
                    {
                        int q = p % 4;
                        int r = p / 4;
                        float s = 0.2f / t.requiredTileCount;
                        float cons = Mathf.Pow(-1f, q);
                        float cons1 = Mathf.Round((q + 1) / 2);


                        cpos.x += ((0.32f * (2 * (2 - cons1)) * cons - 0.2f) * 1.8f / Colomns) * scalemultiplier / 5;
                        cpos.y -= ((r * 1.4f - 1.24f) * 2 / Colomns) * scalemultiplier / 5;
                    }
                    else
                    {
                        int q = p % 3;
                        int r = p / 3;
                        float s = 0.2f / t.requiredTileCount;
                        float cons = Mathf.Pow(-1f, q);
                        float cons1 = Mathf.Round((q + 1) / 2);


                        cpos.x += ((0.25f * (2 * cons1) * cons) * 3 / Colomns) * scalemultiplier / 5;
                        cpos.y -= ((s * (2 * cons1) + r * 0.6f - 0.8f) * 3 / Rows) * scalemultiplier / 5;
                    }


                    GameObject chaGO = Instantiate(ChaPrefab, cpos, Quaternion.identity, c.GO.transform);
                    if (t.MyCharacterObjects == null)
                    {
                        t.MyCharacterObjects = new HashSet<GameObject>();
                    }
                    t.MyCharacterObjects.Add(chaGO);
                }

                t.tilemap = this;

                t.RightCol = i;
                t.LeftCol = i;
                t.TopRaw = j;
                t.BottomRaw = j;

                t.FRightc = i;
                t.FLeftc = i;
                t.FTopr = j;
                t.FBottomr = j;

                t.SetTileBounds();

                t.Locked = false;
            }
        }
    }



    public Cell GetCellAt(int x, int y)
    {
        if (Cells == null)
        {
            Debug.LogError("cells array not instanciated");
            return null;
        }

        x = x % Colomns;
        if (x < 0)
        {
            x += Colomns;
        }

        y = y % Rows;
        if (y < 0)
        {
            y += Rows;
        }

        try
        {
            return Cells[x, y];
        }

        catch
        {
            Debug.LogError("GetCellAt: " + x + "," + y);
            return null;
        }
    }


    public void CheckLevel()
    {
        bool levelPass = true;



        foreach (Tile t in Tiles)
        {
            if (t == null)
            {
                levelPass = false;
                break;
            }

            if ((t.Show && !t.Locked))
            {

                levelPass = false;
                break;
            }
        }

        if (levelPass)
        {
            //LevelComplete();
            levelPass = false;
            StartCoroutine(LevelComplete());
            // Debug.LogError("LEVEL UP");

        }


    }
    /*void LevelComplete()
    {
        lvlmanager.NextLevel();
        LevelPassObject.SetActive(true);

    }*/

    public void NextLevel()
    {
        lvlmanager.NextLevel();
        StartGame();
        LevelPassObject.SetActive(false);

    }

    public IEnumerator LevelComplete()
    {
        yield return new WaitForSeconds(1f);

        LevelPassObject.SetActive(true);




        pause();

    }
    public void TilePresed()
    {
        audioManager.ClickSound();


    }

    private void LateUpdate()
    {
        TimerUpadte();
    }

    void TimerUpadte()
    {
        if (!TimerOn)
        {
            return;
        }
        if (isPaused)
        {
            return;
        }

        System.TimeSpan ctu = EndTime - System.DateTime.UtcNow;
        if (ctu <= System.TimeSpan.Zero)
        {
            GameOver();

        }
        // ctu.Minutes + "m " +
        CountDownText.text = ctu.Minutes + "m " + ctu.Seconds + "s ";

    }

    public void GameOver()
    {
        GameOverPanel.SetActive(true);
        TimerOn = false;
    }

    public void pause()
    {
        isPaused = true;
        // Time.timeScale = 0;
    }

    public void Resume()
    {
        TimerOn = true;
        isPaused = false;
        Time.timeScale = 1.0f;
    }
    public void AddTime()
    {
        if (gameManager.BuyGems(1))
        {

            AddTimeBy(15);
            Resume();

        }



    }
}
