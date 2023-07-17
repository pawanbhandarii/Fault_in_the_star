using System.Collections;
using System.Collections.Generic;
//using UnityEditorInternal;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class PlayerPrefsManager 
{
    public static int GetHealth()
    {
        if (PlayerPrefs.HasKey("Health"))
        {
            return PlayerPrefs.GetInt("Health");
        }
        else
        {
            return 0;
        }
    }

    public static void SetHealth(int health)
    {
        PlayerPrefs.SetInt("Health", health);
    }

    public static int GetScore()
    {
        if (PlayerPrefs.HasKey("Score"))
        {
            return PlayerPrefs.GetInt("Score");
        }
        else
        {
            return 0;
        }
    }

    public static void SetScore(int score)
    {
        PlayerPrefs.SetInt("Score", score);
    }

    public static int GetHighScore()
    {
        if (PlayerPrefs.HasKey("Highscore"))
        {
            return PlayerPrefs.GetInt("Highscore");
        }
        else
        {
            return 0;
        }
    }

    public static void SetHighScore(int highscore)
    {
        PlayerPrefs.SetInt("Highscore", highscore);
    }

    //store the current player state info into PlayerPrefs

    public static void SavePlayerState(int score, int highScore, int health)
    {
        //save currentscore and lives to PlayerPrefs for moving to next level
        PlayerPrefs.SetInt("Score", score);
        PlayerPrefs.SetInt("Highscore", highScore);
        PlayerPrefs.SetInt("Health", health);
    }

    //reset stored player state and variables back to defaults
    public static void ResetPlayerState(int startHealth, bool resetHighScore)
    {
        PlayerPrefs.SetInt("Health", startHealth);
        PlayerPrefs.SetInt("Score", 0);

        if (resetHighScore)
        {
            PlayerPrefs.SetInt("Highscore", 0);
        }
    }

    //store a key for the name of the current level to indicate it is unlocked
    public static void UnlockLevel()
    {
        //get current scene
        Scene scene = SceneManager.GetActiveScene();
        PlayerPrefs.SetInt(scene.name, 1);
    }

    //determine if a levelname is currently unlocked(i.e. it has a key set)
    public static bool LevelIsUnlocked(string levelName)
    {
        return (PlayerPrefs.HasKey(levelName));
    }

    public static void ShowPlayerPrefs()
    {
        // store the PlayerPref keys to output to the console
        string[] values = { "Score", "Highscore", "Lives" };

        // loop over the values and output to the console
        foreach (string value in values)
        {
            if (PlayerPrefs.HasKey(value))
            {
                Debug.Log(value + " = " + PlayerPrefs.GetInt(value));
            }
            else
            {
                Debug.Log(value + " is not set.");
            }
        }
    }
}
