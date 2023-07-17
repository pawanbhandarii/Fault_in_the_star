using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor; //this is needed since this script is refernce to the unity editor

[CustomEditor(typeof(GameManager))]
public class GameManagerEditor : Editor //extends the editor class
{
    //called when Unity Editor is updated
    public override void OnInspectorGUI()
    {
        //show the default inspector stuff for this component
        DrawDefaultInspector();

        //get a reference to the GameManager script on this target gameobject
        GameManager myGM = (GameManager)target;

        //adds a custom button to the Inspector
        if(GUILayout.Button("Reset Player State"))
        {
            //if button pressed then call the function in script
            PlayerPrefsManager.ResetPlayerState(myGM.startHealth, false);
        }

        //adds a custom button to the Inspector
        if(GUILayout.Button("Reset Player HighScore"))
        {
            //if button pressed then call the function in script
            PlayerPrefsManager.SetHighScore(0);
        }

        //adds a custom button to the Inspector
        if (GUILayout.Button("Output Player HighScore"))
        {
            //if button pressed then call the function in script
            PlayerPrefsManager.ShowPlayerPrefs();
        }
    }
}
