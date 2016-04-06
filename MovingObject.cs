using UnityEngine;
using System.Collections;

public abstract class MovingObject : MonoBehaviour {

	public float moveTime = 0.2f;
	public LayerMask blockingLayer;

	private BoxCollider2D boxCollider;
	private Rigidbody2D rb2D;
	private float inverseMoveTime;

	// Use this for initialization
	protected virtual void Start ()
	{
		boxCollider = GetComponent<BoxCollider2D> ();	//Store components
		rb2D = GetComponent<Rigidbody2D> ();			
		inverseMoveTime = 1f / moveTime; 				//For computational efficiency, we do this once
	}

	protected bool Move(int xDir, int yDir, out RaycastHit2D hit)
	{
		Vector2 start = transform.position;				//Implicitly converts to Vec2
		Vector2 end = start + new Vector2 (xDir, yDir); 

		boxCollider.enabled = false;
		hit = Physics2D.Linecast (start, end, blockingLayer);
		boxCollider.enabled = true;

		//RayCast didn't hit anything in box, can move there
		if (hit.transform == null) 
		{
			StartCoroutine(SmoothMovement (end));
			return true;
		}

		return false;									//Hit a blocking layer member, can't move
	}

	protected virtual void AttemptMove <T> (int xDir, int yDir) //Generic lets us use this for Player and Enemy
		where T : Component
	{
		RaycastHit2D hit;
		bool canMove = Move (xDir, yDir, out hit);

		if (hit.transform == null) 
		{
			return;
		}

		T hitComponent = hit.transform.GetComponent<T> ();

		if (!canMove && hitComponent != null)
			OnCantMove (hitComponent);
		 
	}


	protected IEnumerator SmoothMovement (Vector3 end)  //Syntax for Coroutine
	{
		float sqrRemainingDistance = (transform.position - end).sqrMagnitude;

		while (sqrRemainingDistance > float.Epsilon) 
		{
			Vector3 newPosition = Vector3.MoveTowards (rb2D.position, end, inverseMoveTime*Time.deltaTime);
			rb2D.MovePosition (newPosition);
			sqrRemainingDistance = (transform.position - end).sqrMagnitude;
			yield return null;
		}
	}

	protected abstract void OnCantMove <T> (T component) 
		where T : Component;
}
