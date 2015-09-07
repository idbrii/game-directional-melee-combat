using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ColliderCollector : MonoBehaviour
{
    List<GameObject> currentCollection = new List<GameObject>();

    public GameObject GetFirstOverlappingRootObject()
    {
        foreach (var other in currentCollection)
        {
            Transform other_root = other.transform.root;
            // Don't consider myself to be overlapping with my other parts.
            if (other_root != transform.root)
            {
                return other_root.gameObject;
            }
        }

        return null;
    }

    void OnTriggerEnter(Collider other)
    {
        currentCollection.Add(other.gameObject);
    }

    void OnTriggerExit(Collider other)
    {
        currentCollection.Remove(other.gameObject);
    }
}
