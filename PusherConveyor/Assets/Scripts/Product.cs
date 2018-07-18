using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.TwinCAT;

public class Product : MonoBehaviour {

    private Rigidbody thisRigidBody;
    private GameObject spawnlocation;

    private Conveyor currentConveyor = null;
    private List<GameObject> conveyorEndPoints = new List<GameObject>();
    private GameObject currentTarget = null;

    private bool clonedMyself = false;

    [HideInInspector]
    public bool IamMoving = false;
    [HideInInspector]
    public float forceToBePushedWith = 0.001f;

    void Start ()
    {
        spawnlocation = GameObject.FindGameObjectWithTag("Respawn");
        thisRigidBody = GetComponent<Rigidbody>();
        conveyorEndPoints.AddRange(GameObject.FindGameObjectsWithTag("ConveyorEnd"));
    }
	

	void Update ()
    {
        
        if (currentTarget == null)
        {
            currentTarget = FindClosestGameObject(conveyorEndPoints.ToArray());
            if (currentTarget != null)
                currentConveyor = currentTarget.gameObject.GetComponentInParent<Conveyor>();
        }
        else
        {
            OnConveyor();
        }
        
	}
    
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Pusher")
        {
            //GameObject gameobject = transform.Find("Spatula/MyActualLocation").gameObject;
            PushMe(forceToBePushedWith);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("I am triggered by " + other.name);
        if (!clonedMyself)
        {
            if (other.tag == "ConveyorBelt")
            {
                Debug.Log("Out of bounds");
                Instantiate(gameObject, spawnlocation.transform.position, new Quaternion(0, 0, 0, 0));
                Destroy(gameObject, 10);
                clonedMyself = true;
            }
            else if (other.CompareTag("Box"))
            {
                Debug.Log("Inside box");
                ReachedCheckPoint();
                Instantiate(gameObject, spawnlocation.transform.position, new Quaternion(0, 0, 0, 0));
                clonedMyself = true;
            }
        }
    }

    /*
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.transform.parent != null)
        {
            if (collision.gameObject.transform.parent.gameObject.name == "ShakingPart")
            {
                ShakeMe();
            }
        }

    }
    */
    public void ShakeMe()
    {
        
        thisRigidBody.AddForce(new Vector3(
                               Random.Range(-10, 11), 
                               0, 
                               Random.Range(-10, 11)
                               ) * 10);
       
    }

    public void PushMe(float force)
    {
        //thisRigidBody.AddForce(Vector3.forward * 350);
        thisRigidBody.velocity = (Vector3.forward * 10);
    }

    
    public void PushMe(Collision collision, float force)
    {
        // Source: https://answers.unity.com/questions/1100879/push-object-in-opposite-direction-of-collision.html

        // Calculate Angle Between the collision point and the player
        Vector3 dir = collision.contacts[0].point - transform.position;
        // We then get the opposite (-Vector3) and normalize it
        dir = -dir.normalized;
        Debug.Log(dir.ToString());
        // And finally we add force in the direction of dir and multiply it by force. 
        // This will push back the player
        //thisRigidBody.AddForce(dir * force);
        thisRigidBody.transform.Translate(dir * force);
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
                IamMoving = false;
                
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
