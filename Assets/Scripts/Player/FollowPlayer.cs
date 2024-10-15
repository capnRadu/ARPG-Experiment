using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField] private GameObject player;
    private Vector3 offset = new Vector3(6.67f, 10.217f, 5.18f);

    private void Update()
    {
        if (player != null)
        {
            transform.position = player.transform.position + offset;
        }
    }
}
