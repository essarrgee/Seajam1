using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eye : MonoBehaviour
{
	public Transform model;
	
	public float openRange = 13f;
	
	protected Animator animator;
	
	protected GameObject playerObject;
	
	protected bool opened;
	
	
    protected virtual void Awake()
	{
		animator = GetComponent<Animator>();
		
		playerObject = GameObject.FindWithTag("Player");
		
		opened = false;
	}
	
	protected virtual void OnEnable()
	{
		if (animator != null) {
			animator.SetTrigger("Close");
		}
		opened = false;
	}
	
	protected virtual void Update()
	{
		if (playerObject != null && animator != null && !opened && model != null &&
		(playerObject.transform.position - model.position).magnitude <= openRange
		&& gameObject.activeSelf) {
			opened = true;
			StartCoroutine(Open(Random.Range(0f,0.4f)));
		}
	}
	
	protected virtual IEnumerator Open(float delay)
	{
		yield return new WaitForSeconds(delay);
		
		animator.SetTrigger("Open");
	}
}
