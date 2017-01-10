using UnityEngine;
using System.Collections;

public class LerpExample : MonoBehaviour
{
    public float lerpTime = 1f;
    float currentLerpTime;

    public float moveDistance = 5f;

    Vector3 startPos;
    Vector3 endPos;

    protected void Start()
    {
        startPos = transform.position;
        endPos = transform.position + transform.up * moveDistance;
    }

    protected void Update()
    {
        //reset when we press spacebar
        if (Input.GetKeyDown(KeyCode.Space))
        {
            currentLerpTime = 0f;
        }

        //increment timer once per frame
        currentLerpTime += Time.deltaTime;
        if (currentLerpTime > lerpTime)
        {
            currentLerpTime = lerpTime;
        }

        //lerp!
        float t = currentLerpTime / lerpTime;
        // grado 5
        //t = t * t * t * (t * (6f * t - 15f) + 10f);
        // grado 7
        //t = -20 * Mathf.Pow(t, 7f) + 70 * Mathf.Pow(t, 6f) - 84 * Mathf.Pow(t, 5f) + 35 * Mathf.Pow(t, 4f);
        // grado 9
        t = 70 * Mathf.Pow(t, 9f) - 315 * Mathf.Pow(t, 8f) + 540 * Mathf.Pow(t, 7f) - 420 * Mathf.Pow(t, 6f) + 126 * Mathf.Pow(t, 5f);
        transform.position = Vector3.Lerp(startPos, endPos, t);
    }
}