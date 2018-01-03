﻿using System;

namespace AlienFaces
{
    using System.Reflection;

    using FacialStuff;

    using Harmony;

    using Verse;

    using FacialStuff.Harmony;

    using global::AlienRace;

    [StaticConstructorOnStartup]
    public static class HarmonyPatchesAlien
    {
        private static bool modCheck;
        private static bool loadedAliens;

        static HarmonyPatchesAlien()
        {
            HarmonyInstance harmony = HarmonyInstance.Create("rimworld.alienface.patches");
            harmony.PatchAll(Assembly.GetExecutingAssembly());

            if (!modCheck)
            {
                loadedAliens = false;
                foreach (ModContentPack ResolvedMod in LoadedModManager.RunningMods)
                {
                    if (loadedAliens) break; //Save some loading
                    if (ResolvedMod.Name.Contains("Humanoid Alien Races"))
                    {
                        Log.Message("AF :: Aliens Detected.");
                        loadedAliens = true;
                    }
                }
                modCheck = true;
            }

            if (loadedAliens)
            {
                try
                {
                    harmony.Patch(
                    AccessTools.Method(typeof(HarmonyPatchesFS), nameof(HarmonyPatchesFS.OpenStylingWindow)),
                    new HarmonyMethod(typeof(HarmonyPatchesAlien), nameof(OpenFSDialog_Prefix)),
                    null);

                }
                catch (Exception e)
                {
                }
            }
        }

        public static bool OpenFSDialog_Prefix(Pawn pawn)
        {
            if (pawn.def is ThingDef_AlienRace alienProp)
            {
                pawn.GetCompFace(out CompFace face);
                Find.WindowStack.Add(new Dialog_AlienFaceStyling(face, alienProp));
                return false;
            }
            return true;
        }
    }
}
