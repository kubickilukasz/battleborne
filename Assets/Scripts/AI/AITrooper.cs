using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum TrooperStates
{
	IDLE,
	CHASE,
	DODGE
}

public class AITrooper : AIEnemy
{
#region Values
    //Timers
    protected EnemyTimer dodgeTimer;
    protected EnemyTimer fireTimer;
    protected EnemyTimer ignoreTargetTimer;
    protected EnemyTimer imprecisionTimer;



    //Timer Values
    [SerializeField]
    protected float dodgingStateCooldown;
    [SerializeField]
    protected float fireCooldown;
    [SerializeField]
    protected float ignoreTargetCooldown;
    [SerializeField]
    protected float newImprecisionCooldown;
	


    //Extra Positions
    protected Vector3 dodgepos;
    protected Vector3 idlepos;
    [SerializeField]
    protected float crashDangerRange;



    //Shooting Related
    protected GameObject target;
    [SerializeField]
    protected float minDistanceDetectTarget;
    [SerializeField]
    protected float minAngleShootTarget;
    [SerializeField]
    protected float minDistanceShootTarget;



    //State: Idle
	[SerializeField]
	protected float maxSpeedIdle;
	[SerializeField]
	protected float accelerationIdle;



    //State: Chase
	[SerializeField]
	protected float maxSpeedChase;
	[SerializeField]
	protected float accelerationChase;



	//States
	private TrooperStates state;
	private TrooperStates prevState;
#endregion

	protected override void SetupStartValues()
	{
		target = jetSpawn?.jetReference;
		
		dodgeTimer = new EnemyTimer(0, dodgingStateCooldown);
		fireTimer = new EnemyTimer(0, fireCooldown);
		imprecisionTimer = new EnemyTimer(0, newImprecisionCooldown);

		SetState(TrooperStates.IDLE);
	}

	protected override void UpdateTimers()
    {
        dodgeTimer.UpdateTimer();
		fireTimer.UpdateTimer();
		imprecisionTimer.UpdateTimer();

		GenerateNewImprecisionIfPossible();
		UpdateTargetIfNull();
    }

	protected void SetState(TrooperStates newstate)
	{
		switch (newstate)
		{
			case TrooperStates.IDLE:
				idlepos = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z);
				state = newstate;
				break;
			case TrooperStates.CHASE:
				state = newstate;
				break;
			case TrooperStates.DODGE:
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



	//IDLE
	private void SetToChaseIfPlayer()
	{
		if (
			target &&
			enemyShooting.IsPositionInRange(target.transform.position, minDistanceDetectTarget)
		) 
			SetState(TrooperStates.CHASE);
	}

	//CHASE
	private void SetToIdleIfLost()
	{
		if (!target || !enemyShooting.IsPositionInRange(target.transform.position, minDistanceDetectTarget))
			SetState(TrooperStates.IDLE);
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
			SetState(TrooperStates.DODGE);
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
			target = jetSpawn?.jetReference;
		}
	}



	private void Idle()
	{
		if (debugStates) Debug.Log("===== IDLE =====");
		enemyMoveable.Accelerate(Vector3.forward, accelerationIdle, maxSpeedIdle, debugSpeed);
		enemyMoveable.StrafeTowardsConstPos(idlepos + enemyImprecision.randomImprecision);

		SetToChaseIfPlayer();
	}

	private void Chase()
	{
		if (debugStates) Debug.Log("===== CHASE =====");
		enemyMoveable.Accelerate(Vector3.forward, accelerationChase, maxSpeedChase, debugSpeed);
		enemyMoveable.StrafeTowardsObject(target, Vector3.zero);

		SetToIdleIfLost();
		ShootIfTargetInRange();
	}

	private void Dodge()
	{
		if (debugStates) Debug.Log("===== DODGE =====");
		enemyMoveable.Accelerate(Vector3.forward, accelerationIdle, maxSpeedIdle, debugSpeed);
		enemyMoveable.StrafeTowardsConstPos(dodgepos + enemyImprecision.randomImprecision);

		ReturnToPrevState();
	}
}
