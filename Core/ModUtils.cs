using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
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
}
