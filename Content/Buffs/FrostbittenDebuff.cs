using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace ForgottenFacets.Content.Buffs
{
    internal class FrostbittenDebuff : ModBuff
    {
        public override string Texture => "ForgottenFacets/Assets/Buffs/FrostbittenDebuff";
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true;
            Main.buffNoSave[Type] = true;
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            npc.GetGlobalNPC<FrostbittenGlobalNpc>().frozen = true;
        }
    }

    public class FrostbittenGlobalNpc : GlobalNPC
    {
        public bool frozen;
        private bool defenseReducedBoss;
        private bool defenseReduced;
        public override bool InstancePerEntity => true;


        public override void ResetEffects(NPC npc)
        {
            frozen = false;

            if (defenseReduced)
            {
                npc.defense += 10;
                defenseReduced = false;
            }
            if (defenseReducedBoss)
            {
                npc.defense += 7;
                defenseReducedBoss = false;
            }
        }

        public override void PostAI(NPC npc)
        {
            if (frozen)
            {
                if (npc.boss && !defenseReducedBoss)
                {
                    npc.velocity *= 0.98f;
                    npc.defense -= 7;
                    defenseReducedBoss = true;
                }
                else if (defenseReduced)
                {
                    npc.velocity *= 0.93f;
                    npc.defense -= 10;
                    defenseReduced = true;
                }
            }
        }

        public override void DrawEffects(NPC npc, ref Color drawColor)
        {
            if (frozen)
                drawColor = Color.Lerp(drawColor, Color.Cyan, 0.5f);
        }
    }
}
