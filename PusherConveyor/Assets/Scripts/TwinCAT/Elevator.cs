using UnityEngine;
using Assets.Scripts.Models;

namespace Assets.Scripts.TwinCAT
{
    public class Elevator : MonoBehaviour
    {

        public string elevatorName = "Elevator";
        public string programOrganizationUnit = "MAIN";

        private TwinCATVariable elevatorTableUp;
        private TwinCATVariable elevatorTableDown;
        private TwinCAT_ADS twincatADS;
        Animator animator;

        private bool elevatorMoving;

        void Awake()
        {
            elevatorTableUp = new TwinCATVariable(elevatorName + "TableUp", programOrganizationUnit);
            elevatorTableDown = new TwinCATVariable(elevatorName + "TableDown", programOrganizationUnit);
            elevatorMoving = false;
        }

        void Start()
        {
            twincatADS = GetComponentInParent<TwinCAT_ADS>();
            animator = GetComponent<Animator>();
        }


        void Update()
        {
            if(!elevatorMoving)
                ReadAndCheck();
        }

        private void ReadAndCheck()
        {
            if (twincatADS.ReadFromTwincat(elevatorTableUp.name))
            {
                elevatorMoving = true;
                animator.SetTrigger("Up");
            }

            if (twincatADS.ReadFromTwincat(elevatorTableDown.name))
            {
                elevatorMoving = true;
                animator.SetTrigger("Down");
            }
        }

        public void AnimationFinished()
        {
            elevatorMoving = false;
        }

    }
}
