using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : UnityEngine.MonoBehaviour
{
    public Transform camPos;
    // Update is called once per frame
    void Update()
    {
        transform.position = camPos.position;
    }
}
