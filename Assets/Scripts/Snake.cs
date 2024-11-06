using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour
{

    private enum Direction {
        Left,
        Right,
        Up, 
        Down
    }

    private enum State {
        Alive,
        Dead
    }

    public enum Player {
        PlayerOne,
        PlayerTwo
    }

    private Player player;
    private State state;
    private Direction gridMoveDirection;
    private Vector2Int gridPosition;
    private float gridMoveTimer;
    private float gridMoveTimerMax;
    private LevelGrid levelGrid;
    private int SnakeBodySize;
    private List<SnakeMovePosition> snakeMovePositionList;
    private List<SnakeBodyPart> snakeBodyPartList;
    public List<Vector2Int> snakeBodyPositionList;
    public List<Vector2Int> foodPositionList;
    public List<GameObject> foodGameObjectList;

    public void Setup(LevelGrid levelGrid, int playerNumber) {
        this.levelGrid = levelGrid;
        if(playerNumber == 1) {
            player = Player.PlayerOne;
            gridPosition = new Vector2Int(5, 5);
        } else if(playerNumber == 2) {
            player = Player.PlayerTwo;
            gridPosition = new Vector2Int(15, 15);
        }
    }

    private void Awake() {

        gridMoveTimerMax = 0.8f;
        gridMoveTimer = gridMoveTimerMax;
        gridMoveDirection = Direction.Right;

        snakeMovePositionList = new List<SnakeMovePosition>();
        snakeBodyPartList = new List<SnakeBodyPart>();
        snakeBodyPositionList = new List<Vector2Int>();
        foodPositionList = new List<Vector2Int>();
        foodGameObjectList = new List<GameObject>();
        SnakeBodySize = 0;

        state = State.Alive;
    }

    void Update()
    {
        switch (state) {
            case State.Alive:
                HandleInput();
                HandleGridMovement(); 
                break;
            case State.Dead:
                break;
        }
    }

    private void HandleInput() {

        if(player == Player.PlayerOne) {
            if(Input.GetKeyDown(KeyCode.UpArrow)) {
                if(gridMoveDirection != Direction.Down) {
                    gridMoveDirection = Direction.Up;
                }
            }
            
            if(Input.GetKeyDown(KeyCode.DownArrow)) {
                if(gridMoveDirection != Direction.Up) {
                    gridMoveDirection = Direction.Down;
                }
            }

            if(Input.GetKeyDown(KeyCode.LeftArrow)) {
                if(gridMoveDirection != Direction.Right) {
                    gridMoveDirection = Direction.Left;
                }
            }

            if(Input.GetKeyDown(KeyCode.RightArrow)) {
                if(gridMoveDirection != Direction.Left) {
                    gridMoveDirection = Direction.Right;
                }
            }
        }

        if(player == Player.PlayerTwo) {
            if(Input.GetKeyDown(KeyCode.W)) {
                if(gridMoveDirection != Direction.Down) {
                    gridMoveDirection = Direction.Up;
                }
            }
            
            if(Input.GetKeyDown(KeyCode.S)) {
                if(gridMoveDirection != Direction.Up) {
                    gridMoveDirection = Direction.Down;
                }
            }

            if(Input.GetKeyDown(KeyCode.A)) {
                if(gridMoveDirection != Direction.Right) {
                    gridMoveDirection = Direction.Left;
                }
            }

            if(Input.GetKeyDown(KeyCode.D)) {
                if(gridMoveDirection != Direction.Left) {
                    gridMoveDirection = Direction.Right;
                }
            }
        }
    }

    private void HandleGridMovement() {
        gridMoveTimer += Time.deltaTime;
        if(gridMoveTimer >= gridMoveTimerMax) {
            gridMoveTimer -= gridMoveTimerMax;

            SnakeMovePosition previousSnakeMovePosition = null;

            if (snakeMovePositionList.Count > 0) {
                previousSnakeMovePosition = snakeMovePositionList[0];
            }
            
            SnakeMovePosition snakeMovePosition = new SnakeMovePosition(previousSnakeMovePosition, gridPosition, gridMoveDirection);
            snakeMovePositionList.Insert(0, snakeMovePosition);

            Vector2Int gridMoveDirectionVector;
            switch(gridMoveDirection) {
                default:
                case Direction.Right: gridMoveDirectionVector = new Vector2Int(1, 0); break;
                case Direction.Left:  gridMoveDirectionVector = new Vector2Int(-1, 0); break;
                case Direction.Up:    gridMoveDirectionVector = new Vector2Int(0,+1); break;
                case Direction.Down:  gridMoveDirectionVector = new Vector2Int(0,-1); break;
            }
            //if(player == Player.PlayerOne) {
                gridPosition += gridMoveDirectionVector;
            //}

            if(snakeMovePositionList.Count >= SnakeBodySize + 1) {
                snakeMovePositionList.RemoveAt(snakeMovePositionList.Count - 1);
            }

            UpdateSnakeBodyParts();

            foreach (SnakeBodyPart snakeBodyPart in snakeBodyPartList) {
                Vector2Int snakeBodyPartGridPosition = snakeBodyPart.GetGridPosition();
                snakeBodyPositionList.Add(snakeBodyPartGridPosition);
                if(gridPosition == snakeBodyPartGridPosition) {
                    state = State.Dead;
                    Debug.Log("Snake has eaten itself.");
                }
            }

            if(levelGrid.SnakeHitBorder(gridPosition)) {
                state = State.Dead;
                Debug.Log(player +" has hit the border.");
            }

            transform.position = new Vector3(gridPosition.x, gridPosition.y);
            transform.eulerAngles = new Vector3(0, 0, GetAngleFromVector(gridMoveDirectionVector) - 90);

        }
    }

    public void RemoveFoodLists(int i){
        foodPositionList.RemoveAt(i);
        foodGameObjectList.RemoveAt(i);
    }
    
    public void SnakeAteFood() {
        SnakeBodySize++;
        CreateSnakeBody();
    }

    public Vector2Int GridPosition() {
        return gridPosition;
    }

    public Vector2Int SnakeBodyPartGridPosition() {
        foreach (SnakeBodyPart snakeBodyPart in snakeBodyPartList) {
            Vector2Int snakeBodyPartGridPosition = snakeBodyPart.GetGridPosition();
            return snakeBodyPartGridPosition;
        }
        return gridPosition;
    }

    public void SetPlayerDead() {
        state = State.Dead;
    }

    public int PlayerStatus() {
        if(state == State.Alive) {
            return 1;
        } else {
            return 2;
        }
    }

    private void CreateSnakeBody() {
        snakeBodyPartList.Add(new SnakeBodyPart(snakeBodyPartList.Count, player));
    }

    private void UpdateSnakeBodyParts() {

        for(int i = 0; i < snakeBodyPartList.Count; i++) {
            snakeBodyPartList[i].SetSnakeMovePosition(snakeMovePositionList[i]);
        } 
    
    }

    private float GetAngleFromVector(Vector2 dir) {
        float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if(n < 0) n += 360; 
        return n;
    }

    public Vector2Int GetGridPosition() {
        return gridPosition;
    }

    public List<Vector2Int> GetFullSnakeGridPosition() {
        List<Vector2Int> gridPositionList = new List<Vector2Int>() { gridPosition };
        foreach (SnakeMovePosition snakeMovePosition in snakeMovePositionList) {
            gridPositionList.Add(snakeMovePosition.GetGridPosition());
        }
        return gridPositionList;
    }

    private class SnakeBodyPart {

        private SnakeMovePosition snakeMovePosition; 
        private Transform transform;

        private Player player;

        public SnakeBodyPart(int bodyIndex, Player player) {
            this.player = player;
            GameObject snakeBodyGameObject = new GameObject("SnakeBody", typeof(SpriteRenderer));
            snakeBodyGameObject.GetComponent<SpriteRenderer>().sprite = GameAssets.instance.snakeBodySprite;
            snakeBodyGameObject.GetComponent<SpriteRenderer>().sortingOrder = -1 - bodyIndex;

            if(player == Player.PlayerOne) {
                snakeBodyGameObject.GetComponent<SpriteRenderer>().color = Color.blue;
            } else if(player == Player.PlayerTwo) {
                snakeBodyGameObject.GetComponent<SpriteRenderer>().color = Color.red;
            }
            transform = snakeBodyGameObject.transform;
        }

        public void SetSnakeMovePosition(SnakeMovePosition snakeMovePosition){
            this.snakeMovePosition = snakeMovePosition;
            transform.position = new Vector3(snakeMovePosition.GetGridPosition().x, snakeMovePosition.GetGridPosition().y);

            float angle;
            switch (snakeMovePosition.GetDirection()) {
                default:
                case Direction.Up: 
                    switch (snakeMovePosition.GetPreviousDirection()) {
                        default:        
                            angle = 0; break;
                        case Direction.Left: 
                            angle = 0 + 45; break;
                        case Direction.Right: 
                            angle = 0 - 45; break;
                    }
                    break;
                case Direction.Down: 
                    switch (snakeMovePosition.GetPreviousDirection()) {
                        default:        
                            angle = 180; break;
                        case Direction.Left: 
                            angle = 180 + 45; break;
                        case Direction.Right: 
                            angle = 180 - 45; break;
                    }
                    break;
                case Direction.Left: 
                    switch (snakeMovePosition.GetPreviousDirection()) {
                        default:        
                            angle = -90; break;
                        case Direction.Down: 
                            angle = -45; break;
                        case Direction.Up: 
                            angle = 45; break;
                    }
                    break;
                case Direction.Right: 
                    switch (snakeMovePosition.GetPreviousDirection()) {
                        default:        
                            angle = 90; break;
                        case Direction.Down: 
                            angle = 45; break;
                        case Direction.Up: 
                            angle = -45; break;
                    }
                    break;
            }
            transform.eulerAngles = new Vector3(0, 0, angle);
        } 

        public Vector2Int GetGridPosition() {
            return snakeMovePosition.GetGridPosition();
        }

    }

    private class SnakeMovePosition { 

        private  SnakeMovePosition previousSnakeMovePosition;
        private Vector2Int gridPosition;
        private Direction direction;

        public SnakeMovePosition(SnakeMovePosition previousSnakeMovePosition, Vector2Int gridPosition, Direction direction) {
            this.previousSnakeMovePosition = previousSnakeMovePosition;
            this.gridPosition = gridPosition;
            this.direction = direction;
        }

        public Vector2Int GetGridPosition() {
            return gridPosition;
        }

        public Direction GetDirection() {
            return direction;
        }

        public Direction GetPreviousDirection() {
            if(previousSnakeMovePosition == null) {
                return Direction.Right;
            } else {
                return previousSnakeMovePosition.direction;
            }
        }
      
    }

}
