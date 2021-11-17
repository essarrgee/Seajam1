using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{	
	public GameObject endCutsceneGood;
	public GameObject endCutsceneBad;
	public Animator trashDisplayAnimator;
	public TextMeshProUGUI trashDisplay;
	protected Animator endCutsceneGoodAnimator;
	protected Animator endCutsceneBadAnimator;

	protected float showTrash;
	protected int totalTrashCount;
	public int trashCollected;
	
    protected virtual void Awake()
	{
		showTrash = 5;
		trashCollected = 0;
		
		if (endCutsceneGood != null) {
			endCutsceneGoodAnimator = endCutsceneGood.GetComponent<Animator>();
		}
		
		if (endCutsceneBad != null) {
			endCutsceneBadAnimator = endCutsceneBad.GetComponent<Animator>();
		}
	}
	
	protected virtual void Update()
	{
		showTrash = showTrash > 0 ? showTrash - Time.deltaTime : 0;
		
		if (trashDisplayAnimator != null) {
			trashDisplayAnimator.SetBool("Show", showTrash > 0);
		}
		if (trashDisplay != null) {
			trashDisplay.text = 
				trashCollected.ToString() + "/" + totalTrashCount.ToString();
		}
	}
	
	public virtual void CollectTrash()
	{
		trashCollected++;
		showTrash = 3f;
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
