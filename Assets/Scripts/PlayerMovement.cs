using System;
using System.Collections;
using System.Collections.Generic;
using Rewired;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	public float speed = 5;
	private Player player;
	private int playerID = 0;

	private Animator animator;

	private float x, y;
	private bool isInteracting;
	private Rigidbody2D rb;

	public event Action OnEnteredTriggerEvent;

	void Start()
	{
		animator = GetComponent<Animator>();
		rb = GetComponent<Rigidbody2D>();
		player = ReInput.players.GetPlayer(playerID);
		player.AddInputEventDelegate(GetX, UpdateLoopType.Update, InputActionEventType.AxisActiveOrJustInactive, RewiredConsts.Action.Horizontal_Movement);
		player.AddInputEventDelegate(GetY, UpdateLoopType.Update, InputActionEventType.AxisActiveOrJustInactive, RewiredConsts.Action.Vertical_Movement);
		player.AddInputEventDelegate(GetInteract, UpdateLoopType.Update, InputActionEventType.ButtonPressed, RewiredConsts.Action.Interact);
	}
	private void FixedUpdate()
	{
		if (isInteracting)
		{
			Interact();
		}
		
		Move();
		SetRotation();
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.GetComponent<EventHotSpot>())
		{
			//Debug.Log($"event invoked");
			OnEnteredTriggerEvent?.Invoke();
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

	private void GetInteract(InputActionEventData i)
	{
		isInteracting = i.GetButtonDown(); 
	}

	private void Interact()
	{
		Debug.Log($"{isInteracting}");
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
		}
	}
}