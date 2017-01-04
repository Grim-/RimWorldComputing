using System;
using UnityEngine;
using Verse;
using RimWorld;
using System.Collections.Generic;
using static RimWorldComputing.DataNet;

namespace RimWorldComputing
{
    /// <summary>
    /// Width needs to be devisible by 4
    /// </summary>
    class MainTabWindow_DataNew : MainTabWindow
    {
        private float headerTextHeight = 40f;
        private float headerTextWidth = 100f;
        private float perSectionWidth;

        private float deviceIconSize = 45f;
        private float deviceIconMargin;


        private float deviceIconsPerRow;
        private float deviceIconsPerCol;

        private float verticalSectionDividerMargin = -8f;

        private Rect deviceBox1, deviceBox2, deviceBox3, deviceBox4;

        private float numberOfSections = 4f;
        private float sectionOneXStart;
        private float sectionTwoXStart;
        private float sectionThreeXStart;
        private float sectionFourXStart;
        private float sectionOneXEnd;
        private float sectionTwoXEnd;
        private float sectionThreeXEnd;
        private float sectionFourXEnd;


        private float devicesPerGroup;
        private List<DeviceGridItem> gridList1;

        private DeviceGroup group1, group2, group3, group4;
        private DataNet datanet;
        private Map visibleMap;

        public MainTabWindow_DataNew()
        {
            perSectionWidth = (RequestedTabSize.x / numberOfSections);

            sectionOneXStart = 0f;
            sectionOneXEnd = perSectionWidth;


            sectionTwoXStart = perSectionWidth;
            sectionTwoXEnd = perSectionWidth * 2;


            sectionThreeXStart = perSectionWidth * 2f;
            sectionThreeXEnd = perSectionWidth * 3f;

            sectionFourXStart = perSectionWidth * 3f;
            sectionFourXEnd = perSectionWidth * 4f;
            //ROW = Y+ UP ^ , COL = X < >
            deviceIconsPerRow = (RequestedTabSize.y  / deviceIconSize);
            deviceIconsPerCol = (perSectionWidth / deviceIconSize);

            devicesPerGroup = Mathf.Round(deviceIconsPerCol * deviceIconsPerRow - 10.1f);
            //devicesPerGroup = 5f;


            //                     0f               0f   800 / 4 = 200f    200f
            deviceBox1 = new Rect(sectionOneXStart, 0f, perSectionWidth, RequestedTabSize.y);
            //                     200f             0f   200f              200f
            deviceBox2 = new Rect(sectionTwoXStart, 0f, perSectionWidth, RequestedTabSize.y);
            //                     400f             0f   200f              200f
            deviceBox3 = new Rect(sectionThreeXStart, 0f, perSectionWidth, RequestedTabSize.y);
            //                     600f             0f   200f              200f
            deviceBox4 = new Rect(sectionFourXStart, 0f, perSectionWidth, RequestedTabSize.y);

            Log.Message("Grid: Max Devices Per Group:  " + devicesPerGroup.ToString());
            visibleMap = Find.VisibleMap;
            datanet = new DataNet(visibleMap.listerBuildings.allBuildingsColonist);

            //set the information the four display boxes will display
            group1 = datanet.deviceGroups[0];
            group2 = datanet.deviceGroups[1];
            group3 = datanet.deviceGroups[2];
            group4 = datanet.deviceGroups[3];
        }

        public override Vector2 RequestedTabSize
        {
            get
            {
                return new Vector2(1200f, 300f);
            }
        }

        public override void PreOpen()
        {
            base.PreOpen();
        }

        public override void DoWindowContents(Rect mainRect)
        {
            base.DoWindowContents(mainRect);


            datanet.RebuildListOfDevices(visibleMap);



            DoDeviceBox(deviceBox1, group1);

            GUI.color = Color.grey;
            Widgets.DrawLineVertical(deviceBox1.x + deviceBox1.width + verticalSectionDividerMargin, 0f, deviceBox1.height);

            DoDeviceBox(deviceBox2, group2);
            GUI.color = Color.grey;
            Widgets.DrawLineVertical(deviceBox2.x + deviceBox2.width + verticalSectionDividerMargin, 0f, deviceBox2.height);

            DoDeviceBox(deviceBox3, group3);
            GUI.color = Color.grey;
            Widgets.DrawLineVertical(deviceBox3.x + deviceBox3.width + verticalSectionDividerMargin, 0f, deviceBox3.height);

            DoDeviceBox(deviceBox4, group4);
            //GUI.color = Color.grey;
            Widgets.DrawLineVertical(deviceBox4.x + deviceBox4.width + verticalSectionDividerMargin + 20f, 0f, deviceBox4.height);

        }

        private void DoDeviceBox(Rect devicebox, DeviceGroup group)
        {       
            GUI.color = Color.white;
            Text.Anchor = TextAnchor.UpperCenter;
            Text.Font = GameFont.Small;

            var list = group.getDevicesInGroup();


            var groupHeaderRect = new Rect(devicebox.x, 0f, headerTextWidth, headerTextHeight);         
            Widgets.Label(groupHeaderRect, group.groupName);

            //if check for device box type 

            var groupDeviceCountPos = new Rect(groupHeaderRect.x + groupHeaderRect.width + 5f, groupHeaderRect.y, 70f, 30f);

            //var icon1 = new Rect(groupDeviceCountPos.x + groupDeviceCountPos.width, groupDeviceCountPos.y, 20f, 20f);
            //GUI.DrawTexture(icon1, Resources.settingsIcon);

            //var icon2 = new Rect(icon1.x + icon1.width +5f, icon1.y, 20f, 20f);
            //GUI.DrawTexture(icon2, Resources.settingsIcon);

            //var icon3 = new Rect(icon2.x + icon2.width +5f, icon2.y, 20f, 20f);
            //GUI.DrawTexture(icon3, Resources.settingsIcon);

            //var icon4 = new Rect(groupDeviceCountPos.x + groupDeviceCountPos.width + 60f, groupDeviceCountPos.y, 20f, 20f);
           // GUI.DrawTexture(icon4, Resources.settingsIcon);

            //if(Widgets.ButtonImage(icon4,Resources.settingsIcon))
            //{
            //    Find.WindowStack.Add(new Dialog_Config(datanet, group));
            //}


            gridList1 =  CreateGrid(devicebox, (int)deviceIconsPerRow,(int) deviceIconsPerCol, true);

            int deviceCounter = 0;
            //if there any devices to add to the panel
            if (list.Count != 0)
            {
                //for each device in the list
                for (int i = 0; i < list.Count; i++)
                {
                    //Log.Message("Devices: " + i);
                    //if there are more devices than there is devicesPerGroup 
                    if (i >= devicesPerGroup)
                    {
                        //Log.Message("Maxium device limit reached");
                        //Log.Message("Devices: " + i);
                        //we still want to increment the counter so the user can see
                        deviceCounter++;
                        //break the current iteration as we want it to run to the devicecounter increment again then break the current go to next etc
                        continue;
                    }
                    //Add the first device we find to slot 1
                    //second to slot two etc
                    //the gridlist has as exactly many slots as it can fit within its rect 
                    //once there are no more devices to fill up slots the for loop ends
                    gridList1[i].setDevice(list[i]);
                    deviceCounter++;
                }

                    //once we have our gridlist we need to draw it
                    DrawGrid(gridList1);           
            }

            //Device counter if there are more devices than spaces 
            if(deviceCounter >= devicesPerGroup)
            {
                //turn text red
                GUI.contentColor = Color.red;
                Widgets.Label(groupDeviceCountPos,  deviceCounter.ToString() + " / " + devicesPerGroup);
                //set color back to white otherwise everything drawn after this will also be red
                GUI.contentColor = Color.white;
            }
            else
            {
                //other wise normal white
                Widgets.Label(groupDeviceCountPos,  deviceCounter.ToString() + " / " + devicesPerGroup);
                //draw a vertical line at the end of the section (sectionEndX)
            }
          
        }

        //private void DoDeviceBoxGroup(DeviceGroup group)
        //{
        //    GUI.color = Color.white;
        //    Text.Anchor = TextAnchor.UpperCenter;
        //    Text.Font = GameFont.Small;

        //    var list = datanet.GetAllDevicesList();


        //    var groupHeaderRect = new Rect(group.displayRect.x, 0f, headerTextWidth, headerTextHeight);
        //    Widgets.Label(groupHeaderRect, group.groupName);
 
        //    var groupDeviceCountPos = new Rect(groupHeaderRect.x + groupHeaderRect.width + 5f, groupHeaderRect.y, 70f, 30f);

        //    var icon1 = new Rect(groupDeviceCountPos.x + groupDeviceCountPos.width, groupDeviceCountPos.y, 20f, 20f);
        //    GUI.DrawTexture(icon1, Resources.settingsIcon);

        //    var icon2 = new Rect(icon1.x + icon1.width + 5f, icon1.y, 20f, 20f);
        //    GUI.DrawTexture(icon2, Resources.settingsIcon);

        //    var icon3 = new Rect(icon2.x + icon2.width + 5f, icon2.y, 20f, 20f);
        //    GUI.DrawTexture(icon3, Resources.settingsIcon);

        //    var icon4 = new Rect(icon3.x + icon3.width + 5f, icon3.y, 20f, 20f);
        //    GUI.DrawTexture(icon4, Resources.settingsIcon);



        //    gridList1 = CreateGrid(group.displayRect, (int)deviceIconsPerRow, (int)deviceIconsPerCol, true);

        //    int deviceCounter = 0;
        //    //if there any devices to add to the panel
        //    if (list.Count != 0)
        //    {
        //        //for each device in the list
        //        for (int i = 0; i < list.Count; i++)
        //        {
        //            //Log.Message("Devices: " + i);
        //            //if there are more devices than there is devicesPerGroup 
        //            if (i >= devicesPerGroup)
        //            {
        //                //Log.Message("Maxium device limit reached");
        //                //Log.Message("Devices: " + i);
        //                //we still want to increment the counter so the user can see
        //                deviceCounter++;
        //                //break the current iteration as we want it to run to the devicecounter increment again then break the current go to next etc
        //                continue;
        //            }
        //            //Add the first device we find to slot 1
        //            //second to slot two etc
        //            //the gridlist has as exactly many slots as it can fit within its rect 
        //            //once there are no more devices to fill up slots the for loop ends
        //            gridList1[i].setDevice(list[i]);
        //            deviceCounter++;
        //        }

        //        //once we have our gridlist we need to draw it
        //        DrawGrid(gridList1);
        //    }

        //    //Device counter if there are more devices than spaces 
        //    if (deviceCounter >= devicesPerGroup)
        //    {
        //        //turn text red
        //        GUI.contentColor = Color.red;
        //        Widgets.Label(groupDeviceCountPos, deviceCounter.ToString() + " / " + devicesPerGroup);
        //        //set color back to white otherwise everything drawn after this will also be red
        //        GUI.contentColor = Color.white;
        //    }
        //    else
        //    {
        //        //other wise normal white
        //        Widgets.Label(groupDeviceCountPos, deviceCounter.ToString() + " / " + devicesPerGroup);
        //        //draw a vertical line at the end of the section (sectionEndX)
        //    }

        //}


        //private void DoDeviceBox2(Rect devicebox2)
        //{
        //    GUI.color = Color.white;

        //    var pos = new Rect(sectionTwoXStart, 0f, deviceBox2.width - 10f, headerTextHeight);
        //    Widgets.Label(pos, "Device Group 2");
        //    //if check for device box type 


        //    var list = datanet.GetAllTrapsList();

        //    var gridList = CreateGrid(deviceBox2,(int)deviceIconsPerRow, (int)deviceIconsPerCol, true);
        //    //if there any devices to add to the panel
        //    if (list.Count != 0)
        //    {

        //        for (int i = 0; i < list.Count; i++)
        //        {
        //            gridList[i].setDevice(list[i]);
        //        }


        //        DrawGrid(gridList);
        //    }

        //    GUI.color = Color.grey;
        //    Widgets.DrawLineVertical(sectionTwoXEnd - verticalSectionDividerMargin, 0f, devicebox2.height);
        //}

        //private void DoDeviceBox3(Rect devicebox3)
        //{
        //    GUI.color = Color.white;

        //    var pos = new Rect(sectionThreeXStart, 0f, deviceBox3.width - 10f, headerTextHeight);
        //    Widgets.Label(pos, "Device Group 3");

        //    GUI.color = Color.grey;

        //    Widgets.DrawLineVertical(sectionThreeXEnd - verticalSectionDividerMargin, 0f, devicebox3.height);
        //}

        //private void DoDeviceBox4(Rect devicebox4)
        //{
        //    GUI.color = Color.white;

        //    var pos = new Rect(sectionFourXStart, 0f, deviceBox4.width - 10f, headerTextHeight);
        //    Widgets.Label(pos, "Device Group 4");

        //    GUI.color = Color.grey;

        //    Widgets.DrawLineVertical(sectionFourXEnd - verticalSectionDividerMargin, 0f, devicebox4.height);
        //}

        private List<DeviceGridItem> CreateGrid(Rect rect, int rows, int cols, bool growHorizontally)
        {
            var gridList = new List<DeviceGridItem>();

      
            //if we want the grid to grow horizontally then we will need to increment the rows before the cols the Y (height) before X (width), usually its X then Y
            if (growHorizontally)
            {
                //for each row
                for (int y = 0; y < rows; y++)
                {
                    //if statement is in first loop so we dont even start the second one if we're not going to need it
                    if (gridList.Count >= devicesPerGroup)
                        break;

                    //yeah
                    for (int x = 0; x < cols; x++)
                    {
                        //each grid items position is determined by the rect passed in so it knows where to start from
                        //for example anything in grid two must start from atleast 200f(perSection)so as to not push into group 1
                        var position = new Rect(rect.x + x * deviceIconSize , rect.y + y * deviceIconSize + 30f, deviceIconSize, deviceIconSize);

                        //Widgets.DrawBoxSolid(position, Color.gray);
                        //then we add the new griditem which takes only a position in the constructor and a null device reference which we can set a device to later in each of the DoDevice Methods
                        gridList.Add(new DeviceGridItem(position.x, position.y));
                    }
                }
            }

            else
            {
                //do everything above but grow vertically first then horizontally X, Y
                for (int x = 0; x < rows; x++)
                {
                    if (gridList.Count >= devicesPerGroup)
                        break;

                    for (int y = 0; y < cols; y++)
                    {
                        
                        var position = new Rect(x * deviceIconSize + rect.x, y * deviceIconSize + 30f + rect.y, deviceIconSize - 2f, deviceIconSize - 2f);

                        //Widgets.DrawBoxSolid(position, Color.gray);
                        gridList.Add(new DeviceGridItem(position.x, position.y));
                    }
                }
            }
            return gridList;
        }

        private void DrawGrid(List<DeviceGridItem> gridList)
        {

            //go through each devicegriditem in the list
            for (int i = 0; i < gridList.Count; i++)
            {
                //if the 
                if (i > devicesPerGroup)
                    return;

                if (gridList[i].device != null)
                {
                     if (gridList[i].device.building != null)
                    {
                        //Draw the Device
                         var pos = new Rect(gridList[i].xPos, gridList1[i].yPos, deviceIconSize, deviceIconSize);
                        HandleDeviceInput(pos, gridList[i].device);
                        switch (gridList[i].device.type)
                        {
                            case Device.DeviceTypes.TURRET:
                                GUI.DrawTexture(pos, Resources.turretIcon);
                                break;
                            case Device.DeviceTypes.TRAP:
                                GUI.DrawTexture(pos, Resources.trapIcon);
                                break;
                            case Device.DeviceTypes.FLICKABLE:
                                GUI.DrawTexture(pos, Resources.powerIcon);
                                break;
                            case Device.DeviceTypes.TEMPCONTROL:
                                GUI.DrawTexture(pos, Resources.thermometerIcon);
                                break;
                            default:
                                break;
                        }
                        
                       // Widgets.Label(pos, gridList[i].device.type.ToString());

                    }
                }
            }
        }

        private void HandleDeviceInput(Rect rect, Device d)
        {

            if (Mouse.IsOver(rect))
            {
               GenDraw.DrawArrowPointingAt( d.building.Position.ToVector3() );
            }

            if (Widgets.ButtonInvisible(rect))
            {

                if(Event.current.button == 0)
                {
                    //default left click action for each device type
                    switch (d.type)
                    {
                        case Device.DeviceTypes.TURRET:
                            datanet.ToggleDevicePower(d);
                            break;
                        case Device.DeviceTypes.FLICKABLE:
                            datanet.ToggleDevicePower(d);
                            break;
                        case Device.DeviceTypes.TRAP:
                            datanet.DetonateExplosive(d);

                            break;
                        case Device.DeviceTypes.TEMPCONTROL:
                            datanet.ToggleDevicePower(d);
                            break;
                    }
                }
                else if(Event.current.button == 1)
                {
                    //a list of things device types can do on right click
                    var list = new List<FloatMenuOption>();
                    switch (d.type)
                    {
                        case Device.DeviceTypes.TURRET:
                            break;
                        case Device.DeviceTypes.TEMPCONTROL:
                            list.Add(new FloatMenuOption("Increase by 5", delegate { datanet.TempControlChange(d, 5f); }));
                            list.Add(new FloatMenuOption("Increase by 10", delegate { datanet.TempControlChange(d, 10f); }));
                            list.Add(new FloatMenuOption("Decrease by 5", delegate { datanet.TempControlChange(d, -5f); }));
                            list.Add(new FloatMenuOption("Decrease by 10", delegate { datanet.TempControlChange(d, -10f); }));
                            break;
                        case Device.DeviceTypes.TRAP:
                            list.Add(new FloatMenuOption("Detonate", delegate { datanet.DetonateExplosive(d); }));

                            break;
                        default:
                            break;
                    }


                    list.Add(new FloatMenuOption("Find Device", delegate {
                        Find.CameraDriver.JumpTo(d.building.Position);
                        GenDraw.DrawArrowPointingAt(new Vector3(d.building.Position.x, d.building.Position.y, d.building.Position.z));
                    } ));
                           
                    Find.WindowStack.Add(new FloatMenu(list));
                }
                
            }         
        }
   
        private void DebugRectOnClick(Rect rect, float x = 0f, float y =0f)
        {
            if (Widgets.ButtonInvisible(rect))
            {
                Log.Message("Debug : [Rect] #" + x + " " + y + ": X = " + rect.x + " Y = " + rect.y + " W = " + rect.width + " H = " + rect.height);
            }
        }

    }
}
