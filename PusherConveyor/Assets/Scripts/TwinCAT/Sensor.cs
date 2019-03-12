using UnityEngine;

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

        private TwincatVariable sensor;
        private TwincatAdsController twincatADS;

        private bool objectIsOnSensor = false;
        private int sensorState = 0;

        private void Awake()
        {
            sensor = new TwincatVariable(sensorName, programOrganizationUnit);
        }

        private void Start()
        {
            twincatADS = GetComponentInParent<TwincatAdsController>();
            
            // Infrared 
            shootingpoint = transform.
                Find("PassiveInfraredSensor/PointToShootRayFrom").gameObject;

            infraredRayDefaultEnd = new Vector3(
                shootingpoint.transform.position.x,
                shootingpoint.transform.position.y,
                0.4f);

            infraredRayCurrentEnd = infraredRayDefaultEnd;
            infraredRay = DrawLine(
                shootingpoint.transform.position, 
                infraredRayDefaultEnd, 
                Color.red);

            twincatADS.WriteToTwincat(sensor.Name, sensor.Data);
        }

        private void Update()
        {
            InfraredRay();
        }

        private void InfraredRay()
        {
            var direction = transform.TransformDirection(Vector3.forward);

            RaycastHit hit;

            if (Physics.Raycast(shootingpoint.transform.position, direction, out hit, 5))
            {
                infraredRayCurrentEnd = infraredRayDefaultEnd;
                infraredRayCurrentEnd.z = hit.transform.position.z;
                Destroy(infraredRay);
                infraredRay = DrawLine(
                    shootingpoint.transform.position,
                    infraredRayCurrentEnd,
                    Color.red);

                Debug.Log("There is an object on " + sensor.Name);

                if (hit.collider.gameObject.tag == "Product")
                {
                    objectIsOnSensor = true;
                    CheckAndWrite();
                }         
            }
            else
            {
                // Check whethever the default infrared has been created or not. 
                // No use destroying and creating it each frame.
                if (infraredRayCurrentEnd != infraredRayDefaultEnd)
                {
                    Destroy(infraredRay);
                    infraredRay = DrawLine(shootingpoint.transform.position, infraredRayDefaultEnd, Color.red);
                }
                objectIsOnSensor = false;
                CheckAndWrite();
            }

        }

        private GameObject DrawLine(Vector3 start, Vector3 end, Color color)
        {
            // New GameObject for the line
            GameObject myLine = new GameObject
            {
                name = sensor.Name + " infrared light"
            };

            myLine.transform.position = start;
            myLine.AddComponent<LineRenderer>();
            LineRenderer lineRenderer = myLine.GetComponent<LineRenderer>();
            lineRenderer.material = new Material(
                Shader.Find("Particles/Alpha Blended Premultiply"));

            // Set color
            lineRenderer.startColor = color;
            lineRenderer.endColor = color;

            // Set width
            lineRenderer.startWidth = 0.1f;
            lineRenderer.endWidth = 0.1f;

            // Set position
            lineRenderer.SetPosition(0, start);
            lineRenderer.SetPosition(1, end);
            return myLine;
        }

        private void CheckAndWrite()
        {
            if (objectIsOnSensor && sensorState == 0)
            {
                sensor.Data = true;
                sensorState = 1;
                twincatADS.WriteToTwincat(sensor.Name, sensor.Data);
            }
            if (!objectIsOnSensor && sensorState == 1)
            {
                sensorState = 0;
                sensor.Data = false;
                twincatADS.WriteToTwincat(sensor.Name, sensor.Data);
            }
        }

    }
}
