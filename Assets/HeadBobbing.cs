using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HeadBobbing : MonoBehaviour
{
    [SerializeField] private bool enable = true;
    [SerializeField,Range(0,10)] private float amplitude = 1.5f;
    [SerializeField, Range(0, 30)] private float Defaultfrequency = 10;

    [SerializeField] private Transform camera = null;
    [SerializeField] private Transform camHolder = null;
    [SerializeField] private Transform bobStabliser = null;
    private WeaponInventory weaponInventory;

    private float toggleSpeed = 3;
    private Vector3 startingPos;
    private PlayerMovement playerMovement;

    Rigidbody rb;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerMovement = GetComponent<PlayerMovement>();
        weaponInventory = GetComponent<WeaponInventory>();
        startingPos = camera.localPosition;
    }
    private void Update()
    {
        if(enable == true)
        {   
            CheckMotion();
            bobStabliser.LookAt(FocusTarget());

        }
        ResetPosition();
    }
    private Vector3 walkingBob(float multiplier, float frequencyMultiplier)
    {
        Vector3 pos = Vector3.zero;
        pos.y += Mathf.Sin(Time.time * (Defaultfrequency * frequencyMultiplier) * multiplier) * (amplitude * multiplier / 50); 
        pos.x += Mathf.Cos(Time.time * (Defaultfrequency * frequencyMultiplier) / 2 * multiplier) * (amplitude * multiplier / 50)/2;

        return pos;
    }
    void CheckMotion()
    {
  

        if(rb.velocity != Vector3.zero)
        {
            switch (playerMovement.state)
            {
                case PlayerMovement.MovementState.walking:
                    PlayMotion(walkingBob(1,1));
                    break;
                case PlayerMovement.MovementState.sprinting:
                    PlayMotion(walkingBob(1.5f, 1));
                    break;
                case PlayerMovement.MovementState.wallrunning:
                    PlayMotion(walkingBob(1.5f, 1));
                    break;
                case PlayerMovement.MovementState.crouching:
                    PlayMotion(walkingBob(0.5f, 1));
                    break;
                case PlayerMovement.MovementState.proning:
                    PlayMotion(walkingBob(2, 0.5f));
                    break;
                case PlayerMovement.MovementState.sliding:
                    break;
            }
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
    Vector3 FocusTarget()
    {
        Vector3 pos = new Vector3(transform.position.x, transform.position.y + camHolder.localPosition.y, transform.position.z);
        pos += camHolder.forward * 15;
        return pos;
    }
}
