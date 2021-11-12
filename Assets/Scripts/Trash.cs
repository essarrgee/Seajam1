using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trash : MonoBehaviour
{
	protected GameObject playerObject;
	protected Player player;
	protected Collider playerCollider;
	protected GameObject gameManagerObject;
	protected GameManager gameManager;
	
	public bool collected = false;
	
	
	protected virtual void Awake()
	{
		playerObject = GameObject.FindWithTag("Player");
		if (playerObject != null) {
			player = playerObject.GetComponent<Player>();
			playerCollider = playerObject.GetComponent<Collider>();
		}
		
		gameManagerObject = GameObject.FindWithTag("GameController");
		if (gameManagerObject != null) {
			gameManager = gameManagerObject.GetComponent<GameManager>();
		}
		
		collected = false;
	}
	
    protected virtual void OnTriggerEnter(Collider collision)
	{
		if (!collected && gameManager != null && player != null && 
		playerCollider != null && collision == playerCollider) {
			collected = true;
			gameManager.CollectTrash();
			gameObject.SetActive(false);
		}
	}
}
