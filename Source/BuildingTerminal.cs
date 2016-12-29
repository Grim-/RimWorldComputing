﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using RimWorld;

namespace RimWorldComputing
{
    [StaticConstructorOnStartup]
    public class Building_Terminal : Building
	{
		// 60Ticks = 1s // 20000Ticks = 1 Day
		// Destroyed flag. Most of the time not really needed, but sometimes...
		
		private bool destroyedFlag = false;
		public DataNet dataNet;
        private Map gameMap;
		protected CompPowerTransmitter powerComp;


		
		/// <summary>
		/// Do something after the object is spawned
		/// </summary>
		public override void SpawnSetup(Map map)
		{
			// Do the work of the base class (Building)
			base.SpawnSetup(map);

            
			//Log.Message("Spawn Setup of " + this.LabelShort);
            gameMap = map;
			SetComponentReferences();
            dataNet = new DataNet(gameMap.listerBuildings.allBuildingsColonist);
        }


        /// <summary>
        /// Find the PowerCompTransmitter
        /// </summary>
        private void SetComponentReferences()
		{
			this.powerComp = base.GetComp<CompPowerTransmitter>();
	
		}

		// ===================== Destroy =====================

		/// <summary>
		/// Clean up when this is destroyed
		/// </summary>
		public override void Destroy(DestroyMode mode = DestroyMode.Vanish)
		{
			// block further ticker work
			destroyedFlag = true;

			base.Destroy(mode);
		}

		// ===================== Ticker =====================

		/// <summary>
		/// This is used, when the Ticker in the XML is set to 'Rare'
		/// This is a tick thats done once every 250 normal Ticks
		/// </summary>
		public override void TickRare()
		{
			if (destroyedFlag) // Do nothing further, when destroyed (just a safety)
				return;
			// Don't forget the base work
			base.TickRare();
			// Call work function
			DoTickerWork(250);
		}



        //public override IEnumerable<FloatMenuOption> GetFloatMenuOptions(Pawn myPawn)
        //{
        //    List<FloatMenuOption> menuOptions = new List<FloatMenuOption>();


        //    Create a float menu option for each device
        //    var devices = dataNet.GetDeviceList();

        //    foreach (var device in devices)
        //    {

        //        if (device.type == DataNet.DeviceTypes.TURRET)
        //        {
        //            FloatMenuOption item = new FloatMenuOption("Terminal: TURRET.TOGGLE_POWER_BY_ID(" + devices.IndexOf(device).ToString() + ") ", delegate
        //             {
        //                 dataNet.ToggleDevicePower(device);
        //             });
        //            menuOptions.Add(item);
        //        }
        //        else if (device.type == DataNet.DeviceTypes.TRAP)
        //        {
        //            FloatMenuOption item = new FloatMenuOption("Terminal: EXPLOSIVE.REMOTE_DETONATE_BY_ID(" + devices.IndexOf(device).ToString() + ") ", delegate
        //            {
        //                dataNet.DetonateExplosive(device);
        //            });
        //            menuOptions.Add(item);
        //        }
        //        else if (device.type == DataNet.DeviceTypes.LIGHT)
        //        {
        //            FloatMenuOption item = new FloatMenuOption("Terminal: LAMP.TOGGLE_POWER_BY_ID(" + devices.IndexOf(device).ToString() + ") ", delegate
        //            {
        //                dataNet.ToggleDevicePower(device);
        //            });
        //            menuOptions.Add(item);
        //        }


        //    }
        //    return menuOptions;
        //}

        /// <summary>
        /// This is used, when the Ticker in the XML is set to 'Normal'
        /// This Tick is done often (60 times per second)
        /// </summary>
        public override void Tick()
		{
			if (destroyedFlag) // Do nothing further, when destroyed (just a safety)
				return;
			base.Tick();
			// Call work function
			DoTickerWork(1);
		}
		
		
		
		/// <summary>
		/// This string will be shown when the object is selected (focus)
		/// </summary>
		/// <returns></returns>
		public override string GetInspectString()
		{
			StringBuilder stringBuilder = new StringBuilder();

			// Add the inspections string from the base
			stringBuilder.Append(base.GetInspectString());
			stringBuilder.AppendLine();
			// return the complete string
			return stringBuilder.ToString();
		}

		/// ===================== Main Work Function =====================
		/// <summary>
		/// This will be called from one of the Ticker-Functions.
		/// </summary>
		/// <param name="tickerAmount"></param>
		private void DoTickerWork(int tickerAmount)
		{
            Log.Message("Doing ticker work, rebuilding device list");
            dataNet.RebuildListOfDevices(gameMap);
		}
	}
}