using UnityEngine;
using System; //Gives us Serializable
using System.Collections.Generic;
using Random = UnityEngine.Random; //Says we are using Unity's random not System's


public class BoardManager : MonoBehaviour {

	[Serializable]
	public class Count
	{
		public int minimum;
		public int maximum;

		public Count(int min, int max)
		{
			minimum = min;
			maximum = max;
		}
	}

	public int columns = 8;
	public int rows = 8;

	public GameObject exit; //Holds exit prefab
	public GameObject[] floorTiles; //Holds floor prefabs
	public GameObject[] outerWallTiles; //Holds outer wall prefabs
	public GameObject[] wallTiles; //Holds wall prefabs
	public GameObject[] foodTiles; //Holds food prefabs
	public GameObject[] enemyTiles; //Holds enemmy prefabs

	private Transform boardHolder; //Parent Game Objects to this to keep inspector neat
	private List<Vector3> gridPositions = new List<Vector3>();

	void InitialiseList()
	{
		gridPositions.Clear ();

		for (int i = 1; i < columns-1; i++) 
		{
			for(int j = 1 ; j < rows-1; j++)
			{
				gridPositions.Add (new Vector3(i,j,0f));
			}
		}
	}

	void BoardSetup()
	{
		boardHolder = new GameObject ("Board").transform;

		//Sets up floor tiles and outer walls
		for (int i = -1; i < columns + 1; i++) 
		{
			for(int j = -1; j < rows + 1; j++)
			{
				GameObject toInstantiate = floorTiles[Random.Range (0, floorTiles.Length)];
				if( (i == -1 || i == columns)||(j==-1 || j == rows))
				{
					toInstantiate = outerWallTiles[Random.Range( 0, outerWallTiles.Length)];
				}

				GameObject instance = Instantiate(toInstantiate, new Vector3(i,j,0f), Quaternion.identity) as GameObject;
				instance.transform.SetParent (boardHolder); //Parent new tiles to the board holder's transform
			}
		}
	}

	Vector3 RandomPosition()
	{
		int randomIndex = Random.Range (0, gridPositions.Count);
		Vector3 randomPosition = gridPositions [randomIndex];

		gridPositions.RemoveAt (randomIndex); //Make Sure we dont spawn at same location twice

		return randomPosition;
	}

	void LayoutObjectAtRandom(GameObject[] tileArray, int min, int max)
	{
		int objectCount = Random.Range (min, max + 1);

		for (int i = 0; i < objectCount; i++) 
		{
			Vector3 spawnPosition = RandomPosition ();
			GameObject tileChoice = tileArray[Random.Range (0, tileArray.Length)];
			Instantiate (tileChoice, spawnPosition, Quaternion.identity);
		}
	}

	//Only public func, called by Game Manager
	public void SetupScene(int level, BoardConfig currentBoardConfig)
	{
		BoardSetup ();
		InitialiseList ();
        Debug.Log(currentBoardConfig);
		LayoutObjectAtRandom (wallTiles, currentBoardConfig.wallCount.minimum, currentBoardConfig.wallCount.maximum);
		LayoutObjectAtRandom (foodTiles, currentBoardConfig.foodCount.minimum, currentBoardConfig.foodCount.maximum);
		LayoutObjectAtRandom (enemyTiles, currentBoardConfig.enemyCount.minimum, currentBoardConfig.enemyCount.maximum);

		Instantiate (exit, new Vector3(columns - 1, rows - 1, 0f), Quaternion.identity);
	}

}
