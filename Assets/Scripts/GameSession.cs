using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameSession : MonoBehaviour
{
    [SerializeField] int playerLives = 3;
    [SerializeField] int score = 0;
    [SerializeField] TextMeshProUGUI livesText;
    [SerializeField] TextMeshProUGUI scoreText;

    void Awake()
    {   
        //runs game session on clicking play button, if there is more than 1 it will destroy current game session so there isn't any overlap
        int numGameSessions = FindObjectsOfType<GameSession>().Length;
        if(numGameSessions > 1) {
            Destroy(gameObject);
        } else {
            DontDestroyOnLoad(gameObject);
        }
    }

    //This changes the canvas text numbers to show lives and score
    void Start() {
        livesText.text = playerLives.ToString();
        scoreText.text = score.ToString();
    }

    //This function runs the two functions to either take a life off the player, or reset the game if the player is out of lives
    public void ProcessPlayerDeath(){
        if(playerLives > 1){
            TakeLife();
        } else {
            ResetGameSession();
        }
    }

    //This is a public funtion that will add points to score when a coin is picked up (check coinpickup script)
    public void AddToScore(int pointsToAdd){
        score += pointsToAdd;
        scoreText.text = score.ToString();
    }

    //This function takes a life and then resets the level
    void TakeLife()
    {
        playerLives--;
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
        livesText.text = playerLives.ToString();
    }

    //This function resets the level back to 0, and destroys itself so if the play button is pressed again it isn't hanging around
    void ResetGameSession()
    {
        FindObjectOfType<ScenePersist>().ResetScenePersist();
        SceneManager.LoadScene(0);
        Destroy(gameObject);
    }
}
