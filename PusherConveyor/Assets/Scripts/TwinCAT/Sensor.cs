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
        private GameObject infraredRay;
        private Vector3 infraredRayDefaultEnd;
        private Vector3 infraredRayCurrentEnd;

        private TwinCATVariable sensor;
        private TwinCAT_ADS twincatADS;

        private bool objectIsOnSensor = false;
        private int sensorState = 0;

        void Awake()
        {
            sensor = new TwinCATVariable(sensorName, programOrganizationUnit);
        }

        void Start()
        {
            twincatADS = GetComponentInParent<TwinCAT_ADS>();
            twincatADS.WriteToTwincat(sensor.name, sensor.state);
            shootingpoint = transform.Find("PassiveInfraredSensor/PointToShootRayFrom").gameObject;

            infraredRayDefaultEnd = new Vector3(shootingpoint.transform.position.x, shootingpoint.transform.position.y, 0.4f);
            infraredRayCurrentEnd = infraredRayDefaultEnd;

            infraredRay = DrawLine(shootingpoint.transform.position, infraredRayDefaultEnd, Color.red);
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
            //Debug.DrawRay(shootingpoint.transform.position, direction * 5, Color.red);

            if (Physics.Raycast(shootingpoint.transform.position, direction, out hit, 5))
            {
                infraredRayCurrentEnd = infraredRayDefaultEnd;
                infraredRayCurrentEnd.z = hit.transform.position.z;
                Destroy(infraredRay);
                infraredRay = DrawLine(shootingpoint.transform.position, infraredRayCurrentEnd, Color.red);

                Debug.Log("HIT");

                if (hit.collider.gameObject.tag == "Product")
                {
                    objectIsOnSensor = true;
                    CheckAndWrite();
                }         
            }
            else
            {
                // Check whethever the default infrared has been created or not. No use destroying and creating it each frame.
                if (infraredRayCurrentEnd != infraredRayDefaultEnd)
                {
                    Destroy(infraredRay);
                    infraredRay = DrawLine(shootingpoint.transform.position, infraredRayDefaultEnd, Color.red);
                }
                objectIsOnSensor = false;
                CheckAndWrite();
            }

        }

        GameObject DrawLine(Vector3 start, Vector3 end, Color color, float duration = 0.2f)
        {
            GameObject myLine = new GameObject();
            myLine.name = sensor.name + " infrared light";
            myLine.transform.position = start;
            myLine.AddComponent<LineRenderer>();
            LineRenderer lr = myLine.GetComponent<LineRenderer>();
            lr.material = new Material(Shader.Find("Particles/Alpha Blended Premultiply"));
            lr.startColor = color;
            lr.endColor = color;
            lr.startWidth = 0.1f;
            lr.endWidth = 0.1f;
            lr.SetPosition(0, start);
            lr.SetPosition(1, end);
            //GameObject.Destroy(myLine, duration);
            return myLine;
        }


        private void CheckAndWrite()
        {
            if (objectIsOnSensor && sensorState == 0)
            {
                sensor.state = true;
                sensorState = 1;
                twincatADS.WriteToTwincat(sensor.name, sensor.state);
            }
            if (!objectIsOnSensor && sensorState == 1)
            {
                sensorState = 0;
                sensor.state = false;
                twincatADS.WriteToTwincat(sensor.name, sensor.state);
            }
        }


    }
}
