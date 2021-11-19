using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{	
	public GameObject endCutsceneGood;
	public GameObject endCutsceneBad;
	public Animator trashDisplayAnimator;
	protected RectTransform trashDisplayTransform;
	public TextMeshProUGUI trashDisplay;
	protected Animator endCutsceneGoodAnimator;
	protected Animator endCutsceneBadAnimator;

	protected float showTrash;
	protected int totalTrashCount;
	public int trashCollected;
	
    protected virtual void Awake()
	{
		showTrash = 0;
		trashCollected = 0;
		
		if (trashDisplayAnimator != null) {
			trashDisplayTransform = trashDisplayAnimator.GetComponent<RectTransform>();
		}
		
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
		
		// Instant show
		if (Time.timeScale == 0) {
			ShowTrash(true);
		}
		else {
			ShowTrash(false);
		}
	}
	
	public virtual void CollectTrash()
	{
		trashCollected++;
		showTrash = 3f;
		// print(trashCollected);
	}
	
	public virtual void ShowTrash()
	{
		showTrash = Mathf.Max(showTrash, 0.5f);
	}
	
	public virtual void ShowTrash(bool instant)
	{
		if (trashDisplayTransform != null && trashDisplayAnimator != null) {
			trashDisplayAnimator.enabled = !instant;
			if (instant) {
				trashDisplayTransform.anchoredPosition = new Vector2(3.5f,3.5f);
			}
		}
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
		
		StartCoroutine(EndScene());
	}
	
	protected virtual IEnumerator EndScene()
	{
		yield return new WaitForSeconds(5f);
		
		SceneManager.LoadScene("Title");
	}
	
}
