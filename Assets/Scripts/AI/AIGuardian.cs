using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
* States of Guardian
*/
public enum GuardianStates
{
	IDLE, /*! < When Guardian hovers around idle position */
	CHASE, /*! < When Guardian chases the player */
	RETREAT, /*! < When Guardian comes back to idle position */
	DODGE /*! < When Guardian dodges an obstacle */
}

/**
* Class represents behaviour of Guardian - Enemy that guards the target
*/
public class AIGuardian : AIEnemy
{
#region Values
    //Timers
    protected EnemyTimer dodgeTimer;
    protected EnemyTimer fireTimer;
    protected EnemyTimer ignoreTargetTimer;
    protected EnemyTimer imprecisionTimer;
	protected EnemyTimer idlePointUpdateTimer;



	/**
	* How long should enemy dodge an object
	*/
    [SerializeField]
    protected float dodgingStateCooldown;
	/**
	* Cooldown before shooting next bullet
	*/
    [SerializeField]
    protected float fireCooldown;
	/**
	* Cooldown before generating new imprecise position
	*/
    [SerializeField]
    protected float newImprecisionCooldown;
	/**
	* Duration, when target should be ignored
	*/
    [SerializeField]
    protected float ignoreTargetCooldown;
	/**
	* Cooldown between updating guarding point (it could change position)
	*/
	[SerializeField]
	protected float updateNewIdlePointCooldown;



	/**
	* Selected position to follow when dodging
	*/
    protected Vector3 dodgepos;
	/**
	* Guarding position
	*/
    protected Vector3 idlepos;
	/**
	* Distance between enemy and obstacle, after which enemy switches to dodge state
	*/
    [SerializeField]
    protected float crashDangerRange;



	/**
	* Target of an enemy
	*/
    protected GameObject target;
	/**
	* Minimum angle between enemy's view and target's position, after which enemy can start shooting
	*/
    [SerializeField]
    protected float minAngleShootTarget;
	/**
	* Minimum distance between enemy and target, after which enemy can start shooting
	*/
    [SerializeField]
    protected float minDistanceShootTarget;
	/**
	* Minimum distance between enemy and target, after which enemy notices target
	*/
    [SerializeField]
    protected float minDistanceDetectTarget;


	
	/**
	* Objects that guardian guards
	*/
	[SerializeField]
	public GameObject guardingObject;
	/**
	* Minimum distance between enemy and guarding point, after which enemy notices guarding point
	*/
	[SerializeField]
	protected float minDistanceDetectGuardingObject;
	/**
	* Minimum distance between enemy and guarding point, after which enemy loses sight of guarding point
	*/
	[SerializeField]
	protected float minDistanceLoseGuardingObject;



	/**
	* Maximum speed of an enemy when in idle state
	*/
	[SerializeField]
	protected float maxSpeedIdle;
    /**
    * Size of imprecison box - used mainly with idle state
    */
	[SerializeField]
	protected float accelerationIdle;



	/**
	* Maximum speed of an enemy when in chase state
	*/
	[SerializeField]
	protected float maxSpeedChase;
	/**
	* Acceleration of an enemy when in chase state
	*/
	[SerializeField]
	protected float accelerationChase;



	/**
	* Maximum speed of an enemy when in retreat state
	*/
	[SerializeField]
	protected float maxSpeedRetreat;
	/**
	* Acceleration of an enemy when in retreat state
	*/
	[SerializeField]
	protected float accelerationRetreat;



	/**
	* Current state of an enemy
	*/
	private GuardianStates state;
	/**
	* Previous state of an enemy
	*/
	private GuardianStates prevState;
#endregion

    /**
    * Sets up start values
    */
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

    /**
    * Updates timers
    */
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

    /**
    * Updates the state of an enemy
	* @param newstate New state
    */
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

    /**
    * Determines behaviour of an enemy, depending on his state
    */
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



    /**
    * Behaviour of an enemy - Switches to idle if guarding point is reached
    */
	private void SetToIdleIfInTarget()
	{
		if (enemyShooting.IsPositionInRange(idlepos, minDistanceDetectGuardingObject)) 
		{
			SetState(GuardianStates.IDLE);
		}
	}

    /**
    * Behaviour of an enemy - Starts chasing player if it is in range
    */
	private void SetToChaseIfPlayer()
	{
		if (
			target &&
			enemyShooting.IsPositionInRange(target.transform.position, minDistanceDetectTarget) &&
			ignoreTargetTimer.IsTimerZero()
		) 
			SetState(GuardianStates.CHASE);
	}

    /**
    * Behaviour of an enemy - Retreats to guarding point if guarding point has moved too far from enemy
    */
	private void SetToRetreatIfTooFar()
	{
		if (
			!enemyShooting.IsPositionInRange(idlepos, minDistanceDetectGuardingObject)
		) 
			SetState(GuardianStates.RETREAT);
	}

    /**
    * Behaviour of an enemy - Retreats to guarding point if enemy is too far from it or player has been lost
    */
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

    /**
    * Behaviour of an enemy - Shoots a target (player) if enemy is close enough
    */
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

    /**
    * Behaviour of an enemy - Returns to previous state after dodging an obstacle
    */
	private void ReturnToPrevState()
	{
		if (dodgeTimer.IsTimerZero())
			SetState(prevState);
	}

    /**
    * Behaviour of an enemy - Dodges an obstacle if distance is critical
    */
	private void SetToDodgeIfCrashCourse()
	{
		if (dodgeTimer.IsTimerZero() && enemyMoveable.CheckCrashCourse(Vector3.forward, crashDangerRange))
		{
			dodgeTimer.ResetTimer();
			SetState(GuardianStates.DODGE);
		}
	}

    /**
    * Behaviour of an enemy - Generates new imprecision (used to randomize dodging position)
    */
	private void GenerateNewImprecisionIfPossible()
	{
		if(imprecisionTimer.IsTimerZero())
		{
			enemyImprecision.GenerateRandomImprecision();
			imprecisionTimer.ResetTimer();
		}
	}

    /**
    * Behaviour of an enemy - Picks new target (player) if previous was destroyed
    */
	private void UpdateTargetIfNull()
	{
		if(!target)
			target = jetSpawn?.jetReference;
	}

    /**
    * Behaviour of an enemy - Updates position of guarding position
    */
	private void UpdateGuardingPos()
	{
		if(guardingObject && idlePointUpdateTimer.IsTimerZero())
		{
			idlepos = guardingObject.transform.position;
			idlePointUpdateTimer.ResetTimer();
		}
	}



    /**
    * Set of behaviours of an enemy if in idle state
    */
	private void Idle()
	{
		if (debugStates) Debug.Log("===== IDLE =====");
		enemyMoveable.Accelerate(Vector3.forward, accelerationIdle, maxSpeedIdle, debugSpeed);
		enemyMoveable.StrafeTowardsConstPos(idlepos+enemyImprecision.randomImprecision);

		SetToChaseIfPlayer();
		SetToRetreatIfTooFar();
	}

    /**
    * Set of behaviours of an enemy if in chase state
    */
	private void Chase()
	{
		if (debugStates) Debug.Log("===== CHASING =====");
		enemyMoveable.Accelerate(Vector3.forward, accelerationChase, maxSpeedChase, debugSpeed);
		enemyMoveable.StrafeTowardsObject(target, Vector3.zero);

		SetToRetreatIfLostOrTooFar();
		ShootIfTargetInRange();
	}

    /**
    * Set of behaviours of an enemy if in retreat state
    */
	private void Retreat()
	{
		if (debugStates) Debug.Log("===== RETREAT =====");
		enemyMoveable.Accelerate(Vector3.forward, accelerationRetreat, maxSpeedRetreat, debugSpeed);
		enemyMoveable.StrafeTowardsConstPos(idlepos+Vector3.zero);

		SetToChaseIfPlayer();
		SetToIdleIfInTarget();
	}

    /**
    * Set of behaviours of an enemy if in dodge state
    */
	private void Dodge()
	{
		if (debugStates) Debug.Log("===== DODGE =====");
		enemyMoveable.Accelerate(Vector3.forward, accelerationIdle, maxSpeedIdle, debugSpeed);
		enemyMoveable.StrafeTowardsConstPos(dodgepos+enemyImprecision.randomImprecision);

		ReturnToPrevState();
	}
}
