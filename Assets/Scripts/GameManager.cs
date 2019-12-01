using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;
    //Highscore text for bothplayers
    public Text Player1HighScore, Player2HighScore;
    //score text for bothplayers
    public Text Player1Score, Player2Score;

    //panel references
    public GameObject startMenu, endMenu;

    public PlayerManager playerManager;

    public GameObject customerManager;

    //String to store Highscore in  playerprefs.
    private string PLAYER1HIGHSCORE = "PLAYER1HIGHSCORE", PLAYER2HIGHSCORE = "PLAYER2HIGHSCORE";

    private void Awake()
    {
        instance = this;
        customerManager.GetComponent<CustomerManager>().enabled = false;
        playerManager.stopPlayers(false);
    }
    /// <summary>
    /// start game function
    /// </summary>
    public void StartGame()
    {
        startMenu.SetActive(false);
        customerManager.GetComponent<CustomerManager>().enabled = true;
        playerManager.stopPlayers(true);
    }

    /// <summary>
    /// GameOver function
    /// </summary>
    public void DisplayGameOverPanel()
    {
        endMenu.SetActive(true);
        if (PlayerPrefs.GetInt(PLAYER1HIGHSCORE, 0) < playerManager.player1.playerScore)
        {
            Player1HighScore.text = "PLAYER 1 HIGHSCORE :" + playerManager.player1.playerScore;
            PlayerPrefs.SetInt(PLAYER1HIGHSCORE, playerManager.player1.playerScore);
        }
        else
        {
            Player1HighScore.text = "PLAYER 1 HIGHSCORE :" + PlayerPrefs.GetInt(PLAYER1HIGHSCORE);
        }
        if (PlayerPrefs.GetInt(PLAYER2HIGHSCORE, 0) < playerManager.player2.playerScore)
        {
            Player2HighScore.text = "PLAYER 2 HIGHSCORE :" + playerManager.player2.playerScore;
            PlayerPrefs.SetInt(PLAYER2HIGHSCORE, playerManager.player2.playerScore);
        }
        else
        {
            Player1HighScore.text = "PLAYER 2 HIGHSCORE :" + PlayerPrefs.GetInt(PLAYER2HIGHSCORE);
        }
        Player1Score.text = playerManager.player1.playerScore.ToString();
        Player2Score.text = playerManager.player2.playerScore.ToString();

        customerManager.GetComponent<CustomerManager>().enabled = false;
        playerManager.stopPlayers(false);
    }

    /// <summary>
    /// Restart Scene
    /// </summary>
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
