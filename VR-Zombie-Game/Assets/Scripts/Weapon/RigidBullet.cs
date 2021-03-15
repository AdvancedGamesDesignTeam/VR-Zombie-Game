using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidBullet : MonoBehaviour
{
    private float currentLifeTime = 0f;
    [SerializeField] private float lifeTime;
    [SerializeField] private float bulletForce;
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        gameObject.SetActive(false);
    }

    private void Update()
    {
        if (!gameObject.activeSelf) return;
        currentLifeTime += Time.deltaTime;
        if (currentLifeTime > lifeTime)
        {
            StopBullet();
        }
    }

    public void LaunchProjectile(Transform spawnPosition)
    {
        StopBullet();
        transform.rotation = spawnPosition.rotation;
        transform.position = spawnPosition.position;
        gameObject.SetActive(true);
        rb.AddForce(transform.forward * bulletForce, ForceMode.Impulse);
    }

    private void StopBullet()
    {
        rb.velocity = Vector3.zero;
        gameObject.SetActive(false);
        currentLifeTime = 0f;
    }
}
