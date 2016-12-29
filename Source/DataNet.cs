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
        private List<Device> deviceList;


        public enum DeviceTypes
        {
            TURRET,
            LIGHT,
            TRAP
        }

        public DataNet(List<Building> colonistBuildings)
        {
            this.allColonistBuildings = colonistBuildings;

            deviceList = new List<Device>();

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
               // Log.Message("Datanet:  Thing: " + currentBuilding.Label + " it has a power trader component and currently has power");
               // Log.Message("Datanet:  Thing: " + currentBuilding.Label + " is Type " + currentBuilding.GetType().ToString());

                if (currentBuilding.GetType() == typeof(Building_TurretGun))
                {
                   // Log.Message("Datanet:  Thing: " + currentBuilding.Label + " is Type " + currentBuilding.GetType().ToString() + " and can be added to the devices list");

                    AddDevice(new Device(currentBuilding.thingIDNumber, currentBuilding, DeviceTypes.TURRET));
                }
                else if (currentBuilding.GetType() == typeof(Building_TrapExplosive))
                {
                    //Log.Message("Datanet:  Thing: " + currentBuilding.Label + " is Type " + currentBuilding.GetType().ToString() + " and can be added to the devices list");

                    AddDevice(new Device(currentBuilding.thingIDNumber, currentBuilding, DeviceTypes.TRAP));
                }
                else if (currentBuilding.Label == "standing lamp")
                {
                   // Log.Message("Datanet:  Thing: " + currentBuilding.Label + " is Type " + currentBuilding.GetType().ToString() + " and can be added to the devices list");

                    AddDevice(new Device(currentBuilding.thingIDNumber, currentBuilding, DeviceTypes.LIGHT));
                }


            }
        }
        public void RebuildListOfDevices(Map map)
        {

            BuildListOfDevices(map.listerBuildings.allBuildingsColonist);

        }


        public List<Device> GetDeviceList()
        {

            return this.deviceList;
        }

        public int GetDeviceListCount()
        {
            if (deviceList != null)
                return deviceList.Count;

            return 0;
        }

        public void ToggleDevicePower(Device d)
        {
            var device = this.deviceList.Find(x => x.thingID == d.thingID);
            device.building.TryGetComp<CompFlickable>().DoFlick();
        }

        public void DetonateExplosive(Device d)
        {
            var device = this.deviceList.Find(x => x.thingID == d.thingID);
            device.building.TryGetComp<CompExplosive>().StartWick(null);
        }

        public void AddDevice(Device d)
        {

            deviceList.Add(d);
        }

        public void RemoveDevice(Device d)
        {


        }


    }

    public class Device {

        public Building building;
        public DataNet.DeviceTypes type;
        public float thingID;


        public Device(float thingID, Building building, DataNet.DeviceTypes type)
        {
            this.thingID = thingID;
            this.building = building;
            this.type = type;
        }
  }

}