using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class LoadLevel : MonoBehaviour
{
    public string LevelAfterVictory;
    public GameObject DefaultSelectedButton;

     void Start()
    {
        EventSystem.current.SetSelectedGameObject(DefaultSelectedButton);
    }
    public void LoadLevelMainMenu()
    {
        SceneManager.LoadScene(LevelAfterVictory);
    }
}
