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

	#region Unity Callbacks

	private void OnEnable()
	{
		playerMovement = FindObjectOfType<PlayerMovement>();
		playerMovement.OnEnteredTriggerEvent += CallShowEvent;
	}

	private void OnDisable()
	{
		playerMovement.OnEnteredTriggerEvent -= CallShowEvent;
	}

	#endregion

	#region Methods

	private void CallShowEvent()
	{
		Debug.Log($"Event Recived");
		//Event.EventHandler.Instance.ShowEvent(theEvent);
	}

	#endregion
}