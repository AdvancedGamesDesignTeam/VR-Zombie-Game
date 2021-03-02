using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WeaponInteract;

public class Barrel : MonoBehaviour
{
    public enum ShootingType
    {
        Rigidbody,
        Ray
    }
    public ShootingType shootingType;
    public bool isOnlySingleFireMode;
    public bool isOnSingleFireMode;
    public int magazineSize;
    public int currentMagazineSize;
    public float fireSpeed;
    private bool singleFired;
    private Weapon _weapon;
    private Coroutine firing;
    private Coroutine reloading;
    private WaitForSeconds waitForSeconds;
    private WaitForSeconds waitForSecondsRel;
    private RaycastHit _hit;
    private BulletPooling _bulletPoolingScript;
    [SerializeField] private float reloadTime;
    public bool SingleFired
    {
        set => singleFired = value;
    }

    public void Setup(Weapon weapon)
    {
        _weapon = weapon;
    }
    
    private void Awake()
    {
        waitForSeconds = new WaitForSeconds(fireSpeed);
        waitForSecondsRel = new WaitForSeconds(reloadTime);
        _bulletPoolingScript = FindObjectOfType<BulletPooling>();
        currentMagazineSize = magazineSize;
    }

    public void Fire()
    {
        if (singleFired) return;
        if (isOnSingleFireMode) singleFired = true;
        if (currentMagazineSize <= 0) return;
        firing = StartCoroutine(FiringSeq());
    }
    
    public void StopFire()
    {
        try
        {
            StopCoroutine(firing);
        }
        catch (Exception e)
        {
            // ignored
        }
    }
    
    public void Reload()
    {
        if (reloading == null) reloading = StartCoroutine(ReloadRoutine());
    }

    public void StopReload()
    {
        if (reloading == null) return;
        StopCoroutine(reloading);
        reloading = null;
    }

    private void UsePellet()
    {
        if (currentMagazineSize == 0)
        {
            StopCoroutine(firing);
            return;
        }
        currentMagazineSize--;
        if (shootingType == ShootingType.Rigidbody)
        {
            _bulletPoolingScript.currentBullet++;
            _bulletPoolingScript.CheckCurrentBullet();
            RigidShoot(_bulletPoolingScript.GetCurrentBullet());
        }
        else RayBulletShoot();
    }
    
    private IEnumerator FiringSeq()
    {
        while (gameObject.activeSelf)
        {
            UsePellet();
            _weapon.Recoil();
            if (singleFired) yield break;
            yield return waitForSeconds;
        }
    }

    private IEnumerator ReloadRoutine()
    {
        yield return waitForSecondsRel;
        currentMagazineSize = magazineSize;
        _weapon.UseVibration();
        yield return null;
    }

    private void RayBulletShoot()
    {
        if (Physics.Raycast(transform.position, transform.forward, out _hit, 100))
        {
            Debug.Log(_hit.transform.name);
        }
    }

    private void RigidShoot(RigidBullet bullet)
    {
        bullet.LaunchProjectile(transform);
    }
    
    //Still need to make so weapons can have only single fire mode instead of both single/automatic
}
