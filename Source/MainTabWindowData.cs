using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using RimWorld;

namespace Computing
{
	public class MainTabWindowData : MainTabWindow
	{
		//private DataNet dn = new DataNet();
		
		private static readonly Texture2D turretIcon = Resources.Load<Texture2D>("Textures/UI/icons/turret");
		
		
		public override void DoWindowContents(Rect rect)
		{
			base.DoWindowContents(rect);


            this.drawDeviceList(rect);
		}

		public void drawDeviceList(Rect rect)
		{
			
		}


	}
	
}