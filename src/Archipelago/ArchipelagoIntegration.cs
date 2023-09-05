using Archipelago.MultiClient.Net.Enums;
using Archipelago.MultiClient.Net;
using BepInEx.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Archipelago.MultiClient.Net.Models;
using System.Collections.Concurrent;
using System.Threading;
using UnityEngine.SceneManagement;
using UnityEngine;
using Archipelago.MultiClient.Net.BounceFeatures.DeathLink;
using Newtonsoft.Json;
using System.Globalization;
using Archipelago.MultiClient.Net.Packets;
using System.IO;
using System.Runtime.InteropServices.ComTypes;
using Archipelago.MultiClient.Net.Helpers;

namespace TunicArchipelago {
    public class ArchipelagoIntegration {

        private ManualLogSource Logger = TunicArchipelago.Logger;
        
        public bool connected;

        public ArchipelagoSession session;
        private IEnumerator<bool> incomingItemHandler;
        private IEnumerator<bool> outgoingItemHandler;
        private IEnumerator<bool> checkItemsReceived;
        private ConcurrentQueue<(NetworkItem NetworkItem, int index)> incomingItems;
        private ConcurrentQueue<NetworkItem> outgoingItems;
        private DeathLinkService deathLinkService;
        public Dictionary<string, object> slotData;
        private int ItemIndex = 0;

        public void Update() {
            if (!connected) {
                return;
            }

            if (checkItemsReceived != null) {
                checkItemsReceived.MoveNext();
            }

            if (SceneManager.GetActiveScene().name != "TitleScreen" && SceneManager.GetActiveScene().name != "Loading" && PlayerCharacter.instance != null) {

                if (incomingItemHandler != null) {
                    incomingItemHandler.MoveNext();
                }

                if (outgoingItemHandler != null) {
                    outgoingItemHandler.MoveNext();
                }
            }

        }

        public void TryConnect() {

            RandomizerSettings settings = JsonConvert.DeserializeObject<RandomizerSettings>(File.ReadAllText(TunicArchipelago.SettingsPath));
            TunicArchipelago.Settings.ConnectionSettings = settings.ConnectionSettings;

            LoginResult LoginResult;
            if (connected) {
                return;
            }
            if (session == null) {
                session = ArchipelagoSessionFactory.CreateSession(TunicArchipelago.Settings.ConnectionSettings.Hostname, TunicArchipelago.Settings.ConnectionSettings.Port);
            }
            incomingItemHandler = IncomingItemHandler();
            outgoingItemHandler = OutgoingItemHandler();
            checkItemsReceived = CheckItemsReceived();
            incomingItems = new ConcurrentQueue<(NetworkItem NetworkItem, int index)>();
            outgoingItems = new ConcurrentQueue<NetworkItem>();

            TunicArchipelago.Tracker = new ItemTracker();

            try {
                LoginResult = session.TryConnectAndLogin("Tunic", TunicArchipelago.Settings.ConnectionSettings.Player, ItemsHandlingFlags.AllItems, requestSlotData: true, password: TunicArchipelago.Settings.ConnectionSettings.Password);
            } catch (Exception e) {
                LoginResult = new LoginFailure(e.GetBaseException().Message);
            }

            if (LoginResult is LoginSuccessful LoginSuccess) {

                slotData = LoginSuccess.SlotData;

                connected = true;
                Logger.LogInfo("Successfully connected to Archipelago Multiworld server!");

                deathLinkService = session.CreateDeathLinkService();

                deathLinkService.OnDeathLinkReceived += (deathLinkObject) => {
                    if (SceneManager.GetActiveScene().name != "TitleScreen") {
                        Logger.LogInfo("Death link received");
                        PlayerCharacterPatches.DeathLinkMessage = deathLinkObject.Cause == null ? $"\"{deathLinkObject.Source} died and took you with them.\"": $"\"{deathLinkObject.Cause}\"";
                        PlayerCharacterPatches.DiedToDeathLink = true;
                    }
                };

                if (TunicArchipelago.Settings.DeathLinkEnabled) {
                    deathLinkService.EnableDeathLink();
                }



            } else {
                LoginFailure loginFailure = (LoginFailure)LoginResult;
                Logger.LogInfo("Error connecting to Archipelago:");
                ShowNotification($"\"Failed to connect to Archipelago!\"", $"\"Check your settings and/or log output.\"");
                foreach (string Error in loginFailure.Errors) {
                    Logger.LogInfo(Error);
                }
                foreach (ConnectionRefusedError Error in loginFailure.ErrorCodes) {
                    Logger.LogInfo(Error);
                }
                connected = false;
            }
        }

        public void TryDisconnect() {

            try {
                
                if (session != null) {
                    session.Socket.DisconnectAsync();
                    session = null;
                }

                connected = false;
                incomingItemHandler = null;
                outgoingItemHandler = null;
                checkItemsReceived = null;
                incomingItems = new ConcurrentQueue<(NetworkItem NetworkItem, int ItemIndex)>();
                outgoingItems = new ConcurrentQueue<NetworkItem>();
                deathLinkService = null;
                slotData = null;
                ItemIndex = 0;
                Locations.CheckedLocations.Clear();
                ItemLookup.ItemList.Clear();

                Logger.LogInfo("Disconnected from Archipelago");
            } catch (Exception e) {
                Logger.LogInfo("Encountered an error disconnecting from Archipelago!");
            }
        }

        private IEnumerator<bool> CheckItemsReceived() {
            while (connected) {
                if (session.Items.AllItemsReceived.Count > ItemIndex) {
                    NetworkItem Item = session.Items.AllItemsReceived[ItemIndex];
                    string ItemReceivedName = session.Items.GetItemName(Item.Item);
                    Logger.LogInfo("Placing item " + ItemReceivedName + " with index " + ItemIndex + " in queue.");
                    incomingItems.Enqueue((Item, ItemIndex));
                    ItemIndex++;
                    yield return true;
                } else {
                    yield return true;
                    continue;
                }
            }
        }

        private IEnumerator<bool> IncomingItemHandler() {
            while (connected) {

                if (!incomingItems.TryPeek(out var pendingItem)) {
                    yield return true;
                    continue;
                }

                var networkItem = pendingItem.NetworkItem;
                var itemName = session.Items.GetItemName(networkItem.Item);
                var itemDisplayName = itemName + " (" + networkItem.Item + ") at index " + pendingItem.index;

                if (SaveFile.GetInt($"randomizer processed item index {pendingItem.index}") == 1) {
                    incomingItems.TryDequeue(out _);
                    TunicArchipelago.Tracker.SetCollectedItem(itemName, false);
                    Logger.LogInfo("Skipping item " + itemName + " at index " + pendingItem.index + " as it has already been processed.");
                    yield return true;
                    continue;
                }

                // Delay until a few seconds after connecting/screen transition
                while (SpeedrunData.inGameTime < SceneLoaderPatches.TimeOfLastSceneTransition + 3.0f) {
                    yield return true;
                }

                if (networkItem.Player != session.ConnectionInfo.Slot) {
                    var sender = session.Players.GetPlayerName(networkItem.Player);
                    ShowNotification($"\"{sender}\" sehnt yoo \"{itemName}!\"", $"Rnt #A nIs\"?\"");
                    yield return true;
                }

                var handleResult = ItemPatches.GiveItem(itemName, networkItem);
                switch (handleResult) {
                    case ItemPatches.ItemResult.Success:
                        Logger.LogInfo("Recieved " + itemDisplayName + " from " + session.Players.GetPlayerName(networkItem.Player));

                        incomingItems.TryDequeue(out _);
                        SaveFile.SetInt($"randomizer processed item index {pendingItem.index}", 1);

                        // Wait for all interactions to finish
                        while (
                            GenericMessage.instance.isActiveAndEnabled ||
                            GenericPrompt.instance.isActiveAndEnabled ||
                            ItemPresentation.instance.isActiveAndEnabled ||
                            PageDisplay.instance.isActiveAndEnabled ||
                            NPCDialogue.instance.isActiveAndEnabled || 
                            PlayerCharacter.InstanceIsDead) {
                            yield return true;
                        }

                        // Pause before processing next item
                        DateTime postInteractionStart = DateTime.Now;
                        while (DateTime.Now < postInteractionStart + TimeSpan.FromSeconds(2.5)) {
                            yield return true;
                        }

                        break;

                    case ItemPatches.ItemResult.TemporaryFailure:
                        Logger.LogDebug("Player is busy, will retry processing item: " + itemDisplayName);
                        break;

                    case ItemPatches.ItemResult.PermanentFailure:
                        Logger.LogWarning("Failed to process item " + itemDisplayName);
                        incomingItems.TryDequeue(out _);
                        SaveFile.SetInt($"randomizer processed item index {pendingItem.index}", 1);
                        break;
                }

                yield return true;
            }
        }

        private IEnumerator<bool> OutgoingItemHandler() {
            while (connected) {
                if (!outgoingItems.TryDequeue(out var networkItem)) {
                    yield return true;
                    continue;
                }

                var itemName = session.Items.GetItemName(networkItem.Item);
                var location = session.Locations.GetLocationNameFromId(networkItem.Location);
                var receiver = session.Players.GetPlayerName(networkItem.Player);

                Logger.LogInfo("Sent " + itemName + " at " + location + " for " + receiver);

                if (networkItem.Player != session.ConnectionInfo.Slot) {
                    SaveFile.SetInt("archipelago items sent to other players", SaveFile.GetInt("archipelago items sent to other players")+1);
                    ShowNotification($"yoo sehnt \"{itemName.Replace("_", " ")}\" too \"{receiver}!\"", $"hOp #A lIk it!");
                } else {
                    //ShowNotification($"yoo \"FOUND {itemName}!\"", $"$oud bE yoosfuhl!");
                }
                
                yield return true;
            }
        }

        public void ActivateCheck(string LocationId) {
            var sceneName = SceneManager.GetActiveScene().name;
            
            if (LocationId != null) {
                Logger.LogInfo("Checked location " + LocationId);
                var location = this.session.Locations.GetLocationIdFromName(this.session.ConnectionInfo.Game, LocationId);

                session.Locations.CompleteLocationChecks(location);

                string GameObjectId = Locations.LocationDescriptionToId[LocationId];
                SaveFile.SetInt(ItemPatches.SaveFileCollectedKey + GameObjectId, 1);
                Locations.CheckedLocations[GameObjectId] = true;
                if (GameObject.Find($"fairy target {GameObjectId}")) {
                    GameObject.Destroy(GameObject.Find($"fairy target {GameObjectId}"));
                }
                if (Locations.VanillaLocations.Keys.Where(key => Locations.VanillaLocations[key].Location.SceneName == SceneLoaderPatches.SceneName && !Locations.CheckedLocations[key]).ToList().Count == 0) {
                    FairyTargets.CreateLoadZoneTargets();
                }
                session.Locations.ScoutLocationsAsync(location)
                    .ContinueWith(locationInfoPacket =>
                        outgoingItems.Enqueue(locationInfoPacket.Result.Locations[0]));

            } else {
                Logger.LogWarning(
                    "Failed to get unique name for check " + LocationId);

                ShowNotification($"\"Unknown Check: {LocationId}\"", $"\"Please file a bug!\"");
            }
        }

        public void SendCompletion() { 
            StatusUpdatePacket statusUpdatePacket = new StatusUpdatePacket();
            statusUpdatePacket.Status = ArchipelagoClientState.ClientGoal;
            session.Socket.SendPacket(statusUpdatePacket);
        }

        public void EnableDeathLink() {
            if (deathLinkService == null) {
                Logger.LogWarning("Cannot enable death link service as it is null.");
            }
            
            Logger.LogInfo("Enabled death link service");
            deathLinkService.EnableDeathLink();
        }

        public void DisableDeathLink() {
            if (deathLinkService == null) {
                Logger.LogWarning("Cannot disable death link service as it is null.");
            }

            Logger.LogInfo("Disabled death link service");
            deathLinkService.DisableDeathLink();
        }

        public void SendDeathLink() {
            string Player = TunicArchipelago.Settings.ConnectionSettings.Player;
            string AreaDiedIn = "";
            if (DeathLinkMessages.Causes.ContainsKey(SceneLoaderPatches.SceneName)) {
                AreaDiedIn = SceneLoaderPatches.SceneName;
            } else {
                foreach (string key in Locations.MainAreasToSubAreas.Keys) {
                    if (Locations.MainAreasToSubAreas[key].Contains(SceneLoaderPatches.SceneName)) {
                        AreaDiedIn = key;
                        break;
                    }
                }
            }
            if (AreaDiedIn == "") {
                AreaDiedIn = "Generic";
            }

            deathLinkService.SendDeathLink(new DeathLink(Player, $"{Player}{DeathLinkMessages.Causes[AreaDiedIn][new System.Random().Next(DeathLinkMessages.Causes[AreaDiedIn].Count)]}"));
        }

        public void ShowNotification(string topLine, string bottomLine) {
            var topLineObject = ScriptableObject.CreateInstance<LanguageLine>();
            topLineObject.text = topLine;

            var bottomLineObject = ScriptableObject.CreateInstance<LanguageLine>();
            bottomLineObject.text = bottomLine;

            var areaData = ScriptableObject.CreateInstance<AreaData>();
            areaData.topLine = topLineObject;
            areaData.bottomLine = bottomLineObject;

            AreaLabel.ShowLabel(areaData);
        }

    }
}
