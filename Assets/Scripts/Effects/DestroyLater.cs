using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
* Simple destroyer of object
*/
public class DestroyLater : MonoBehaviour
{

    /**
    * Time to destroy object
    */
    [SerializeField]
    private float timeInSeconds;

    void Start()
    {
        StartCoroutine(InokeAfterTime(() => Destroy(gameObject)));
    }

    /**
    * After designated seconds invoke function
    * @param action - function what to do after designated time
    */
    IEnumerator InokeAfterTime(System.Action action)
    {
        yield return new WaitForSeconds(timeInSeconds);
        action?.Invoke();
    }
}
