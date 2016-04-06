using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	public float levelStartDelay = 2f;
	public static GameManager instance = null; 			//Static GameManager instance allows for singleton
	public BoardManager boardScript;
    public BoardConfig boardConfig;                     //Hold onto current config
	public float turnDelay = 0.1f;
	public int playerFoodPoints = 100;
	public bool playerTurn = true;

	private Text debugText;
	private Text levelText;
	private GameObject levelImage;
	private int level = 1;
	private List<Enemy> enemies;
	[SerializeField]private bool enemiesMoving;
	private bool doingSetup = true;

	// Use this for initialization
	void Awake () 
	{
		if (instance == null) 
		{
			instance = this;
		} 
		else if (instance != this) 
		{
			Destroy (gameObject);
		}

		DontDestroyOnLoad (gameObject);

		enemies = new List<Enemy>();
		boardScript = GetComponent<BoardManager> ();
		InitGame ();
	}

	void OnDestroy()
	{
		Debug.Log("GameManager Destroyed");
	}
	
	private void OnLevelWasLoaded(int index)
	{
		level++;

        boardConfig = boardConfig.GetNext();

		InitGame ();
	}

	void InitGame()
	{
		doingSetup = true;

		debugText = GameObject.Find ("DebugText").GetComponent<Text>();

		levelImage = GameObject.Find ("LevelImage");
		levelText = GameObject.Find ("LevelText").GetComponent<Text> ();
		levelText.text = "Day " + level;
		levelImage.SetActive (true);

		enemies.Clear ();
		boardScript.SetupScene (level, boardConfig);
		Invoke ("HideLevelImage", levelStartDelay);
	}

	public void HideLevelImage()
	{
		levelImage.SetActive(false);
		doingSetup = false;
	}

	public void GameOver()
	{
		levelText.text = "Days survived: " + (level - 1);
		levelImage.SetActive (true);
		enabled = false; 		//disable game manager when game is over
	}

	// Update is called once per frame
	void Update () 
	{
		debugText.text = (playerTurn?"Player: true":"Player: false") + 
						(enemiesMoving?"  Enemy: true":"  Enemy: false") ;
		
		if (playerTurn || enemiesMoving || doingSetup)
			return;

		StartCoroutine (MoveEnemies ());		//Not PLayers turn and enemies arent moving yet
	}

	//Lets enemies add hemselves to list
	public void AddEnemyToList (Enemy script)
	{
		enemies.Add (script);
	}

	IEnumerator MoveEnemies()
	{
		enemiesMoving = true;
		yield return new WaitForSeconds (turnDelay);
		if (enemies.Count == 0) 
		{
			yield return new WaitForSeconds(turnDelay);
		}

		for(int i = 0; i < enemies.Count; ++i)
		{
			enemies[i].MoveEnemy();
			yield return new WaitForSeconds(enemies[i].moveTime);
		}

		playerTurn = true;
		enemiesMoving = false;
	}
}
