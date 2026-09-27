using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace ForgottenFacets.Content.Buffs
{
    internal class VolcanoBuff : ModBuff
    {
        public override string Texture => "ForgottenFacets/Assets/Buffs/VolcanoBuff";
        public override void Update(Player player, ref int buffIndex)
        {
            player.moveSpeed += 0.2f;
            player.GetDamage(DamageClass.Melee) += 0.25f;
            player.GetAttackSpeed(DamageClass.Melee) += 0.25f;
        }
    }
}
