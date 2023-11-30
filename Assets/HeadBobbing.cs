using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadBobbing : MonoBehaviour
{
    [SerializeField] private bool enable = true;
    [SerializeField,Range(0,10)] private float amplitude = 0.0015f;
    [SerializeField, Range(0, 30)] private float frequency = 10;

    [SerializeField] private Transform camera = null;
    [SerializeField] private Transform camHolder = null;

    private float toggleSpeed = 3;
    private Vector3 startingPos;
    private PlayerMovement playerMovement;
    private void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        startingPos = camera.localPosition;
    }
    private void Update()
    {
        if(enable == true)
        {
            CheckMotion();
 

        }
    }
    private Vector3 walkingBob(bool sprinting)
    {
        Vector3 pos = Vector3.zero;
        float sprintMultiplier;
        if(sprinting == true)
        {
            sprintMultiplier = 1.5f;
        }
        else
        {
            sprintMultiplier = 1;
        }
        pos.y += Mathf.Sin(Time.time * frequency * sprintMultiplier) * (amplitude * sprintMultiplier / 1000); 
        pos.x += Mathf.Cos(Time.time * frequency/2 * sprintMultiplier) * (amplitude * sprintMultiplier / 1000)/2;

        return pos;
    }
    void CheckMotion()
    {
        ResetPosition();
        if (playerMovement.state == PlayerMovement.MovementState.walking)
        {
            PlayMotion(walkingBob(false));
        }
        else if(playerMovement.state == PlayerMovement.MovementState.sprinting)
        {
            PlayMotion(walkingBob(true));
        }
    }
    void ResetPosition()
    {
        if(camera.localPosition != startingPos)
        {
            camera.localPosition = Vector3.Lerp(camera.localPosition, startingPos, 1*Time.deltaTime);
        }

    }
    void PlayMotion(Vector3 motion)
    {
        camera.localPosition += motion;
    }
}
