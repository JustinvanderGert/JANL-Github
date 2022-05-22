using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HermitCrab : MonoBehaviour
{
    [SerializeField]
    Animator animator;


    enum AttackPhase { Idle, Phase1, Phase2, Phase3, Defeated }
    [SerializeField]
    AttackPhase attackPhase = AttackPhase.Idle;

    [SerializeField]
    List<Transform> weakSpots = new List<Transform>();

    [SerializeField]
    Collider[] armHitTriggers;
    Coroutine idleRoutine;

    bool leftArmAttack = false;
    bool attackStarted = false;


    public List<GameObject> activeShootables = new List<GameObject>();
    public GameObject shootable;

    public float idleTime = 3;
    public float armAttackDelay = 5;


    void Start()
    {
        gameObject.GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            SetPhase();
        }
        /*
        if (gameObject.GetComponent<Animation>()["LeftArmAttack"].normalizedTime == 0.575)
        {
            Debug.Log("Left Attack retrieving");
            ArmAttackDelay();
        } else if (gameObject.GetComponent<Animation>()["LeftArmAttack"].normalizedTime == 0.75)
        {
            Debug.Log("Right Attack retrieving");
            ArmAttackDelay();
        }*/
    }

    
    //Spawn the shootable areas depending on the current phase.
    public void SpawnShootables()
    {
        activeShootables.Clear();
        switch (attackPhase)
        {
            case AttackPhase.Phase1:
                Debug.Log("Phase 1");
                activeShootables.Add(Instantiate(shootable, weakSpots[0].position, weakSpots[0].rotation, transform));

                idleRoutine = StartCoroutine(IdleTime());
                break;

            case AttackPhase.Phase2:
                Debug.Log("Phase 2");
                activeShootables.Add(Instantiate(shootable, weakSpots[1].position, weakSpots[1].rotation, transform));
                activeShootables.Add(Instantiate(shootable, weakSpots[2].position, weakSpots[2].rotation, transform));

                idleRoutine = StartCoroutine(IdleTime());
                break;

            case AttackPhase.Phase3:
                Debug.Log("Phase 3");
                activeShootables.Add(Instantiate(shootable, weakSpots[3].position, weakSpots[3].rotation, transform));

                idleRoutine = StartCoroutine(IdleTime());
                break;
        }
    }

    IEnumerator IdleTime()
    {
        yield return new WaitForSeconds(idleTime);

        if (!attackStarted)
        {
            attackStarted = true;
            Attack();
        }
    }

    public void Attack()
    {
        //Throw trash
        if (attackPhase == AttackPhase.Phase1)
        {

        }

        //Swing arms
        else if(attackPhase == AttackPhase.Phase2)
        {
            if (leftArmAttack)
            {
                leftArmAttack = false;
                armHitTriggers[1].enabled = true;

                animator.SetBool("RightAttack", false);
                animator.SetTrigger("ArmAttack");
            }
            else
            {
                leftArmAttack = true;
                armHitTriggers[0].enabled = true;

                animator.SetBool("RightAttack", true);
                animator.SetTrigger("ArmAttack");
            }

            StartCoroutine(ArmAttackDelay());
        }

        //Slam Attack (Wave of trash)
        else if (attackPhase == AttackPhase.Phase3)
        {

        }
    }

    //Time between arm attacks
    IEnumerator ArmAttackDelay()
    {
        animator.ResetTrigger("ArmAttack");

        //Turn triggers off
        foreach (Collider arm in armHitTriggers)
        {
            arm.enabled = false;
        }
        yield return new WaitForSeconds(armAttackDelay);

        Attack();
    }


    //What happens when the boss has been hit
    public void Hit()
    {
        activeShootables.RemoveAll(item => item == null);

        if (activeShootables.Count <= 1)
        {
            SetPhase();
        }
    }

    //Progress to the new Phase and do anything else that needs to happen at the start of the Phase
    void SetPhase()
    {
        activeShootables.RemoveAll(item => item == null);
        attackStarted = false;

        switch (attackPhase)
        {
            case AttackPhase.Idle:
                attackPhase = AttackPhase.Phase1;
                animator.SetTrigger("StartNewPhase");
                SpawnShootables();
                break;

            case AttackPhase.Phase1:
                attackPhase = AttackPhase.Phase2;
                animator.SetTrigger("StartNewPhase");
                SpawnShootables();
                break;

            case AttackPhase.Phase2:
                attackPhase = AttackPhase.Phase3;
                animator.SetTrigger("StartNewPhase");
                SpawnShootables();
                break;

            case AttackPhase.Phase3:
                attackPhase = AttackPhase.Defeated;
                break;

            //This one is for testing (Delete before publish)
            case AttackPhase.Defeated:
                attackPhase = AttackPhase.Idle;
                break;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        //Hit the player
    }
}