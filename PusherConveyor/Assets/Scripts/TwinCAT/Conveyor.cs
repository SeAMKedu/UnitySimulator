using UnityEngine;

namespace Assets.Scripts.TwinCAT
{
    public class Conveyor : MonoBehaviour
    {
        [SerializeField]
        private string conveyorName = "Conveyor";
        [SerializeField]
        private string programOrganizationUnit = "MAIN";

        private bool writeSucceeded = true;

        // output
        [HideInInspector]
        public TwincatVariable conveyorOn;

        // input
        private TwincatVariable conveyorRunning;

        private TwincatAdsController twincatADS;

        void Awake()
        {
            conveyorOn = new TwincatVariable(
                conveyorName, 
                programOrganizationUnit,
                TwincatVariableType.Output);

            conveyorRunning = new TwincatVariable(
                conveyorName + "Running",
                programOrganizationUnit,
                TwincatVariableType.Input);
        }
        
        void Start()
        {
            twincatADS = GetComponentInParent<TwincatAdsController>();
        }

        void Update()
        {
            if (!writeSucceeded)
                WriteToTwincat();

            ReadAndCheck();
        }

        private void ReadAndCheck()
        {
            if (twincatADS.ReadFromTwincat(conveyorOn.Name) 
                && !conveyorOn.DataAsBool())
            {
                Debug.Log(conveyorName + " is on");
                conveyorOn.Data = true;

                if (!conveyorRunning.DataAsBool())
                {
                    conveyorRunning.Data = true;
                    WriteToTwincat();
                }
            }
            else if (!twincatADS.ReadFromTwincat(conveyorOn.Name))
            {
                Debug.Log(conveyorName + " is off");
                conveyorOn.Data = false;

                if (conveyorRunning.DataAsBool())
                {
                    conveyorRunning.Data = false;
                    WriteToTwincat();
                }
            }
        }

        private void WriteToTwincat()
        {
            if (twincatADS.WriteToTwincat(conveyorRunning))
                writeSucceeded = true;

            else
                writeSucceeded = false;
        }

    }
}