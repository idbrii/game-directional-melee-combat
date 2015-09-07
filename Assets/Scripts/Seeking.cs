using UnityEngine;
using System.Collections;

public class Seeking : MonoBehaviour {

    [Tooltip("The Tag for the object we are seeking.")]
    public string targetTag = "";

    [Tooltip("How far we move in one frame.")]
    public float stepDistance = 5;
    
    private GameObject selectedTarget = null;
    private GameObject ticketAcquired = null;

    public GameObject SelectedTarget
    {
        get;
        private set;
    }

    void Start()
    {
        // HACK: For now, just find the player immediately.
        selectedTarget = GameObject.FindGameObjectsWithTag(targetTag)[0].transform.parent.gameObject;
    }

    void Update()
    {
        if (ticketAcquired == null)
        {
            Transform closest_locator = FindClosestTaggedChild(selectedTarget, transform.position);
            if (closest_locator)
            {
                bool success = AcquireTicket(selectedTarget, closest_locator.gameObject);
                if (success)
                {
                    transform.LookAt(closest_locator);
                }
            }
        }
        else
        {
            Vector3.MoveTowards(transform.position, ticketAcquired.transform.position, stepDistance);
        }
    }

    bool AcquireTicket(GameObject ticketer, GameObject ticket_type)
    {
        var tickets = ticketer.GetComponents<Ticketing>();
        foreach (var ticket_dispenser in tickets)
        {
            if (ticket_dispenser.ticketType == ticket_type)
            {
                bool success = ticket_dispenser.AcquireTicket(gameObject);
                if (success)
                {
                    ticketAcquired = ticket_dispenser.gameObject;
                }

                // We found the right ticket type, give up on finding another
                // even if acquiring failed.
                return success;
            }
        }

        return false;
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


// TODO: Doesn't work. Never gets a target. Come back to this later when I've
// got more combat figured out.
    void TryToTarget(GameObject other)
    {
        if (other.CompareTag(targetTag))
        {
            selectedTarget = other;
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
