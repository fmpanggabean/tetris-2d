using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tetramino : MonoBehaviour
{
    public InputHandler input;

    public ScriptableBoard board;
    public Transform pivot;

    public List<SingleBlock> blocks;

    public void Start() {
        blocks = GetComponentsInChildren<SingleBlock>().ToList<SingleBlock>();

        translate(0, 0);
    }

    public bool translate(int x, int y) {
        if (!isInsideBoundary(x, y)) {
            return false;
        }
        if (!isBlocksEmpty(x,y)) {
            return false;
        }

        transform.position += new Vector3(x, y, 0);

        transform.position = new Vector3(
            Mathf.Round(transform.position.x)
            , Mathf.Round(transform.position.y)
            , Mathf.Round(transform.position.z)
            );
        return true;
    }

    public bool isBlocksEmpty (int x, int y) {
        foreach (SingleBlock sb in blocks) {
            int nextX = sb.getX() + x;
            int nextY = sb.getY() + y;

            if (board.board[nextY, nextX] != null) {
                return false;
            }
        }
        return true;
    }

    private bool isInsideBoundary(int x, int y) {
        foreach(SingleBlock sb in blocks) {
            if (!sb.isInsideBoundary(x + sb.getX(), y + sb.getY())) {
                return false;
            }
        }

        return true;
    }

    private void rotate(float direction) {
        transform.RotateAround(pivot.position, Vector3.forward, 90 * -direction);
        
        if (!isInsideBoundary(0, 0)) {
            transform.RotateAround(pivot.position, Vector3.forward, 90 * direction);
        } else if (!isBlocksEmpty(0, 0)) {
            transform.RotateAround(pivot.position, Vector3.forward, 90 * direction);
        }
    }

    internal void addBlockToBoard() {
        foreach(SingleBlock sb in blocks) {

        }
    }

    internal void freezeBlock() {
        foreach(SingleBlock sb in blocks) {
            board.fillBoardAt(sb.getX(), sb.getY(), sb);
        }
    }

    internal void removeParent() {
        foreach(SingleBlock sb in blocks) {
            sb.transform.parent = null;
        }
        Destroy(gameObject);
    }

    internal bool belowIsEmpty() {
        foreach (SingleBlock sb in blocks) {
            int nextY = sb.getY() - 1;
            int nextX = sb.getX();

            //check boundary
            if (!sb.isInsideBoundary(nextX, nextY)) {
                return false;
            }
            //check for other blocks
            if (board.board[nextY, nextX] != null) {
                return false;
            }
        }

        return true;
    }

    private void OnMove(float direction) {
        int x = Mathf.RoundToInt(direction);

        translate(x, 0);
    }

    private void OnRotate(float direction) {
        rotate(direction);
    }

    private void OnEnable() {
        input.OnMoveAction += OnMove;
        input.OnRotateAction += OnRotate;
    }

    private void OnDisable() {
        input.OnMoveAction -= OnMove;
        input.OnRotateAction -= OnRotate;
    }
}
