using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HermitCrab : MonoBehaviour
{
    enum AttackPhase { Idle, Phase1, Phase2, Phase3, Defeated }
    [SerializeField]
    AttackPhase attackPhase = AttackPhase.Idle;
    [SerializeField]
    List<Transform> weakSpots = new List<Transform>();

    public List<GameObject> activeShootables = new List<GameObject>();
    public GameObject shootable;



    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            SetPhase();
        }
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
                break;

            case AttackPhase.Phase2:
                Debug.Log("Phase 2");
                activeShootables.Add(Instantiate(shootable, weakSpots[1].position, weakSpots[1].rotation, transform));
                activeShootables.Add(Instantiate(shootable, weakSpots[2].position, weakSpots[2].rotation, transform));
                break;

            case AttackPhase.Phase3:
                Debug.Log("Phase 3");
                activeShootables.Add(Instantiate(shootable, weakSpots[3].position, weakSpots[3].rotation, transform));
                break;
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

        }

        //Slam Attack (Wave of trash)
        else if (attackPhase == AttackPhase.Phase3)
        {

        }
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
        switch (attackPhase)
        {
            case AttackPhase.Idle:
                attackPhase = AttackPhase.Phase1;
                SpawnShootables();
                break;

            case AttackPhase.Phase1:
                attackPhase = AttackPhase.Phase2;
                SpawnShootables();
                break;

            case AttackPhase.Phase2:
                attackPhase = AttackPhase.Phase3;
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
}