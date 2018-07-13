using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwinCAT.Ads;
using UnityEngine;

namespace Assets.Scripts.TwinCAT
{
    public class TwinCAT_ADS : MonoBehaviour
    {
        public int twincatAdsPort = 851;
        private TcAdsClient twincatAdsClient;

        void Awake()
        {
            twincatAdsClient = new TcAdsClient();
            twincatAdsClient.Connect(twincatAdsPort);
        }

        void Start()
        {
            
        }

        public void WriteToTwincat(string name, object state)
        {
            Debug.Log("Wrote: " + name + " to " + state.ToString());
            twincatAdsClient.WriteAny(twincatAdsClient.CreateVariableHandle(name), state);
        }

        public bool ReadFromTwincat(string name)
        {
            //Debug.Log("Reading: " + name);
            return bool.Parse(twincatAdsClient.ReadAny(twincatAdsClient.CreateVariableHandle(name), typeof(bool)).ToString());
        }
    }
}
