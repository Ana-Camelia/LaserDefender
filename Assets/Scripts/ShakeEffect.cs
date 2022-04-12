using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeEffect : MonoBehaviour
{
    [SerializeField] float shakeTime = 0.5f;
    [SerializeField] float shakePower = 0.25f;

    Vector3 initialPos;
    void Start()
    {
        initialPos = transform.position;
    }

    // Update is called once per frame
    public void StartShaking()
    {
        StartCoroutine(Shake());
    }

    IEnumerator Shake()
    {
        float elapsedTime = 0;
        while(elapsedTime < shakeTime)
        {
            transform.position = initialPos + (Vector3)Random.insideUnitCircle * shakePower;
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        transform.position = initialPos;
    }
}
