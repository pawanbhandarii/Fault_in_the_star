using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    public int startHealth = 100; // how much health to start the game

    //references to Submenues
    public GameObject MainMenu;
    public GameObject LeveLsMenu;
    public GameObject InstructionMenu;
    public GameObject StoryMenu;

    //reference to Button GameObjects
    public GameObject MenuDefaultButton;
    public GameObject InstructionDefaultButton;
    public GameObject LevelSelectDefaultButton;
    public GameObject QuitButton;
    public GameObject StoryDefaultButton;

    //list the level names
    public string[] LevelNames;

    // reference to the LevelIsPanel gameObject where the button should be childed
    public GameObject LevelsPanel;

    //refernce to the default Level Button template
    public GameObject LevelButtonPrefab;

    //reference the titleText so we can change it dynamically
    public Text titleText;

    //store the initial title so we can set it back
    private string mainTitle;

    //initialize menu

    private void Awake()
    {
        //store the initial title so we can set it back
        mainTitle = titleText.text;

        //disable/enable Level button based on player progress
        setLevelSelect();

        //determine if Quit button should be shown;
        displayQuitWhenAppropriate();

        //Show the proper menu
        ShowMenu("MainMenu");

    }

    //loop through all the levelbuttons and set them to interactable
    //based on if PlayerPref key is set for the level

    void setLevelSelect()
    {
        //turn on levels menu while setting it up so no null refs
        LeveLsMenu.SetActive(true);

        //loop through each levelName defined in the editor
        for(int i = 0; i < LevelNames.Length; i++)
        {
            //get the level name
            string levelname = LevelNames[i];

            //dynamically create a button from the template
            GameObject levelButton = Instantiate(LevelButtonPrefab, Vector3.zero, Quaternion.identity);

            //name the game object
            levelButton.name = levelname + "Button";

            //set the parent of the button as the LevelsPanel so it will be dynamically arrange based on the defined layout
            levelButton.transform.SetParent(LevelsPanel.transform, false);

            //get the Button Script attach to the button
            Button levelButtonScript = levelButton.GetComponent<Button>();

            //setup the listner to loadlevel when clicked
            levelButtonScript.onClick.RemoveAllListeners();
            levelButtonScript.onClick.AddListener(() => loadLevel(levelname));

            //set the label of the button
            Text levelButtonLabel = levelButton.GetComponentInChildren<Text>();
            levelButtonLabel.text = levelname;

            //determine if the button should be interactable based on if the level is unlocked
            if (PlayerPrefsManager.LevelIsUnlocked(levelname))
            {
                levelButtonScript.interactable = true;
            }
            else
            {
                levelButtonScript.interactable = false;
            }
        }
    }

    //determine if the Quit Button should be present based on what is the build
    void displayQuitWhenAppropriate()
    {
        switch (Application.platform)
        {
            //platforms that should have quit button
            case RuntimePlatform.WindowsPlayer:
            case RuntimePlatform.OSXPlayer:
            case RuntimePlatform.LinuxPlayer:
                QuitButton.SetActive(true);
                break;

            //platforms that should not have quit button
            case RuntimePlatform.WindowsEditor:
            case RuntimePlatform.OSXEditor:
            case RuntimePlatform.IPhonePlayer:
            case RuntimePlatform.WebGLPlayer:
                QuitButton.SetActive(false);
                break;

            //all other platforms default to no quit button
            default:
                QuitButton.SetActive(false);
                break;
        }
    }

    //Public functions below that are available via the UI Event Trigger, such as on Button

    //Show the proper menu

    public void ShowMenu(string name)
    {
        //turn all menus off
        MainMenu.SetActive(false);
        InstructionMenu.SetActive(false);
        LeveLsMenu.SetActive(false);
        StoryMenu.SetActive(false);

        //turn on desired menu and set default selected button fpr controller input
        switch (name)
        {
            case "MainMenu":
                MainMenu.SetActive(true);
                EventSystem.current.SetSelectedGameObject(MenuDefaultButton);
                titleText.text = mainTitle;
                break;
            case "LevelSelect":
                LeveLsMenu.SetActive(true);
                EventSystem.current.SetSelectedGameObject(LevelSelectDefaultButton);
                titleText.text = "Level Select";
                break;
            case "Instruction":
                InstructionMenu.SetActive(true);
                EventSystem.current.SetSelectedGameObject(InstructionDefaultButton);
                titleText.text = "Instructions";
                break;
            case "Story":
                StoryMenu.SetActive(true);
                EventSystem.current.SetSelectedGameObject(StoryDefaultButton);
                titleText.text = "Game Story";
                break;
        }
    }

    //load the specidied Unity level

    public void loadLevel(string levelToLoad)
    {
        //start new game so initialize player state
        PlayerPrefsManager.ResetPlayerState(startHealth, false);

        //load the specified level
        SceneManager.LoadScene(levelToLoad);

    }

    //quit the game

    public void QuitGAme()
    {
        Application.Quit();
    }
    
}
