using System;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class EventHotSpot : MonoBehaviour
{
	#region Fields

	// Static

	// Public
	public Event.Event theEvent;
	// Hidden Public
	private PlayerMovement playerMovement;
	// Properties

	// Components

	// Events

	#endregion

	#region Methods

	public void CallShowEvent()
	{
		Debug.Log($"Event Recived");
		if (theEvent != null) {
			Event.EventHandler.Instance.ShowEvent(theEvent);
		}
	}

	#endregion
}