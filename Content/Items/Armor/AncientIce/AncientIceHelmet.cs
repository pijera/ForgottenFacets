using ForgottenFacets.Content.Dusts;
using ForgottenFacets.Content.Projectiles;
using ForgottenFacets.Core;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace ForgottenFacets.Content.Items.Armor.AncientIce
{
    [AutoloadEquip(EquipType.Head)]
    internal class AncientIceHelmet : ModItem
    {
        public override string Texture => "ForgottenFacets/Assets/Items/Armor/AncientIce/AncientIceHelmet";

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;

            Item.rare = ItemRarityID.Blue;
            Item.defense = 4;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetDamage(DamageClass.Ranged) *= 1.04f;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs) => body.type == ModContent.ItemType<AncientIceChestpiece>() && legs.type == ModContent.ItemType<AncientIceLeggings>();

        public override void UpdateArmorSet(Player player)
        {
            var modPlayer = player.GetModPlayer<AncientIceModPlayer>();

            modPlayer.fullSet = true;
            player.setBonus = "Critical strikes cause freezing explosions";

            if (modPlayer.cooldown <= 0 && Main.rand.NextBool(20))
            {
                Dust.NewDustPerfect(player.Center + Main.rand.NextVector2Circular(player.width / 2, player.height / 2), ModContent.DustType<GlowDust>(),
                    -Vector2.UnitY * 0.66f, 0, new Color(149, 71, 234), Main.rand.NextFloat(0.5f, 1f)).noGravity = true;
            }
        }

        public override void ArmorSetShadows(Player player)
        {
            var modPlayer = player.GetModPlayer<AncientIceModPlayer>();

            if (modPlayer.fullSet && modPlayer.cooldown <= 0)
                player.armorEffectDrawShadow = true;
        }

    }

    public class AncientIceModPlayer : ModPlayer
    {
        public const int MAX_COOLDOWN = 10 * 60;
        public bool fullSet;
        public int cooldown;

        public override void ResetEffects()
        {

            fullSet = false;

            if (cooldown > 0)
            {
                if (cooldown == 1)
                {
                    for (int i = 0; i < 15; i++)
                    {
                        Dust.NewDustPerfect(Player.Center + Main.rand.NextVector2Circular(30f, 30f), ModContent.DustType<SparkleDust>(), -Vector2.UnitY, 0, Color.White, 0.15f);
                    }

                    SoundEngine.PlaySound(SoundID.MaxMana, Player.Center);
                }

                cooldown--;
            }
        }

        public override void DrawEffects(PlayerDrawSet drawInfo, ref float r, ref float g, ref float b, ref float a, ref bool fullBright)
        {
            if (drawInfo.shadow != 0 && fullSet)
            {
                Color purpule = new Color(255, 255, 255);

                r = purpule.R;
                g = purpule.G;
                b = purpule.B;

                a *= 0;
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (!fullSet)
                return;

            if (hit.Crit && cooldown == 0)
            {
                cooldown = MAX_COOLDOWN;
                DoCritEffect(target);
            }
        }

        private void DoCritEffect(NPC target)
        {
            if (Main.myPlayer == Player.whoAmI)//multiplayer compentability
                Projectile.NewProjectile(Player.GetSource_OnHit(target), target.Center, Vector2.Zero, ModContent.ProjectileType<AncientIceArmorProjectile>(), 30, 2f, Player.whoAmI, 50);


            SoundEngine.PlaySound(SoundID.Item120 with { PitchRange = (0.8f, 1.2f) }, target.Center);
            Player.GetModPlayer<ScreenShake>().AddShake(5);

            for (int i = 0; i < 25; i++)
            {
                Dust.NewDustPerfect(target.Center, DustID.IceRod, -Main.rand.NextVector2Circular(5f, 5f), 120 + Main.rand.Next(120), Color.Cyan, 1f);

                Dust.NewDustPerfect(target.Center, DustID.IceRod, -Main.rand.NextVector2Circular(5f, 5f), 40 + Main.rand.Next(120), Color.LightCyan, Main.rand.NextFloat(0.3f, 0.6f));

                Dust.NewDustPerfect(target.Center, DustID.Ice, Main.rand.NextVector2Circular(5f, 5f), 150, default, 0.8f);

                Dust.NewDustPerfect(target.Center, DustID.Ice, Main.rand.NextVector2Circular(8f, 8f), 100, default, 2f).noGravity = true;
            }
        }
    }

}
