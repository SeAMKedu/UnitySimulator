using UnityEngine;
using Assets.Scripts.Models;

namespace Assets.Scripts.TwinCAT
{
    public class Conveyor : MonoBehaviour
    {

        public string conveyorName = "Conveyor";
        public string programOrganizationUnit = "MAIN";

        [HideInInspector]
        public TwinCATVariable conveyorOn;
        private TwinCAT_ADS twincatADS;

        void Awake()
        {
            conveyorOn = new TwinCATVariable(conveyorName, programOrganizationUnit);
        }
        
        void Start()
        {
            twincatADS = GetComponentInParent<TwinCAT_ADS>();
        }


        void Update()
        {
            ReadAndCheck();
        }

        private void ReadAndCheck()
        {
            if (twincatADS.ReadFromTwincat(conveyorOn.name) && (bool)conveyorOn.state == false)
            {
                Debug.Log("Conveyer is on");
                conveyorOn.state = true;
            }
            if (twincatADS.ReadFromTwincat(conveyorOn.name) == false && (bool)conveyorOn.state)
            {
                Debug.Log("Conveyer is off");
                conveyorOn.state = false;
            }
        }
        /*
        private void MoveObject(Collision hit)
        {
            hit.transform.Translate(Vector3.left*5);
        }

        void OnCollisionStay(Collision hit)
        {
            if (hit.gameObject.tag == "Product")
            {
                Debug.Log(conveyorOn.state);
                if ((bool)conveyorOn.state)
                    MoveObject(hit);
            }
        }
        */

        /*
        void OnTriggerStay(Collider hit)
        {
            
        }
        */

        /*
        void OnCollisionExit(Collision leaver)
        {
            leaver.transform.Translate(Vector3.left * 50);
        }
        */
        /*
        void OnCollisionEnter(Collision collidedObject)
        {
            //objectToMove = collidedObject.rigidbody;
        }

        void OnTriggerEnter()
        {

        }
        */
    }
}