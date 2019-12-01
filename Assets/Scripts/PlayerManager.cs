using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;
    //Player one reference
    public Player player1, player2;

    //Players  score text
    public Text player1Score,player2Score;

    //Players timer tex
    public Text player1Timer, player2Timer;

    //Penalize player count
    public int penalizeAmount = 10;

    //power values
    private int playerPowerUpSpeed = 20,playerPowerupTimer=20,playerPowerUpScore=20,playerNormalSpeed;

    //Power gameobject reference
    public GameObject speedPower, timerPower, scorePower;
    void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        UpdatePlayerScore();
        playerNormalSpeed = player1.playerSpeed;
    }
    /// <summary>
    /// Update players Scores
    /// </summary>
    public void UpdatePlayerScore()
    {
        player1Score.text ="PLAYER1 :" +player1.playerScore.ToString();
        player2Score.text ="PLAYER2 :" + player2.playerScore.ToString();
    }

    /// <summary>
    /// Update players Time
    /// </summary>
    public void UpdatePlayerTimer()
    {
        player1Timer.text = player1.playertime.ToString();
        player2Timer.text = player2.playertime.ToString();
    }

    /// <summary>
    /// penalizePlayer if wrong food is served
    /// </summary>
    public void PenalizePlayer()
    {
        player1.playerScore -= penalizeAmount;
        player2.playerScore -= penalizeAmount;

        UpdatePlayerScore();
    }
    /// <summary>
    /// SpeedPower
    /// </summary>
    /// <param name="player"></param>
    public  void ActivateSpeedPowerToPlayer(Player player)
    {
        player.playerSpeed = playerPowerUpSpeed;
        StartCoroutine(TimerForPower(player));
    }

    int powertime = 20;
    IEnumerator TimerForPower(Player player)
    {
        print("TimerForPower");
        int powerReferenceTime = powertime;
        while (powerReferenceTime > 0)
        {
            powerReferenceTime--;
            yield return new WaitForSeconds(1f);
        }
        player.playerSpeed = playerNormalSpeed;
    }
    /// <summary>
    /// TimerPower
    /// </summary>
    /// <param name="player"></param>
    public void ActivateTimerPower(Player player)
    {
        player.playertime += playerPowerupTimer;
        print("ActivateTimerPower");
        UpdatePlayerTimer();
    }

    /// <summary>
    /// score power
    /// </summary>
    /// <param name="player"></param>
    public void ActivateScorePower(Player player)
    {
        player.playerScore += playerPowerUpScore;
        print("ActivateScorePower");
        UpdatePlayerScore();
    }
    /// <summary>
    /// To check time for both player is  0
    /// </summary>
    public void checkTimeIsComplete()
    {
        if (player1.playertime <= 0 && player2.playertime <= 0)
        {
            GameManager.instance.DisplayGameOverPanel();
        }
    }
   /// <summary>
   /// Based on value  player will active and deactive.
   /// </summary>
   /// <param name="value"></param>
    public void stopPlayers(bool value)
    {
        player1.gameObject.SetActive(value);
        player2.gameObject.SetActive(value);
    }
}
