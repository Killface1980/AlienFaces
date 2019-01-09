using FacialStuff;
using UnityEngine;
using AlienRace;


namespace AlienFaces
{
    public class AlienHeadDrawer : HumanHeadDrawer
    {
        // Vanilla offsets
        public new static readonly float[] HorHeadOffsets = {0f, 0.04f, 0.1f, 0.09f, 0.1f, 0.09f};

        public override void BaseHeadOffsetAt(ref Vector3 offset, bool portrait)
        {
            base.BaseHeadOffsetAt(ref offset, portrait);
            HarmonyPatches.BaseHeadOffsetAtPostfix(this.Pawn.Drawer.renderer, ref offset );
        }

        protected override Mesh GetPawnMesh(bool wantsBody, bool portrait)
        {
            return HarmonyPatches.GetPawnMesh(portrait, this.Pawn, wantsBody ? this.BodyFacing : this.HeadFacing,
                                              wantsBody);
        }


        public override Mesh GetPawnHairMesh(bool portrait)
        {
            return HarmonyPatches.GetPawnHairMesh(portrait, this.Pawn, this.HeadFacing, this.Graphics);
        }



        public override void DrawAlienHeadAddons(Vector3 headPos, bool portrait, Quaternion headQuat, Vector3 currentLoc)
        {
            base.DrawAlienHeadAddons(headPos, portrait, headQuat, currentLoc);
        }
    }
}