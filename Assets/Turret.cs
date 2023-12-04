using DesignPatterns.ObjectPool;
using UnityEngine;

public class Turret : MonoBehaviour
{
    private Transform target;
    public float range = 15f;
    public float fireRate = 10;
    int projectileSpeed = 500;
    private float fireCountdown = 0f;
    public string playerTag = "Player";
    public Transform partToRotate;
    public float turnSpeed = 10f;
    [SerializeField] 
    private ProjectileManager bulletProjectileManager;
    [SerializeField]
    private Transform muzzlePos;
    void Start()
    {
        InvokeRepeating("UpdateTarget",0.5f,1);
    }

    private void UpdateTarget()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, range);

        target = null;

        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Player"))
            {
                target = collider.transform;
                
            }
        }
    }

    void Update()
    {
        if (target == null)
        {
            return;
        }
        Vector3 dir = target.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(dir);
        Vector3 rotation = Quaternion.Lerp(partToRotate.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;
        partToRotate.rotation = Quaternion.Euler(0f, rotation.y, 0f);

        if (fireCountdown <= 0)
        {
            Shoot();
            fireCountdown = 1f / fireRate;
        }

        fireCountdown -= Time.deltaTime;
    }

    void Shoot()
    {
        bulletProjectileManager.shootProjectile(muzzlePos,partToRotate.transform.forward, projectileSpeed);
    }

}