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

        private TwinCATVariable sensor;
        private TwinCAT_ADS twincatADS;
        Animator animator;

        private bool objectIsOnSensor = false;

        void Awake()
        {
            sensor = new TwinCATVariable(sensorName, programOrganizationUnit);
        }

        void Start()
        {
            twincatADS = GetComponentInParent<TwinCAT_ADS>();
            animator = GetComponent<Animator>();
        }


        void Update()
        {
            var up = transform.TransformDirection(Vector3.up);
            //note the use of var as the type. This is because in c# you 
            // can have lamda functions which open up the use of untyped variables
            //these variables can only live INSIDE a function. 
            RaycastHit hit;
            Debug.DrawRay(transform.position, -up * 2, Color.green);

            if (Physics.Raycast(transform.position, -up, out hit, 2))
            {

                Debug.Log("HIT");

                if (hit.collider.gameObject.name == "floor")
                {
                    Destroy(GetComponent("Rigidbody"));
                }
            }

        }

        private void CheckAndWrite()
        {
            if (objectIsOnSensor)
            {
                animator.SetTrigger("Hit");
                sensor.state = true;
                twincatADS.WriteToTwincat(sensor.name, sensor.state);
            }
            else
            {
                animator.SetTrigger("Normal");
                sensor.state = false;
                twincatADS.WriteToTwincat(sensor.name, sensor.state);
            }
        }


    }
}
