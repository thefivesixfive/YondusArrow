using Terraria.ModLoader;

namespace YondusArrowMod
{
	class YondusArrowMod : Mod
	{
		public YondusArrowMod()
		{
			// Stuff I dont know why I need
			Properties = new ModProperties()
            {
                Autoload = true,
                AutoloadGores = true,
                AutoloadSounds = true
            };
		}
	}
}
