using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.TwinCAT
{
    public class Product : MonoBehaviour
    {
        private Rigidbody thisRigidBody;
        private GameObject spawnlocation;

        private Conveyor currentConveyor = null;
        private List<GameObject> conveyorEndPoints = new List<GameObject>();
        private GameObject currentTarget = null;

        private bool clonedMyself = false;

        [HideInInspector]
        public bool Moving = false;
        [HideInInspector]
        public float forceToBePushedWith = 0.001f;

        void Start()
        {
            spawnlocation = GameObject.FindGameObjectWithTag("Respawn");
            thisRigidBody = GetComponent<Rigidbody>();
            conveyorEndPoints.AddRange(GameObject.FindGameObjectsWithTag("ConveyorEnd"));
        }

        void Update()
        {
            if (currentTarget == null)
            {
                currentTarget = FindClosestGameObject(conveyorEndPoints);
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
                PushMe(forceToBePushedWith);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!clonedMyself)
            {
                if (other.tag == "ConveyorBelt")
                {
                    Debug.Log("Out of bounds");

                    Instantiate(
                        gameObject,
                        spawnlocation.transform.position,
                        new Quaternion(0, 0, 0, 0));

                    Destroy(gameObject, 10);
                    clonedMyself = true;
                }
                else if (other.CompareTag("Box"))
                {
                    Debug.Log("Inside box");
                    ReachedCheckPoint();

                    Instantiate(
                        gameObject, 
                        spawnlocation.transform.position, 
                        new Quaternion(0, 0, 0, 0));

                    clonedMyself = true;
                }
            }
        }

        public void PushMe(float force)
        {
            thisRigidBody.velocity = (Vector3.forward * 10);
        }

        private void OnConveyor()
        {
            if (currentConveyor != null)
            {
                if ((bool)currentConveyor.conveyorOn.Data && Time.timeScale != 0)
                {
                    Moving = true;
                    thisRigidBody.transform.Translate(Vector3.left * 0.1f);
                }
                else
                    Moving = false;
            }
        }

        public void ReachedCheckPoint()
        {
            conveyorEndPoints.Remove(currentTarget);
            currentTarget = null;
            currentConveyor = null;
        }

        private GameObject FindClosestGameObject(List<GameObject> gameObjecsToCheck)
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
}