using System;
using UnityEngine;
using Verse;

namespace Computing
{
	public abstract class MainTabWindow : Window
	{


		 public MainTabWindow()
		{
			this.layer = WindowLayer.GameUI;
			this.soundAppear = null;
			this.soundClose = null;
			this.doCloseButton = false;
			this.doCloseX = false;
			this.closeOnEscapeKey = true;
			this.preventCameraMotion = false;
		}


		public virtual Vector2 RequestedTabSize
		{
			get
			{
				return new Vector2(1010f, 684f);
			}
		}

		public override Vector2 InitialSize
		{
			get
			{
				Vector2 requestedTabSize = this.RequestedTabSize;
				if (requestedTabSize.y > (float)(Screen.height - 35))
				{
					requestedTabSize.y = (float)(Screen.height - 35);
				}
				if (requestedTabSize.x > (float)Screen.width)
				{
					requestedTabSize.x = (float)Screen.width;
				}
				return requestedTabSize;
			}
		}

		public virtual float TabButtonBarPercent
		{
			get
			{
				return 0f;
			}
		}

		public override void DoWindowContents(Rect inRect)
		{
			this.SetInitialSizeAndPosition();
		}

		protected override void SetInitialSizeAndPosition()
		{
			base.SetInitialSizeAndPosition();
			
		}
	}
}
