using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class MapHotSpot : EventHotSpot {
	#region Fields

	public List<GameObject> thisMap;
	public List<GameObject> otherMap;
	
	[FoldoutGroup("Songs")]
	public string songToDestination, songToStop, songToStartNext;

	public Vector3 teleportPosition;

	#endregion

	#region Methods

	public override void CallShowEvent() {
		Debug.Log($"Event Recived");
		if (CameraHandler.Instance != null) {
			CameraHandler.Instance.ZoomTowards(PlayerMovement.Instance.transform.position, ChangeMap);
			SoundManager.Instance.StartMapThemeAfterTime(songToDestination, songToStop, songToStartNext);
			Debug.LogWarning("REMEMBER TO CHANGE KEY STRING SONG");
		}
		else {
			Event.EventHandler.Instance.ShowEvent(theEvent);
		}
	}

	private void ChangeMap() {
		foreach (GameObject gameObject in thisMap) {
			gameObject.SetActive(false);
		}
		foreach (GameObject gameObject in otherMap) {
			gameObject.SetActive(true);
		}
		PlayerMovement.Instance.canMove = true;
	}

	#endregion
}
