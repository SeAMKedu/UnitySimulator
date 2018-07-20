using UnityEngine;
using Assets.Scripts.Models;

namespace Assets.Scripts.TwinCAT
{
    public class Pusher : MonoBehaviour
    {

        public string pusherName = "Pusher";
        public string programOrganizationUnit = "MAIN";

        private int pusherPosition = 0;
        private bool pusherMoving = false;
        private bool writeSucceeded = true;

        private TwinCATVariable pusherPush;
        private TwinCATVariable pusherRetract;

        private TwinCATVariable pusherPushed;
        private TwinCATVariable pusherRetracted;

        private TwinCAT_ADS twincatADS;
        Animator animator;
        
        void Awake()
        {
            pusherPush = new TwinCATVariable(pusherName + "Push", programOrganizationUnit);
            pusherRetract = new TwinCATVariable(pusherName + "Retract", programOrganizationUnit);
            pusherPushed = new TwinCATVariable(pusherName + "Pushed", programOrganizationUnit);
            pusherRetracted = new TwinCATVariable(pusherName + "Retracted", programOrganizationUnit);
        }

        void Start()
        {
            twincatADS = GetComponentInParent<TwinCAT_ADS>();
            animator = GetComponent<Animator>();
            Setup();
        }

        private void Setup()
        {
            pusherPushed.state = false;
            pusherRetracted.state = true;
            if (twincatADS.WriteToTwincat(pusherRetracted.name, pusherRetracted.state)
                && twincatADS.WriteToTwincat(pusherPushed.name, pusherPushed.state))
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
            if (twincatADS.ReadFromTwincat(pusherPush.name) && pusherPosition == 0)
            {
                pusherMoving = true;
                pusherPosition = 1;
                animator.SetTrigger("Pushing");
            }

            if (twincatADS.ReadFromTwincat(pusherRetract.name) && pusherPosition == 1)
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
                pusherRetracted.state = true;
                if (!twincatADS.WriteToTwincat(pusherRetracted.name, pusherRetracted.state))
                    return false;
            }
            else
            {
                pusherRetracted.state = false;
                if (!twincatADS.WriteToTwincat(pusherRetracted.name, pusherRetracted.state))
                    return false;
            }

            if (pusherPosition == 1)
            {
                pusherPushed.state = true;
                if (!twincatADS.WriteToTwincat(pusherPushed.name, pusherPushed.state))
                    return false;
            }
            else
            {
                pusherPushed.state = false;
                if (!twincatADS.WriteToTwincat(pusherPushed.name, pusherPushed.state))
                    return false;
            }

            return true;
        }

    }
}
