using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using RimWorld;

namespace RimWorldComputing
{
    [StaticConstructorOnStartup]
    public class DataNet 
	{
		
		private List<Building> allColonistBuildings;
        private List<Building> deviceList;
		
		public DataNet(List<Building> colonistBuildings)
		{
            this.allColonistBuildings = colonistBuildings;

            deviceList = new List<Building>();

			BuildListOfDevices(this.allColonistBuildings);
		}
		
		
	    /// <summary>
		/// Find all the devices we are going to be controlling and add them to lists of type
		/// </summary>
		private void BuildListOfDevices(List<Building> allColonistBuildings)
		{

            Log.Message("Datanet: Building list of devices");
            Log.Message("Datanet: Pre Loop - Number Of Devices(colonist buildings) # " + allColonistBuildings.Count);
            for (int i = 0; i < allColonistBuildings.Count; i++)
			{
				var currentBuilding = allColonistBuildings[i];
                var currentBuildingCompPowTrader = currentBuilding.TryGetComp<CompPowerTrader>();


                if (currentBuilding != null && currentBuildingCompPowTrader != null && currentBuildingCompPowTrader.PowerOn == true)
				{
                    //this is a colonist building, it has a power trader component and currently has power
                    Log.Message("Datanet:  Thing: " + currentBuilding.Label + " it has a power trader component and currently has power");
                    Log.Message("Datanet:  Thing: " + currentBuilding.Label + " is Type " + currentBuilding.GetType().ToString() );

                    if (currentBuilding.GetType() == typeof(Building_TurretGun) )
					{
                        Log.Message("Datanet:  Thing: " + currentBuilding.Label + " is Type " + currentBuilding.GetType().ToString() + " and can be added to the devices list");

                            AddDevice(allColonistBuildings[i] as Building_TurretGun);
					}
					
				}
					
			}
		}

        /// <summary>
        /// Run in Computing TickWorker checks devices every NormalTick or more likely RareTick and removes devices that no longer have a comppowertrader component or power
        /// </summary>
        public void checkDevicesStatus()
        {

            if (this.allColonistBuildings == null)
                return;

            Log.Message("Datanet: Checking status of Devices in list");
            for (int i = 0; i < this.allColonistBuildings.Count; i++)
            {
                var currentBuilding = this.allColonistBuildings[i];
                var currentBuildingCompPowTrader = currentBuilding.TryGetComp<CompPowerTrader>();

                if (currentBuilding != null && currentBuildingCompPowTrader != null &&  currentBuildingCompPowTrader.PowerOn == false)
                {
                    allColonistBuildings.Remove(currentBuilding);
                }
            }
        }
		
		public List<Building> GetDeviceList()
        {

            return this.deviceList;
        }

        public int GetDeviceListCount()
        {
            if (deviceList != null)
                return deviceList.Count;

            return 0;
        }
		
        public void PowerOffDevice(Building b)
        {
            var device = this.allColonistBuildings.Find(x => x.thingIDNumber == b.thingIDNumber);
            device.TryGetComp<CompPowerTrader>().ReceiveCompSignal("FlickedOff");
            device.TryGetComp<CompPowerTrader>().PowerOn = false;
        }

		public void AddDevice(Building b)
		{

            deviceList.Add(b);
		}
	
		public void RemoveDevice(Building b)
		{
			
			
		}

		
	}

}