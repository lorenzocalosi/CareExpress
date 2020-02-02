using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class MapHotSpot : EventHotSpot {
	#region Fields

	public List<GameObject> thisMap;
	public List<GameObject> otherMap;

	public Animator animator;
	
	[FoldoutGroup("Songs")]
	public string songToDestination, songToStop, songToStartNext;

	public Vector3 teleportPosition;

	#endregion

	#region Methods

	public override void CallShowEvent() {
		MapChanger.Instance.animator.enabled = true;
		Debug.Log($"Event Recived");
		SoundManager.Instance.StartMapThemeAfterTime(songToDestination, songToStop, songToStartNext);
		PlayerMovement.Instance.GetComponentInChildren<SpriteRenderer>().enabled = false;
		animator.SetTrigger("PlaneToHawaii");
	}

	public void AnimationOver() {
		Debug.Log("Arrived");
	}

	public void ChangeMap() {
		foreach (GameObject gameObject in thisMap) {
			gameObject.SetActive(false);
		}
		foreach (GameObject gameObject in otherMap) {
			gameObject.SetActive(true);
		}
	}

	#endregion
}
