using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DropState {
    Normal, Immediate, Fast
}

public class TetraminoManager : MonoBehaviour
{
    private static TetraminoManager instance;

    public InputHandler input;
    public ScriptableBoard board;
    public ScriptableInteger score;

    public List<ObjectWithWeight> tetraminos;
    public Tetramino activeTetramino;
    public DropState state;
    public bool isGameover = false;

    private float timer = 0;
    private float normalDelay = 1f;
    private float fastDelay = 0.05f;
    private float immediateDelay = 0f;

    private void Awake() {
        if (instance == null) {
            instance = this;
        } else {
            Destroy(gameObject);
        }
    }

    private void Start() {

    }

    private void Update() {
        if (isGameOver()) {
            return;
        }

        if (isTimerReachZero()) {
            if (haveActiveTetramino()) {
                if (belowIsEmpty()) {
                    drop();
                } else {
                    freezeBlock();
                    removeActiveTetramino();
                    lineCheck();
                }
            }
            else {
                spawn();
            }
            resetTimer();
        }

        timerUpdate();
    }

    private void gameOver() {
        Debug.Log("gameover");
        isGameover = true;
    }

    private void lineCheck() {
        int lineCleared = 0;

        for (int y=0;y<=board.getLastIndexY();y++) {
            bool isFullLine = true;

            for (int x=0;x<=board.getLastIndexX();x++) {
                if (board.board[y,x] == null) {
                    isFullLine = false;
                    break;
                }
            }
            
            if (isFullLine) {
                board.clearLine(y);
                board.shiftDown(y);
                y--;
                lineCleared++;
            }
        }

        if (lineCleared > 0) {
            addScore(lineCleared);
        }
    }

    private void addScore(int lineCleared) {
        if (lineCleared == 1) {
            score.value += 10;
        } 
        else if (lineCleared == 2) {
            score.value += 25;
        }
        else if (lineCleared == 3) {
            score.value += 45;
        }
        else if (lineCleared == 4) {
            score.value += 70;
        }
    }

    private void removeActiveTetramino() {
        activeTetramino = null;
    }

    private void freezeBlock() {
        activeTetramino.freezeBlock();
        activeTetramino.removeParent();
    }

    private bool belowIsEmpty() {
        if (activeTetramino.belowIsEmpty()) {
            return true;
        }
        return false;
    }

    private void timerUpdate() {
        timer -= Time.deltaTime;
    }

    private void drop() {
        activeTetramino.translate(0, -1);
    }

    private void resetTimer() {
        timer = getDelay();
    }

    private bool haveActiveTetramino() {
        return activeTetramino != null;
    }

    private bool isTimerReachZero() {
        return timer <= 0f;
    }

    private bool isGameOver() {
        return isGameover;
    }

    public void spawn() {
        GameObject go = Instantiate(getRandomObject(), transform.position, transform.rotation);
        int x = (int)go.transform.position.x;
        int y = (int)go.transform.position.y;

        if (board.board[y,x] != null) {
            gameOver();
            Destroy(go);
            return;
        }

        activeTetramino = go.GetComponent<Tetramino>();
        state = DropState.Normal;
    }

    private GameObject getRandomObject() {
        int weight = 0;
        int random = 0;

        foreach(ObjectWithWeight ob in tetraminos) {
            weight += ob.weight;
        }

        random = Random.Range(0, weight);

        foreach(ObjectWithWeight ob in tetraminos) {
            if (random < ob.weight) {
                return ob.prefab;
            } else {
                random -= ob.weight;
            }
        }

        return null;
    }

    private float getDelay() {
        switch(state) {
            case DropState.Normal:  return normalDelay;
            case DropState.Fast:    return fastDelay;
            default:                return immediateDelay;
        }
    }

    private void OnImmediateDrop() {
        state = DropState.Immediate;
        timer = getDelay();
    }

    private void OnFastDropStart() {
        state = DropState.Fast;
        timer = getDelay();
    }

    private void OnFastDropEnd() {
        state = DropState.Normal;
        timer = getDelay();
    }

    public static TetraminoManager GetInstance() {
        return instance;
    }

    private void OnEnable() {
        input.OnImmediateDropAction += OnImmediateDrop;
        input.OnFastDropStartAction += OnFastDropStart;
        input.OnFastDropEndAction += OnFastDropEnd;
    }

    private void OnDisable() {
        input.OnImmediateDropAction -= OnImmediateDrop;
        input.OnFastDropStartAction -= OnFastDropStart;
        input.OnFastDropEndAction -= OnFastDropEnd;
    }
}
