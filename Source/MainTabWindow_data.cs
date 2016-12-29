using System;
using UnityEngine;
using Verse;
using RimWorld;
using System.Collections.Generic;
using static RimWorldComputing.DataNet;

namespace RimWorldComputing
{
    public class MainTabWindow_Data : MainTabWindow_PawnList
    {

        public static bool isDirty = false;

        private List<Device> deviceList;
        private DataNet datanet;
        private Map visibleMap;

        private static float headerTextHeight = 30f;
        //private static float deviceRowHeight = 80f;
        private static float deviceTextRowHeight = 30f;

        public MainTabWindow_Data()
        {
            visibleMap = Find.VisibleMap;
            datanet = new DataNet(visibleMap.listerBuildings.allBuildingsColonist);
        }

        public override Vector2 RequestedTabSize
        {
            get
            {
                return new Vector2(800f, 200f);
            }
        }

        public override void PreOpen()
        {
            base.PreOpen();
        }

        public override void DoWindowContents(Rect mainRect)
        {
            base.DoWindowContents(mainRect);
            if (isDirty)
                BuildDeviceList();

        

            var headerTextPosition = new Rect(0f, 0f, mainRect.width, headerTextHeight);
           // GUI.BeginGroup(headerTextPosition);
            drawHeader(mainRect, "Turrets ", true, 20f);

            var turretRowsPosition = new Rect(0f, 30f, mainRect.width, deviceTextRowHeight);
            drawTurretDevicesRow(turretRowsPosition, datanet.GetDeviceList());


           // GUI.EndGroup();
        }

        private void BuildDeviceList()
        {
            //var t =  Find.VisibleMap.listerBuildings.allBuildingsColonist.Find(x => x.GetType() == typeof(Building_Terminal)) as Building_Terminal;
            // datanet = t.dataNet;
            //deviceList =  t.dataNet.GetDeviceList();
            // Log.Message(deviceList.Count.ToString());

            datanet.GetDeviceList().Clear();

            datanet.RebuildListOfDevices(visibleMap);
        }

        private void drawHeader(Rect rect, String headerString, bool underline, float lineMargin = 10f )
        {
            var posititon = new Rect(rect.x, rect.y, rect.width, rect.height);
            
            Widgets.Label(posititon, headerString);

            if (underline)
            {
                GUI.color = new Color(1f, 1f, 1f, 0.2f);
                Verse.Widgets.DrawLineHorizontal(rect.x, rect.y + lineMargin, rect.width);
            }
        }

        private void drawTurretDevicesRow(Rect rect, List<Device> devices)
        {
            float num = 0f;
            int turretIndex = 1;
            for(var i =0; i < devices.Count;i++)
            {
                if (devices[i].type == DeviceTypes.TURRET)
                {
                    //                  0  across   40f down + 30f      mainrect width devicetextheight
                    var rowPosition = new Rect(rect.x, rect.y + num, rect.width, rect.height);
                    Log.Message("Drawing Device Row TURRET " + devices[i].building.Label + "at " + rowPosition);
                    GUI.color = new Color(1f, 1f, 1f, 0.9f);
                    Widgets.Label(rowPosition, "Turret " + turretIndex);

                    drawDevicetCols(rowPosition, devices[i]);

                        num += deviceTextRowHeight;

                    turretIndex++;
                }
 
            }
        
        }

        private void drawDevicetCols(Rect deviceRowRect, Device d)
        {

            var position = new Rect(deviceRowRect.x + 80f, deviceRowRect.y + 1f, 20f, 20f);
            Widgets.DrawBoxSolid(position, Color.grey);
            if (Widgets.ButtonInvisible(position))
                datanet.ToggleDevicePower(d);
        }

        protected override void BuildPawnList()
        {
            BuildDeviceList();
        }

        protected override void DrawPawnRow(Rect rect, Pawn p)
        {
            throw new NotImplementedException();
        }
    }
}