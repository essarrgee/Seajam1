using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{	
	public GameObject endCutsceneGood;
	public GameObject endCutsceneBad;
	protected Animator endCutsceneGoodAnimator;
	protected Animator endCutsceneBadAnimator;

	protected int totalTrashCount;
	public int trashCollected;
	
    protected virtual void Awake()
	{
		trashCollected = 0;
		
		if (endCutsceneGood != null) {
			endCutsceneGoodAnimator = endCutsceneGood.GetComponent<Animator>();
		}
		
		if (endCutsceneBad != null) {
			endCutsceneBadAnimator = endCutsceneBad.GetComponent<Animator>();
		}
	}
	
	public virtual void CollectTrash()
	{
		trashCollected++;
		// print(trashCollected);
	}
	
	public virtual void SetTotalTrashCount(int total)
	{
		this.totalTrashCount = total;
	}
	
	public virtual bool GetAllTrashCollected()
	{
		return (trashCollected >= totalTrashCount) ? true : false;
	}
	
	public virtual void PlayEndingGood(float delay)
	{
		StartCoroutine(PlayEndingDelay(delay, endCutsceneGoodAnimator));
	}
	
	public virtual void PlayEndingBad(float delay)
	{
		StartCoroutine(PlayEndingDelay(delay, endCutsceneBadAnimator));
	}
	
	protected virtual IEnumerator PlayEndingDelay(float delay, Animator animator)
	{
		yield return new WaitForSeconds(delay);
		
		if (animator != null) {
			animator.SetTrigger("Play");
		}
	}
	
}
