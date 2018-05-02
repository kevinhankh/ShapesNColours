using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    const float COUNTDOWN_TIME = 3.0f;
    const float VOLUME_SCALE = 1.0f;
    const int TOTAL_GOAL_SPAWNER = 12;
    const int MIN_CHOICE = 0, MAX_CHOICE = 3;
    const int SHAPES_COLOURS = 3;
    const int MAIN_MENU = 0;
    const int LEVEL_ONE = 1;
    const string PlayerPrefsKey = "highscore";

    public AudioClip newHighScore;
    public AudioClip applause;
    
    float timer;
    int playerScore, highScore;
    int i;
    int goalLocation;
    int chooseColour, chooseShape, chooseSpawner, lastColour, lastShape, lastSpawner;
    bool goalAlive, gameOver;
    Color[] GoalColour;
    Transform[] GoalSpawner;
    GameObject[] GoalObject;
    GameObject SpawnedGoalObject, PlayerScoreText, TimerText, GameOverText, PlayAgainButton, MainMenuButton;
    AudioSource AudioPlayer;

	// Use this for initialization
	void Start () {
        AudioPlayer = GetComponent<AudioSource>();
        PlayerScoreText = GameObject.FindWithTag("ScoreText");
        TimerText = GameObject.FindWithTag("TimerText");
        GameOverText = GameObject.FindWithTag("GameOverText");
        GameOverText.SetActive(false);
        PlayAgainButton = GameObject.FindWithTag("PlayAgainButton");
        PlayAgainButton.SetActive(false);
        MainMenuButton = GameObject.FindWithTag("MainMenuButton");
        MainMenuButton.SetActive(false);
        GoalObject = new GameObject[SHAPES_COLOURS] { GameObject.FindWithTag("GoalCube"), GameObject.FindWithTag("GoalSphere"), GameObject.FindWithTag("GoalTriangle") };
        
        GoalSpawner = new Transform[TOTAL_GOAL_SPAWNER];
        for (i = 0; i < TOTAL_GOAL_SPAWNER; i++)
        {
            GoalSpawner[i] = transform.GetChild(i).transform;
        }
        
        GoalColour = new Color[SHAPES_COLOURS] { Color.red, Color.green, Color.blue };
        goalAlive = false;
        gameOver = false;

        chooseShape = Random.Range(MIN_CHOICE, MAX_CHOICE);
        chooseColour = Random.Range(MIN_CHOICE, MAX_CHOICE);
        chooseSpawner = Random.Range(MIN_CHOICE, TOTAL_GOAL_SPAWNER);

        timer = 60;
        highScore = PlayerPrefs.GetInt(PlayerPrefsKey, 0);

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
	}
	
	// Update is called once per frame
	void Update () {
        if (timer > 0)
            timer -= 1 * Time.deltaTime;
        else
        {
            if(!gameOver)
                GameOver();
        }

        TimerText.GetComponent<Text>().text = string.Format("{0:#0}", timer);

        if (!goalAlive && !gameOver)
            SpawnGoal();
	}

    void SpawnGoal()
    {
        while ((chooseShape == lastShape) && (chooseColour == lastColour))
        {
            chooseShape = Random.Range(MIN_CHOICE, MAX_CHOICE);
            chooseColour = Random.Range(MIN_CHOICE, MAX_CHOICE);
        }
        while (chooseSpawner == lastSpawner)
        {
            chooseSpawner = Random.Range(MIN_CHOICE, TOTAL_GOAL_SPAWNER);
        }
        lastShape = chooseShape;
        lastColour = chooseColour;
        lastSpawner = chooseSpawner;

        SpawnedGoalObject = (GameObject)Instantiate(GoalObject[chooseShape], GoalSpawner[chooseSpawner].position, GoalSpawner[chooseSpawner].rotation);
        SpawnedGoalObject.GetComponent<Light>().color = GoalColour[chooseColour];

        goalAlive = true;
    }

    public void UpdateGoal(int addScore)
    {
        playerScore += addScore;
        PlayerScoreText.GetComponent<Text>().text = string.Format("Score: {0:#0}", playerScore);
        goalAlive = false;
    }
    
    void GameOver()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        gameOver = true;
        Destroy(SpawnedGoalObject);
        if (playerScore > highScore)
        {
            highScore = playerScore;
            GameOverText.GetComponent<Text>().text = string.Format("Game Over\n New Highscore!", highScore);
            PlayerPrefs.SetInt(PlayerPrefsKey, playerScore);
            PlayerPrefs.Save();
            AudioPlayer.PlayOneShot(newHighScore, VOLUME_SCALE);
        }
        else
        {
            GameOverText.GetComponent<Text>().text = string.Format("Game Over\n Current Highscore: {0}", highScore);
            AudioPlayer.PlayOneShot(applause, VOLUME_SCALE);
        }
        GameOverText.SetActive(true);
        PlayAgainButton.SetActive(true);
        MainMenuButton.SetActive(true);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(MAIN_MENU);
    }

    public void PlayAgain()
    {
        SceneManager.LoadScene(LEVEL_ONE);
    }
}
