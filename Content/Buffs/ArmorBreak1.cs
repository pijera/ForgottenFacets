using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ForgottenFacets.Content.Buffs
{
    internal class ArmorBreak1 : ModBuff
    {
        public override string Texture => "ForgottenFacets/Assets/Buffs/FrostbittenDebuff";

        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true;
            Main.buffNoSave[Type] = true;
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            npc.GetGlobalNPC<ArmorBroken1NPC>().armorBroken = true;
        }
    }

    public class ArmorBroken1NPC : GlobalNPC
    {
        public bool armorBroken;
        private bool defenseReduced;

        public override bool InstancePerEntity => true;

        public override void ResetEffects(NPC npc)
        {
            armorBroken = false;

            if (defenseReduced)
            {
                npc.defense += 3;
                defenseReduced = false;
            }
        }

        public override void PostAI(NPC npc)
        {
            if (armorBroken && !defenseReduced)
            {
                npc.defense -= 3;
                defenseReduced = true;
            }
        }

        public override void DrawEffects(NPC npc, ref Color drawColor)
        {
            if (armorBroken && Main.rand.NextBool(2))
            {
                Dust dust = Dust.NewDustDirect(npc.Center - new Vector2(10, 20),npc.width,npc.height,DustID.Stone);

                dust.noGravity = true;
            }
        }
    }
}