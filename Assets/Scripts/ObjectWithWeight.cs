using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ObjectWithWeight
{
    public GameObject prefab;
    [Range(1, 1000)] public int weight;
}
