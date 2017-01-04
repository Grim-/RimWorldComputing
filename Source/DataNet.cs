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
        public List<DeviceGroup> deviceGroups;
        public enum GroupMode
        {
            STANDARD,
            CUSTOM
        }
        private List<Building> allColonistBuildings;
        private List<Device> allDevicesList;

        public DataNet(List<Building> colonistBuildings)
        {
            this.allColonistBuildings = colonistBuildings;

            allDevicesList = new List<Device>();
            //allTurretsList = new List<Building_TurretGun>();
            //allLightsList = new List<Building>();
            //allExplosiveTrapsList = new List<Building_TrapExplosive>();

            deviceGroups = new List<DeviceGroup>();

            //set up some default groups
            deviceGroups.Add(new DeviceGroup("Turrets"));
            deviceGroups.Add(new DeviceGroup("Traps"));
            deviceGroups.Add(new DeviceGroup("Flickables"));
            deviceGroups.Add(new DeviceGroup("Temperature Controls"));

            BuildListOfDevices(this.allColonistBuildings);
        }


        /// <summary>
        /// Find all the devices we are going to be controlling and add them to the device list
        /// </summary>
        private void BuildListOfDevices(List<Building> allColonistBuildings)
        {
            allDevicesList.Clear();

            foreach (var group in deviceGroups)
            {
                group.getDevicesInGroup().Clear();
            }


            Log.Message("Datanet: Building list of devices");
            for (int i = 0; i < allColonistBuildings.Count; i++)
            {
                var currentBuilding = allColonistBuildings[i];
                var currentBuildingCompPowTrader = currentBuilding.TryGetComp<CompPowerTrader>();


                if (currentBuilding != null)
                {
                    if (currentBuilding.GetType() == typeof(Building_TurretGun))
                    {
                        var d = new Device(currentBuilding.thingIDNumber, currentBuilding, Device.DeviceTypes.TURRET);
                        AddDevice(d);
                        deviceGroups[0].AddDeviceToGroup(d);
                    }
                    else if (currentBuilding.GetType() == typeof(Building_TrapExplosive))
                    {
                        var d = new Device(currentBuilding.thingIDNumber, currentBuilding, Device.DeviceTypes.TRAP);
                        AddDevice(d);
                        deviceGroups[1].AddDeviceToGroup(d);
                    }
                    else if (currentBuilding.Label == "standing lamp"  || currentBuilding.GetType() == typeof(Building_PowerSwitch) || currentBuilding.GetType() == typeof(Building_PlantGrower))
                    {
                        var d = new Device(currentBuilding.thingIDNumber, currentBuilding, Device.DeviceTypes.FLICKABLE);
                        AddDevice(d);
                        deviceGroups[2].AddDeviceToGroup(d);
                    }
                    else if(currentBuilding.GetType() == typeof(Building_Heater) || currentBuilding.GetType() == typeof(Building_Cooler))
                    {
                        var d = new Device(currentBuilding.thingIDNumber, currentBuilding, Device.DeviceTypes.TEMPCONTROL);
                        AddDevice(d);
                        deviceGroups[3].AddDeviceToGroup(d);
                    }


                }

            }
        }
        public void RebuildListOfDevices(Map map)
        {
            BuildListOfDevices(map.listerBuildings.allBuildingsColonist);


        }


        public List<Device> GetAllDevicesList()
        {

            return this.allDevicesList;
        }


        public List<Device> GetAllofTypeList(Device.DeviceTypes type)
        {

            return this.allDevicesList.FindAll(x => x.type == type);
        }

        public List<DeviceGroup> GetAllGroups()
        {
            return this.deviceGroups;
        }

        public int GetAllDeviceListCount()
        {
            if (allDevicesList != null)
                return allDevicesList.Count;

            return 0;
        }

        public void ToggleDevicePower(Device d)
        {
            var device = this.allDevicesList.Find(x => x.thingID == d.thingID);
            device.building.TryGetComp<CompFlickable>().DoFlick();
        }

        public void DetonateExplosive(Device d)
        {
            var device = this.allDevicesList.Find(x => x.thingID == d.thingID);
            device.building.TryGetComp<CompExplosive>().StartWick(null);
            allDevicesList.Remove(device);
            //call rebuild list because detonating the explosive will remove it.........duh
           // RebuildListOfDevices(Find.VisibleMap);
        }

        public void AddDevice(Device d)
        {
           allDevicesList.Add(d);        
        }
        public void TempControlChange(Device d, float increaseby)
        {
            var deviceToIncrease = allDevicesList.Find(x => x.thingID == d.thingID);
            deviceToIncrease.building.TryGetComp<CompTempControl>().targetTemperature += increaseby;
        }
        public void RemoveDevice(Device d)
        {
           var deviceToRemove = allDevicesList.Find(x => x.thingID == d.thingID);
            allDevicesList.Remove(deviceToRemove);
        }


    }

    public class Device {


        public enum DeviceTypes
        {
            TURRET,
            TRAP,
            FLICKABLE, 
            TEMPCONTROL
        }

        public float thingID;
        public Building building;

        public DeviceGroup Group = null;
        public DeviceTypes type;


        static int idCount = 0;
        private int _ID;

        public bool isInGroup
        {
            get
            {
                return Group == null ? this.isInGroup = false : this.isInGroup = true; 
            }

            set
            {
                isInGroup = value;
            }
        }

        public int ID
        {
            get { return _ID; }
        }


        public Device(float thingID, Building building, DeviceTypes type)
        {
            idCount++;
            _ID = idCount;

            this.thingID = thingID;
            this.building = building;
            this.type = type;
        }
  }

    public class DeviceGroup
    {
        private List<Device> devicesInGroup;
        public string groupName;
        //TODO: Add Device grouping so devices can be assigned to a group and interacted at once
        public DeviceGroup(string name)
        {
            devicesInGroup = new List<Device>();
            this.groupName = name;
        }

        public void AddDeviceToGroup(Device d)
        {
            devicesInGroup.Add(d);
        }

        public List<Device> getDevicesInGroup()
        {
            return devicesInGroup;
        }

    }

    public class DeviceGrid
    {
        public DeviceGrid(int rows, int cols)
            {

            }


    }

    public class DeviceGridItem
    {
        public float xPos;
        public float yPos;
        public Device device;

        public DeviceGridItem(float x, float y)
        {
            this.xPos = x;
            this.yPos = y;
        
        }

        public void setDevice(Device d)
        {
            this.device = d;
        }
    }
}