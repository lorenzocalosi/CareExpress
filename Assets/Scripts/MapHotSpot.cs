using System.Collections.Generic;
using UnityEngine;

public class MapHotSpot : EventHotSpot {
	#region Fields

	public List<GameObject> thisMap;
	public List<GameObject> otherMap;

	public Vector3 teleportPosition;

	#endregion

	#region Methods

	public override void CallShowEvent() {
		Debug.Log($"Event Recived");
		if (CameraHandler.Instance != null) {
			CameraHandler.Instance.ZoomTowards(PlayerMovement.Instance.transform.position, ChangeMap);
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
