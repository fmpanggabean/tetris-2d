using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleBlock : MonoBehaviour
{
    public ScriptableBoard board;

    void Start()
    {
        
    }

    internal int getX() {
        return Mathf.RoundToInt(transform.position.x);
    }

    internal int getY() {
        return Mathf.RoundToInt(transform.position.y);
    }

    internal bool isInsideBoundary(int x, int y) {
        if (x < 0
            || y < 0
            || x > board.getLastIndexX()
            || y > board.getLastIndexY()) {
            return false;
        }
        return true;
    }
    
    internal int getYBelow() {
        return getY() - 1;
    }

    internal void setPosition(int x, int y) {
        transform.position = new Vector3(x, y, 0);
    }
}
