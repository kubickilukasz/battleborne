using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GuardianStates
{
	IDLE,
	CHASE,
	RETREAT,
	DODGE
}

public class AIGuardian : AIEnemy
{
#region Values
    //Timers
    protected EnemyTimer dodgeTimer;
    protected EnemyTimer fireTimer;
    protected EnemyTimer ignoreTargetTimer;
    protected EnemyTimer imprecisionTimer;
	protected EnemyTimer idlePointUpdateTimer;



    //Timer Values
    [SerializeField]
    protected float dodgingStateCooldown;
    [SerializeField]
    protected float fireCooldown;
    [SerializeField]
    protected float ignoreTargetCooldown;
    [SerializeField]
    protected float newImprecisionCooldown;
	[SerializeField]
	protected float updateNewIdlePointCooldown;



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


	
    //Guarding Object
	[SerializeField]
	public GameObject guardingObject;
	[SerializeField]
	protected float minDistanceDetectGuardingObject;
	[SerializeField]
	protected float minDistanceLoseGuardingObject;



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



    //State: Retreat
	[SerializeField]
	protected float maxSpeedRetreat;
	[SerializeField]
	protected float accelerationRetreat;



	//States
	private GuardianStates state;
	private GuardianStates prevState;
#endregion

	protected override void SetupStartValues()
	{
		target = jetSpawn?.jetReference;
		if(guardingObject)
			idlepos = guardingObject.transform.position;
		else
			idlepos = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z);

		dodgeTimer = new EnemyTimer(0, dodgingStateCooldown);
		fireTimer = new EnemyTimer(0, fireCooldown);
		ignoreTargetTimer = new EnemyTimer(0, ignoreTargetCooldown);
		imprecisionTimer = new EnemyTimer(0, newImprecisionCooldown);
		idlePointUpdateTimer = new EnemyTimer(0, updateNewIdlePointCooldown);

		SetState(GuardianStates.RETREAT);
	}

    protected override void UpdateTimers()
    {
        dodgeTimer.UpdateTimer();
		fireTimer.UpdateTimer();
		ignoreTargetTimer.UpdateTimer();
		imprecisionTimer.UpdateTimer();
		idlePointUpdateTimer.UpdateTimer();

		GenerateNewImprecisionIfPossible();
		UpdateTargetIfNull();
		UpdateGuardingPos();
    }

	protected void SetState(GuardianStates newstate)
	{
		switch (newstate)
		{
			case GuardianStates.IDLE:
				idlepos = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z);
				state = newstate;
				break;
			case GuardianStates.CHASE:
				state = newstate;
				break;
			case GuardianStates.RETREAT:
				state = newstate;
				break;
			case GuardianStates.DODGE:
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



	//RETREAT
	private void SetToIdleIfInTarget()
	{
		if (enemyShooting.IsPositionInRange(idlepos, minDistanceDetectGuardingObject)) 
		{
			SetState(GuardianStates.IDLE);
		}
	}

	//IDLE, RETREAT
	private void SetToChaseIfPlayer()
	{
		if (
			target &&
			enemyShooting.IsPositionInRange(target.transform.position, minDistanceDetectTarget) &&
			ignoreTargetTimer.IsTimerZero()
		) 
			SetState(GuardianStates.CHASE);
	}

	//IDLE
	private void SetToRetreatIfTooFar()
	{
		if (
			!enemyShooting.IsPositionInRange(idlepos, minDistanceDetectGuardingObject)
		) 
			SetState(GuardianStates.RETREAT);
	}

	//CHASE
	private void SetToRetreatIfLostOrTooFar()
	{
		if (!target || !enemyShooting.IsPositionInRange(target.transform.position, minDistanceDetectTarget))
			SetState(GuardianStates.RETREAT);
		else if (!enemyShooting.IsPositionInRange(idlepos, minDistanceLoseGuardingObject))
		{
			ignoreTargetTimer.ResetTimer();
			SetState(GuardianStates.RETREAT);
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
			SetState(GuardianStates.DODGE);
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

	private void UpdateTargetIfNull()
	{
		if(!target)
			target = jetSpawn?.jetReference;
	}

	private void UpdateGuardingPos()
	{
		if(guardingObject && idlePointUpdateTimer.IsTimerZero())
		{
			idlepos = guardingObject.transform.position;
			idlePointUpdateTimer.ResetTimer();
		}
	}



	private void Idle()
	{
		if (debugStates) Debug.Log("===== IDLE =====");
		enemyMoveable.Accelerate(Vector3.forward, accelerationIdle, maxSpeedIdle, debugSpeed);
		enemyMoveable.StrafeTowardsConstPos(idlepos+enemyImprecision.randomImprecision);

		SetToChaseIfPlayer();
		SetToRetreatIfTooFar();
	}

	private void Chase()
	{
		if (debugStates) Debug.Log("===== CHASING =====");
		enemyMoveable.Accelerate(Vector3.forward, accelerationChase, maxSpeedChase, debugSpeed);
		enemyMoveable.StrafeTowardsObject(target, Vector3.zero);

		SetToRetreatIfLostOrTooFar();
		ShootIfTargetInRange();
	}

	private void Retreat()
	{
		if (debugStates) Debug.Log("===== RETREAT =====");
		enemyMoveable.Accelerate(Vector3.forward, accelerationRetreat, maxSpeedRetreat, debugSpeed);
		enemyMoveable.StrafeTowardsConstPos(idlepos+Vector3.zero);

		SetToChaseIfPlayer();
		SetToIdleIfInTarget();
	}

	private void Dodge()
	{
		if (debugStates) Debug.Log("===== DODGE =====");
		enemyMoveable.Accelerate(Vector3.forward, accelerationIdle, maxSpeedIdle, debugSpeed);
		enemyMoveable.StrafeTowardsConstPos(dodgepos+enemyImprecision.randomImprecision);

		ReturnToPrevState();
	}
}
