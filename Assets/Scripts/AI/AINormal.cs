using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
* States of Normal
*/
public enum NormalStates
{
	FALLING /*! < When Normal is falling */
}

/**
* Class represents behaviour of Normal - Enemy that slowly falls and shoots buildings from afar
*/
public class AINormal : AIEnemy
{
#region Values
    protected EnemyTimer fireTimer;



	/**
	* Cooldown before shooting next bullet
	*/
    [SerializeField]
    protected float fireCooldown;



	/**
	* Maximum speed of an enemy when in falling state
	*/
	[SerializeField]
	protected float maxFallingSpeed;
	/**
	* Acceleration of an enemy when in falling state
	*/
	[SerializeField]
	protected float fallingAcceleration;



	/**
	* Target of an enemy
	*/
    protected GameObject target;



	/**
	* Current state of an enemy
	*/
	private NormalStates state;
#endregion

    /**
    * Sets up start values
    */
	protected override void SetupStartValues()
	{
		target = city.GetRandomBuilding();
		transform.Rotate(90, 0, 0);

		fireTimer = new EnemyTimer(0, fireCooldown);

		SetState(NormalStates.FALLING);
	}

    /**
    * Updates timers
    */
    protected override void UpdateTimers()
    {
		fireTimer.UpdateTimer();

		UpdateTargetIfNull();
    }

    /**
    * Updates the state of an enemy
	* @param newstate New state
    */
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

    /**
    * Determines behaviour of an enemy, depending on his state
    */
	protected override void UpdateStateMethods()
	{
		switch (state)
		{
			case NormalStates.FALLING:
				Falling();
				break;
		}
	}



    /**
    * Behaviour of an enemy - Shoots a target (building)
    */
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


    /**
    * Behaviour of an enemy - Picks new target (building) if previous was destroyed
    */
	private void UpdateTargetIfNull()
	{
		if(!target)
			target = city.GetRandomBuilding();
	}



   /**
    * Set of behaviours of an enemy if in falling state
    */
	private void Falling()
	{
		if (debugStates) Debug.Log("===== FALLING =====");
		enemyMoveable.Accelerate(Vector3.forward, fallingAcceleration, maxFallingSpeed, debugSpeed);
		ShootTarget();
	}

}
