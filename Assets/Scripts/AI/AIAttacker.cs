using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum AttackerStates
{
	CHASE,
	DODGE
}

public class AIAttacker : AIEnemy
{
#region Values
    //Timers
    protected EnemyTimer dodgeTimer;
    protected EnemyTimer fireTimer;
    protected EnemyTimer imprecisionTimer;



    //Timer Values
    [SerializeField]
    protected float dodgingStateCooldown;
    [SerializeField]
    protected float fireCooldown;
    [SerializeField]
    protected float newImprecisionCooldown;
	


    //Extra Positions
    protected Vector3 dodgepos;
    [SerializeField]
    protected float crashDangerRange;



    //Shooting Related
    protected GameObject target;
    [SerializeField]
    protected float minAngleShootTarget;
    [SerializeField]
    protected float minDistanceShootTarget;



    //State: Chase
	[SerializeField]
	protected float maxSpeedChase;
	[SerializeField]
	protected float accelerationChase;



	//States
	private AttackerStates state;
	private AttackerStates prevState;
#endregion

	protected override void SetupStartValues()
	{
		target = city.GetRandomBuilding();
		
		dodgeTimer = new EnemyTimer(0, dodgingStateCooldown);
		fireTimer = new EnemyTimer(0, fireCooldown);
		imprecisionTimer = new EnemyTimer(0, newImprecisionCooldown);

		SetState(AttackerStates.CHASE);
	}

	protected override void UpdateTimers()
    {
        dodgeTimer.UpdateTimer();
		fireTimer.UpdateTimer();
		imprecisionTimer.UpdateTimer();

		GenerateNewImprecisionIfPossible();
		UpdateTargetIfNull();
    }

	protected void SetState(AttackerStates newstate)
	{
		switch (newstate)
		{
			case AttackerStates.CHASE:
				state = newstate;
				break;
			case AttackerStates.DODGE:
				prevState = state;
				Vector3 backward = this.transform.position - this.transform.forward * 25;
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
			case AttackerStates.CHASE:
				Chase();
				break;
			case AttackerStates.DODGE:
				Dodge();
				break;
		}
	}


	private void ShootIfTargetInRange()
	{
		if (
			target &&
			fireTimer.IsTimerZero() &&
			enemyShooting.IsPositionInRange(target.transform.position, minDistanceShootTarget) &&
			enemyShooting.IsAngleInRange(target.transform.position, minAngleShootTarget) &&
			enemyShooting.CheckCrashCollisions(Vector3.forward, minDistanceShootTarget)
		)
		{
			Shoot();
			fireTimer.ResetTimer();
		}
	}
	
	//DODGE
	private void ReturnToPrevState()
	{
		if (dodgeTimer.IsTimerZero())
			SetState(prevState);
	}

	//ALWAYS
	private void SetToDodgeIfCrashCourse()
	{
		if (dodgeTimer.IsTimerZero() && enemyMoveable.CheckCrashCourse(Vector3.forward, crashDangerRange))
		{
			dodgeTimer.ResetTimer();
			SetState(AttackerStates.DODGE);
		}
	}

	private void GenerateNewImprecisionIfPossible()
	{
		if(imprecisionTimer.IsTimerZero())
		{
			enemyImprecision.GenerateRandomImprecision();
			imprecisionTimer.ResetTimer();
		}
	}

	protected void UpdateTargetIfNull()
	{
		if(!target)
		{
			target = city.GetRandomBuilding();
		}
	}

	private void Chase()
	{
		if (debugStates) Debug.Log("===== CHASE =====");
		enemyMoveable.Accelerate(Vector3.forward, accelerationChase, maxSpeedChase, debugSpeed);
		enemyMoveable.StrafeTowardsObject(target, Vector3.zero);

		ShootIfTargetInRange();
	}

	private void Dodge()
	{
		if (debugStates) Debug.Log("===== DODGE =====");
		enemyMoveable.Accelerate(Vector3.forward, accelerationChase, maxSpeedChase, debugSpeed);
		enemyMoveable.StrafeTowardsConstPos(dodgepos + enemyImprecision.randomImprecision);

		ReturnToPrevState();
	}
}
