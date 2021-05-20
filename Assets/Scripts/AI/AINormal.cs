using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NormalStates
{
	FALLING
}


public class AINormal : AIEnemy
{
#region Values
    //Timers
    protected EnemyTimer fireTimer;



	//Timer Values
    [SerializeField]
    protected float fireCooldown;


    //Extra Positions
    [SerializeField]
    protected float crashDangerRange;



    //Falling
	[SerializeField]
	protected float maxFallingSpeed;
	[SerializeField]
	protected float fallingAcceleration;



    //Shooting Related
    protected GameObject target;
    [SerializeField]
    protected float minDistanceDetectTarget;



	//States
	private NormalStates state;
#endregion

	protected override void SetupStartValues()
	{
		target = city.GetRandomBuilding();
		transform.Rotate(90, 0, 0);

		fireTimer = new EnemyTimer(0, fireCooldown);

		SetState(NormalStates.FALLING);
	}

    protected override void UpdateTimers()
    {
		fireTimer.UpdateTimer();

		UpdateTargetIfNull();
    }

	protected void SetState(NormalStates newstate)
	{
		switch (newstate)
		{
			case NormalStates.FALLING:
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
		}

		//SetTo0hpIfCrashCourse();
	}



	//FALLING
	private void ShootTarget()
	{
		if (
			target &&
			fireTimer.IsTimerZero() &&
			enemyShooting.CheckCrashCollisions(Vector3.forward, Mathf.Infinity)
		)
		{
			ShootDirection(target.transform.position - transform.position);
			fireTimer.ResetTimer();
		}

	}


	//ALWAYS
	private void UpdateTargetIfNull()
	{
		if(!target)
			target = jetSpawn?.jetReference;
	}



	private void Falling()
	{
		if (debugStates) Debug.Log("===== FALLING =====");
		enemyMoveable.Accelerate(Vector3.forward, fallingAcceleration, maxFallingSpeed, debugSpeed);
		ShootTarget();
	}

}
