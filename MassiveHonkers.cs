using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gungeon;
using UnityEngine;
using ItemAPI;
namespace ConvictButWithBigHonkers
{
    class HonkerPistol : AdvancedGunBehaviour
    {
        private static Gun BRevolv;
        public static void Add()
        {
             BRevolv = (PickupObjectDatabase.GetById(80) as Gun);

            Gun gun = ETGMod.Databases.Items.NewGun("Budget Honker", "budget_honker");
            Game.Items.Rename("outdated_gun_mods:budget_honker", "honkies:budget_honker");
            gun.gameObject.AddComponent<HonkerPistol>();
            gun.SetShortDescription("Affordable Arms");
            gun.SetLongDescription("This trumpet was brought to the Gungeon by an infamous fugitive.\n\nProvided by the Hegemony Regional Magistrate. The Convict won their plea to face the Gungeon in lieu of life imprisonment; undo their crimes, or face an eternity in Gungeon. With no cost to the state, it was an acceptable arrangement.\n\nCheaply made and prone to jams, the trumpet used by only the most desperate Gungeoneers.");
            gun.SetupSprite(null, "budget_honker_idle_001", 8);
            gun.SetAnimationFPS(gun.shootAnimation, 10);
            gun.SetAnimationFPS(gun.reloadAnimation, 10);
            gun.AddProjectileModuleFrom(BRevolv, true, false);

            gun.DefaultModule.ammoType = BRevolv.AmmoType;
            gun.DefaultModule.ammoCost = BRevolv.DefaultModule.ammoCost;
            gun.DefaultModule.shootStyle = BRevolv.DefaultModule.shootStyle;
            gun.DefaultModule.sequenceStyle = BRevolv.DefaultModule.sequenceStyle;
            gun.DefaultModule.angleVariance = BRevolv.DefaultModule.angleVariance;
            gun.DefaultModule.cooldownTime = BRevolv.DefaultModule.cooldownTime;
            gun.DefaultModule.numberOfShotsInClip = BRevolv.DefaultModule.numberOfShotsInClip;
            gun.reloadTime = BRevolv.reloadTime;            
            gun.gunSwitchGroup = BRevolv.gunSwitchGroup;
            gun.muzzleFlashEffects = BRevolv.muzzleFlashEffects;
            gun.SetBaseMaxAmmo(BRevolv.GetBaseMaxAmmo());
            gun.quality = BRevolv.quality;
            gun.encounterTrackable.EncounterGuid = "i hate you so fucking much you stupid pervs";
            gun.sprite.IsPerpendicular = true;
            gun.barrelOffset.transform.localPosition = new Vector3(1.4375f, 0.34375f, 0f);
            gun.gunClass = BRevolv.gunClass;
            gun.InfiniteAmmo = BRevolv.InfiniteAmmo;
            Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(gun.DefaultModule.projectiles[0]);
            projectile.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(projectile.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(projectile);
            gun.DefaultModule.projectiles[0] = projectile;
            projectile.transform.parent = gun.barrelOffset;
            projectile.baseData.damage *= 1f;
            projectile.baseData.speed *= 1f;
            projectile.baseData.force *= 1f;


            BRevolv.quality = PickupObject.ItemQuality.EXCLUDED;
            ETGMod.Databases.Items.Add(gun, null, "ANY");
        }
        private bool HasReloaded;
        protected override void Update()
        {
            base.Update();
            if (gun.CurrentOwner)
            {

                if (!gun.PreventNormalFireAudio)
                {
                    this.gun.PreventNormalFireAudio = true;
                }
                if (!BRevolv.PreventNormalFireAudio)
                    BRevolv.PreventNormalFireAudio = true;
                if (!gun.IsReloading && !HasReloaded)
                {
                    this.HasReloaded = true;
                }
            }
        }
        public override void OnPostFired(PlayerController player, Gun gun)
        {
            BRevolv.PreventNormalFireAudio = true;
            gun.PreventNormalFireAudio = true;
            AkSoundEngine.PostEvent("Play_Trumpet_Honk", gameObject);
        }
        public HonkerPistol()
        {

        }
    }
    class HonkerShotgun: AdvancedGunBehaviour
    {
        private static Gun SHonk;
        public static void Add()
        {
             SHonk = (PickupObjectDatabase.GetById(202) as Gun);

            Gun gun = ETGMod.Databases.Items.NewGun("Sawed-off Honker", "sawed-off_honker");
            Game.Items.Rename("outdated_gun_mods:sawed-off_honker", "honkies:sawed-off_honker");
            gun.gameObject.AddComponent<HonkerShotgun>();
            gun.SetShortDescription("No Butts About It");
            gun.SetLongDescription("A trombone modified for easy concealment. The shorter barrel widens the spread, but up-close, it's just as deadly as its full-barreled cousin.");
            gun.SetupSprite(null, "sawed-off_honker_idle_001", 8);
            gun.SetAnimationFPS(gun.shootAnimation, 10);
            gun.SetAnimationFPS(gun.reloadAnimation, 10);
            for (int i = 0; i < 4; i++)
            {
                GunExt.AddProjectileModuleFrom(gun, SHonk, true, false);
            }
            foreach (ProjectileModule projectileModule in gun.Volley.projectiles)
            {
                gun.DefaultModule.ammoType = SHonk.AmmoType;
                gun.DefaultModule.ammoCost = SHonk.DefaultModule.ammoCost;
                gun.DefaultModule.shootStyle = SHonk.DefaultModule.shootStyle;
                gun.DefaultModule.sequenceStyle = SHonk.DefaultModule.sequenceStyle;
                gun.DefaultModule.angleVariance = SHonk.DefaultModule.angleVariance;
                gun.DefaultModule.cooldownTime = SHonk.DefaultModule.cooldownTime;
                gun.DefaultModule.numberOfShotsInClip = SHonk.DefaultModule.numberOfShotsInClip;
                Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(projectileModule.projectiles[0]);
                projectile.gameObject.SetActive(false);
                FakePrefab.MarkAsFakePrefab(projectile.gameObject);
                UnityEngine.Object.DontDestroyOnLoad(projectile);
                gun.DefaultModule.projectiles[0] = projectile;
                projectile.transform.parent = gun.barrelOffset;
                projectile.baseData.damage *= 1f;
                projectile.baseData.speed *= 1f;
                projectile.baseData.force *= 1f;

                bool flag = projectileModule == gun.DefaultModule;
                if (flag)
                {
                    projectileModule.ammoCost = 1;
                }
                else
                {
                    projectileModule.ammoCost = 0;
                }
            }
            
            gun.reloadTime = SHonk.reloadTime;
            gun.gunSwitchGroup = SHonk.gunSwitchGroup;
            gun.muzzleFlashEffects = SHonk.muzzleFlashEffects;
            gun.SetBaseMaxAmmo(SHonk.GetBaseMaxAmmo());
            gun.quality = SHonk.quality;
            gun.encounterTrackable.EncounterGuid = "why did i make this fucking mod i hate it whyyyyy";
            gun.sprite.IsPerpendicular = true;
            gun.barrelOffset.transform.localPosition = new Vector3(1.625f, 0.5f, 0f);
            gun.gunClass = SHonk.gunClass;
            
            ETGMod.Databases.Items.Add(gun, null, "ANY");
        }
        private bool HasReloaded;
        protected override void Update()
        {
            base.Update();
            if (gun.CurrentOwner)
            {

                if (!gun.PreventNormalFireAudio)
                {
                    this.gun.PreventNormalFireAudio = true;
                }
                if(!SHonk.PreventNormalFireAudio)
                    SHonk.PreventNormalFireAudio = true;
                if (!gun.IsReloading && !HasReloaded)
                {
                    this.HasReloaded = true;
                }
            }
        }
        public override void OnPostFired(PlayerController player, Gun gun)
        {
            SHonk.PreventNormalFireAudio = true;
            gun.PreventNormalFireAudio = true;
            AkSoundEngine.PostEvent("Play_Trombone_Honk", gameObject);
        }
        public HonkerShotgun()
        {

        }
    }
}
