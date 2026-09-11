using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace ForgottenFacets.Content.Materials.Essences
{
    internal class LifeEssence : ModItem
    {
        public override string Texture => "ForgottenFacets/Assets/Materials/Essences/LifeEssence";

        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 25;
            ItemID.Sets.SortingPriorityMaterials[Type] = 60;

            ItemID.Sets.AnimatesAsSoul[Type] = true;
            Main.RegisterItemAnimation(Type, new DrawAnimationVertical(8, 12));
        }
        public override void SetDefaults()
        {
            Item.width = 16;
            Item.height = 16;

            Item.sellPrice(silver: 1, copper: 50);
            Item.rare = ItemRarityID.Blue;
            Item.maxStack = 9999;
        }

        public override void Update(ref float gravity, ref float maxFallSpeed)
        {
            gravity = 0f;
            maxFallSpeed = 0f;

            Item.velocity.Y *= 0.95f;
        }
        public override void OnSpawn(IEntitySource source)
        {
            Item.velocity.Y -= 3f;
        }

        public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
        {
            Texture2D texture = TextureAssets.Item[Type].Value;

            int frameCount = 12;

            int frameHeight = texture.Height / frameCount;
            int frame = (int)(Main.GlobalTimeWrappedHourly * 8f) % frameCount;

            Rectangle sourceRectangle = new Rectangle(0, frameHeight * frame, texture.Width, frameHeight);
            Vector2 origin = new Vector2(sourceRectangle.Width / 2f, sourceRectangle.Height / 2f);


            float bob = (float)Math.Sin(Main.GlobalTimeWrappedHourly * 3f) * 3f;

            Vector2 position = Item.position + Item.Size / 2f;
            position.Y += bob;

            spriteBatch.Draw(texture, position - Main.screenPosition, sourceRectangle, lightColor, rotation, origin, scale, SpriteEffects.None, 0f);

            for (int i = 0; i < 2; i++)
            {
                float offset = MathHelper.TwoPi * i / 4f;

                Vector2 glowOffset = new Vector2((float)Math.Cos(offset) * 2f, (float)Math.Sin(offset) * 2f);
                spriteBatch.Draw(texture, position - Main.screenPosition + glowOffset, sourceRectangle, new Color(255, 255, 255, 0) * 0.25f, rotation, origin, scale, SpriteEffects.None, 0f);
            }

            return false;
        }

    }
}

