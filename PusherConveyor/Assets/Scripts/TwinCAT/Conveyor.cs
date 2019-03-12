using UnityEngine;

namespace Assets.Scripts.TwinCAT
{
    public class Conveyor : MonoBehaviour
    {

        public string conveyorName = "Conveyor";
        public string programOrganizationUnit = "MAIN";

        [HideInInspector]
        public TwincatVariable conveyorOn;
        private TwincatAdsController twincatADS;

        void Awake()
        {
            conveyorOn = new TwincatVariable(conveyorName, programOrganizationUnit);
        }
        
        void Start()
        {
            twincatADS = GetComponentInParent<TwincatAdsController>();
        }


        void Update()
        {
            ReadAndCheck();
        }

        private void ReadAndCheck()
        {
            if (twincatADS.ReadFromTwincat(conveyorOn.Name) && (bool)conveyorOn.Data == false)
            {
                Debug.Log(conveyorName + " is on");
                conveyorOn.Data = true;
            }
            if (twincatADS.ReadFromTwincat(conveyorOn.Name) == false && (bool)conveyorOn.Data)
            {
                Debug.Log(conveyorName + " is off");
                conveyorOn.Data = false;
            }
        }

    }
}