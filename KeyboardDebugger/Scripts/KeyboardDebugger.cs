using UnityEngine;
using UnityEngine.Events;
using System.Collections;

// TODO: editor support to bind function with key via GUI

/// <summary>
/// Map alphabet keys to external functions for debugging
/// Used to avoid re-binding of debugging keys in developing stage
/// </summary>
[System.Serializable]
public class KeyboardDebugger : MonoBehaviour
{
    public delegate void BindingAction();
    [SerializeField]
    public UnityEvent[] keyboardEvents = new UnityEvent[26];
	
    void Awake()
    {
        
    }

	// Update is called once per frame
	void Update ()
    {
	    for (int i = 0; i < 26; i++)
        {
            if (keyboardEvents[i] != null && Input.GetKeyDown(KeyCode.A + i))
            {
                Debug.Log("Key " + (char)('a' + i) + " pressed");
                keyboardEvents[i].Invoke();
            }
        }
	}

    void Reset()
    {
    }
}
