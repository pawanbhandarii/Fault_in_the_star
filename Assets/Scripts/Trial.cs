using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Trial : MonoBehaviour
{
    public void OnInvoke(Button button)
    {
        print("Invoked:" + button.name);     
    }
}
