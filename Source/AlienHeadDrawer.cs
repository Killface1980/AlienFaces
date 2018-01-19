using System.Collections.Generic;
using System.Linq;
using AlienRace;
using FacialStuff;
using UnityEngine;
using Verse;

namespace AlienFaces
{
    public class AlienHeadDrawer : HumanHeadDrawer
    {
        // Vanilla offsets
        public static readonly float[] HorHeadOffsets = { 0f, 0.04f, 0.1f, 0.09f, 0.1f, 0.09f };

        public override void BaseHeadOffsetAt(ref Vector3 offset, bool portrait)
        {
            Pawn pawn = this.Pawn;

            // + Alien offsets if any
            if (pawn.def is ThingDef_AlienRace alienProps)
            {
                Vector2 alienOff = alienProps.alienRace.generalSettings.alienPartGenerator.headOffset;
                if (alienOff != Vector2.zero)
                {
                    // Vanilla offsets for Aliens
                    Vector3 o = offset;
                    float num = HorHeadOffsets[(int)pawn.story.bodyType];
                    switch (this.BodyFacing.AsInt)
                    {
                        case 0:
                            o = new Vector3(0f, 0f, 0.34f);
                            break;
                        case 1:
                            o = new Vector3(num, 0f, 0.34f);
                            break;
                        case 2:
                            o = new Vector3(0f, 0f, 0.34f);
                            break;
                        case 3:
                            o = new Vector3(-num, 0f, 0.34f);
                            break;
                        default:
                            Log.Error("BaseHeadOffsetAt error in " + pawn);
                            o = Vector3.zero;
                            break;
                    }
                    o.x += alienOff.x;
                    o.z += alienOff.y;
                    offset = o;
                    return;
                }

            }

            base.BaseHeadOffsetAt(ref offset, portrait);

        }

        public override Mesh GetPawnMesh(bool wantsBody, bool portrait) =>
        this.CompFace.Pawn.GetComp<AlienPartGenerator.AlienComp>() is AlienPartGenerator.AlienComp alienComp ?
        portrait ?
        wantsBody ?
        alienComp.alienPortraitGraphics.bodySet.MeshAt(this.BodyFacing) :
        alienComp.alienPortraitGraphics.headSet.MeshAt(this.HeadFacing) :
        wantsBody ?
        alienComp.alienGraphics.bodySet.MeshAt(this.BodyFacing) :
        alienComp.alienGraphics.headSet.MeshAt(this.HeadFacing) :
        wantsBody ?
        MeshPool.humanlikeBodySet.MeshAt(this.BodyFacing) :
        MeshPool.humanlikeHeadSet.MeshAt(this.HeadFacing);



        public override Mesh GetPawnHairMesh(bool portrait) =>
        this.CompFace.Pawn.GetComp<AlienPartGenerator.AlienComp>() is AlienPartGenerator.AlienComp alienComp ?
        (this.CompFace.Pawn.story.crownType == CrownType.Narrow ?
         (portrait ?
          alienComp.alienPortraitGraphics.hairSetNarrow :
          alienComp.alienGraphics.hairSetNarrow) :
         (portrait ?
          alienComp.alienPortraitGraphics.hairSetAverage :
          alienComp.alienGraphics.hairSetAverage)).MeshAt(this.HeadFacing) :
        Graphics.HairMeshSet.MeshAt(this.HeadFacing);

        public override void DrawAlienBodyAddons(Quaternion quat, Vector3 rootLoc, bool portrait, bool renderBody)
        {
            Pawn pawn = this.CompFace.Pawn;
            if (pawn.def is ThingDef_AlienRace alienProps)
            {

                List<AlienPartGenerator.BodyAddon> addons = alienProps.alienRace.generalSettings.alienPartGenerator.bodyAddons;
                AlienPartGenerator.AlienComp alienComp = pawn.GetComp<AlienPartGenerator.AlienComp>();
                for (int i = 0; i < addons.Count; i++)
                {
                    AlienPartGenerator.BodyAddon ba = addons[i];


                    if (ba.CanDrawAddon(pawn))
                    {

                        Mesh mesh = portrait ? alienComp.alienPortraitGraphics.addonMeshFlipped : alienComp.alienGraphics.addonMesh;

                        Rot4 rotation = pawn.Rotation;
                        if (portrait)
                            rotation = Rot4.South;
                        AlienPartGenerator.RotationOffset offset = rotation == Rot4.South ? ba.offsets.front : rotation == Rot4.North ? ba.offsets.back : ba.offsets.side;
                        //Log.Message("front: " + (offset == ba.offsets.front).ToString() + "\nback: " + (offset == ba.offsets.back).ToString() + "\nside :" + (offset == ba.offsets.side).ToString());
                        Vector2 bodyOffset = offset?.bodyTypes?.FirstOrDefault(to => to.bodyType == pawn.story.bodyType)?.offset ?? Vector2.zero;
                        Vector2 crownOffset = offset?.crownTypes?.FirstOrDefault(to => to.crownType == alienComp.crownType)?.offset ?? Vector2.zero;

                        //front 0.42f, -0.3f, -0.22f
                        //back     0f,  0.3f, -0.55f
                        //side -0.42f, -0.3f, -0.22f   

                        float MoffsetX = 0.42f;
                        float MoffsetZ = -0.22f;
                        float MoffsetY = ba.inFrontOfBody ? 0.3f : -0.3f;
                        float num = ba.angle;

                        if (rotation == Rot4.North)
                        {
                            MoffsetX = 0f;
                            MoffsetY = !ba.inFrontOfBody ? 0.3f : -0.3f;
                            MoffsetZ = -0.55f;
                            num = 0;
                        }

                        MoffsetX += bodyOffset.x + crownOffset.x;
                        MoffsetZ += bodyOffset.y + crownOffset.y;

                        if (rotation == Rot4.East)
                        {
                            MoffsetX = -MoffsetX;
                            num = -num; //Angle
                            mesh = alienComp.alienGraphics.addonMeshFlipped;
                        }

                        Vector3 scaleVector = new Vector3(MoffsetX, MoffsetY, MoffsetZ);
                        scaleVector.x *= 1f + (1f - (portrait ?
                                                        alienComp.customPortraitDrawSize :
                                                        alienComp.customDrawSize)
                                                    .x);
                        scaleVector.z *= 1f + (1f - (portrait ?
                                                        alienComp.customPortraitDrawSize :
                                                        alienComp.customDrawSize)
                                                    .y);

                        GenDraw.DrawMeshNowOrLater(mesh, rootLoc + scaleVector, Quaternion.AngleAxis(num, Vector3.up), alienComp.addonGraphics[i].MatAt(rotation), portrait);
                    }
                }
            }

        }
}
}
