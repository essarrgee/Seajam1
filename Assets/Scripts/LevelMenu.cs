using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelMenu : MonoBehaviour
{
	public float animatorDelay = 0f;
	
    public Trash trash;
	public bool hasTrashDefault;
	
	public List<Transform> hazardList;
	
	protected List<Transform> hazardAllList; // All transform.GetChild() instances
	protected List<List<Transform>> hazardCollectiveList;
	protected List<int> hazardSpawnAngleList;
	
	protected Animator animator;
	
	
	protected virtual void Awake()
	{
		animator = GetComponent<Animator>();
		if (animator != null) {
			animator.enabled = false;
		}
		StartCoroutine(DelayAnimator());
		
		hazardAllList = new List<Transform>();
		for (int i=0; i<transform.Find("Hitbox").childCount; i++) {
			hazardAllList.Add(transform.Find("Hitbox").GetChild(i));
		}
		hazardCollectiveList = new List<List<Transform>>();
		hazardCollectiveList.Add(hazardList);
		
		int spawnAngle = Random.Range(0,36)*10;
		transform.eulerAngles = new Vector3(0,spawnAngle,0);
		bool hasTrash = Random.Range(1,7) > 3 || hasTrashDefault;
		
		
		GenerateHazards(spawnAngle, hasTrash);
		
		if (trash != null) {
			trash.gameObject.SetActive(hasTrash);
			trash.collected = false;
			trash.transform.localPosition = 
				new Vector3(0,Random.Range(-3f,3f),Random.Range(0.5f,3f));
		}
	}
	
	protected virtual IEnumerator DelayAnimator()
	{
		yield return new WaitForSeconds(animatorDelay);
		
		if (animator != null) {
			animator.enabled = true;
		}
	}
	
	protected virtual void GenerateHazards(int spawnAngle, bool hasTrash)
	{
		// Initialize all child hazards
		for (int i=0; i<hazardAllList.Count; i++) {
			hazardAllList[i].gameObject.SetActive(false);
		}
		
		// Generate list of angles hazards can spawn in
		// Remove angle and adjacent angles that level spawn in
		hazardSpawnAngleList = new List<int>();
		for (int i=0; i<36; i++) {
			if (spawnAngle != i*10) {
				hazardSpawnAngleList.Add(i*10);
			}
		}
		
		List<Transform> currentHazardList = hazardCollectiveList[0];
		if (currentHazardList != null) {
			float previousYPosition = 0;
			
			for (int i=0; i<currentHazardList.Count; i++) {
				
				bool willSpawn = Random.Range(0,20) <= 12 ? true : false;
				currentHazardList[i].gameObject.SetActive(willSpawn);
				
				if (willSpawn) {
					bool sameYPosition = Random.Range(0,18) <= 11 ? true : false;
					int angleIndex = Random.Range(0, hazardSpawnAngleList.Count);
					float yPosition = sameYPosition ? previousYPosition : 
						Random.Range(-4f,4f);
					currentHazardList[i].eulerAngles = 
						new Vector3(0,hazardSpawnAngleList[angleIndex],0);
					currentHazardList[i].localPosition = 
						new Vector3(0,yPosition,0);
					hazardSpawnAngleList.RemoveAt(angleIndex);
					previousYPosition = yPosition;
				}
				
			}
			
		}
		
	}
}
