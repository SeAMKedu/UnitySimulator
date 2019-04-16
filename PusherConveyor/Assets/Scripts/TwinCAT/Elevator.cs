using UnityEngine;

namespace Assets.Scripts.TwinCAT
{
    public class Elevator : MonoBehaviour
    {
        [SerializeField]
        private string elevatorName = "Elevator";
        [SerializeField]
        private string programOrganizationUnit = "MAIN";

        private bool elevatorMoving = false;
        private bool writeSucceeded = true;

        private TwincatVariable elevatorTableUp;
        private TwincatVariable elevatorTableDown;

        // Output
        private TwincatVariable elevatorTableIsLifted;
        private TwincatVariable elevatorTableIsDescended;

        private TwincatAdsController twincatADS;
        private Animator animator;

        void Awake()
        {
            elevatorTableUp = new TwincatVariable(
                elevatorName + "Up",
                programOrganizationUnit,
                TwincatVariableType.Output);

            elevatorTableDown = new TwincatVariable(
                elevatorName + "Down",
                programOrganizationUnit,
                TwincatVariableType.Output);

            elevatorTableIsLifted = new TwincatVariable(
                elevatorName + "IsLifted",
                programOrganizationUnit,
                TwincatVariableType.Input);

            elevatorTableIsDescended = new TwincatVariable(
                elevatorName + "IsDescended",
                programOrganizationUnit,
                TwincatVariableType.Input);

            // Set current state.
            elevatorTableIsDescended.Data = true;
        }

        void Start()
        {
            twincatADS = GetComponentInParent<TwincatAdsController>();
            animator = GetComponent<Animator>();
            WriteToTwincat();
        }

        /// <summary>
        /// Write the current values to the TwinCAT ADS.
        /// </summary>
        private void WriteToTwincat()
        {
            if (twincatADS.WriteToTwincat(elevatorTableIsDescended)
             && twincatADS.WriteToTwincat(elevatorTableIsLifted))
                writeSucceeded = true;

            else
                writeSucceeded = false;
        }

        void Update()
        {
            try
            {
                if (!writeSucceeded)
                    WriteToTwincat();

                if (!elevatorMoving)
                    ReadAndCheck();
            }
            catch (System.Exception)
            {
                writeSucceeded = false;
                Debug.Log("TwinCAT is not running.");
            }
        }

        /// <summary>
        /// Check if a value has changed in the ADS.
        /// </summary>
        private void ReadAndCheck()
        {
            if (twincatADS.ReadFromTwincat(elevatorTableUp.Name) 
                && elevatorTableIsDescended.DataAsBool())
            {
                elevatorMoving = true;
                elevatorTableIsDescended.Data = false;
                elevatorTableIsLifted.Data = true;
                animator.SetTrigger("Up");
            }

            if (twincatADS.ReadFromTwincat(elevatorTableDown.Name) 
                && elevatorTableIsLifted.DataAsBool())
            {
                elevatorMoving = true;
                elevatorTableIsDescended.Data = true;
                elevatorTableIsLifted.Data = false;
                animator.SetTrigger("Down");
            }
        }

        /// <summary>
        /// Called in the animation when it finishes.
        /// </summary>
        public void AnimationFinished()
        {
            elevatorMoving = false;
            WriteToTwincat();
        }
    }
}
