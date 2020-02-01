using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class EventHotSpot : MonoBehaviour {
	#region Fields

	// Static

	// Public
	public GameObject aButton;
	public Event.Event theEvent;
	// Hidden Public
	private PlayerMovement playerMovement;
	// Properties

	// Components

	// Events

	#endregion

	#region Methods

	public virtual void CallShowEvent() {
		Debug.Log($"Event Recived");
		if (theEvent != null) {
			if (CameraHandler.Instance != null) {
				CameraHandler.Instance.ZoomTowards(PlayerMovement.Instance.transform.position, () => Event.EventHandler.Instance.ShowEvent(theEvent));
			}
			else {
				Event.EventHandler.Instance.ShowEvent(theEvent);
			}
		}
	}

	#endregion
}
