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
            deviceList.Clear();
            Log.Message("Datanet: Building list of devices");
            //Log.Message("Datanet: Pre Loop - Number Of Devices(colonist buildings) # " + allColonistBuildings.Count);
            for (int i = 0; i < allColonistBuildings.Count; i++)
			{
				var currentBuilding = allColonistBuildings[i];
                var currentBuildingCompPowTrader = currentBuilding.TryGetComp<CompPowerTrader>();


                    //this is a colonist building, it has a power trader component and currently has power
                    Log.Message("Datanet:  Thing: " + currentBuilding.Label + " it has a power trader component and currently has power");
                    Log.Message("Datanet:  Thing: " + currentBuilding.Label + " is Type " + currentBuilding.GetType().ToString() );

                if (currentBuilding.GetType() == typeof(Building_TurretGun) )
				{
                        Log.Message("Datanet:  Thing: " + currentBuilding.Label + " is Type " + currentBuilding.GetType().ToString() + " and can be added to the devices list");

                            AddDevice(currentBuilding as Building_TurretGun);
				}
                else if (currentBuilding.GetType() == typeof(Building_TrapExplosive))
                {
                    Log.Message("Datanet:  Thing: " + currentBuilding.Label + " is Type " + currentBuilding.GetType().ToString() + " and can be added to the devices list");

                    AddDevice(currentBuilding as Building_TrapExplosive);
                }
                else if (currentBuilding.Label == "standing lamp")
                {
                    Log.Message("Datanet:  Thing: " + currentBuilding.Label + " is Type " + currentBuilding.GetType().ToString() + " and can be added to the devices list");

                    AddDevice(allColonistBuildings[i]);
                }


            }
		}
        public void RebuildListOfDevices(Map map)
        {
            
            BuildListOfDevices(map.listerBuildings.allBuildingsColonist);

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
		
        public void ToggleDevicePower(Building b)
        {
            var device = this.deviceList.Find(x => x.thingIDNumber == b.thingIDNumber);
            device.TryGetComp<CompFlickable>().DoFlick();
        }

        public void DetonateExplosive(Building b)
        {
            var device = this.deviceList.Find(x => x.thingIDNumber == b.thingIDNumber);
            device.TryGetComp<CompExplosive>().StartWick(null);
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