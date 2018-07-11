using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.TwinCAT;

public class Product : MonoBehaviour {

    private List<Conveyor> encounteredConveyers;
    private Conveyor currentConveyor = null;
    private Rigidbody thisRigidBody;
    private GameObject[] conveyorEndPoints;
    private GameObject currentTarget = null;

    void Start ()
    {
        thisRigidBody = GetComponent<Rigidbody>();
        encounteredConveyers = new List<Conveyor>();
        conveyorEndPoints = GameObject.FindGameObjectsWithTag("ConveyorEnd");
    }
	

	void Update ()
    {
        OnConveyor();

        if (currentTarget = null)
        {
            currentTarget = FindClosestGameObject(conveyorEndPoints);
            currentConveyor = currentTarget.gameObject.GetComponentInParent<Conveyor>();
        }
        else
        {
            OnConveyor();
        }
	}

    private void OnConveyor()
    {
        if (currentConveyor != null)
        {
            if ((bool)currentConveyor.conveyorOn.state)
            {
                
                thisRigidBody.transform.Translate(Vector3.left * 5);
            }
        }
    }

    void OnTriggerEnter(Collider collidedObject)
    {
        if (collidedObject.gameObject.CompareTag("Conveyor"))
        {
            encounteredConveyers.Add(collidedObject.gameObject.GetComponentInChildren<Conveyor>());
            currentConveyor = encounteredConveyers[0];
        }
    }

    void OnTriggerExit(Collider collidedObject)
    {
        encounteredConveyers.RemoveAt(0);
        if (encounteredConveyers.Count > 0)
        {
            currentConveyor = encounteredConveyers[0];
        }
        else
        {
            currentConveyor = null;
        }
        
    }

    GameObject FindClosestGameObject(GameObject[] gameObjecsToCheck)
    {
        GameObject closest = null;
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;
        foreach (GameObject go in gameObjecsToCheck)
        {
            Vector3 diff = go.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance)
            {
                closest = go;
                distance = curDistance;
            }
        }

        return closest;
    }

}
