using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace ForgottenFacets.Content.Dusts
{
    internal class GlowDust : ModDust
    {
        public override string Texture => "ForgottenFacets/Assets/Misc/Invisible";

        public override void OnSpawn(Dust dust)
        {
            dust.frame = new Rectangle(0,0,4,4);
        }
        public override bool Update(Dust dust)
        {
            dust.position += dust.velocity;
            dust.velocity *= 0.95f;

            dust.alpha += 5;

            dust.alpha = (int)(dust.alpha * 1.01f);
            dust.scale *= 0.965f;

            Lighting.AddLight(dust.position, dust.color.ToVector3() * 0.5f);

            if (dust.alpha >= 255)
                dust.active = false;

            return false;
        }
        public override bool PreDraw(Dust dust)
        {
            float lerper = 1f - (dust.alpha / 255f);

            Texture2D tex = ModContent.Request<Texture2D>("ForgottenFacets/Assets/Misc/GlowHarshAlpha").Value;

            Main.spriteBatch.Draw(tex, dust.position - Main.screenPosition, null, dust.color with { A = 0 } * lerper, dust.rotation, tex.Size() / 2f, dust.scale * lerper, 0f, 0f);

            float glowScale = dust.scale * 0.25f;

            Main.spriteBatch.Draw(tex, dust.position - Main.screenPosition, null, dust.color with { A = 0 } * lerper, dust.rotation, tex.Size() / 2f, dust.scale * lerper, 0f, 0f);

            return false;
        }



    }
}
