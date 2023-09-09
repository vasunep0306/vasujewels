using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundManager : MonoBehaviour
{
    public float roundTime = 60f;
    public Board board;
    public int currentScore;
    public float displayScore;
    public float scoreSpeed;

    private UIManager uiMan;
    private bool endingRound = false;
    // Start is called before the first frame update
    void Awake()
    {
        uiMan = FindObjectOfType<UIManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(roundTime > 0)
        {
            roundTime -= Time.deltaTime;

            if (roundTime <= 0)
            {
                roundTime = 0;

                endingRound = true;
            }
        }

        if (endingRound && board.currentState == Board.BoardState.move ) 
        { 
            WinCheck();
            endingRound = false;
        }
        displayScore = Mathf.Lerp(displayScore, currentScore, scoreSpeed * Time.deltaTime); // Interpolate displayScore to currentScore smoothly using Mathf.Lerp method
        uiMan.timeText.text = roundTime.ToString("0.0") + "s";
        uiMan.scoreText.text = displayScore.ToString("0");
    }

    private void WinCheck()
    {
        uiMan.roundOverScreen.SetActive(true);
    }
}
