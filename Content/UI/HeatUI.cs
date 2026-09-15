using ForgottenFacets.Content.ModPlayers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;

namespace ForgottenFacets.Content.UI
{
    internal class HeatUI : UIState
    {
        public override void OnInitialize()
        {
            HeatBar heatBar = new HeatBar();

            heatBar.Left.Set(-96, 0.5f);
            heatBar.Top.Set(35, 0f);

            heatBar.Width.Set(192f, 0f);
            heatBar.Height.Set(48f, 0f);

            Append(heatBar);
        }
    }

    internal class HeatBar : UIElement
    {
        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            var heatBorder = ModContent.Request<Texture2D>("ForgottenFacets/Assets/UI/HeatBarBorder").Value;

            var heatBar = ModContent.Request<Texture2D>("ForgottenFacets/Assets/UI/HeatBar").Value;

            Player player = Main.LocalPlayer;

            SuperHeatingWeapons heatPlayer = player.GetModPlayer<SuperHeatingWeapons>();

            if (!heatPlayer.isHoldingHeatWeapon())
                return;

            float heatPercent = MathHelper.Clamp(heatPlayer.Heat / 100f, 0f, 1f);

            CalculatedStyle dimensions = GetDimensions();

            Vector2 position = dimensions.Position();


            int borderWidth = 192;
            int borderHeight = 48;

            spriteBatch.Draw(heatBorder, new Rectangle((int)position.X, (int)position.Y, borderWidth,borderHeight),Color.White);
            int fillWidth = 176;
            int fillHeight = 14;

            // Center inside border
            int fillX = (int)position.X + (borderWidth - fillWidth) / 2;

            int fillY = (int)position.Y + (borderHeight - fillHeight) / 2;

            int sourceWidth = (int)(heatBar.Width * heatPercent);

            if (sourceWidth > 0)
            {
                Rectangle sourceRectangle = new Rectangle(0, 0, sourceWidth, heatBar.Height);

                int destinationWidth = (int)(fillWidth * heatPercent);

                Rectangle destinationRectangle = new Rectangle(fillX, fillY, destinationWidth, fillHeight);

                spriteBatch.Draw(heatBar,destinationRectangle,sourceRectangle,Color.White * 0.8f);
            }
        }
    }
}