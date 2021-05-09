using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NormalStates
{
	FALLING, //Spadanie w d� w kierunku miasta
	FALLINGSPOTTED //Te� spadanie, ale przeciwnik zauwa�y� gracza
}

public class AINormal : AIEnemy
{
	[SerializeField]
	private float maxFallingSpeed;
	[SerializeField]
	private float fallingAcceleration;

	private NormalStates state;

	protected override void SetupStartValues()
	{
		rotateSpeed = 0f;
		maxSpeed = maxFallingSpeed;
		acc = fallingAcceleration;
		detectRange = playerDetectRange;
		SetState(NormalStates.FALLING);
	}

	protected void SetState(NormalStates newstate)
	{
		switch (newstate)
		{
			case NormalStates.FALLING:

				state = newstate;
				break;
			case NormalStates.FALLINGSPOTTED:

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
			case NormalStates.FALLING:
				Falling();
				break;
			case NormalStates.FALLINGSPOTTED:
				FallingSpotted();
				break;
		}
	}



	private void SetToSpottedIfPlayer()
	{
		if (IsPlayerInRange()) SetState(NormalStates.FALLINGSPOTTED);
	}

	private void SetToFallingIfNoPlayer()
	{
		if (!IsPlayerInRange()) SetState(NormalStates.FALLING);
	}

	private void SetTo0hpIfCrashCourse()
	{
		if (dodgingTime <= 0 && CheckRaycast(Vector3.down))
		{
			dodgingTime = nextDodgingTime;
			health = 0;
		}
	}



	private void Falling()
	{
		if (debug) Debug.Log("===== FALLING =====");
		Accelerate(Vector3.down);
		SetToSpottedIfPlayer();
		SetTo0hpIfCrashCourse();
	}

	private void FallingSpotted()
	{
		if (debug) Debug.Log("===== FALLING SPOTTED =====");
		Accelerate(Vector3.down);
		SetToFallingIfNoPlayer();
		SetTo0hpIfCrashCourse();    
	}

}
