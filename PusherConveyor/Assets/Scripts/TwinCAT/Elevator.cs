using UnityEngine;
using Assets.Scripts.Models;

namespace Assets.Scripts.TwinCAT
{
    public class Elevator : MonoBehaviour
    {

        public string elevatorName = "Elevator";
        public string programOrganizationUnit = "MAIN";

        private int elevatorPosition = 0;

        private TwinCATVariable elevatorTableUp;
        private TwinCATVariable elevatorTableDown;

        private TwinCATVariable elevatorTableIsLifted;
        private TwinCATVariable elevatorTableIsDescended;

        private TwinCAT_ADS twincatADS;
        Animator animator;

        private bool elevatorMoving;

        void Awake()
        {
            elevatorTableUp = new TwinCATVariable(elevatorName + "TableUp", programOrganizationUnit);
            elevatorTableDown = new TwinCATVariable(elevatorName + "TableDown", programOrganizationUnit);
            elevatorTableIsLifted = new TwinCATVariable(elevatorName + "TableIsLifted", programOrganizationUnit);
            elevatorTableIsDescended = new TwinCATVariable(elevatorName + "TableIsDescended", programOrganizationUnit);
            elevatorMoving = false;
            elevatorTableIsDescended.state = true;
        }

        void Start()
        {
            twincatADS = GetComponentInParent<TwinCAT_ADS>();
            animator = GetComponent<Animator>();
            twincatADS.WriteToTwincat(elevatorTableIsDescended.name, elevatorTableIsDescended.state);
            twincatADS.WriteToTwincat(elevatorTableIsLifted.name, elevatorTableIsLifted.state);
        }


        void Update()
        {
            if(!elevatorMoving)
                ReadAndCheck();
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
            CheckAndWrite();
        }

        private void CheckAndWrite()
        {
            if (elevatorPosition == 0)
            {
                elevatorTableIsDescended.state = true;
                twincatADS.WriteToTwincat(elevatorTableIsDescended.name, elevatorTableIsDescended.state);
            }
            else
            {
                elevatorTableIsDescended.state = false;
                twincatADS.WriteToTwincat(elevatorTableIsDescended.name, elevatorTableIsDescended.state);
            }

            if (elevatorPosition == 1)
            {
                elevatorTableIsLifted.state = true;
                twincatADS.WriteToTwincat(elevatorTableIsLifted.name, elevatorTableIsLifted.state);
            }
            else
            {
                elevatorTableIsLifted.state = false;
                twincatADS.WriteToTwincat(elevatorTableIsLifted.name, elevatorTableIsLifted.state);
            }
        }
    }
}
