using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gem : MonoBehaviour
{
    [HideInInspector]
    public Vector2Int posIndex;
    [HideInInspector]
    public Board board;

    private Vector2 firstTouchPosition;
    private Vector2 finalTouchPosition;

    private bool mousePress;
    private float swipeAngle = 0;

    private Gem otherGem;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector2.Lerp(transform.position, posIndex, board.gemSpeed * Time.deltaTime);
        if(mousePress && Input.GetMouseButtonUp(0))
        {
            mousePress = false;
            finalTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            CalculateAngle();
        }
    }

    public void SetupGem(Vector2Int pos, Board theBoard)
    {
        posIndex = pos;
        board = theBoard;
    }

    private void OnMouseDown()
    {
        firstTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePress = true;
    }

    private void CalculateAngle()
    {
        swipeAngle = Mathf.Atan2(finalTouchPosition.y - firstTouchPosition.y, finalTouchPosition.x- firstTouchPosition.x);
        swipeAngle = swipeAngle * 180 / Mathf.PI;
        float distance = Vector3.Distance(firstTouchPosition, finalTouchPosition);
        if (distance > .5f)
        {
            MovePieces();
        }
    }

    private void MovePieces()
    {
        if(swipeAngle < 45 && swipeAngle > -45 && posIndex.x < board.width - 1)
        {
            // Move Right
            otherGem = board.allGems[posIndex.x + 1, posIndex.y];
            otherGem.posIndex.x--;
            posIndex.x++;
        } else if (swipeAngle > 45 && swipeAngle <= 135 && posIndex.y < board.height - 1)
        {
            // Move Up
            otherGem = board.allGems[posIndex.x, posIndex.y + 1];
            otherGem.posIndex.y--;
            posIndex.y++;
        } else if(swipeAngle < 45 && swipeAngle >= -135 && posIndex.y > 0)
        {
            // Move Down
            otherGem = board.allGems[posIndex.x, posIndex.y - 1];
            otherGem.posIndex.y++;
            posIndex.y--;
        } else if(swipeAngle > 135 || swipeAngle < -135 && posIndex.x > 0)
        {
            // Move Left
            otherGem = board.allGems[posIndex.x - 1, posIndex.y];
            otherGem.posIndex.x++;
            posIndex.x--;
        }

        board.allGems[posIndex.x, posIndex.y] = this;
        board.allGems[otherGem.posIndex.x, otherGem.posIndex.y] = otherGem;
    }
}
