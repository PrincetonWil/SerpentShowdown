using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

public class LevelGrid
{
   
    private Vector2Int foodGridPosition;
    private GameObject foodGameObject;
    private int width;
    private int height;
    private Snake playerOne;
    private Snake playerTwo;
    Vector2Int foodGameObjectPosition;
    public List<Vector2Int> foodPositionList;
    public List<GameObject> foodGameObjectList;

    public LevelGrid(int width, int height) {
        this.width = width;
        this.height = height;
    }

    private void Awake() {
        foodPositionList = new List<Vector2Int>();
        foodGameObjectList = new List<GameObject>();
    }

    public void Setup(Snake playerOne, Snake playerTwo) {
        this.playerOne = playerOne;
        this.playerTwo = playerTwo;
    }

    public void SpawnFood() {
        do {
            foodGridPosition = new Vector2Int(Random.Range(1, width -1), Random.Range(1, height -1));  
        } while(playerOne.GetFullSnakeGridPosition().IndexOf(foodGridPosition) != -1 || playerTwo.GetFullSnakeGridPosition().IndexOf(foodGridPosition) != -1);

        foodGameObject = new GameObject("Food", typeof(SpriteRenderer));
        foodGameObject.GetComponent<SpriteRenderer>().sprite = GameAssets.instance.foodSprite;
        foodGameObject.transform.position = new Vector3(foodGridPosition.x, foodGridPosition.y);
        foodGameObjectPosition = new Vector2Int(foodGridPosition.x, foodGridPosition.y);
        playerOne.foodPositionList.Add(foodGameObjectPosition);
        playerOne.foodGameObjectList.Add(foodGameObject);
        playerTwo.foodPositionList.Add(foodGameObjectPosition);
        playerTwo.foodGameObjectList.Add(foodGameObject);
    }

    public void CheckSnakeEatFood(Vector2Int snakeGridPosition) {
        if(snakeGridPosition == foodGameObjectPosition) {
            Object.Destroy(foodGameObject);
            SpawnFood();
            Debug.Log("Snake has hit Food");
        }
    }
    
    public void TrySnakeEatFood(Snake snake, GameObject foodObject) {
        Object.Destroy(foodObject);
        SpawnFood();
    }

    public bool SnakeHitBorder(Vector2Int gridPosition) {
        if(gridPosition.x <= 0) {
            return true;
        } 
        
        if(gridPosition.x > width - 1) {
            return true;
        }

        if(gridPosition.y <= 0) {
            return true;
        }

        if(gridPosition.y > height - 1) {
            return true;
        }

        return false;
    }

}
