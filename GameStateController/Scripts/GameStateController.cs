using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using System;

public class GameStateController : MonoBehaviour, ISerializationCallbackReceiver
{
    [System.Serializable]
    public class StateEntry
    {
        public string stateName;
        public UnityEvent stateCallbackFunctions;
    }

    public List<StateEntry> gameStates;
    Dictionary<string, int> lookupTable = new Dictionary<string, int>();

    public void CallStateEvent(string stateName)
    {
        if (lookupTable == null)
        {
            return;
        }
        int index = -1;
        if (lookupTable.ContainsKey(stateName))
        {
            index = lookupTable[stateName];
        }
        if (index < 0)
        {
            Debug.Log("Statename not found");
        }
        else
        {
            gameStates[index].stateCallbackFunctions.Invoke();
        }
    }

    public void OnBeforeSerialize()
    {
        // Keep empty
    }

    public void OnAfterDeserialize()
    {
        if (lookupTable == null)
        {
            lookupTable = new Dictionary<string, int>();
        }
        for (int i = 0; i < gameStates.Count; i++)
        {
            lookupTable[gameStates[i].stateName] = i;
        }
    }
}
