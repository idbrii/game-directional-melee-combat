using UnityEngine;
using System.Collections;

// TODO: Doesn't work. Never gets a target. Come back to this later when I've
// got more combat figured out.
public class Seeking : MonoBehaviour {

    [Tooltip("The Tag for the object we are seeking.")]
    public string targetTag = "";

    [Tooltip("How far we move in one frame.")]
    public float stepDistance = 5;
    
    
    public GameObject target = null;

    void Start()
    {
    }

    void Update()
    {
        Transform closest_locator = FindClosestTaggedChild(target, transform.position);
        if (closest_locator)
        {
            transform.LookAt(closest_locator);
            Vector3.MoveTowards(transform.position, closest_locator.position, stepDistance);
        }
    }

    Transform FindClosestTaggedChild(GameObject parent, Vector3 pos)
    {
        Transform closest_locator = null;

        if (parent != null)
        {
            float closest_distance_sqr = Mathf.Infinity;

            foreach (Transform child in parent.transform)
            {
                Vector3 delta = child.position - pos;
                float distance_sqr = delta.sqrMagnitude;
                if (distance_sqr < closest_distance_sqr)
                {
                    closest_distance_sqr = distance_sqr;
                    closest_locator = child;
                }
            }
        }

        return closest_locator;
    }


    void TryToTarget(GameObject other)
    {
        if (other.CompareTag(targetTag))
        {
            target = other;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        TryToTarget(other.gameObject);
    }

    void OnTriggerStay(Collider other)
    {
        TryToTarget(other.gameObject);
    }
}
