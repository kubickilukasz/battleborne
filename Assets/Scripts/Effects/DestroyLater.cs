using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyLater : MonoBehaviour
{

    [SerializeField]
    private float timeInSeconds;

    void Start()
    {
        StartCoroutine(InokeAfterTime(() => Destroy(gameObject)));
    }

    IEnumerator InokeAfterTime(System.Action action)
    {
        yield return new WaitForSeconds(timeInSeconds);
        action?.Invoke();
    }
}
