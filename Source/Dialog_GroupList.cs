using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorldComputing
{
    class Dialog_GroupList : Window
    {
        private DeviceGroup currentGroup;



        private float deviceLabelWidth = 60f;
        private float deviceLabelHeight = 20f;
        private float deviceRowHeight = 50f;

        private Vector2 scrollPosition = default(Vector2);

        public Dialog_GroupList(DeviceGroup group)
        {
            this.currentGroup = group;
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
                320f,
                ((float)UI.screenHeight - this.InitialSize.y) / 2f,
                this.InitialSize.x, this.InitialSize.y).Rounded();
        }

        public override void DoWindowContents(Rect inRect)
        {

            var content = inRect;

            this.doCloseX = true;
            this.draggable = true;

            Text.Anchor = TextAnchor.UpperCenter;






            var groupDevices = currentGroup.getDevicesInGroup();
            var mainRect = inRect.AtZero();

            GUI.BeginGroup(mainRect);

            float listHeight = groupDevices.Count * deviceRowHeight;
            var viewRect = new Rect(0f, 0f, inRect.width - 16f, listHeight);
            var outRect = new Rect(inRect.AtZero());

            Widgets.BeginScrollView(outRect, ref this.scrollPosition, viewRect);

            var currentY = 0f;

            Text.Font = GameFont.Medium;
            var groupNameLabel = new Rect(0f, 0f, content.width, 30f);
            Widgets.Label(groupNameLabel, currentGroup.groupName + " devices");
            Widgets.DrawLineHorizontal(0f, 25f, content.width);
            Text.Font = GameFont.Small;

            if (groupDevices.Count != 0)
            {

                for (var i = 0; i < groupDevices.Count(); i++)
                {

                    Text.Anchor = TextAnchor.MiddleCenter;
                    //a row rect will hold the x,y position of the row, then each element in the row positions itself along the Y axis
                    var rowRect = new Rect(groupNameLabel.x, groupNameLabel.y + i * deviceRowHeight + 20f, InitialSize.x, deviceRowHeight);

                    var deviceNameLabel = new Rect(rowRect.x, rowRect.y, deviceLabelWidth, rowRect.height);

                    if (Widgets.ButtonInvisible(deviceNameLabel))
                    {
                        Find.CameraDriver.JumpTo(groupDevices[i].building.Position);
                    }
                    // Widgets.DrawLineHorizontal(groupNameLabel.x - 2f, groupNameLabel.height + (i * deviceLabelHeight) + deviceLabelHeight, InitialSize.x);
                    Widgets.Label(deviceNameLabel, groupDevices[i].type.ToString());

                    currentY++;
                }
            }
            GUI.EndGroup();
            Widgets.EndScrollView();
        }

    }
}
