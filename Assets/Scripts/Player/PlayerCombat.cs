using System;
using UnityEngine;
using UnityEngine.Pool;

[RequireComponent(typeof(PlayerStats), typeof(PlayerMovement))]
public class PlayerCombat : MonoBehaviour
{
    [SerializeField] private Bullet bulletPrefab;
    private PlayerStats playerStats;
    private PlayerMovement playerMovement;
    private float nextFireTime = 0f;
    private ObjectPool<Bullet> bulletPool;

    private void Awake()
    {
        playerStats = GetComponent<PlayerStats>();
        playerMovement = GetComponent<PlayerMovement>();
    }
    private void Start()
    {
        bulletPool = new ObjectPool<Bullet>(CreateBullet, ActivateBullet, DesactivateBullet, DestroyBullet, false, 3, 15);
    }

    private Bullet CreateBullet()
    {
        Bullet newBullet = Instantiate(bulletPrefab);
        return newBullet;
    }
    private void ActivateBullet(Bullet bullet)
    {
        bullet.gameObject.SetActive(true);
    }

    private void DesactivateBullet(Bullet bullet)
    {
        bullet.gameObject.SetActive(false);
    }
    private void DestroyBullet(Bullet bullet)
    {
        Destroy(bullet.gameObject);
    }


    private void Update()
    {
        if (playerStats.isFreezed) return;

        HandleShootingInput();
    }

    private void HandleShootingInput()
    {
        Vector2 shootDirection = Vector2.zero;

        if (Input.GetKey(KeyCode.UpArrow)) shootDirection = Vector2.up;
        else if (Input.GetKey(KeyCode.DownArrow)) shootDirection = Vector2.down;
        else if (Input.GetKey(KeyCode.RightArrow)) shootDirection = Vector2.right;
        else if (Input.GetKey(KeyCode.LeftArrow)) shootDirection = Vector2.left;

        if (shootDirection != Vector2.zero && Time.time >= nextFireTime)
        {
            Shoot(shootDirection);
            float fireRate = playerStats.GetStat(PlayerStats.StatType.FireRate);
            nextFireTime = Time.time + (1f / fireRate);
        }
    }

    private void Shoot(Vector2 direction)
    {
        Vector3 spawnPos = transform.position + (Vector3)(direction * 0.5f) + new Vector3(0, 0.5f, 0);
        Bullet bullet = bulletPool.Get();
        bullet.transform.position = spawnPos;
        bullet.transform.rotation = Quaternion.identity;

        if (bullet.TryGetComponent<Bullet>(out var bulletScript))
        {
            bulletScript.SetBulletStats(
                playerStats.GetStat(PlayerStats.StatType.Damage),
                playerStats.GetStat(PlayerStats.StatType.ShotSpeed),
                playerStats.GetStat(PlayerStats.StatType.Range),
                direction,
                playerMovement.Inertia,
                bulletPool
            );
        }
    }
}