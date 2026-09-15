using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;

namespace ForgottenFacets.Content.UI
{
    internal class UiSystems : ModSystem
    {
        internal HeatUI HeatUI;
        internal UserInterface HeatInterface;

        public override void Load()
        {
            if (Main.dedServ)
                return;
            //HEAT UI
            HeatUI = new HeatUI();
            HeatUI.Activate();

            HeatInterface = new UserInterface();
            HeatInterface.SetState(HeatUI);
        }
        public override void UpdateUI(GameTime gameTime)
        {
            if (HeatInterface != null)
                HeatInterface.Update(gameTime);
        }
        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int mouseTextIndex = layers.FindIndex(layers => layers.Name.Equals("Vanilla: Mouse Text"));

            if (mouseTextIndex != -1)
            {
                layers.Insert(mouseTextIndex, new LegacyGameInterfaceLayer("Heat Stacks", delegate
                        {
                            HeatInterface.Draw(Main.spriteBatch, new GameTime());
                            return true;
                        },
                        InterfaceScaleType.UI
                    )
                );
            }
        }
    }
}
