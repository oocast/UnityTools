using UnityEngine;
using System.Collections;

public class GameStateControllerTest : MonoBehaviour {
    GameStateController controller;

    // Use this for initialization
    void Start () {
        controller = GameObject.Find("GameStateController").GetComponent<GameStateController>();
        controller.CallStateEvent("BattleStart");

    }
	
	// Update is called once per frame
	void Update () {
        controller.CallStateEvent("OnBattleStart");
    }

    public void BattleStart()
    {
        Debug.Log("Debug start");
    }

    void PrivateFunction()
    {

    }
}
