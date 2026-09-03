using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;

namespace ForgottenFacets.Content.Dusts
{
    internal class SparkleDust : ModDust
    {
        public override string Texture => "ForgottenFacets/Assets/Misc/Invisible";

        public override void OnSpawn(Dust dust)
        {
            dust.frame = new Rectangle(0, 0, 4, 4);
        }

        public override bool Update(Dust dust)
        {
            dust.position += dust.velocity;
            dust.velocity *= 0.95f;

            if (dust.customData is not null && (bool)dust.customData)
                dust.rotation += dust.velocity.Length() * 0.05f;

            dust.scale *= 0.95f;

            if (dust.scale < 0.02f)
                dust.active = false;

            Lighting.AddLight(dust.position, dust.color.ToVector3() * 0.15f);
            return false;
        }

        public override bool PreDraw(Dust dust)
        {
            Color color = dust.color;

            float lerper = 1f - dust.alpha / 255f;

            Texture2D starTex = TextureAssets.Projectile[79].Value;
            Texture2D bloomTex = TextureAssets.Projectile[540].Value;

            Main.spriteBatch.Draw(bloomTex, dust.position - Main.screenPosition, null, color * lerper * 0.05f, dust.rotation, bloomTex.Size() / 2f, dust.scale * 0.8f * lerper, 0f, 0f);
            Main.spriteBatch.Draw(starTex, dust.position - Main.screenPosition, null, color * lerper, dust.rotation, starTex.Size() / 2f * 0.9f, dust.scale * lerper, 0f, 0f);

            Main.spriteBatch.Draw(starTex, dust.position - Main.screenPosition, null, Color.White with { A = 0 } * lerper, dust.rotation, starTex.Size() / 2f * 0.9f, dust.scale * 0.7f * lerper, 0f, 0f);

            return false;
        }
    }
}
