using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Ticketing : MonoBehaviour {

    // TODO: Consider removing type and just using the parent gameObject
    // instead.
    [Tooltip("The category of ticket: it's purpose.")]
    public GameObject ticketType;
    [Tooltip("Maximum allowed tickets of this type that can be dispensed.")]
    public int maxDispensed = 10;

    private List<GameObject> ticketHolders = new List<GameObject>();
    

    /**
     * Try to acquire a ticket and return whether a valid ticket was acquired.
     */
    public bool AcquireTicket(GameObject requester)
    {
        if (ticketHolders.Count < maxDispensed)
        {
            return false;
        }

        // TODO: support multiple requests from the same object?
        Dbg.Assert(ticketHolders.IndexOf(requester) == -1, "Object already has ticket. Not supporting multiple requests from same object.");

        ticketHolders.Add(requester);
        return true;
    }

    /**
     * Release a ticket and return whether a valid ticket was released.
     */
    public bool ReleaseTicket(GameObject requester)
    {
        return ticketHolders.Remove(requester);
    }
}
