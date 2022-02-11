using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Tetris Board", menuName = "Scriptable Object/Tetris Board")]
public class ScriptableBoard : ScriptableObject
{
    public int width;
    public int height;
    public SingleBlock[,] board;

    public void OnEnable() {
        board = new SingleBlock[height, width];
    }

    public int getLastIndexX() {
        return width - 1;
    }

    public int getLastIndexY() {
        return height - 1;
    }

    internal void fillBoardAt(int x, int y, SingleBlock sb) {
        board[y, x] = sb;
    }

    internal void clearLine(int y) {
        for (int x=0;x<=getLastIndexX();x++) {
            Destroy(board[y,x].gameObject);
            board[y, x] = null;
        }
    }

    internal void shiftDown(int clearedY) {
        for (int y=clearedY+1; y<=getLastIndexY(); y++) {
            for (int x=0; x<=getLastIndexX(); x++) {
                //shift blocks down
                board[y - 1, x] = board[y, x];
                board[y, x] = null;

                //move the gameobject
                if (board[y - 1, x] != null) {
                    board[y - 1, x].setPosition(x, y - 1);
                }
            }
        }
    }
}
