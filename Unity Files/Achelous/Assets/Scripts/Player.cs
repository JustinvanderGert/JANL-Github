using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerMovement_FirstPerson moveScript;
    public List <GameObject> visibleTargets = new List<GameObject>();
    public Transform projectileSpawnpoint;
    public GameObject projectile;

    public float minTargetingRange;
    public float shootingCooldown;

    bool rechargeShot = false;

    GameObject currentTarget;



    private void Start()
    {
        moveScript = GetComponent<PlayerMovement_FirstPerson>();
    }

    void Update()
    {
        //If targets are visible find the closest
        if (visibleTargets.Count > 0)
        {
            currentTarget = GetClosestTarget();

            if (currentTarget.GetComponent<Target>().currentCharge == 0)
            {
                currentTarget.GetComponent<Target>().startCharging = true;
            }

            //If you press left click, shoot
            if (!rechargeShot && Input.GetMouseButtonDown(0))
            {
                Shoot();
            }
        }
    }

    GameObject GetClosestTarget()
    {
        GameObject closestObj = null;
        float closestDist = -1;

        for (int i = 0; i < visibleTargets.Count; i++)
        {
            GameObject current = visibleTargets[i];

            float dist = Vector2.Distance(transform.position, current.transform.position);

            if (dist < closestDist || closestDist == -1)
            {
                closestObj = current;
                closestDist = dist;
            }
        }

        if(closestObj != currentTarget)
        {
            closestObj.GetComponent<Target>().isTarget(true);
            if (currentTarget != null)
                currentTarget.GetComponent<Target>().isTarget(false);
        }
        return closestObj;
    }

    void Shoot()
    {
        StartCoroutine(Cooldown());
        GameObject Shotprojectile = Instantiate(projectile, projectileSpawnpoint.position, projectileSpawnpoint.rotation);
        Shotprojectile.GetComponent<Projectile>().SetTarget(currentTarget);
    }

    IEnumerator Cooldown()
    {
        rechargeShot = true;
        yield return new WaitForSeconds(shootingCooldown);
        rechargeShot = false;
    }
}