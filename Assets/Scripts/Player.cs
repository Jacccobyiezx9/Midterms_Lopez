using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float shootInterval = 1.0f;
    public float detectionRange = 5f;
    public Color currentColor;

    private DetectionZone detectionZone;
    private Animator animator;
    private Coroutine shootingCoroutine;

    private Color[] colorCycle = { Color.red, Color.blue, Color.green, Color.yellow };
    private int currentColorIndex;
    private int currentColorValue;

    void Start()
    {
        detectionZone = GetComponentInChildren<DetectionZone>();
        animator = GetComponent<Animator>();

        currentColorIndex = Random.Range(0, colorCycle.Length);
        currentColor = colorCycle[currentColorIndex];
        currentColorValue = currentColorIndex;
        GetComponent<SpriteRenderer>().color = currentColor;

        shootingCoroutine = null;
    }

    private void StartShooting()
    {
        if (shootingCoroutine == null)
        {
            shootingCoroutine = StartCoroutine(ShootBulletRoutine());
        }
    }

    private void StopShooting()
    {
        if (shootingCoroutine != null)
        {
            StopCoroutine(shootingCoroutine);
            shootingCoroutine = null;
        }
    }

    IEnumerator ShootBulletRoutine()
    {
        while (true)
        {
            Shoot();
            yield return new WaitForSeconds(shootInterval);
        }
    }

    void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

        bullet.GetComponent<Bullet>().SetColorValue(currentColorValue);

        Collider2D nearestEnemy = FindNearestEnemy();
        if (nearestEnemy != null)
        {
            bullet.GetComponent<Bullet>().SetTarget(nearestEnemy.transform.position);
        }

        animator.SetTrigger("swordAttack");
    }

    Collider2D FindNearestEnemy()
    {
        if (detectionZone == null || detectionZone.detectObjs == null || detectionZone.detectObjs.Count == 0)
            return null;

        Collider2D nearestEnemy = null;
        float closestDistance = detectionRange;

        foreach (var enemy in detectionZone.detectObjs)
        {
            float distance = Vector2.Distance(transform.position, enemy.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                nearestEnemy = enemy;
            }
        }

        return nearestEnemy;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            CycleColor(1);
        }
        else if (Input.GetMouseButtonDown(1))
        {
            CycleColor(-1);
        }
    }

    void CycleColor(int direction)
    {
        currentColorIndex = (currentColorIndex + direction + colorCycle.Length) % colorCycle.Length;
        currentColor = colorCycle[currentColorIndex];
        currentColorValue = currentColorIndex;
        GetComponent<SpriteRenderer>().color = currentColor;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            StartShooting();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (detectionZone.detectObjs.Count == 0)
            {
                StopShooting();
            }
        }
    }
}