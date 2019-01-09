using System.Diagnostics.CodeAnalysis;
using AlienRace;
using FacialStuff;

using UnityEngine;
using Verse;

namespace AlienFaces
{
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public class AlienBipedDrawer : HumanBipedDrawer
    {
    public override void DrawAlienBodyAddons(bool portrait, Vector3 rootLoc, Quaternion quat, bool renderBody, Rot4 rotation)
    {
        HarmonyPatches.DrawAddons(portrait, this.Pawn, rootLoc, quat, rotation);
    }
    }
}
