using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHandler : MonoBehaviour
{

    [SerializeField] private Snake playerOne;
    [SerializeField] private Snake playerTwo;
    private LevelGrid levelGrid;
    private List<Vector2Int> playerOneSnakeBodyPositionList;
    private List<Vector2Int> playerTwoSnakeBodyPositionList;
    private List<Vector2Int> foodPositionList;
    private List<GameObject> foodGameObjectList;

    void Start()
    {
        Debug.Log("GameHandler.Start");

        levelGrid = new LevelGrid(20, 20);

        playerOne.Setup(levelGrid, 1);
        playerTwo.Setup(levelGrid, 2);
        
        levelGrid.Setup(playerOne, playerTwo);

        playerOneSnakeBodyPositionList = playerOne.snakeBodyPositionList;
        playerTwoSnakeBodyPositionList = playerTwo.snakeBodyPositionList;
        foodPositionList = playerOne.foodPositionList;
        foodPositionList.AddRange(playerTwo.foodPositionList);
        foodGameObjectList = playerOne.foodGameObjectList;
        foodGameObjectList.AddRange(playerTwo.foodGameObjectList);

        levelGrid.SpawnFood();
        levelGrid.SpawnFood();
    }

    void FixedUpdate() {

        if(playerOne.PlayerStatus() == 2) {
            playerTwo.SetPlayerDead();
        }
        if(playerTwo.PlayerStatus() == 2) {
            playerOne.SetPlayerDead();
        }

        for (int i = 0; i < playerOneSnakeBodyPositionList.Count; i++) {
            Vector2Int snakeBodyPosition = playerOneSnakeBodyPositionList[i];

            if (playerTwo.GridPosition() == snakeBodyPosition) {
                playerTwo.SetPlayerDead();
                Debug.Log("Player 2 Hit Player 1");
                    
            }

            playerOneSnakeBodyPositionList.RemoveAt(i);
            break;

        }

        for (int i = 0; i < playerTwoSnakeBodyPositionList.Count; i++) {
            Vector2Int snakeBodyPosition = playerTwoSnakeBodyPositionList[i];

            if (playerOne.GridPosition() == snakeBodyPosition) {
                playerOne.SetPlayerDead();
                Debug.Log("Player 1 Hit Player 2");

            }

                playerTwoSnakeBodyPositionList.RemoveAt(i);
                break;
        }

        for (int i = 0; i < foodPositionList.Count; i++) {
            Vector2Int foodPosition = foodPositionList[i];
            GameObject foodGameObject = foodGameObjectList[i];
            
            if (playerOne.GridPosition() == foodPosition) {
                Debug.Log("Snake hit food!");
                levelGrid.TrySnakeEatFood(playerOne, foodGameObject);
                playerOne.SnakeAteFood();

                foodPositionList.RemoveAt(i);
                foodGameObjectList.RemoveAt(i);
                    
                break;
            } 
            
            if (playerTwo.GridPosition() == foodPosition) {
                Debug.Log("Snake hit food!");
                levelGrid.TrySnakeEatFood(playerTwo, foodGameObject);
                playerTwo.SnakeAteFood();
                
                foodPositionList.RemoveAt(i);
                foodGameObjectList.RemoveAt(i);
                    
                break;
            }
        }

        for (int i = 0; i < foodPositionList.Count; i++) {
            Vector2Int foodPosition = foodPositionList[i];
            GameObject foodGameObject = foodGameObjectList[i];

            if (playerOne.GridPosition() == foodPosition) {
                Debug.Log("Snake hit food!");
                levelGrid.TrySnakeEatFood(playerOne, foodGameObject);
                playerOne.SnakeAteFood();
                playerOne.RemoveFoodLists(i);
                playerTwo.RemoveFoodLists(i);
                foodPositionList.RemoveAt(i);
                foodGameObjectList.RemoveAt(i);
                break;
            } 
            if (playerTwo.GridPosition() == foodPosition) {
                Debug.Log("Snake hit food!");
                levelGrid.TrySnakeEatFood(playerTwo, foodGameObject);
                playerTwo.SnakeAteFood();
                playerOne.RemoveFoodLists(i);
                playerTwo.RemoveFoodLists(i);
                foodPositionList.RemoveAt(i);
                foodGameObjectList.RemoveAt(i);
                break;
            } 
        }

    }    
}
