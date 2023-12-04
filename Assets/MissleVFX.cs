using DesignPatterns.ObjectPool;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;
using static UnityEngine.GraphicsBuffer;

public class MissleVFX : UnityEngine.MonoBehaviour
{
    // deactivate after delay
    [SerializeField] private float timeoutDelay = 10f;



    [SerializeField] private GameObject rocketRenderer;
    [SerializeField] private List<ParticleSystem> particleSystems;
    [SerializeField] private AudioSource explosionSound;
    [SerializeField] private ParticleSystem explosionVFX;


    [SerializeField] private float impactRadius = 5f;
    [SerializeField] private float rocketDamage;
    [SerializeField] private float knockback = 100;

    [Header("REFERENCES")]
    private Rigidbody _rb;
    private helicopterMovement _target;

    [Header("MOVEMENT")]
    [SerializeField] private float _speed = 15;
    [SerializeField] private float _rotateSpeed = 95;

    [Header("PREDICTION")]
    [SerializeField] private float _maxDistancePredict = 100;
    [SerializeField] private float _minDistancePredict = 5;
    [SerializeField] private float _maxTimePrediction = 5;
    private Vector3 _standardPrediction, _deviatedPrediction;

    [Header("DEVIATION")]
    [SerializeField] private float _deviationAmount = 50;
    [SerializeField] private float _deviationSpeed = 2;

    private float detectionRadius = 5f;
    bool targetFound = false;
    private IObjectPool<MissleVFX> objectPool;

    // public property to give the projectile a reference to its ObjectPool
    public IObjectPool<MissleVFX> ObjectPool { set => objectPool = value; }

    bool exploded;
    private bool isExploding = false;
    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }
    void Update()
    {
        CheckForTarget();
        //blowup missle when target is gone
        if (targetFound == true && exploded == false)
        {
            if (_target == null)
            {
                DetonateMissle();
            }
        }
      
    }
    private void PredictMovement(float leadTimePercentage)
    {
        var predictionTime = Mathf.Lerp(0, _maxTimePrediction, leadTimePercentage);

        _standardPrediction = _target.rb.position + _target.rb.velocity * predictionTime;
    }

    private void AddDeviation(float leadTimePercentage)
    {
        var deviation = new Vector3(Mathf.Cos(Time.time * _deviationSpeed), 0, 0);

        var predictionOffset = transform.TransformDirection(deviation) * _deviationAmount * leadTimePercentage;

        _deviatedPrediction = _standardPrediction + predictionOffset;
    }

    private void RotateRocket()
    {
        var heading = _deviatedPrediction - transform.position;

        var rotation = Quaternion.LookRotation(heading);
        _rb.MoveRotation(Quaternion.RotateTowards(transform.rotation, rotation, _rotateSpeed * Time.deltaTime));
    }

    private void FixedUpdate()
    {
        if(targetFound == true && exploded == false)
        {
            _rb.velocity = transform.forward * _speed;



            var leadTimePercentage = Mathf.InverseLerp(_minDistancePredict, _maxDistancePredict, Vector3.Distance(transform.position, _target.transform.position));


            PredictMovement(leadTimePercentage);

            AddDeviation(leadTimePercentage);

            RotateRocket();
        }

    }
    void CheckForTarget()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRadius);

        foreach (Collider collider in colliders)
        {
            helicopterMovement helicopterTarget = collider.GetComponent<helicopterMovement>();
            if (helicopterTarget != null && targetFound == false)
            {
                //found a target
                _target= helicopterTarget;
                _rb.isKinematic = false;
                targetFound = true;

            }

        }
    }
    public void Deactivate()
    {
        StartCoroutine(DeactivateRoutine(timeoutDelay));
    }
    private void OnTriggerEnter(Collider collison)
    {
        if (!collison.gameObject.CompareTag("Bullet") && exploded == false)
        {
            DetonateMissle();
        }
    }
    void DetonateMissle()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.velocity = Vector3.zero;
        if (!isExploding)
        {
            CheckForTargetsInImpactArea();
        }
        explosionVFX.Play();
        explosionSound.Play();
        for (int x = 0; x < particleSystems.Count; x++)
        {
            particleSystems[x].Stop();
        }
        exploded = true;
        rocketRenderer.SetActive(false);
        StartCoroutine(RemoveRocketOnCollision());
    }
    // Call this method to check for objects within the impact area during runtime
    public void CheckForTargetsInImpactArea()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, impactRadius);

        foreach (Collider collider in colliders)
        {
            // Get the closest point on the collider to the center of the impact area
            Vector3 closestPoint = collider.ClosestPoint(transform.position);

            // Calculate the distance between the rocket and the closest point on the collider
            float distance = Vector3.Distance(transform.position, closestPoint);

            float calculatedDamage = CalculateDamage(rocketDamage, distance);
            ITarget target = collider.GetComponent<ITarget>();
            if (target != null)
            {
                Rigidbody rb = collider.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    Vector3 forceDirection = collider.transform.position - transform.position;
                    Debug.Log(forceDirection);
                    forceDirection.Normalize(); // Normalize to get a unit vector
                    rb.AddForce(forceDirection * knockback);

                    Vector3 pushVelocity = forceDirection * calculatedDamage * 10;
                    //keep velocity if its crate
                    Crate crateBox = collider.GetComponent<Crate>();
                    if (crateBox != null)
                    {
                        crateBox.setInitialVelocity(pushVelocity);
                    }
                }
            }
            IDestroyable destroyable = collider.GetComponent<IDestroyable>();
            if (destroyable != null)
            {
                destroyable.OnDamage((int)calculatedDamage);
            }
        }
    }
    private float CalculateDamage(float baseDamage, float distance)
    {
        // Use an inverse-square law for damage falloff
        float falloffFactor = 1f / (distance * distance);
        float calculatedDamage = baseDamage * falloffFactor;

        // Ensure damage is not greater than the base damage
        calculatedDamage = Mathf.Min(calculatedDamage, baseDamage);
        // Ensure damage is not negative
        return Mathf.Max(calculatedDamage, 10);
    }

    IEnumerator RemoveRocketOnCollision()
    {
        yield return new WaitForSeconds(4);

        explosionVFX.Stop();
        explosionSound.Stop();

        // release the projectile back to the pool
        objectPool.Release(this);
    }

    IEnumerator DeactivateRoutine(float delay)
    {
        rocketRenderer.SetActive(true);
        _rb.velocity = Vector3.zero;
        targetFound = false;
        exploded = false;
        explosionVFX.Stop();
        explosionSound.Stop();
        yield return new WaitForSeconds(delay);
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.velocity = Vector3.zero;
        for (int x = 0; x < particleSystems.Count; x++)
        {
            particleSystems[x].Stop();
        }

        rocketRenderer.SetActive(false);
        yield return new WaitForSeconds(4);
        // release the projectile back to the pool
        objectPool.Release(this);
    }
}
