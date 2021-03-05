using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using System;
using System.Linq;
using UnityEditor;
using UnityEngine.UI;

public class ShoppingAgent : Agent
{
    // Start is called before the first frame update

    private CharacterController controller;
    public GameObject spawn;
    public GameObject exit;
    private GameObject[] shopTargets;
    private ArrayList ownTargets;
    public int numberOfItems = 2;
    private int nrCollectedItems = 0;
    private GameObject closestTarget;
    private float speed = 15.0F;
    private float rotateSpeed = 4.0F;
    GameObject rewardText;
    void Start()
    {

    }

    public override void Initialize()
    {
   
        controller = gameObject.GetComponent<CharacterController>();
        controller.enabled = false;
        transform.position = spawn.transform.position;
        controller.enabled = true;
        shopTargets = GameObject.FindGameObjectsWithTag("ShopTarget");
        rewardText =  GameObject.FindGameObjectWithTag("Text");


    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(rewardText);
        Text rw = rewardText.GetComponentInChildren<Text>();
        if (rw != null)
        {
            rw.text = "Reward: " + GetCumulativeReward();

        }
    }

    public override void OnEpisodeBegin()
    {
        nrCollectedItems = 0;
        System.Random random = new System.Random();
        ownTargets = new ArrayList();

        for (int i = 0; i < numberOfItems; i++)
        {
            int randIndex = random.Next(0, shopTargets.Length);
            if (!ownTargets.Contains(shopTargets[randIndex])){
                ownTargets.Add(shopTargets[randIndex]);
            }
            else
            {
                i--;
            }

        }

        
        controller.enabled = false;
        transform.position = spawn.transform.position;
        controller.enabled = true;

        TrailRenderer tr = GetComponentInChildren<TrailRenderer>();
        if (tr != null)
        {
        
        tr.Clear();

    }
    closestTarget = GetNextTarget();


    }

     void OnControllerColliderHit(ControllerColliderHit hit)
    {
       /* if (hit.gameObject.CompareTag("Obstacle"))
        {
            AddReward(-0.000000001f);
        }

        if (hit.gameObject.CompareTag("Player"))
        {
            AddReward(-0.000000001f);
        }
        */

    }

    public void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.CompareTag("Exit"))
        {
            AddReward(50 * (nrCollectedItems / numberOfItems));
            MeshRenderer exitMesh = exit.GetComponent<MeshRenderer>();
            exitMesh.enabled = true;
            StartCoroutine(DisableMesh(exitMesh));

            EndEpisode();
        }


        if (other.gameObject.CompareTag("ShopTarget"))
        {
            if (ownTargets.Contains(other.gameObject))
                {
                AddReward(5f);
               nrCollectedItems++;
                ownTargets.Remove(other.gameObject);
                MeshRenderer mesh = other.gameObject.GetComponent<MeshRenderer>();
                mesh.enabled = true;
         
                StartCoroutine(DisableMesh(mesh));


                closestTarget = GetNextTarget();
                
            }
        }

        if (other.gameObject.CompareTag("Aerosole") && this.StepCount>100)
        {
                //Add Reward id agent hits aerosole-zone of other agents
               AddReward(-0.01f);
        }
    }

    IEnumerator DisableMesh(MeshRenderer mesh)
    {

        yield return new WaitForSeconds(1);
        mesh.enabled = false;

    }

    public override void OnActionReceived(ActionBuffers actionBuffers)

    {
        
        var horizontal = actionBuffers.ContinuousActions[0];
        var Vertical = actionBuffers.ContinuousActions[1];

    
        if(transform.position.y < -5)
        {
            AddReward(-5f);
      
            EndEpisode();
        }

        transform.Rotate(0, horizontal * rotateSpeed, 0);

        Vector3 forward = transform.TransformDirection(Vector3.forward);
        float curSpeed = speed * Vertical;
        controller.SimpleMove(forward * curSpeed);


        AddReward(-1f / MaxStep);


    }


    private GameObject GetNextTarget()
    {

        GameObject nextTarget = null;

        float smallestDistance = 9999999f;
        foreach (GameObject target in ownTargets)
        {
            var dist = Vector3.Distance(transform.position, target.transform.position);

            if ((float)dist < (float)smallestDistance)
            {
                nextTarget = target;
                smallestDistance = dist;
            }

        }


        if (nextTarget == null)
        {
            nextTarget = exit;

        }

        return nextTarget;
    }

    public override void CollectObservations(VectorSensor sensor)
    {

        sensor.AddObservation(transform.position); // 3
        sensor.AddObservation(closestTarget.transform.position); // 3
        sensor.AddObservation(exit.transform.position); // 3
        sensor.AddObservation(nrCollectedItems / numberOfItems); //1

    }



    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var continuousActionsOut = actionsOut.ContinuousActions;
        continuousActionsOut[0] = Input.GetAxis("Horizontal");
        continuousActionsOut[1] = Input.GetAxis("Vertical");

    }

}
