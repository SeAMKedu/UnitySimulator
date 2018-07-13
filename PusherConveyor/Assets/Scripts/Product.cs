using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.TwinCAT;

public class Product : MonoBehaviour {

    //private List<Conveyor> encounteredConveyers;
    public Conveyor currentConveyor = null;
    private Rigidbody thisRigidBody;
    public List<GameObject> conveyorEndPoints = new List<GameObject>();
    public GameObject currentTarget = null;

    [HideInInspector]
    public bool IamMoving = false;

    void Start ()
    {
        thisRigidBody = GetComponent<Rigidbody>();
        //encounteredConveyers = new List<Conveyor>();
        //conveyorEndPoints = GameObject.FindGameObjectsWithTag("ConveyorEnd");
        //GameObject[] holder = GameObject.FindGameObjectsWithTag("ConveyorEnd");
        conveyorEndPoints.AddRange(GameObject.FindGameObjectsWithTag("ConveyorEnd"));
    }
	

	void Update ()
    {
        if (currentTarget == null)
        {
            currentTarget = FindClosestGameObject(conveyorEndPoints.ToArray());
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
                IamMoving = true;
                thisRigidBody.transform.Translate(Vector3.left * 0.1f);
            }
            else
            {
                thisRigidBody.velocity = Vector3.zero;
                thisRigidBody.angularVelocity = Vector3.zero;
                IamMoving = false;
            }
                
        }
    }

    public void ReachedCheckPoint()
    {
        conveyorEndPoints.Remove(currentTarget);
        currentTarget = null;
        currentConveyor = null;
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
