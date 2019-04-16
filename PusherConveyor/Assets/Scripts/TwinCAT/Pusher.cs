using UnityEngine;

namespace Assets.Scripts.TwinCAT
{
    public class Pusher : MonoBehaviour
    {
        [SerializeField]
        private string pusherName = "Pusher";
        [SerializeField]
        private string programOrganizationUnit = "MAIN";

        private bool pusherMoving = false;
        private bool writeSucceeded = true;

        private TwincatVariable pusherPush;
        private TwincatVariable pusherRetract;

        private TwincatVariable pusherPushed;
        private TwincatVariable pusherRetracted;

        private TwincatAdsController twincatADS;
        Animator animator;
        
        void Awake()
        {
            pusherPush = new TwincatVariable(
                pusherName + "Push",
                programOrganizationUnit,
                TwincatVariableType.Output);

            pusherRetract = new TwincatVariable(
                pusherName + "Retract",
                programOrganizationUnit,
                TwincatVariableType.Output);

            pusherPushed = new TwincatVariable(
                pusherName + "Pushed",
                programOrganizationUnit,
                TwincatVariableType.Input);

            pusherRetracted = new TwincatVariable(
                pusherName + "Retracted",
                programOrganizationUnit,
                TwincatVariableType.Input);

            // Set current state.
            pusherRetracted.Data = true;
        }

        void Start()
        {
            twincatADS = GetComponentInParent<TwincatAdsController>();
            animator = GetComponent<Animator>();
            WriteToTwincat();
        }

        private void WriteToTwincat()
        {
            if (twincatADS.WriteToTwincat(pusherRetracted)
                && twincatADS.WriteToTwincat(pusherPushed))
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

                if (!pusherMoving)
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
            if (twincatADS.ReadFromTwincat(pusherPush.Name) && pusherRetracted.DataAsBool())
            {
                pusherMoving = true;
                pusherRetracted.Data = false;
                pusherPushed.Data = true;
                animator.SetTrigger("Pushing");
            }

            if (twincatADS.ReadFromTwincat(pusherRetract.Name) && pusherPushed.DataAsBool())
            {
                pusherMoving = true;
                pusherRetracted.Data = true;
                pusherPushed.Data = false;
                animator.SetTrigger("Retracting");
            }
        }

        public void AnimationFinished()
        {
            pusherMoving = false;
            WriteToTwincat();
        }

    }
}
