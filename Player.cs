using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Player : MovingObject {

	public int wallDamage = 1;					//Damage player applies to wall tiles
	public int pointsPerFood = 10;
	public int pointsPerSoda = 20;

	public float restartLevelDelay = 1f;

	public Text foodText;
	private Animator animator;
	private int food;							//Store during play and pass to GM to save between scenes

	// Use this for initialization
	protected override void Start () 
	{
		foodText.text = "Food: " + food;
		animator = GetComponent<Animator> ();

		food = GameManager.instance.playerFoodPoints;

		base.Start();
	}

	//Function called when gameobject is disabled
	private void OnDisable()
	{
		GameManager.instance.playerFoodPoints = food;
	}

	// Update is called once per frame
	void Update () 
	{
		if (!GameManager.instance.playerTurn)
			return;

		int horizontal = 0;
		int vertical = 0;

		horizontal = (int)(Input.GetAxisRaw ("Horizontal"));
		vertical = (int)(Input.GetAxisRaw ("Vertical"));

		if (horizontal != 0)
			vertical = 0;

		if (horizontal != 0 || vertical != 0) 
		{
			AttemptMove<Wall> (horizontal, vertical);
		}
	}

	protected override void AttemptMove<T>(int xDir, int yDir)
	{
		food--;
		foodText.text = "Food: " + food;
		base.AttemptMove<T> (xDir, yDir);

		RaycastHit2D hit;

		if(Move (xDir, yDir, out hit))
		{
			//Play sfx;
		}

		CheckGameOver ();

		GameManager.instance.playerTurn = false;
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag == "Exit") {
			Invoke ("Restart", restartLevelDelay);			//Calls Restart with delay
			enabled = false;
		} else if (other.tag == "Food") {
			foodText.text = "Food: " + food + " + *" + pointsPerFood + "*";
			food += pointsPerFood;
			other.gameObject.SetActive (false);
		} else if (other.tag == "Soda") {
			foodText.text = "Food: " + food + " + *" + pointsPerSoda + "*";
			food += pointsPerSoda;
			other.gameObject.SetActive(false);
		}

	}

	protected override void OnCantMove<T>(T component)
	{
		Wall hitWall = component as Wall;				//Casts parameter to wall type
		hitWall.DamageWall (wallDamage);
		animator.SetTrigger ("playerChop");
	}

	private void Restart()
	{
		Application.LoadLevel (Application.loadedLevel);	//Load last scene that was loaded (main scene == only scene since procedural gen)
	}

	public void LoseFood(int loss)
	{
		animator.SetTrigger ("playerHit");
		foodText.text = "Food: " + food + " + *" + pointsPerSoda + "*";
		food -= loss;
		CheckGameOver ();
	}

	private void CheckGameOver()
	{
		if (food <= 0)
			GameManager.instance.GameOver ();
	}
}
