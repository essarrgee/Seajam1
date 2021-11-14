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
	
	protected List<float> hazardSpawnAngleList;
	
	public virtual void Respawn(bool hasTrash)
	{	
		if (trash != null) {
			int spawnAngle = Random.Range(0,360)
			firstTube.transform.eulerAngles = 
					new Vector3(0,spawnAngle,0);
			
			hazardSpawnAngleList = new List<float>();
			// Generate list of angles hazards can spawn in
			// Remove angle that level spawn in
			
			gameObject.SetActive(hasTrash);
			trash.collected = false;
			transform.localPosition = 
				new Vector3(0,0,Random.Range(0.5f,3f));
		}
	}
}
