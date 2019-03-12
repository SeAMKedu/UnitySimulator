using UnityEngine;

namespace Assets.Scripts.TwinCAT
{
    public class Elevator : MonoBehaviour
    {

        public string elevatorName = "Elevator";
        public string programOrganizationUnit = "MAIN";

        private int elevatorPosition = 0;
        private bool elevatorMoving = false;
        private bool writeSucceeded = true;

        private TwincatVariable elevatorTableUp;
        private TwincatVariable elevatorTableDown;

        private TwincatVariable elevatorTableIsLifted;
        private TwincatVariable elevatorTableIsDescended;

        private TwincatAdsController twincatADS;
        Animator animator;

        void Awake()
        {
            elevatorTableUp = new TwincatVariable(elevatorName + "TableUp", programOrganizationUnit);
            elevatorTableDown = new TwincatVariable(elevatorName + "TableDown", programOrganizationUnit);
            elevatorTableIsLifted = new TwincatVariable(elevatorName + "TableIsLifted", programOrganizationUnit);
            elevatorTableIsDescended = new TwincatVariable(elevatorName + "TableIsDescended", programOrganizationUnit);
        }

        void Start()
        {
            twincatADS = GetComponentInParent<TwincatAdsController>();
            animator = GetComponent<Animator>();
            Setup();
        }

        private void Setup()
        {
            elevatorTableIsDescended.Data = true;
            elevatorTableIsLifted.Data = false;
            if (twincatADS.WriteToTwincat(elevatorTableIsDescended.Name, elevatorTableIsDescended.Data)
             && twincatADS.WriteToTwincat(elevatorTableIsLifted.Name, elevatorTableIsLifted.Data))
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
            if (twincatADS.ReadFromTwincat(elevatorTableUp.Name) && elevatorPosition == 0)
            {
                elevatorMoving = true;
                elevatorPosition = 1;
                animator.SetTrigger("Up");
            }

            if (twincatADS.ReadFromTwincat(elevatorTableDown.Name) && elevatorPosition == 1)
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
                elevatorTableIsDescended.Data = true;
                if (!twincatADS.WriteToTwincat(elevatorTableIsDescended.Name, elevatorTableIsDescended.Data))
                    return false;
            }
            else
            {
                elevatorTableIsDescended.Data = false;
                if (!twincatADS.WriteToTwincat(elevatorTableIsDescended.Name, elevatorTableIsDescended.Data))
                    return false;
            }

            if (elevatorPosition == 1)
            {
                elevatorTableIsLifted.Data = true;
                if (!twincatADS.WriteToTwincat(elevatorTableIsLifted.Name, elevatorTableIsLifted.Data))
                    return false;
            }
            else
            {
                elevatorTableIsLifted.Data = false;
                if (!twincatADS.WriteToTwincat(elevatorTableIsLifted.Name, elevatorTableIsLifted.Data))
                    return false;
            }

            return true;
        }


    }
}
