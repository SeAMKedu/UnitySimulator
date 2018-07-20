using UnityEngine;
using Assets.Scripts.Models;

namespace Assets.Scripts.TwinCAT
{
    public class Elevator : MonoBehaviour
    {

        public string elevatorName = "Elevator";
        public string programOrganizationUnit = "MAIN";

        private int elevatorPosition = 0;
        private bool elevatorMoving = false;
        private bool writeSucceeded = true;

        private TwinCATVariable elevatorTableUp;
        private TwinCATVariable elevatorTableDown;

        private TwinCATVariable elevatorTableIsLifted;
        private TwinCATVariable elevatorTableIsDescended;

        private TwinCAT_ADS twincatADS;
        Animator animator;

        void Awake()
        {
            elevatorTableUp = new TwinCATVariable(elevatorName + "TableUp", programOrganizationUnit);
            elevatorTableDown = new TwinCATVariable(elevatorName + "TableDown", programOrganizationUnit);
            elevatorTableIsLifted = new TwinCATVariable(elevatorName + "TableIsLifted", programOrganizationUnit);
            elevatorTableIsDescended = new TwinCATVariable(elevatorName + "TableIsDescended", programOrganizationUnit);
        }

        void Start()
        {
            twincatADS = GetComponentInParent<TwinCAT_ADS>();
            animator = GetComponent<Animator>();
            Setup();
        }

        private void Setup()
        {
            elevatorTableIsDescended.state = true;
            elevatorTableIsLifted.state = false;
            if (twincatADS.WriteToTwincat(elevatorTableIsDescended.name, elevatorTableIsDescended.state)
             && twincatADS.WriteToTwincat(elevatorTableIsLifted.name, elevatorTableIsLifted.state))
                writeSucceeded = true;

            else
                writeSucceeded = false;
        }

        void Update()
        {
            if (!writeSucceeded)
                Setup();

            try
            {
                if (!elevatorMoving)
                    ReadAndCheck();
            }
            catch (System.Exception)
            {
                writeSucceeded = false;
                Debug.Log("TwinCAT not running.");
            }
            
        }

        private void ReadAndCheck()
        {
            if (twincatADS.ReadFromTwincat(elevatorTableUp.name) && elevatorPosition == 0)
            {
                elevatorMoving = true;
                elevatorPosition = 1;
                animator.SetTrigger("Up");
            }

            if (twincatADS.ReadFromTwincat(elevatorTableDown.name) && elevatorPosition == 1)
            {
                elevatorMoving = true;
                elevatorPosition = 0;
                animator.SetTrigger("Down");
            }
        }

        public void AnimationFinished()
        {
            elevatorMoving = false;
            writeSucceeded = CheckAndWrite();
        }

        private bool CheckAndWrite()
        {
            if (elevatorPosition == 0)
            {
                elevatorTableIsDescended.state = true;
                if (!twincatADS.WriteToTwincat(elevatorTableIsDescended.name, elevatorTableIsDescended.state))
                    return false;
            }
            else
            {
                elevatorTableIsDescended.state = false;
                if (!twincatADS.WriteToTwincat(elevatorTableIsDescended.name, elevatorTableIsDescended.state))
                    return false;
            }

            if (elevatorPosition == 1)
            {
                elevatorTableIsLifted.state = true;
                if (!twincatADS.WriteToTwincat(elevatorTableIsLifted.name, elevatorTableIsLifted.state))
                    return false;
            }
            else
            {
                elevatorTableIsLifted.state = false;
                if (!twincatADS.WriteToTwincat(elevatorTableIsLifted.name, elevatorTableIsLifted.state))
                    return false;
            }

            return true;
        }


    }
}
