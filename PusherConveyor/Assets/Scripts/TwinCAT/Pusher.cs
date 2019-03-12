using UnityEngine;

namespace Assets.Scripts.TwinCAT
{
    public class Pusher : MonoBehaviour
    {

        public string pusherName = "Pusher";
        public string programOrganizationUnit = "MAIN";

        private int pusherPosition = 0;
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
            pusherPush = new TwincatVariable(pusherName + "Push", programOrganizationUnit);
            pusherRetract = new TwincatVariable(pusherName + "Retract", programOrganizationUnit);
            pusherPushed = new TwincatVariable(pusherName + "Pushed", programOrganizationUnit);
            pusherRetracted = new TwincatVariable(pusherName + "Retracted", programOrganizationUnit);
        }

        void Start()
        {
            twincatADS = GetComponentInParent<TwincatAdsController>();
            animator = GetComponent<Animator>();
            Setup();
        }

        private void Setup()
        {
            pusherPushed.Data = false;
            pusherRetracted.Data = true;
            if (twincatADS.WriteToTwincat(pusherRetracted.Name, pusherRetracted.Data)
                && twincatADS.WriteToTwincat(pusherPushed.Name, pusherPushed.Data))
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
            if (twincatADS.ReadFromTwincat(pusherPush.Name) && pusherPosition == 0)
            {
                pusherMoving = true;
                pusherPosition = 1;
                animator.SetTrigger("Pushing");
            }

            if (twincatADS.ReadFromTwincat(pusherRetract.Name) && pusherPosition == 1)
            {
                pusherMoving = true;
                pusherPosition = 0;
                animator.SetTrigger("Retracting");
            }
        }

        public void AnimationFinished()
        {
            pusherMoving = false;
            writeSucceeded = CheckAndWrite();
        }

        private bool CheckAndWrite()
        {
            if (pusherPosition == 0)
            {
                pusherRetracted.Data = true;
                if (!twincatADS.WriteToTwincat(pusherRetracted.Name, pusherRetracted.Data))
                    return false;
            }
            else
            {
                pusherRetracted.Data = false;
                if (!twincatADS.WriteToTwincat(pusherRetracted.Name, pusherRetracted.Data))
                    return false;
            }

            if (pusherPosition == 1)
            {
                pusherPushed.Data = true;
                if (!twincatADS.WriteToTwincat(pusherPushed.Name, pusherPushed.Data))
                    return false;
            }
            else
            {
                pusherPushed.Data = false;
                if (!twincatADS.WriteToTwincat(pusherPushed.Name, pusherPushed.Data))
                    return false;
            }

            return true;
        }

    }
}
