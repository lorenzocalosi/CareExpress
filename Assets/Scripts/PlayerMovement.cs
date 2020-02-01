using System;
using System.Collections;
using System.Collections.Generic;
using Rewired;
using UnityEngine;

public class PlayerMovement : Singleton<PlayerMovement>
{
	public float speed = 5;
	
	[HideInInspector] public bool canMove = true;
	private Player player;
	private int playerID = 0;

	private Animator animator;

	private float x, y;
	private bool isInteracting;
	private Rigidbody2D rb;

	private EventHotSpot eventHotspot;
	private bool isPressingCancel;
	public bool IsPressingCancel => isPressingCancel;
	public event Action OnEnteredTriggerEvent;

	private float localScaleX;

	void Start()
	{
		localScaleX = transform.localScale.x;
		animator = GetComponent<Animator>();
		rb = GetComponent<Rigidbody2D>();
		player = ReInput.players.GetPlayer(playerID);
		player.AddInputEventDelegate(GetX, UpdateLoopType.Update, InputActionEventType.AxisActiveOrJustInactive, RewiredConsts.Action.Horizontal_Movement);
		player.AddInputEventDelegate(GetY, UpdateLoopType.Update, InputActionEventType.AxisActiveOrJustInactive, RewiredConsts.Action.Vertical_Movement);
		player.AddInputEventDelegate(GetInteract, UpdateLoopType.Update, InputActionEventType.ButtonPressed, RewiredConsts.Action.Interact);
		player.AddInputEventDelegate(GetCancel, UpdateLoopType.Update, InputActionEventType.ButtonPressed, RewiredConsts.Action.UICancel);
	}

	
	private void FixedUpdate()
	{
		if (isInteracting)
		{
			Interact();
		}

		if (canMove)
		{
			Move();
			SetRotation();
		}
	}

	private void OnTriggerEnter2D(Collider2D other) {
		EventHotSpot otherHotspot = other.GetComponent<EventHotSpot>();

		// start interaction with event
		if (otherHotspot != null && otherHotspot.theEvent != null) {
			eventHotspot = otherHotspot;
		}
	}

	private void OnTriggerExit2D(Collider2D collision) {
		EventHotSpot otherHotspot = collision.GetComponent<EventHotSpot>();
		
		if (otherHotspot != null && otherHotspot == eventHotspot) {
			eventHotspot = null;
		}
	}

	private void GetX(InputActionEventData i)
	{
		x = i.GetAxis();
		animator.SetFloat("x", x);
	}

	void GetY(InputActionEventData i)
	{
		y = i.GetAxis();
		animator.SetFloat("y", y);
	}

	private void GetInteract(InputActionEventData i) {
		isInteracting = i.GetButtonDown();
	}

	private void GetCancel(InputActionEventData i)
	{
		isPressingCancel = i.GetButtonDown();
	}

	private void Interact() {
		if (eventHotspot != null && canMove) {
			canMove = false;
			rb.velocity = Vector2.zero;
			SoundManager.Instance.InterruptSoundTrackAndPlayOther("Walking", "Talking");
			eventHotspot.CallShowEvent();
		}
	}

	private void Move()
	{
		//if(x == 0 && y == 0) return;
		Vector2 prevPos = rb.transform.position;
		rb.velocity = ((new Vector2(x, y)) * speed);
	}

	private void SetRotation()
	{
		Vector2 direction = (new Vector2(x, y)).normalized;

		if (direction != Vector2.zero)
		{
			float correctionAngleBasedOnSprite = -90; // valore da twiccare in base all'angolazione della sprite
			float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
			transform.rotation = Quaternion.AngleAxis(angle + correctionAngleBasedOnSprite, Vector3.forward);

			Vector3 localScaleToApply = transform.localScale;

			localScaleToApply.x = (x >= 0) ? localScaleX : -localScaleX;

			transform.localScale = new Vector3(localScaleToApply.x, transform.localScale.y, transform.localScale.z);
		}
	}
}