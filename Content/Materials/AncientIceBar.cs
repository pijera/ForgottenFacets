using ForgottenFacets.Content.Dusts;
using ForgottenFacets.Content.Tiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ForgottenFacets.Content.Materials
{
    internal class AncientIceBar : ModItem
    {
        public override string Texture => "ForgottenFacets/Assets/Materials/Bars/AncientIceBar";

        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 25;
            ItemID.Sets.SortingPriorityMaterials[Type] = 59;
        }
        public override void SetDefaults()
        {
            Item.DefaultToPlaceableTile(ModContent.TileType<AncientIceBarTile>());
            Item.sellPrice(silver: 15);
            Item.rare = ItemRarityID.Blue;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<AncientIceOre>(), 3)
                .AddTile(TileID.Furnaces)
                .Register();
        }

        public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            Texture2D tex = ModContent.Request<Texture2D>("ForgottenFacets/Assets/Materials/Bars/AncientIceBar").Value;
            Texture2D outline = ModContent.Request<Texture2D>("ForgottenFacets/Assets/Materials/Bars/AncientIceBar_Outline").Value;

            float sin = (float)Math.Abs(Math.Sin(Main.GlobalTimeWrappedHourly)) * 0.5f;

            Main.spriteBatch.Draw(tex, position, null, drawColor, 0f, origin, scale, SpriteEffects.None, 0f);
            Main.spriteBatch.Draw(outline, position, null, Color.White * sin, 0f, outline.Size() / 2f, scale, SpriteEffects.None, 0f);

            return false;
        }

        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Texture2D outline = ModContent.Request<Texture2D>("ForgottenFacets/Assets/Materials/Bars/AncientIceBar_Outline").Value;

            float sin = (float)Math.Abs(Math.Sin(Main.GlobalTimeWrappedHourly)) * 0.5f;

            Main.spriteBatch.Draw(outline, Item.Center - Main.screenPosition + new Vector2(0, -5), null, Color.White * sin, rotation, outline.Size() / 2f, scale, 0f, 0f);
        }

        public override void Update(ref float gravity, ref float maxFallSpeed)
        {
            float sin = (float)Math.Abs(Math.Sin(Main.GlobalTimeWrappedHourly)) * 0.5f;

            Lighting.AddLight(Item.Center, Color.Lerp(new Color(71, 143, 234), new Color(0, 64, 145), 0.5f).ToVector3() * sin);

            if (Main.rand.NextBool(30))
                Dust.NewDustPerfect(Item.Center + Main.rand.NextVector2Circular(20f, 20f), ModContent.DustType<GlowDust>(),
                    -Vector2.UnitY, 0, Color.Lerp(new Color(71, 143, 234), new Color(0, 64, 145), 0.5f), Main.rand.NextFloat(0.3f, 0.5f));

            if (Main.rand.NextBool(30))
                Dust.NewDustPerfect(Item.Center + Main.rand.NextVector2Circular(20f, 20f),
                    ModContent.DustType<SparkleDust>(), -Vector2.UnitY, 0, Color.Lerp(new Color(71, 143, 234), new Color(0, 64, 145), 0.5f), Main.rand.NextFloat(0.3f, 0.5f));


        }


    }
}
