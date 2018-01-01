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
        static HarmonyPatchesAlien()
        {
            HarmonyInstance harmony = HarmonyInstance.Create("rimworld.alienface.patches");
            harmony.PatchAll(Assembly.GetExecutingAssembly());

            harmony.Patch(
                AccessTools.Method(typeof(HarmonyPatchesFS), nameof(HarmonyPatchesFS.OpenStylingWindow)),
                new HarmonyMethod(typeof(HarmonyPatchesAlien), nameof(OpenFSDialog_Prefix)),
                null);
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
