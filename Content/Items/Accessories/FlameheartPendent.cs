using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ForgottenFacets.Content.Items.Accessories
{
    [AutoloadEquip(EquipType.Neck)]
    internal class FlameheartPendent : ModItem
    {
        public override string Texture => "ForgottenFacets/Assets/Items/Accessories/FlameheartPendant";
        public override void SetDefaults()
        {
            Item.DefaultToAccessory();

            Item.width = 24;
            Item.height = 24;

            Item.rare = ItemRarityID.Orange;
            Item.value = Item.sellPrice(0, 0, 1, 0);
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            if (player.HasBuff(BuffID.OnFire))
            {
                player.GetDamage(DamageClass.Generic) += 0.25f;
                player.GetAttackSpeed(DamageClass.Generic) += 0.25f;
                player.moveSpeed += 0.30f;
            }
        }
    }
}
