using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor; //includes unityeditor 

public class DestroyAllPlayerPrefs : ScriptableObject
{
    //DeletePlayerPrefs after a dialog box 
    [MenuItem("Editor/Delete Player Prefs")]
    static void DeletePrefs()
    {
        if (EditorUtility.DisplayDialog("Delete all player prefences",
                                       "Are you sure you want to delete all prefences",
                                       "Yes",
                                       "No"))
        {
            PlayerPrefs.DeleteAll();
        }
    }
}

