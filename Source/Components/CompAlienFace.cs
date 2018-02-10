using AlienRace;
using FacialStuff;
using Verse;

namespace AlienFaces
{
    public class CompAlienFace : CompFace
    {

        public CompProperties_Face Props
        {
            get { return (CompPropertiesAlienFace)this.props; }
        }

        public override CrownType PawnCrownType
        {
            get
            {
                if (this.Pawn.GetComp<AlienPartGenerator.AlienComp>().crownType.Equals(CrownType.Narrow.ToString()))
                {
                    this.Pawn.story.crownType = CrownType.Narrow;
                    return CrownType.Narrow;
                }
                if (this.Pawn.GetComp<AlienPartGenerator.AlienComp>().crownType.Equals(CrownType.Average.ToString()))
                {
                    this.Pawn.story.crownType = CrownType.Average;
                    return CrownType.Average;
                }
                return base.PawnCrownType;
            }
        }
    }
}
