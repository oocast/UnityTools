using UnityEngine;
using System.Collections;

public class SoundSystemTest : MonoBehaviour {
    public SoundSystem soundSystem;

	// Use this for initialization
	void Start () {
        

    }
	
	// Update is called once per frame
	void Update ()
    {
        soundSystem.LoadSound("BladeStorm");
    }
}
