using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingText : MonoBehaviour
{
    private float destroyTime = 1f;
    private Vector3 offset = new Vector3(1, 2, 0);
    private Vector3 randomizeIntensity = new Vector3(0.5f, 0, 0);

    private void Start()
    {
        Destroy(gameObject, destroyTime);

        transform.localPosition += offset;
        transform.localPosition += new Vector3(Random.Range(-randomizeIntensity.x, randomizeIntensity.x),
            Random.Range(-randomizeIntensity.y, randomizeIntensity.y),
            Random.Range(-randomizeIntensity.z, randomizeIntensity.z));
    }

    void Update()
    {
        transform.LookAt(Camera.main.transform, Vector3.down);
    }
}
