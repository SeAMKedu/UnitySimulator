using UnityEngine;
using Assets.Scripts.Models;

namespace Assets.Scripts.TwinCAT
{
    public class Conveyor : MonoBehaviour
    {

        public string conveyorName = "Conveyor";
        public string programOrganizationUnit = "MAIN";

        private TwinCATVariable conveyorOn;
        private TwinCAT_ADS twincatADS;
        private GameObject objectToMove;

        void Awake()
        {
            conveyorOn = new TwinCATVariable(conveyorName, programOrganizationUnit);
            objectToMove = null;
        }
        
        void Start()
        {
            twincatADS = GetComponentInParent<TwinCAT_ADS>();
        }


        void Update()
        {
            ReadAndCheck();
            //MoveObject();
        }

        private void ReadAndCheck()
        {
            if(twincatADS.ReadFromTwincat(conveyorOn.name))
                conveyorOn.state = true;
            if (twincatADS.ReadFromTwincat(conveyorOn.name) == false)
                conveyorOn.state = false;
        }

        private void MoveObject()
        {

        }

        void OnCollisionEnter(Collision collidedObject)
        {

        }

        void OnTriggerEnter()
        {

        }

    }
}