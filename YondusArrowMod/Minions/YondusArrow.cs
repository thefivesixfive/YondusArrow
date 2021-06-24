using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace YondusArrowMod.Minions
{
    [AutoloadEquip(EquipType.Head)]
    public class YondusFin : ModItem
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
			DisplayName.SetDefault("Yondu's Fin");
            Tooltip.SetDefault("Equip to harness the power of Yondu");
        }
        // Item properties
        public override void SetDefaults()
        {
            item.width = 28;
            item.height = 22;
            item.maxStack = 1;
            item.value = 500000;
            item.rare = 1;
            item.defense = 0;
            item.buffType = ModContent.BuffType<YondusBlessing>();
        }
        // When item is equiped add buff
        public override void UpdateEquip(Player player) {
            // i have no idea what im doing help me
            Projectile.NewProjectile(player.position.X + (float)(player.width / 2), player.position.Y + (float)(player.height / 2), 0f, 0f, ModContent.ProjectileType<YondusArrow>(), 1000000000, 0f, player.whoAmI, 0f, 0f);
            player.AddBuff(item.buffType, 2);
        }
    }
    // Buff
    public class YondusBlessing : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Yondu's Blessing");
            Description.SetDefault("Whistle to control the arrow");
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }
        // update
        public override void Update(Player player, ref int buffIndex)
        {
            // i have no idea what this does
            if (player.ownedProjectileCounts[ModContent.ProjectileType<YondusArrow>()] > 0)
            {
                player.buffTime[buffIndex] = 60;
                
            }
            // i have no idea what this does either
            else 
            {
                player.DelBuff(buffIndex);
                buffIndex--;
            }
        }
    }
    // Projectile
    public class YondusArrow : ModProjectile
    {
        // static stuff idk
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Yondu's Arrow");
            Main.projFrames[projectile.type] = 1;
            Main.projPet[projectile.type] = true;
            ProjectileID.Sets.MinionSacrificable[projectile.type] = true;
            ProjectileID.Sets.Homing[projectile.type] = true;
        }
        // defaults idk
        public sealed override void SetDefaults()
        {
            projectile.width = 32;
            projectile.height = 32;
            projectile.tileCollide = false;

            projectile.friendly = true;
            projectile.minion = true;
            projectile.minionSlots = 1f;
            projectile.penetrate = -1;
        }
        // cuts pots
        public override bool? CanCutTiles()
        {
            return true;
        }
        // contact damage
        public override bool MinionContactDamage()
        {
            return true;
        }
        // ai
        public override void AI()
        {
            // player?
            Player player = Main.player[projectile.owner];

            // active checking region
            if (player.dead || !player.active)
            {
                player.ClearBuff(ModContent.BuffType<YondusBlessing>());
            }
            if (!player.HasBuff(ModContent.BuffType<YondusBlessing>()))
            {
                projectile.Kill();
            }

            // idle behavior
            Vector2 idlePosition = player.Center;
            idlePosition.Y -= 16f;
            idlePosition.X -= 16f;
            Vector2 vectorToIdlePosition = idlePosition - projectile.Center;
            float distanceToIdlePosition = vectorToIdlePosition.Length();

            // start search
			float distanceFromTarget = 700f;
			Vector2 targetCenter = projectile.position;
			bool foundTarget = false;

            // hunting
            if (!foundTarget)
            {
                for (int i=0;i<Main.maxNPCs;i++)
                {
                    NPC npc = Main.npc[i];
                    if (npc.CanBeChasedBy()) {
                        float between = Vector2.Distance(npc.Center, projectile.Center);
                        bool closest = Vector2.Distance(projectile.Center, targetCenter) > between;
                        bool inRange = between < distanceFromTarget;
                        if ((closest && inRange) || !foundTarget)
                        {
                            distanceFromTarget = between;
                            targetCenter = npc.Center;
                            foundTarget = true;
                        }
                    }
                }
            }
            projectile.friendly = foundTarget;

            // move
            float speed = 30f;
            float inertia = 10f;

            if (foundTarget)
            {
                if (distanceFromTarget > 40f)
                {
                    Vector2 direction = targetCenter - projectile.Center;
                    direction.Normalize();
                    direction *= speed;
                    projectile.velocity = (projectile.velocity * (inertia-1) + direction) / inertia;
                    projectile.rotation = (float)(Math.Atan2(direction.Y, direction.X) / (Math.PI * 2));
                }
            }
            else 
            {
                // if far from player
                if (distanceToIdlePosition > 64f)
                {
                    vectorToIdlePosition.Normalize();
					vectorToIdlePosition *= speed;
					projectile.velocity = (projectile.velocity * (inertia - 1) + vectorToIdlePosition) / inertia;
                    // animation
                    projectile.rotation = (float)(Math.Atan2(vectorToIdlePosition.Y, vectorToIdlePosition.X) / (Math.PI * 2));
                }
                // otherwise
                else
                {
                    projectile.velocity = new Vector2(0f,0f);
                    projectile.position = idlePosition;
                    projectile.rotation = (float)Math.PI;

                }
            }
        }
    }
}