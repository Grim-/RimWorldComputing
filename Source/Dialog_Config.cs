using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorldComputing
{
    class Dialog_Config : Window
    {
        private DeviceGroup currentGroup;

        private DataNet datanet;

        private float deviceLabelWidth = 60f;
        private float deviceLabelHeight = 20f;
        private float deviceRowHeight = 50f;

        private Vector2 scrollPosition = default(Vector2);

        public Dialog_Config(DataNet datanet , DeviceGroup group)
        {
            this.currentGroup = group;
            this.datanet = datanet;

            Find.WindowStack.Add(new Dialog_GroupList(group));
        }


        public override Vector2 InitialSize
        {
            get
            {
               
                return new Vector2(300f, 550f);
            }
        }

        protected override void SetInitialSizeAndPosition()
        {
            this.windowRect = new Rect(
                0f,
                ((float)UI.screenHeight - this.InitialSize.y) / 2f,
                this.InitialSize.x, this.InitialSize.y).Rounded();
        }

        public override void DoWindowContents(Rect inRect)
        {

            var content = inRect;

            this.doCloseX = true;
            this.draggable = true;

            Text.Anchor = TextAnchor.UpperCenter;
            var allDevicesList = datanet.GetAllDevicesList();
            var mainRect = inRect.AtZero();

            GUI.BeginGroup(mainRect);

            float listHeight = allDevicesList.Count * deviceRowHeight;
            var viewRect = new Rect(0f, 0f, inRect.width - 16f, listHeight);
            var outRect = new Rect(inRect.AtZero());

            Widgets.BeginScrollView(outRect, ref this.scrollPosition, viewRect);

            var currentY = 0f;

            Text.Font = GameFont.Medium;
            var groupNameLabel = new Rect(0f, 0f, content.width, 30f);
            Widgets.Label(groupNameLabel, "All Devices" );
            Widgets.DrawLineHorizontal(0f, 25f, content.width);
            Text.Font = GameFont.Small;

            if (allDevicesList.Count != 0)
            {

                for (var i = 0; i < allDevicesList.Count(); i++)
                {

                    Text.Anchor = TextAnchor.MiddleCenter;
                    //a row rect will hold the x,y position of the row, then each element in the row positions itself along the Y axis
                    var rowRect = new Rect(groupNameLabel.x, groupNameLabel.y + i * deviceRowHeight + 20f, InitialSize.x, deviceRowHeight);

                    var deviceNameLabel = new Rect(rowRect.x, rowRect.y, deviceLabelWidth, rowRect.height);

                    if(Widgets.ButtonInvisible(deviceNameLabel))
                    {
                        Find.CameraDriver.JumpTo(allDevicesList[i].building.Position);
                    }
                    var addDeviceToGroupButton = new Rect(rowRect.x + deviceNameLabel.width + 5f, rowRect.y + deviceLabelHeight - 2f, 105f, 30f);

                    if (Widgets.ButtonText(addDeviceToGroupButton, "Add to group"))
                    {
                        datanet.GetAllGroups().Find(x => x.groupName == currentGroup.groupName).AddDeviceToGroup(allDevicesList[i]);
                        
                    }
                    Widgets.Label(deviceNameLabel, allDevicesList[i].type.ToString() );

                    currentY++;
                }
            }
            GUI.EndGroup();
            Widgets.EndScrollView();
        }

        private void DebugRectOnClick(Rect rect, float x = 0f, float y = 0f)
        {
            if (Widgets.ButtonInvisible(rect))
            {
                Log.Message("Debug : [Rect] #" + x + " " + y + ": X = " + rect.x + " Y = " + rect.y + " W = " + rect.width + " H = " + rect.height);
            }
        }

        private void DebugRect(Rect rect, float x = 0f, float y = 0f)
        {
            Log.Message("Debug : [Rect] #" + x + " " + y + ": X = " + rect.x + " Y = " + rect.y + " W = " + rect.width + " H = " + rect.height);
        }
    }
}
