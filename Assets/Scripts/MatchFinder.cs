using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class MatchFinder : MonoBehaviour
{
    private Board board;
    public List<Gem> currentMatches = new();

    private void Awake()
    {
        board = FindObjectOfType<Board>();
    }

    public void FindAllMatches()
    {
        currentMatches.Clear();

        for (int x = 0; x < board.width; x++)
        {
            for (int y = 0; y < board.height; y++)
            {
                Gem currentGem = board.allGems[x, y];
                if (currentGem != null)
                {
                    // Check for horizontal matches
                    if (x > 0 && x < board.width - 1)
                    {
                        Gem leftGem = board.allGems[x - 1, y];
                        Gem rightGem = board.allGems[x + 1, y];
                        if (leftGem != null && rightGem != null)
                        {
                            if (leftGem.type == currentGem.type && rightGem.type == currentGem.type)
                            {
                                currentGem.isMatched = true;
                                leftGem.isMatched = true;
                                rightGem.isMatched = true;

                                currentMatches.Add(currentGem);
                                currentMatches.Add(leftGem);
                                currentMatches.Add(rightGem);
                            }
                        }
                    }

                    // Check for vertical matches
                    if (y > 0 && y < board.height - 1)
                    {
                        Gem aboveGem = board.allGems[x, y - 1];
                        Gem belowGem = board.allGems[x, y + 1];
                        if (aboveGem != null && belowGem != null)
                        {
                            if (aboveGem.type == currentGem.type && belowGem.type == currentGem.type)
                            {
                                currentGem.isMatched = true;
                                aboveGem.isMatched = true;
                                belowGem.isMatched = true;

                                currentMatches.Add(currentGem);
                                currentMatches.Add(aboveGem);
                                currentMatches.Add(belowGem);
                            }
                        }
                    }
                }
            }
        }

        if(currentMatches.Count > 0)
        {
            currentMatches = currentMatches.Distinct().ToList();
        }

        CheckForBombs();
    }

    private void CheckForBombs()
    {
        for(int i = 0; i < currentMatches.Count; i++)
        {
            Gem gem = currentMatches[i];

            int x = gem.posIndex.x;
            int y = gem.posIndex.y;

            BombXGreaterThan0(gem, x, y);
            BombXLessThanBoardWidth(gem, x, y);

            BombYGreaterThan0(gem, x, y);
            BombYLessThanBoardWidth(gem, x, y);
        }
    }

    private void BombYLessThanBoardWidth(Gem gem, int x, int y)
    {
        if (gem.posIndex.y < board.width - 1)
        {
            if (board.allGems[x, y + 1] != null)
            {
                if (board.allGems[x, y + 1].type == Gem.GemType.Bomb)
                {
                    MarkBombArea(new Vector2Int(x, y + 1), board.allGems[x, y + 1]);
                }
            }
        }
    }

    private void BombYGreaterThan0(Gem gem, int x, int y)
    {
        if (gem.posIndex.y > 0)
        {
            if (board.allGems[x, y - 1] != null)
            {
                if (board.allGems[x, y - 1].type == Gem.GemType.Bomb)
                {
                    MarkBombArea(new Vector2Int(x, y - 1), board.allGems[x, y - 1]);
                }
            }
        }
    }

    private void BombXLessThanBoardWidth(Gem gem, int x, int y)
    {
        if (gem.posIndex.x < board.width - 1)
        {
            if (board.allGems[x + 1, y] != null)
            {
                if (board.allGems[x + 1, y].type == Gem.GemType.Bomb)
                {
                    MarkBombArea(new Vector2Int(x + 1, y), board.allGems[x + 1, y]);
                }
            }
        }
    }

    private void BombXGreaterThan0(Gem gem, int x, int y)
    {
        if (gem.posIndex.x > 0)
        {
            if (board.allGems[x - 1, y] != null)
            {
                if (board.allGems[x - 1, y].type == Gem.GemType.Bomb)
                {
                    MarkBombArea(new Vector2Int(x - 1, y), board.allGems[x - 1, y]);
                }
            }
        }
    }

    private void MarkBombArea(Vector2Int bombPos, Gem theBomb)
    {
        for(int x = bombPos.x - theBomb.blastSize; x <= bombPos.x + theBomb.blastSize; x++)
        {
            for(int y = bombPos.y - theBomb.blastSize; y < bombPos.y + theBomb.blastSize; y++)
            {
                if( x >= 0 && x < board.width && y >= 0 && y < board.height)
                {
                    if(board.allGems[x,y] != null)
                    {
                        board.allGems[x, y].isMatched = true;
                        currentMatches.Add(board.allGems[x, y]);
                    }
                }
            }
        }
        currentMatches = currentMatches.Distinct().ToList();
    }
}
