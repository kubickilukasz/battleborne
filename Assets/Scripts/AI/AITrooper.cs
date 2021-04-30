using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TrooperStates
{
	IDLE, //Kr¹¿y w miejscu
	CHASE, //Wykry³ gracza i go atakuje
	DODGE
}

public class AITrooper : AIEnemy
{
	[SerializeField]
	private float maxSpeedIdle;
	[SerializeField]
	private float accelerationIdle;

	[SerializeField]
	private float maxSpeedChase;
	[SerializeField]
	private float accelerationChase;

	[SerializeField]
	private float torqueAcceleration;

	private TrooperStates prevState;
	private Vector3 idlepos;
	private Vector3 dodgepos;

	private TrooperStates state;

	protected override void SetupStartValues()
	{
		torqueacc = torqueAcceleration;
		detectRange = playerDetectRange;
		ignoreTargetTime = spawnIgnorePlayerTime;
		SetState(TrooperStates.IDLE);
	}

	protected void SetState(TrooperStates newstate)
	{
		switch (newstate)
		{
			case TrooperStates.IDLE:
				maxSpeed = maxSpeedIdle;
				acc = accelerationIdle;
				target = null;
				idlepos = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z);

				state = newstate;
				break;
			case TrooperStates.CHASE:
				maxSpeed = maxSpeedChase;
				acc = accelerationChase;
				target = player;

				state = newstate;
				break;
			case TrooperStates.DODGE:
				prevState = state;
				Vector3 backward = this.transform.position - this.transform.forward * 5;
				dodgepos = new Vector3(backward.x, backward.y, backward.z);

				state = newstate;
				break;
			default:
				break;
		}
	}

	protected override void UpdateStateMethods()
	{
		switch (state)
		{
			case TrooperStates.IDLE:
				Idle();
				break;
			case TrooperStates.CHASE:
				Chase();
				break;
			case TrooperStates.DODGE:
				Dodge();
				break;
		}
	}



	private void SetToIdleIfLost()
	{
		if (!IsPlayerInRange()) SetState(TrooperStates.IDLE);
	}

	private void SetToChaseIfPlayer()
	{
		if (IsPlayerInRange()) SetState(TrooperStates.CHASE);
	}

	private void SetToDodgeIfCrashCourse()
	{
		if (dodgingTime <= 0 && CheckRaycast())
		{
			dodgingTime = nextDodgingTime;
			SetState(TrooperStates.DODGE);
		}
	}

	private void ReturnToPrevState()
	{
		if (dodgingTime <= 0)
			state = prevState;
	}



	private void Idle()
	{
		if (debug) Debug.Log("===== IDLE =====");
		AccelerateForward();
		CycleAroundConstPos(idlepos);
		SetToChaseIfPlayer();
		SetToDodgeIfCrashCourse();
	}

	private void Chase()
	{
		if (debug) Debug.Log("===== CHASE =====");
		AccelerateForward();
		StrafeTowardsFocusTarget();
		SetToIdleIfLost();
		SetToDodgeIfCrashCourse();
	}

	private void Dodge()
	{
		if (debug) Debug.Log("===== DODGE =====");
		AccelerateForward();
		StrafeTowardsConstPos(dodgepos);
		ReturnToPrevState();
	}
}
