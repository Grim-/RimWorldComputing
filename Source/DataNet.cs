using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using RimWorld;

namespace RimWorldComputing
{
	public class DataNet 
	{
		
		private static DataNet instance;
		private List<Building> allColonistBuildings;
        private List<Building> devices;
		
		public DataNet()
		{
			BuildListOfDevices();
		}
		
		public static DataNet Instance
		{
			get
			{
				if(instance == null)
				{
					instance = new DataNet();
				}
				return instance;
			}
		}
		
	    /// <summary>
		/// Find all the devices we are going to be controlling and add them to lists of type
		/// </summary>
		private void BuildListOfDevices()
		{

			allColonistBuildings = Find.AnyPlayerHomeMap.listerBuildings.allBuildingsColonist;

            Log.Message("Datanet: Building list of devices");
            Log.Message("Datanet: Pre Loop - Number Of Devices(buildings) #" + allColonistBuildings.Count);
            for (int i = 0; i < allColonistBuildings.Count; i++)
			{
				var currentBuilding = allColonistBuildings[i];
                var currentBuildingCompPowTrader = currentBuilding.TryGetComp<CompPowerTrader>();


                if (currentBuildingCompPowTrader != null && currentBuildingCompPowTrader.PowerOn == true)
				{
                    //this is a colonist building, it has a power trader component and currently has power
                    Log.Message("Datanet: this is a colonist building, it has a power trader component and currently has power");
                    Log.Message("Datanet:  " + currentBuilding.GetType().ToString() + " " + typeof(Building_Turret).ToString() );


                        

                    if (currentBuilding.GetType() == typeof(Building_TurretGun) )
					{
						Log.Message("Datanet: This is a turret, can be added to list");
                        AddDevice(currentBuilding);
					}
					
				}
				
				
				
			}
		}
		
		public List<Building> GetDeviceList()
        {

            return devices;
        }
		
		
		public void AddDevice(Building b)
		{

            devices.Add(b);
		}
	
		public void RemoveDevice(Building b)
		{
			
			
		}

		
	}

}