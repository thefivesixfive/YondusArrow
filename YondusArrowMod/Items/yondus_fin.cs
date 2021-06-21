using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace YondusArrowMod.Items
{
    [AutoloadEquip(EquipType.Head)]
    public class yondus_fin : ModItem
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
			DisplayName.SetDefault("Yondu's Fin");
            Tooltip.SetDefault("Harness the power of Yondu");
        }
        // Item properties
        public override void SetDefaults()
        {
            item.width = 14;
            item.height = 11;
            item.maxStack = 1;
            item.value = 500000;
            item.rare = 1;
            item.defense = 0;
        }
    }
}