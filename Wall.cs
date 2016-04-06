using UnityEngine;
using System.Collections;

public class Wall : MonoBehaviour {

	public Sprite dmgSprite;	//Sprite to display to show player they dmg'd the wall
	public int hp = 4;			//Wall health


	private SpriteRenderer spriteRenderer;
	// Use this for initialization
	void Awake () {
		spriteRenderer = GetComponent<SpriteRenderer> ();
	}
	
	public void DamageWall(int loss)
	{
		if(spriteRenderer.sprite != dmgSprite)
			spriteRenderer.sprite = dmgSprite;

		hp -= loss;

		if (hp <= 0) 
		{
			gameObject.SetActive (false);
		}
	}
}
