using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    public Trash trash;
	
	public List<Transform> hazardListLevel1;
	public List<Transform> hazardListLevel2;
	public List<Transform> hazardListLevel3;
	public List<Transform> hazardListLevel4;
	public List<Transform> hazardListLevel5;
	public List<Transform> hazardListLevel6;
	
	protected List<Transform> hazardAllList; // All transform.GetChild() instances
	protected List<List<Transform>> hazardCollectiveList;
	protected List<int> hazardSpawnAngleList;
	
	
	protected virtual void Start()
	{
		if (hazardAllList == null) {
			hazardAllList = new List<Transform>();
			for (int i=0; i<transform.Find("Hitbox").childCount; i++) {
				hazardAllList.Add(transform.Find("Hitbox").GetChild(i));
			}
			hazardCollectiveList = new List<List<Transform>>();
			hazardCollectiveList.Add(hazardListLevel1);
			hazardCollectiveList.Add(hazardListLevel2);
			hazardCollectiveList.Add(hazardListLevel3);
			hazardCollectiveList.Add(hazardListLevel4);
			hazardCollectiveList.Add(hazardListLevel5);
			hazardCollectiveList.Add(hazardListLevel6);
		}
	}
	
	public virtual void Respawn(bool hasTrash, int currentLevelIndex, int totalLevelCount)
	{	
		
		int spawnAngle = Random.Range(0,36)*10;
		transform.eulerAngles = new Vector3(0,spawnAngle,0);
		
		float levelRatio = (float)(currentLevelIndex)/(float)(totalLevelCount);
		GenerateHazards(spawnAngle, hasTrash, currentLevelIndex, levelRatio);
			
		if (trash != null) {
			trash.gameObject.SetActive(hasTrash);
			trash.collected = false;
			trash.transform.localPosition = 
				new Vector3(0,Random.Range(-3f,3f),Random.Range(0.5f,2f));
		}
	}
	
	protected virtual void GenerateHazards(int spawnAngle, bool hasTrash, 
	int currentLevelIndex, float levelRatio)
	{	
		// Needs to call since MapHandler has priority and
		// will call Respawn() on this before this gets to call Start()/Awake()
		Start();
		
		// Initialize all child hazards
		for (int i=0; i<hazardAllList.Count; i++) {
			hazardAllList[i].gameObject.SetActive(false);
		}
		
		if (currentLevelIndex > 1) {
			// Generate list of angles hazards can spawn in
			// Remove angle and adjacent angles that level spawn in
			hazardSpawnAngleList = new List<int>();
			for (int i=0; i<36; i++) {
				if (spawnAngle != i*10) {
					hazardSpawnAngleList.Add(i*10);
				}
			}
			
			int level = (int)Mathf.Floor(levelRatio*hazardCollectiveList.Count);
			if (level < hazardCollectiveList.Count) {
				List<Transform> currentHazardList = hazardCollectiveList[level];
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
		
	}
}
