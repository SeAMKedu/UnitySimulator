using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Assets.Scripts.Models;

namespace Assets.Scripts.TwinCAT
{
    public class Sensor : MonoBehaviour
    {
        public string sensorName = "Sensor";
        public string programOrganizationUnit = "MAIN";

        private GameObject shootingpoint;

        private TwinCATVariable sensor;
        private TwinCAT_ADS twincatADS;
        Animator animator;

        private bool objectIsOnSensor = false;
        private int sensorState = 0;

        void Awake()
        {
            sensor = new TwinCATVariable(sensorName, programOrganizationUnit);
        }

        void Start()
        {
            twincatADS = GetComponentInParent<TwinCAT_ADS>();
            animator = GetComponent<Animator>();
            shootingpoint = transform.Find("PassiveInfraredSensor/PointToShootRayFrom").gameObject;
        }

        void Update()
        {
            InfraredRay();
        }

        private void InfraredRay()
        {
            var direction = transform.TransformDirection(Vector3.forward);
            //note the use of var as the type. This is because in c# you 
            // can have lamda functions which open up the use of untyped variables
            //these variables can only live INSIDE a function. 
            RaycastHit hit;
            Debug.DrawRay(shootingpoint.transform.position, direction * 5, Color.red);

            if (Physics.Raycast(shootingpoint.transform.position, direction, out hit, 2))
            {

                Debug.Log("HIT");

                if (hit.collider.gameObject.tag == "Product")
                {
                    //Destroy(GetComponent("Rigidbody"));
                    //Product product = hit.transform.gameObject.GetComponent<Product>();
                    //product.IamMoving
                    objectIsOnSensor = true;
                    sensorState = 1;
                    CheckAndWrite();
                }
                else
                {
                    objectIsOnSensor = false;
                    sensorState = 0;
                    CheckAndWrite();
                }
                    
            }
        }

        private void CheckAndWrite()
        {
            if (objectIsOnSensor && sensorState == 0)
            {
                //animator.SetTrigger("Hit");
                sensor.state = true;
                twincatADS.WriteToTwincat(sensor.name, sensor.state);
            }
            if (!objectIsOnSensor && sensorState == 1)
            {
                //animator.SetTrigger("Normal");
                sensor.state = false;
                twincatADS.WriteToTwincat(sensor.name, sensor.state);
            }
        }


    }
}
