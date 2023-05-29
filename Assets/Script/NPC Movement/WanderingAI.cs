using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WanderingAI : MonoBehaviour
{


    public float wanderRadius;
    public float wanderTimer;
    Vector3 newPos;
    private Transform target;
    private UnityEngine.AI.NavMeshAgent agent;
    private float timer;

    // Use this for initialization
    void OnEnable()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        timer = wanderTimer;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= wanderTimer)
        {
            newPos = RandomNavSphere(transform.position, wanderRadius, -1);
            agent.SetDestination(newPos);
            timer = 0;
        }
        if(Vector3.Distance(this.transform.position,newPos)<0.1f)
        {
            newPos = RandomNavSphere(transform.position, wanderRadius, -1);
            agent.SetDestination(newPos);
            timer = 0;
        }
    }
    private void OnTriggerEnter(Collider other)
    {

        if(other.gameObject.tag=="Wall")
        {
            Vector3 newPos = RandomNavSphere(transform.position, wanderRadius, -1);
            agent.SetDestination(newPos);
            timer = 0;
        }
    }
    public  Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randomPoint = Vector3.zero;
        NavMeshHit hit;
        int attempts = 0;
        do
        {
            float radius = dist;
            randomPoint = transform.position + Random.insideUnitSphere * radius;
            NavMesh.SamplePosition(randomPoint, out hit, radius, NavMesh.AllAreas);
            attempts++;
        } while (!hit.hit && attempts < 30);

        if (!hit.hit)
        {
            Debug.LogError("Failed to find a reachable NavMesh point after 30 attempts!");
            return Vector3.zero;
        }

        return hit.position;
    }
}
