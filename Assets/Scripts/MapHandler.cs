using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapHandler : MonoBehaviour
{
	public Transform endCreature;
	
	public float speed = 1f;
	[Tooltip("Read Only, defaults to # of children in transform")]
	public int currentLevelCount = 0;
	protected int startLevelCount;
	[Tooltip("Read Only, defaults to 1")]
	public int currentLevelIndex = 1;
	public int totalLevelCount = 100;
	[Tooltip("x = level index, y = speed change at index")]
	public List<Vector2> changeSpeedList;
	protected Dictionary<int, float> changeSpeedMap;
	// Stores info on each "level" (Info includes: hasTrash:true, hasLongTeeth:true, etc.)
    protected List<Dictionary<string,bool>> levelInfoList;
	
	protected Queue<Level> tubeQueue;
	
	protected Color fog;
	protected Color currentFog;
	
	protected GameObject gameManagerObject;
	protected GameManager gameManager;
	protected GameObject dialogueHandlerObject;
	protected DialogueHandler dialogueHandler;
	
	protected bool ended;
	protected bool endDialoguePlayed;
	protected bool merging;
	
	protected virtual void Awake()
	{
		ended = false;
		endDialoguePlayed = false;
		merging = false;
		
		fog = RenderSettings.fogColor;
		currentFog = fog;
		
		// End Creature
		if (endCreature != null) {
			endCreature.position = new Vector3(0,-100,0);
		}
		
		// Set up Tubes
		tubeQueue = new Queue<Level>();
		for (int i=0; i<transform.childCount; i++) {
			Level currentLevel = transform.GetChild(i).GetComponent<Level>();
			tubeQueue.Enqueue(currentLevel);
			Animator tubeAnimator = transform.GetChild(i).GetComponent<Animator>();
			if (tubeAnimator != null) {
				tubeAnimator.enabled = false;
				StartCoroutine(StartAnimation(i*0.2f, tubeAnimator));
			}
		}
		
		// Level Info
		startLevelCount = transform.childCount;
		currentLevelCount = transform.childCount;
		currentLevelIndex = 1;
		levelInfoList = new List<Dictionary<string,bool>>();
		for (int i=0; i<totalLevelCount; i++) {
			Dictionary<string,bool> levelInfo = new Dictionary<string,bool>();
			levelInfo.Add("hasTrash", false);
			levelInfoList.Add(levelInfo);
		}
		
		// Speed Change Map
		changeSpeedMap = new Dictionary<int, float>();
		for (int i=0; i<changeSpeedList.Count; i++) {
			changeSpeedMap.Add((int)changeSpeedList[i].x, changeSpeedList[i].y);
		}
		
		// Set Trash
		int levelsPerChunk = 10;
		int chunkCount = (int)Mathf.Floor(totalLevelCount/levelsPerChunk);
		int totalTrashCount = 0;
		
		for (int i=0; i<chunkCount; i++) {
			int startIndex = levelsPerChunk*i;
			int trashCount = Random.Range(2,5);
			List<int> chunkIndexRange = new List<int>();
			for (int o=startIndex; o<startIndex+levelsPerChunk; o++) {
				if (o > 1) { // Discludes first two levels at the very start
					chunkIndexRange.Add(o);
				}
			}
			// List<int> chunkIndexListWithTrash = new List<int>
			for (int o=0; o<trashCount; o++) {
				int chosenIndex = Random.Range(0,chunkIndexRange.Count);
				// chunkIndexListWithTrash.Add(chunkIndexRange[chosenIndex]);
				levelInfoList[chunkIndexRange[chosenIndex]]["hasTrash"] = true;
				// print(chunkIndexRange[chosenIndex]);
				totalTrashCount++;
				chunkIndexRange.RemoveAt(chosenIndex);
			}
		}
		
		for (int i=0; i<transform.childCount; i++) {
			Level firstTube = transform.GetChild(i).GetComponent<Level>();
			firstTube.Respawn(levelInfoList[i]["hasTrash"],
				i, totalLevelCount);
		}
		
		// Set Trash Count to GameManager
		gameManagerObject = GameObject.FindWithTag("GameController");
		if (gameManagerObject != null) {
			gameManager = gameManagerObject.GetComponent<GameManager>();
		}
		if (gameManager != null) {
			gameManager.SetTotalTrashCount(totalTrashCount);
			print(totalTrashCount);
		}
		
		// DialogueHandler
		dialogueHandlerObject = GameObject.FindWithTag("DialogueHandler");
		if (dialogueHandlerObject != null) {
			dialogueHandler = dialogueHandlerObject.GetComponent<DialogueHandler>();
		}
	}
	
	protected virtual void Start()
	{
		if (dialogueHandler != null) {
			dialogueHandler.AddDialogue("WASD/Arrow Keys - Move\nSpacebar - Show Score", 
				5f, true);
		}
	}
	
	protected virtual IEnumerator StartAnimation(float time, Animator animator)
	{
		yield return new WaitForSeconds(time);
		
		if (animator != null) {
			animator.enabled = true;
		}
	}
	
	protected virtual void Update()
	{
		float depth = (float)currentLevelIndex;
		float density = 0.07f;
		if (endCreature != null) {
			density = 0.07f*(endCreature.position.y/-100);
		}
		
		if (depth/totalLevelCount <= 1.5f) {
			currentFog = fog - 
				new Color((depth/totalLevelCount/2), (depth/totalLevelCount/2), 
					(depth/totalLevelCount/2),0);
		}
		RenderSettings.fogColor = 
			Color.Lerp(RenderSettings.fogColor, currentFog, Time.deltaTime*0.5f);
		RenderSettings.fogDensity = density;
	}
	
	protected virtual void FixedUpdate()
	{
		foreach (Level tube in tubeQueue) {
			tube.transform.position = 
				tube.transform.position + new Vector3(0,Mathf.Min(0.025f*speed, 100),0);
		}
		
		if (tubeQueue.Count > 0 && tubeQueue.Peek().transform.position.y >= 10f) {
			Level firstTube = tubeQueue.Dequeue();
			//if (currentLevelIndex <= totalLevelCount + 50) {	
				tubeQueue.Enqueue(firstTube);
				firstTube.transform.position = 
					new Vector3(0,(tubeQueue.Count*-10f)+10f,0);
				//if (currentLevelCount > totalLevelCount + 15) {
				firstTube.gameObject.SetActive(currentLevelCount <= totalLevelCount + 15);
				//}
				if ((currentLevelCount+1) < levelInfoList.Count) {
					firstTube.Respawn(
						levelInfoList[currentLevelCount+1]["hasTrash"], 
						currentLevelIndex, totalLevelCount);
				}
				else {
					firstTube.Respawn(false, currentLevelIndex, totalLevelCount);
				}
				currentLevelCount++;
			//}
			//else {
			//	firstTube.transform.position = new Vector3(0,100,0);
			//}
			currentLevelIndex = (currentLevelIndex < 999) ? 
				currentLevelIndex + 1 : 999;
			
		}
			
		if (changeSpeedMap.ContainsKey(currentLevelIndex)) {
			float lastSpeed = speed;
			speed = changeSpeedMap[currentLevelIndex];
			if (dialogueHandler != null && speed > lastSpeed) {
				dialogueHandler.AddDialogue(
					"Something pulls you in further...", 3f, true);
			}
		}
		
		if (!ended && currentLevelIndex >= totalLevelCount + 14) {
			ended = true;
		}
		if (ended && endCreature != null) {
			if (endCreature.position.y <= -30f || merging) {
				if (endCreature.position.y >= -5f)  {
					merging = false;
				}
				endCreature.position = 
					endCreature.position + new Vector3(0,0.1f,0);
				if (!endDialoguePlayed && endCreature.position.y >= -30f && 
				dialogueHandler != null && gameManager != null) {
					endDialoguePlayed = true;
					if (gameManager.GetAllTrashCollected()) {
						dialogueHandler.AddDialogue(
							"", 2f, true);
						dialogueHandler.AddDialogue(
							"Human, you have done well.", 4f, false);
						dialogueHandler.AddDialogue(
							"For far too long, our ocean has long been polluted by this awful substance.", 6f, false);
						dialogueHandler.AddDialogue(
							"Let us merge as one.", 4f, false);
						dialogueHandler.AddDialogue(
							"Together, we will rid the ocean of this filth.", 4f, false);
						gameManager.PlayEndingGood(21);
						StartCoroutine(EndCreatureMerge(20.5f));
					}
					else {
						dialogueHandler.AddDialogue(
							"", 2f, true);
						dialogueHandler.AddDialogue(
							"Such a disappointment.", 4f, false);
						dialogueHandler.AddDialogue(
							"You have failed to cleanse us.", 4f, false);
						dialogueHandler.AddDialogue(
							"Begone.", 4f, false);
						gameManager.PlayEndingBad(15);
					}
				}
			}
		}
	}
	
	protected virtual IEnumerator EndCreatureMerge(float delay)
	{
		yield return new WaitForSeconds(delay);
		
		merging = true;
	}
}
