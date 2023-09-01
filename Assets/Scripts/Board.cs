using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    public int width;
    public int height;

    public GameObject bgTilePrefab;

    public Gem[] gems;

    public Gem[,] allGems;

    public float gemSpeed =7f;

    [HideInInspector]
    public MatchFinder matchFind;

    private void Awake()
    {
        matchFind = FindObjectOfType<MatchFinder>();
    }

    // Start is called before the first frame update
    void Start()
    {
        allGems = new Gem[width, height];
        Setup();
       
    }

    void Update()
    {
        matchFind.FindAllMatches();
    }

    private void Setup()
    {
        // finish this
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector2 pos = new Vector2(x, y);
                GameObject bgTile = Instantiate(bgTilePrefab, pos, Quaternion.identity);
                bgTile.transform.parent = transform;
                bgTile.name = "BG Tile - " + x + "," + y;

                int gemToUse = Random.Range(0, gems.Length);

                int iterations = 0;
                while(MatchesAt(new Vector2Int(x,y), gems[gemToUse]) && iterations < 100)
                {
                    gemToUse = Random.Range(0, gems.Length);
                    iterations++;
                }

                SpawnGem(new Vector2Int(x, y), gems[gemToUse]);

            }
        }
    }

    private void SpawnGem(Vector2Int pos ,Gem gemToSpawn)
    {
        Gem gem = Instantiate(gemToSpawn, new Vector3(pos.x, pos.y, 0f), Quaternion.identity);
        gem.transform.parent = transform;
        gem.name = "Gem - " + pos.x + "," + pos.y;
        allGems[pos.x, pos.y] = gem;

        gem.SetupGem(pos, this);
    }    


    bool MatchesAt(Vector2Int posToCheck, Gem gemToCheck)
    {
        if(posToCheck.x > 1)
        {
            if(allGems[posToCheck.x - 1, posToCheck.y].type == gemToCheck.type && allGems[posToCheck.x - 2, posToCheck.y].type == gemToCheck.type)
            {
                return true;
            }
        }

        if (posToCheck.y > 1)
        {
            if (allGems[posToCheck.x, posToCheck.y - 1].type == gemToCheck.type && allGems[posToCheck.x, posToCheck.y - 2].type == gemToCheck.type)
            {
                return true;
            }
        }

        return false;
    }


    private void DestroyMatchedGemAt(Vector2Int pos)
    {
        if(allGems[pos.x, pos.y] != null)
        {
            if(allGems[pos.x, pos.y].isMatched)
            {
                Destroy(allGems[pos.x, pos.y].gameObject);
                allGems[pos.x, pos.y] = null;
            }
        }
    }


    public void DestroyMatches()
    {
        for(int i = 0; i < matchFind.currentMatches.Count; i++)
        {
            if(matchFind.currentMatches[i] != null)
            {
                DestroyMatchedGemAt(matchFind.currentMatches[i].posIndex);
            }
        }

        StartCoroutine(DecreaseRowCo());
    }


    // Coroutine to decrease the row of gems in the game board
    private IEnumerator DecreaseRowCo()
    {
        // Wait for 0.2 seconds before executing the next line of code
        yield return new WaitForSeconds(.2f);

        // Counter to keep track of null or missing gems in a column
        int nullCounter = 0;

        // Loop through each column (x)
        for (int x = 0; x < width; x++)
        {
            // Loop through each row (y) in the column
            for (int y = 0; y < height; y++)
            {
                // If the gem at this position is null, increment the null counter
                if (allGems[x, y] == null)
                {
                    nullCounter++;
                }
                // If the gem is not null and there are null gems above it
                else if (nullCounter > 0)
                {
                    // Move the gem down by the number of null gems above it
                    allGems[x, y].posIndex.y -= nullCounter;
                    allGems[x, y - nullCounter] = allGems[x, y];
                    allGems[x, y] = null;
                }
            }
            // Reset the null counter for the next column
            nullCounter = 0;
        }
    }

}
