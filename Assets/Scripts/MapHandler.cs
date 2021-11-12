using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapHandler : MonoBehaviour
{
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
	
	
	protected virtual void Awake()
	{
		fog = RenderSettings.fogColor;
		currentFog = fog;
		
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
			int trashCount = Random.Range(3,5);
			List<int> chunkIndexRange = new List<int>();
			for (int o=startIndex; o<startIndex+levelsPerChunk; o++) {
				chunkIndexRange.Add(o);
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
			SetTrash(firstTube, i);
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
		
		currentFog = fog - 
			new Color((depth/totalLevelCount/2), (depth/totalLevelCount/2), 
				(depth/totalLevelCount/2),0);
		RenderSettings.fogColor = 
			Color.Lerp(RenderSettings.fogColor, currentFog, Time.deltaTime*0.4f);
	}
	
	protected virtual void FixedUpdate()
	{
		foreach (Level tube in tubeQueue) {
			tube.transform.position = 
				tube.transform.position + new Vector3(0,0.025f*speed,0);
		}
		if (tubeQueue.Count > 0 && tubeQueue.Peek().transform.position.y >= 10f) {
			if (currentLevelCount <= totalLevelCount + startLevelCount) {
				Level firstTube = tubeQueue.Dequeue();
				tubeQueue.Enqueue(firstTube);
				firstTube.transform.position = 
					new Vector3(0,(tubeQueue.Count*-10f)+10f,0);
				firstTube.transform.eulerAngles = 
					new Vector3(0,Random.Range(0f,360f),0);
				SetTrash(firstTube, (currentLevelCount+1));
				currentLevelCount++;
			}
			currentLevelIndex = (currentLevelIndex < 999) ? 
				currentLevelIndex + 1 : 999;
			if (changeSpeedMap.ContainsKey(currentLevelIndex)) {
				speed = changeSpeedMap[currentLevelIndex];
				if (dialogueHandler != null && speed > 0) {
					dialogueHandler.AddDialogue(
						"Something pulls you in further...", 3f, true);
				}
			}
		}
	}
	
	protected virtual void SetTrash(Level tube, int index)
	{
		if (tube != null && tube.trash != null && index < levelInfoList.Count) {
			tube.trash.gameObject.SetActive(
				levelInfoList[index]["hasTrash"]);
			tube.trash.collected = false;
			tube.trash.transform.localPosition = 
				new Vector3(0,0,Random.Range(0.5f,3f));
		}
	}
}