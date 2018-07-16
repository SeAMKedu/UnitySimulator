using UnityEngine;
using Assets.Scripts.Models;

namespace Assets.Scripts.TwinCAT
{
    public class Pusher : MonoBehaviour
    {

        public string pusherName = "Pusher";
        public string programOrganizationUnit = "MAIN";

        private int pusherPosition = 0;

        private TwinCATVariable pusherPush;
        private TwinCATVariable pusherRetract;

        private TwinCATVariable pusherPushed;
        private TwinCATVariable pusherRetracted;

        private TwinCAT_ADS twincatADS;
        GameObject plate;
        GameObject location;
        Animator animator;
        

        private bool pusherMoving;

        void Awake()
        {
            pusherPush = new TwinCATVariable(pusherName + "Push", programOrganizationUnit);
            pusherRetract = new TwinCATVariable(pusherName + "Retract", programOrganizationUnit);
            pusherPushed = new TwinCATVariable(pusherName + "Pushed", programOrganizationUnit);
            pusherRetracted = new TwinCATVariable(pusherName + "Retracted", programOrganizationUnit);
            pusherMoving = false;
            pusherRetracted.state = true;
        }

        void Start()
        {
            twincatADS = GetComponentInParent<TwinCAT_ADS>();
            animator = GetComponent<Animator>();
            twincatADS.WriteToTwincat(pusherRetracted.name, pusherRetracted.state);
            plate = transform.Find("Spatula/Plate").gameObject;
            location = transform.Find("Spatula/MyActualLocation").gameObject;
        }


        void Update()
        {
            if (!pusherMoving)
                ReadAndCheck();
        }
        
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.tag == "Product")
            {
                Product product = gameObject.GetComponent<Product>();
                product.PushMe(location, product.forceToBePushedWith);
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
            CheckAndWrite();
        }

        private void CheckAndWrite()
        {
            if (pusherPosition == 0)
            {
                pusherRetracted.state = true;
                twincatADS.WriteToTwincat(pusherRetracted.name, pusherRetracted.state);
            }
            else
            {
                pusherRetracted.state = false;
                twincatADS.WriteToTwincat(pusherRetracted.name, pusherRetracted.state);
            }

            if (pusherPosition == 1)
            {
                pusherPushed.state = true;
                twincatADS.WriteToTwincat(pusherPushed.name, pusherPushed.state);
            }
            else
            {
                pusherPushed.state = false;
                twincatADS.WriteToTwincat(pusherPushed.name, pusherPushed.state);
            }
        }

    }
}
