using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;

namespace ForgottenFacets.Core
{
    internal class ScreenShake : ModPlayer
    {
        public int shakeTimer;

        public override void ModifyScreenPosition()
        {
            if (shakeTimer > 0)
            {
                shakeTimer--;
                Vector2 shake = new Vector2(Main.rand.NextFloat(shakeTimer), Main.rand.NextFloat(shakeTimer));
                Main.screenPosition += shake;
            }
        }
        public void AddShake(int amount,bool clamped = true)
        {
            if (clamped)
            {
                if (shakeTimer < amount)
                    shakeTimer = amount;
            }
            else
                shakeTimer += amount;
        }
    }
}
