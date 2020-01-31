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

	private float x, y;
	private Rigidbody2D rb;

	// Start is called before the first frame update
	void Start()
	{
		rb = GetComponent<Rigidbody2D>();
		player = ReInput.players.GetPlayer(playerID);
		player.AddInputEventDelegate(GetX, UpdateLoopType.Update, InputActionEventType.AxisActiveOrJustInactive, RewiredConsts.Action.Horizontal_Movement);
		player.AddInputEventDelegate(GetY, UpdateLoopType.Update, InputActionEventType.AxisActiveOrJustInactive, RewiredConsts.Action.Vertical_Movement);
	}

	private void FixedUpdate()
	{
		Move();
	}

	private void GetX(InputActionEventData i)
	{
		x = i.GetAxis();
	}

	void GetY(InputActionEventData i)
	{
		y = i.GetAxis();
	}
	
	private void Move()
	{
		if(x == 0 && y == 0) return;
		Vector2 prevPos = transform.position;
		rb.MovePosition(new Vector2(x,y) + prevPos);
	}

	private void SetRotation()
	{
		Vector2 direction = rb.velocity - (Vector2)transform.position;
		transform.LookAt(direction);
	}
}