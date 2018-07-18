using System.Collections.Generic;
using TwinCAT.Ads;
using UnityEngine;
using Assets.Scripts.Models;

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

        public void WriteToTwincat(string name, object state)
        {
            try
            {
                Debug.Log("Wrote: " + name + " to " + state.ToString());
                twincatAdsClient.WriteAny(twincatAdsClient.CreateVariableHandle(name), state);
            }
            catch (AdsErrorException)
            {
                Debug.Log("TwinCAT is not running.");

            }
            
        }

        public void WriteToTwincat(TwinCATVariable twinCATVariable)
        {
            Debug.Log("Wrote: " + name + " to " + twinCATVariable.ToString());
            twincatAdsClient.WriteAny(twincatAdsClient.CreateVariableHandle(twinCATVariable.name), twinCATVariable.state);
        }

        public bool ReadFromTwincat(string name)
        {
            //Debug.Log("Reading: " + name);
            return bool.Parse(twincatAdsClient.ReadAny(twincatAdsClient.CreateVariableHandle(name), typeof(bool)).ToString());
        }

    }
}
