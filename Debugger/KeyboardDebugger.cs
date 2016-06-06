using UnityEngine;
using System.Collections;

// TODO: editor support to bind function with key via GUI

/// <summary>
/// Map alphabet keys to external functions for debugging
/// Used to avoid re-binding of debugging keys in developing stage
/// </summary>
public class KeyboardDebugger : MonoBehaviour
{
    public delegate void BindingAction();
    public BindingAction[] keyboardEvents = new BindingAction[26];
	
	// Update is called once per frame
	void Update ()
    {
	    for (int i = 0; i < 26; i++)
        {
            if (keyboardEvents[i] != null && Input.GetKeyDown(KeyCode.A + i))
            {
                Debug.Log("Key " + (char)('a' + i) + " pressed");
                keyboardEvents[i]();
            }
        }
	}
}
