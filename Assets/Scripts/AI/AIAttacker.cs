using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/**
* States of Attacker
*/
public enum AttackerStates
{
	CHASE, /*! < When Attacker chases the building */
	DODGE /*! < When Attacker dodges an obstacle */
}

/**
* Class represents behaviour of Attacker - Enemy that attack the city
*/
public class AIAttacker : AIEnemy
{
#region Values
    protected EnemyTimer dodgeTimer;
    protected EnemyTimer fireTimer;
    protected EnemyTimer imprecisionTimer;



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
	* Selected position to follow when dodging
	*/
    protected Vector3 dodgepos;
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
	* Current state of an enemy
	*/
	private AttackerStates state;
	/**
	* Previous state of an enemy
	*/
	private AttackerStates prevState;
#endregion

    /**
    * Sets up start values
    */
	protected override void SetupStartValues()
	{
		target = city.GetRandomBuilding();
		
		dodgeTimer = new EnemyTimer(0, dodgingStateCooldown);
		fireTimer = new EnemyTimer(0, fireCooldown);
		imprecisionTimer = new EnemyTimer(0, newImprecisionCooldown);

		SetState(AttackerStates.CHASE);
	}

    /**
    * Updates timers
    */
	protected override void UpdateTimers()
    {
        dodgeTimer.UpdateTimer();
		fireTimer.UpdateTimer();
		imprecisionTimer.UpdateTimer();

		GenerateNewImprecisionIfPossible();
		UpdateTargetIfNull();
    }

    /**
    * Updates the state of an enemy
	* @param newstate New state
    */
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

    /**
    * Determines behaviour of an enemy, depending on his state
    */
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

    /**
    * Behaviour of an enemy - Shoots a target (building) if enemy is close enough
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
			SetState(AttackerStates.DODGE);
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
    * Behaviour of an enemy - Picks new target (building) if previous was destroyed
    */
	protected void UpdateTargetIfNull()
	{
		if(!target)
		{
			target = city.GetRandomBuilding();
		}
	}



    /**
    * Set of behaviours of an enemy if in chase state
    */
	private void Chase()
	{
		if (debugStates) Debug.Log("===== CHASE =====");
		enemyMoveable.Accelerate(Vector3.forward, accelerationChase, maxSpeedChase, debugSpeed);
		enemyMoveable.StrafeTowardsObject(target, Vector3.zero);

		ShootIfTargetInRange();
	}

    /**
    * Set of behaviours of an enemy if in dodge state
    */
	private void Dodge()
	{
		if (debugStates) Debug.Log("===== DODGE =====");
		enemyMoveable.Accelerate(Vector3.forward, accelerationChase, maxSpeedChase, debugSpeed);
		enemyMoveable.StrafeTowardsConstPos(dodgepos + enemyImprecision.randomImprecision);

		ReturnToPrevState();
	}
}
