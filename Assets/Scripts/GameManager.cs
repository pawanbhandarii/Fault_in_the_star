using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    //static refernce to game mananger so can be called from other scripts directly (not just through gameobject component)
    public static GameManager gm;

    //panel reference
    public GameObject VictoryPanel;
    public GameObject VictoryPanelDefaultButton;

    //time bouns interval
    public float timeRange1;
    public float timeRange2;

    //levels to move to on victory and lose
    public string levelAfterVictory;
    public string levelAfterGameOver;

    //game performance
    public int score = 0;
    public int highscore = 0;
    public int startHealth;
    private int currentHealth;

    //UI elements to control
    public Text UIScore;
    public Text UIHighScore;
    public Text UILevel;
    public GameObject UIGamePaused;
    public Text UIVictoryScore;
    public Text UITimeBonus;
    public Text UITotalScore;

    //private variables
    GameObject player;
    Vector3 spawnLocation;
    Scene scene;
    float time;
    int timeBonus;

    private void Awake()
    {
        //setup reference to game manager
        if (gm == null)
        {
            gm = this.GetComponent<GameManager>();
        }
        //setup all the variables, the UI, and providee errors if things not setup properly
        setupDefaults();
    }

    private void Start()
    {
        FindObjectOfType<TimerControllerScript>().StartTime();
    }

    // game loop
    void Update()
    {
        // if ESC pressed then pause the game
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Time.timeScale > 0f)
            {
                UIGamePaused.SetActive(true); // this brings up the pause UI
                Time.timeScale = 0f; // this pauses the game action
            }
            else
            {
                Time.timeScale = 1f; // this unpauses the game action (ie. back to normal)
                UIGamePaused.SetActive(false); // remove the pause UI
            }
        }
    }

    void setupDefaults()
    {
        //setup refernce to player
        if(player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
        if (player == null)
        {
            Debug.LogError("Player not found in Game Manager");
        }

        //get current scene
        scene = SceneManager.GetActiveScene();

        //get initial spawnLocation based on initial position of player
        spawnLocation = player.transform.position;

        //if levels not specified, default to current level
        if (levelAfterVictory == "")
        {
            Debug.LogWarning("Level After Victory not specified, default scene assigned");
            levelAfterVictory = scene.name;
        }

        if (levelAfterGameOver == "")
        {
            Debug.LogWarning("Level After gameOver not specified, default scene assigned");
            levelAfterGameOver = scene.name;
        }

        //friendly error messages
        if (UIScore == null)
            Debug.LogError("Need to set UIScore on Game Manager.");

        if (UIHighScore == null)
            Debug.LogError("Need to set UIHighScore on Game Manager.");

        if (UILevel == null)
            Debug.LogError("Need to set UILevel on Game Manager.");

        if (UIGamePaused == null)
            Debug.LogError("Need to set UIGamePaused on Game Manager.");


        //get stored player prefs
        refreshPlayerState();

        //get the UI ready for the game
        refreshGUI();

        

    }

    // get stored Player Prefs if they exist, otherwise go with defaults set on gameObject
    
    void refreshPlayerState()
    {
        currentHealth = PlayerPrefsManager.GetHealth();

        //special case if health <=0 then must be testing in editor, so reset the player prefs
        if(currentHealth <= 0)
        {
            PlayerPrefsManager.ResetPlayerState(startHealth, false);
            currentHealth = PlayerPrefsManager.GetHealth();
        }
        score = PlayerPrefsManager.GetScore();
        highscore = PlayerPrefsManager.GetHighScore();

        //save that this level has been accessed so the  MainMenu can be enable it
        PlayerPrefsManager.UnlockLevel();
    }

    void refreshGUI()
    {
        //set the text elements of the UI
        UIScore.text = "Score: " + score.ToString();
        UIHighScore.text = "HighScore: " + highscore.ToString();
        UILevel.text = scene.name;

    }

    // public function to add points and update the gui and highscore player prefs accordingly
    public void AddPoints(int amount)
    {
        // increase score
        score += amount;

        // update UI
        UIScore.text = "Score: " + score.ToString();
    }

    // public function to remove player life and reset game accordingly
    public void ResetGame()
    {
        Debug.Log("Resetting");
        Debug.Log(currentHealth);
        refreshGUI();

        if (currentHealth <= 0)
        { // no more health
          //StopTimer
            FindObjectOfType<TimerControllerScript>().StopTime();
          // save the current player prefs before going to GameOver
            PlayerPrefsManager.SavePlayerState(score, highscore, currentHealth);

            // load the gameOver screen
            SceneManager.LoadScene(levelAfterGameOver);
        }
        
    }

    // public function for level complete
    public void LevelCompete()
    {
        // save the current player prefs before moving to the next level
        PlayerPrefsManager.SavePlayerState(score, highscore, currentHealth);

    }

    // load the nextLevel after delay
    public void  LoadNextLevel()
    {
        SceneManager.LoadScene(levelAfterVictory);
    }

    public void LoadVictoryPanel()
    {
        VictoryPanel.SetActive(true);
        EventSystem.current.SetSelectedGameObject(VictoryPanelDefaultButton);
        FindObjectOfType<TimerControllerScript>().StopTime();
        TimeBonusCalaculations();
        UIVictoryScore.text = score.ToString();
        UITimeBonus.text = timeBonus.ToString();
        score += timeBonus;
        UITotalScore.text = score.ToString();

        // if score>highscore then update the highscore UI too
        if (score > highscore)
        {
            highscore = score;
            UIHighScore.text = "Highscore: " + score.ToString();
        }
        LevelCompete();
    }

    public void ResfreshHealth(int health)
    {
        currentHealth = health;
    }

    public void SetHealth(int health)
    {
        startHealth = health;
    }

    public void GetPlayedTime(float theTime)
    {
        time = theTime;
    }

    void TimeBonusCalaculations()
    {
        if(time>0 && time <= timeRange1)
        {
            timeBonus = 10;
        }
        else if(time>timeRange1 && time <=timeRange2)
        {
            timeBonus = 5;
        }
        else
        {
            timeBonus = 0;
        }
    }
}
