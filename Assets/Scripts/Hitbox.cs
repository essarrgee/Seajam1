using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
	protected GameObject playerObject;
	protected Player player;
	protected Collider playerCollider;
	
	protected virtual void Awake()
	{
		playerObject = GameObject.FindWithTag("Player");
		if (playerObject != null) {
			player = playerObject.GetComponent<Player>();
			playerCollider = playerObject.GetComponent<Collider>();
		}
	}
	
    protected virtual void OnTriggerEnter(Collider collision)
	{
		if (player != null && playerCollider != null && collision == playerCollider) {
			player.Damage();
		}
	}
}
