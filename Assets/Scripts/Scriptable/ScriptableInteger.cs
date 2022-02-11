using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Scriptable Integer", menuName = "Scriptable Object/Integer")]
public class ScriptableInteger : ScriptableObject
{
    public int value;
    public int defaultValue;
    public bool resetOnPlay;


    private void OnEnable() {
        if (resetOnPlay) {
            value = defaultValue;
        }
    }
}
