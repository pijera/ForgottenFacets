using ForgottenFacets.Content.Buffs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace ForgottenFacets.Core
{
    public static class ModUtils
    {
        public static bool CheckWoodenArrow(int type,Player player)
        {
            if (player.hasMoltenQuiver && type == ProjectileID.FireArrow)
                return true;
            return type == ProjectileID.WoodenArrowFriendly;
        }
    }

    internal class Tooltips : GlobalItem
    {
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (item.type == ItemID.MoltenFury)
            {
                tooltips.Add(new TooltipLine(Mod, "Heat",
                    "Hits generate a decent amount of [c/FF6428:HEAT]"));


                tooltips.Add(new TooltipLine(Mod, "Heat",
                    "At maximum [c/FF6428:HEAT], shoots a volley flaming arrows"));
            }

            if (item.type == ItemID.PhoenixBlaster)
            {
                tooltips.Add(new TooltipLine(Mod, "Heat",
                    "Hits generate a small amount of [c/FF6428:HEAT]"));


                tooltips.Add(new TooltipLine(Mod, "Heat",
                    "At maximum [c/FF6428:HEAT], fires a series of explosive shots"));
            }

            if (item.type == ItemID.Flamelash)
            {
                tooltips.Add(new TooltipLine(Mod, "Heat",
                    "Hits generate a decent amount of [c/FF6428:HEAT]"));


                tooltips.Add(new TooltipLine(Mod, "Heat",
                    "At maximum [c/FF6428:HEAT], chains an enemy, dealing damage before exploding\r\nThe chain breaks if stretched too far"));
            }




            if (item.type == ItemID.HellwingBow)
            {
                tooltips.Add(new TooltipLine(Mod, "Heat",
                    "Hits generate a small amount of [c/FF6428:HEAT]"));


                tooltips.Add(new TooltipLine(Mod, "Heat",
                    "At maximum [c/FF6428:HEAT], shoots out 5 homing bats"));
            }
            if (item.type == ItemID.ImpStaff)
            {
                tooltips.Add(new TooltipLine(Mod, "Heat",
                    "Hits generate a small amount of [c/FF6428:HEAT]"));


                tooltips.Add(new TooltipLine(Mod, "Heat",
                    "At maximum [c/FF6428:HEAT], makes imps enraged"));
            }

            if (item.type == ItemID.Flamarang)
            {
                tooltips.Add(new TooltipLine(Mod, "Heat",
                    "Hits generate a big amount of [c/FF6428:HEAT]"));


                tooltips.Add(new TooltipLine(Mod, "Heat",
                    "At maximum [c/FF6428:HEAT], unleash multiple boomerangs"));
            }
            if (item.type == ItemID.FieryGreatsword)
            {
                tooltips.Add(new TooltipLine(Mod, "Heat",
                    "Hits generate a decent amount of [c/FF6428:HEAT]"));


                tooltips.Add(new TooltipLine(Mod, "VolcanoBuff",
                    "At maximum [c/FF6428:HEAT], gives       the Fiery Rage buff"));
            }


        }
        public override bool PreDrawTooltipLine(Item item, DrawableTooltipLine line, ref int yOffset)
        {
            if (item.type != ItemID.FieryGreatsword || line.Name != "VolcanoBuff")
                return true;

            Texture2D texture =
                TextureAssets.Buff[ModContent.BuffType<VolcanoBuff>()].Value;

            Main.spriteBatch.Draw(texture, new Vector2(line.X + 210f, line.Y - 5f), null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);

            return true;
        }
    }

}
