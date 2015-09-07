using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ColliderCollector : MonoBehaviour
{
    List<GameObject> currentCollection = new List<GameObject>();

    public GameObject GetFirstOverlappingObject()
    {
        if (currentCollection.Count > 0)
        {
            return currentCollection[0];
        }
        else
        {
            return null;
        }
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
