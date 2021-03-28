using ItemAPI;
using UnityEngine;
using Gungeon;
using System;
using MonoMod;
using System.Collections;
using System.Reflection;
using MonoMod.RuntimeDetour;
using System.Collections.Generic;
namespace ConvictButWithBigHonkers
{

    public class StartThisCursedMod : ETGModule
    {
        public static readonly string Color = "#FCA4E2";
        public static string ZipFilePath;
        public static string FilePath;
        public override void Init()
        {

        }
        public override void Start()
        {
            try
            {
                ZipFilePath = this.Metadata.Archive;
                FilePath = this.Metadata.Directory;
                FakePrefabHooks.Init();
                ItemBuilder.Init();
                AudioResourceLoader.InitAudio();

                HonkerPistol.Add();
                HonkerShotgun.Add();
                Hooks();
                Log("Convict now has massive honkers, you sick fuck.", Color);
            }catch (Exception e)
            {
                Log($"{e}");
            }
           
        }

        
        public static void Log(string text, string color = "FFFFFF")
        {
            ETGModConsole.Log($"<color={color}>{text}</color>");
        }
        public static void Hooks()
        {
            Hook Load = new Hook(typeof(PlayerController).GetMethod("DoInitialFallSpawn", BindingFlags.Instance | BindingFlags.Public), typeof(StartThisCursedMod).GetMethod("OnChamberEnter"));
            Hook OtherLoad = new Hook(typeof(PlayerController).GetMethod("DoSpinfallSpawn", BindingFlags.Instance | BindingFlags.Public), typeof(StartThisCursedMod).GetMethod("OnChamberEnter"));
            
        }
        
        public static void OnChamberEnter(Action<PlayerController, float> orig, PlayerController self, float invisibleDelay)
        {
            bool? isForPlayerOne = null;
            if (GameManager.Instance.PrimaryPlayer == self)
            {
                isForPlayerOne = true;
            }
            else if (GameManager.Instance.SecondaryPlayer == self)
            {
                isForPlayerOne = false;
            }
            orig(self, invisibleDelay);
            if (isForPlayerOne != null)
            {
                GameManager.Instance.StartCoroutine(waitAndExecuteChamberEnter(isForPlayerOne.Value));
            }
        }
        private static IEnumerator waitAndExecuteChamberEnter(bool isForPlayerOne)
        {
            PlayerController player = isForPlayerOne ? GameManager.Instance.PrimaryPlayer : GameManager.Instance.SecondaryPlayer;
            while (player.CurrentInputState != PlayerInputState.AllInput || ETGModConsole.Instance.GUI.Visible)
            {
                yield return null;
            }
            object cc = player.GetComponent("CustomCharacter");
            if (!hasTakenDamage() && GameManager.Instance.CurrentFloor == getStartingFloor())
            {
                if (player.characterIdentity == PlayableCharacters.Convict && player.inventory.ContainsGun(PickupObjectDatabase.GetById(80).PickupObjectId) && player.inventory.ContainsGun(PickupObjectDatabase.GetById(202).PickupObjectId) && cc == null)
                {
                    //player.inventory.DestroyGun(PickupObjectDatabase.GetById(80) as Gun);
                    //player.inventory.DestroyGun(PickupObjectDatabase.GetById(202) as Gun);
                    player.inventory.DestroyAllGuns();

                    player.inventory.AddGunToInventory(ETGMod.Databases.Items["sawed-off_honker"] as Gun, false);
                    player.inventory.AddGunToInventory(ETGMod.Databases.Items["budget_honker"] as Gun, true);
                }
            }
            
        }
        private static bool hasTakenDamage()
        {
            foreach (var player in GameManager.Instance.AllPlayers)
            {
                if (player.HasTakenDamageThisRun)
                    return true;
            }
            return false;
        }
        private static int getStartingFloor()
        {
            int nextLevelIndex;
            if (GameManager.Instance.TargetQuickRestartLevel != -1)
            {
                nextLevelIndex = GameManager.Instance.TargetQuickRestartLevel;
            }
            else
            {
                nextLevelIndex = 1;
                if (GameManager.Instance.CurrentGameMode == GameManager.GameMode.SHORTCUT)
                {
                    nextLevelIndex += GameManager.Instance.LastShortcutFloorLoaded;
                }
            }
            return nextLevelIndex;
        }
        IEnumerator WaitForSelectFinish()
        {
            yield return null;
            yield return null;
            yield return null;
        }
        public override void Exit()
        {
            
        }
    }
}
