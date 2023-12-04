using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UIElements;

public class helicopterMovement : MonoBehaviour, ITarget, IDestroyable
{
    int health = 1000;
    [SerializeField]
    GameObject bulletHole;
    [SerializeField]
    GameObject helicopterRender;
    [SerializeField]
    GameObject propeller;
    [SerializeField]
    GameObject Backpropeller;
    [SerializeField]
    ParticleSystem bigExplosion;
    [SerializeField]
    AudioSource explosionSound;
    private float rotateSpeed = 360;
    private float currentRotation = 0;

    public float verticalSpeed = 2f; // Adjust the speed of the vertical movement
    private float amplitude = 1f; // Adjust the amplitude of the movement
    private float yOffset = 0f; // Offset to control the starting position of the movement

    public float moveSpeed = 2;
    float xPos = 0;
    public float constantRotationSpeed = -20;
    // Start is called before the first frame update

    bool alive = true;
    bool landed = false;
    public Rigidbody rb;
    Collider collider;
    float randomRotation;
    public Transform getParent()
    {
        return this.transform;
    }
    public GameObject generateBulletHole()
    {
        return bulletHole;
    }
    private void Start()
    {
        collider = gameObject.GetComponent<Collider>();
        collider.isTrigger = false;
      
        rb = GetComponent<Rigidbody>();
        rb.isKinematic= true;
    }
    // Update is called once per frame
    void Update()
    {
        currentRotation += rotateSpeed * Time.deltaTime;
        if (alive == true)
        {
            if (currentRotation >= 360)
            {
                currentRotation = 0;
            }

            propeller.transform.rotation = Quaternion.Euler(-90, 0, currentRotation);
            Backpropeller.transform.Rotate(Vector3.right, 1);
            float yPos = Mathf.Sin(Time.time * verticalSpeed) * amplitude + yOffset;

            transform.position = new Vector3(transform.position.x, 10 + yPos, transform.position.z);
            transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
            transform.Rotate(Vector3.up, constantRotationSpeed * Time.deltaTime);
        }
        else
        {
            if(landed == false)
            {
                transform.Rotate(Vector3.up + -Vector3.forward, randomRotation * Time.deltaTime);
            }
  
        }
        
    }
    void Mayday()
    {

        rb.isKinematic = false;
        int RNG = Random.Range(0, 1);
        if(RNG == 0)
        {
            randomRotation = Random.Range(400, 200);
        }
        else
        {
            randomRotation = Random.Range(-200, -400);
        }
        alive = false;
    }
    private void OnCollisionEnter(Collision collision)
    {
        landed = true;
        collider.isTrigger = true;
        rb.isKinematic = true;
        bigExplosion.Play();
        explosionSound.Play();
        helicopterRender.SetActive(false);
        StartCoroutine(RemoveHelicopter());
    }
    IEnumerator RemoveHelicopter()
    {
       
        yield return new WaitForSeconds(4);
        
        Destroy(gameObject);
    }

    public void OnDamage(int damage)
    {
        health -= damage;
        if(health <= 0)
        {
            Mayday();
        }
    }
}
