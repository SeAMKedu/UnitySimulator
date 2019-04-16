using UnityEngine;

namespace Assets.Scripts.TwinCAT
{
    public class Sensor : MonoBehaviour
    {
        [SerializeField]
        private string sensorName = "Sensor";
        [SerializeField]
        private string programOrganizationUnit = "MAIN";

        public GameObject shootingpoint;
        private GameObject infraredRay;
        private Vector3 infraredRayDefaultEnd;
        private Vector3 infraredRayCurrentEnd;

        private TwincatVariable sensor;
        private TwincatAdsController twincatADS;

        private bool objectIsOnSensor = false;

        private void Awake()
        {
            sensor = new TwincatVariable(
                sensorName,
                programOrganizationUnit,
                TwincatVariableType.Input);
        }

        private void Start()
        {
            twincatADS = GetComponentInParent<TwincatAdsController>();

            // Infrared
            if (shootingpoint == null)
            {
                shootingpoint = transform.
                    Find("PassiveInfraredSensor/PointToShootRayFrom").gameObject;
            }

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

        /// <summary>
        /// Create and handle logic of the infrared ray.
        /// </summary>
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
                    CheckThenWrite();
                }         
            }
            else
            {
                // Check whethever the default infrared has been created or not. 
                // No use destroying and creating it each frame.
                if (infraredRayCurrentEnd != infraredRayDefaultEnd)
                {
                    Destroy(infraredRay);
                    infraredRay = DrawLine(
                        shootingpoint.transform.position,
                        infraredRayDefaultEnd,
                        Color.red);
                }
                objectIsOnSensor = false;
                CheckThenWrite();
            }

        }

        /// <summary>
        /// Draw a line.
        /// </summary>
        /// <param name="start">Start vector of line.</param>
        /// <param name="end">End vector of line.</param>
        /// <param name="color">Color of the line.</param>
        /// <returns>GameObject of the drawn line.</returns>
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

        /// <summary>
        /// Check if a value should be changed in the ADS.
        /// </summary>
        private void CheckThenWrite()
        {
            // Object is on sensor and value in ADS is false.
            if (objectIsOnSensor && !sensor.DataAsBool())
            {
                sensor.Data = true;
                twincatADS.WriteToTwincat(sensor.Name, sensor.Data);
            }

            // Object is not on sensor and value in ADS is true.
            if (!objectIsOnSensor && sensor.DataAsBool())
            {
                sensor.Data = false;
                twincatADS.WriteToTwincat(sensor.Name, sensor.Data);
            }
        }

    }
}
