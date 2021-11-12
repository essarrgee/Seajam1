using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{	
	protected int totalTrashCount;
	protected int trashCollected;
	
    protected virtual void Awake()
	{
		trashCollected = 0;
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
}
