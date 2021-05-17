using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GuardianStates
{
	IDLE, //Kr��y w miejscu
	CHASE, //Wykry� gracza i go atakuje
	RETREAT, //Powr�t do punktu startowego
	DODGE
}

public class AIGuardian : AIEnemy
{
	[SerializeField]
	public GameObject guardingObject;
	[SerializeField]
	private float guardingObjectDetectRange;
	[SerializeField]
	private float guardingObjectDetectRangeTooFar;

	[SerializeField]
	private float maxSpeedIdle;
	[SerializeField]
	private float accelerationIdle;

	[SerializeField]
	private float maxSpeedChase;
	[SerializeField]
	private float accelerationChase;

	[SerializeField]
	private float maxSpeedRetreat;
	[SerializeField]
	private float accelerationRetreat;

	[SerializeField]
	private float rotationSpeed;

	[SerializeField]
	private float ignorePlayerTime;

	private GuardianStates prevState;
	private Vector3 dodgepos;

	private GuardianStates state;

	protected override void SetupStartValues()
	{
		rotateSpeed = rotationSpeed;
		ignoreTargetTime = spawnIgnorePlayerTime;
		SetState(GuardianStates.RETREAT);
	}

	protected void SetState(GuardianStates newstate)
	{
		switch (newstate)
		{
			case GuardianStates.IDLE:
				maxSpeed = maxSpeedIdle;
				acc = accelerationIdle;
				target = guardingObject;

				state = newstate;
				break;
			case GuardianStates.CHASE:
				maxSpeed = maxSpeedChase;
				acc = accelerationChase;
				target = player;

				state = newstate;
				break;
			case GuardianStates.RETREAT:
				maxSpeed = maxSpeedRetreat;
				acc = accelerationRetreat;
				target = guardingObject;

				state = newstate;
				break;
			case GuardianStates.DODGE:
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
		SetToDodgeIfCrashCourse();

		switch (state)
		{
			case GuardianStates.IDLE:
				Idle();
				break;
			case GuardianStates.CHASE:
				Chase();
				break;
			case GuardianStates.RETREAT:
				Retreat();
				break;
			case GuardianStates.DODGE:
				Dodge();
				break;
		}
	}



	protected bool IsGuardingPointInRange()
	{
		if ((guardingObject.transform.position - transform.position).magnitude < guardingObjectDetectRange)
			return true;
		else
			return false;
	}

	protected bool IsGuardingPointTooFar()
	{
		if ((guardingObject.transform.position - transform.position).magnitude > guardingObjectDetectRangeTooFar)
			return true;
		else
			return false;
	}

	private void SetToIdleIfInTarget()
	{
		if (IsGuardingPointInRange()) SetState(GuardianStates.IDLE);
	}

	private void SetToRetreatIfLostOrTooFar()
	{
		if (!IsPlayerInRange()) {
			SetState(GuardianStates.RETREAT);
		}
		else if (IsGuardingPointTooFar())
		{
			ignoreTargetTime = ignorePlayerTime;
			SetState(GuardianStates.RETREAT);
		}
	}

	private void SetToChaseIfPlayer()
	{
		if (IsPlayerInRange()) SetState(GuardianStates.CHASE);
	}

	private void SetToDodgeIfCrashCourse()
	{
		if (dodgingTime <= 0 && CheckRaycast(Vector3.forward))
		{
			dodgingTime = nextDodgingTime;
			SetState(GuardianStates.DODGE);
		}
	}

	private void ShootIfTargetInRange()
	{
		if (CanStartShooting()) Shoot();
	}

	private void ReturnToPrevState()
	{
		if (dodgingTime <= 0)
			state = prevState;
	}



	private void Idle()
	{
		if (debug) Debug.Log("===== IDLE =====");
		Accelerate(Vector3.forward);
		StrafeTowardsFocusTarget(randomImprecision);
		SetToChaseIfPlayer();
		SetToDodgeIfCrashCourse();
	}

	private void Chase()
	{
		if (debug) Debug.Log("===== CHASING =====");
		Accelerate(Vector3.forward);
		StrafeTowardsFocusTarget(Vector3.zero);
		SetToRetreatIfLostOrTooFar();
		SetToDodgeIfCrashCourse();
		ShootIfTargetInRange();
	}

	private void Retreat()
	{
		if (debug) Debug.Log("===== RETREAT =====");
		Accelerate(Vector3.forward);
		StrafeTowardsFocusTarget(Vector3.zero);
		SetToChaseIfPlayer();
		SetToIdleIfInTarget();
		SetToDodgeIfCrashCourse();
	}

	private void Dodge()
	{
		if (debug) Debug.Log("===== DODGE, prestate: " + prevState + " =====");
		Accelerate(Vector3.forward);
		StrafeTowardsConstPos(dodgepos, Vector3.zero);
		ReturnToPrevState();
	}
}
