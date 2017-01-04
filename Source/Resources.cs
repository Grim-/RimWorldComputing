using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorldComputing
{
    [StaticConstructorOnStartup]
    public static class Resources
    {

        public static Texture2D turretPanelIcon = ContentFinder<Texture2D>.Get("UI/icons/rmTurret");
        public static Texture2D settingsIcon = ContentFinder<Texture2D>.Get("UI/icons/settings-icon");
        public static Texture2D turretIcon = ContentFinder<Texture2D>.Get("UI/icons/turret-icon");
        public static Texture2D trapIcon = ContentFinder<Texture2D>.Get("UI/icons/trap-icon");
        public static Texture2D thermometerIcon = ContentFinder<Texture2D>.Get("UI/icons/thermometer-icon");
        public static Texture2D lightIcon = ContentFinder<Texture2D>.Get("UI/icons/light-icon");
        public static Texture2D powerIcon = ContentFinder<Texture2D>.Get("UI/icons/power-icon");
    }
}
