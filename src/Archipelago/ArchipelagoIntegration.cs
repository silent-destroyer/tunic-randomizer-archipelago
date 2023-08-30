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

namespace TunicArchipelago {
    public class ArchipelagoIntegration {

        private ManualLogSource Logger = TunicArchipelago.Logger;
        
        public bool connected;

        public ArchipelagoSession session;
        private CancellationTokenSource cancellationTokenSource;
        private IEnumerator<bool> processIncomingItemsStateMachine;
        private IEnumerator<bool> processOutgoingItemsStateMachine;
        public int lastProcessedItemIndex = -1;
        private ConcurrentQueue<(NetworkItem NetworkItem, int ItemIndex)> incomingItems;
        private ConcurrentQueue<NetworkItem> outgoingItems;
        private DeathLinkService deathLinkService;
        public Dictionary<string, object> slotData;

        public void Update() {
            if (!connected) {
                return;
            }
            if (SceneManager.GetActiveScene().name != "TitleScreen") {
                if (processIncomingItemsStateMachine != null) {
                    processIncomingItemsStateMachine.MoveNext();
                }

                if (processOutgoingItemsStateMachine != null) {
                    processOutgoingItemsStateMachine.MoveNext();
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
            cancellationTokenSource = new CancellationTokenSource();
            processIncomingItemsStateMachine = this.ProcessIncomingItemsStateMachine();
            processOutgoingItemsStateMachine = this.ProcessOutgoingItemsStateMachine();
            incomingItems = new ConcurrentQueue<(NetworkItem NetworkItem, int Index)>();
            outgoingItems = new ConcurrentQueue<NetworkItem>();

            TunicArchipelago.Tracker = new ItemTracker();

            try {
                LoginResult = session.TryConnectAndLogin("Tunic", TunicArchipelago.Settings.ConnectionSettings.Player, ItemsHandlingFlags.AllItems, requestSlotData: true, password: TunicArchipelago.Settings.ConnectionSettings.Password);
            } catch (Exception e) {
                LoginResult = new LoginFailure(e.GetBaseException().Message);
            }
            
            session.Items.ItemReceived += (ReceivedItemsHelper) => {
                NetworkItem Item = ReceivedItemsHelper.DequeueItem();
                string ItemReceivedName = ReceivedItemsHelper.GetItemName(Item.Item);
                int ItemIndex = ReceivedItemsHelper.Index;
                incomingItems.Enqueue((Item, ItemIndex));
            };

            if (LoginResult is LoginSuccessful LoginSuccess) {

                slotData = LoginSuccess.SlotData;

                connected = true;
                Logger.LogInfo("Successfully connected to Archipelago Multiworld server!");

                deathLinkService = session.CreateDeathLinkService();

                deathLinkService.OnDeathLinkReceived += (deathLinkObject) => {
                    Logger.LogInfo("Death link received");
                    SaveFile.SetInt("hp_lost", 1000);
                    PlayerCharacter.instance.gameObject.GetComponent<FireController>().FireAmount = 3;
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

                if (cancellationTokenSource != null) {
                    cancellationTokenSource.Cancel();
                }

                if (session != null) {
                    session.Socket.DisconnectAsync();
                    session = null;
                }

                connected = false;
                cancellationTokenSource = null;
                processIncomingItemsStateMachine = null;
                processOutgoingItemsStateMachine = null;
                lastProcessedItemIndex = -1;
                incomingItems = new ConcurrentQueue<(NetworkItem NetworkItem, int ItemIndex)>();
                outgoingItems = new ConcurrentQueue<NetworkItem>();
                deathLinkService = null;
                slotData = null;

                Locations.CheckedLocations.Clear();
                ItemLookup.ItemList.Clear();

                Logger.LogInfo("Disconnected from Archipelago");
            } catch (Exception e) {
                Logger.LogInfo("Encountered an error disconnecting from Archipelago!");
            }
        }

        private IEnumerator<bool> ProcessIncomingItemsStateMachine() {
            while (!cancellationTokenSource.IsCancellationRequested) {

                while (SceneManager.GetActiveScene().name == "TitleScreen" || SceneLoaderPatches.SceneName == "TitleScreen") {
                    yield return true;
                }

                if (!incomingItems.TryPeek(out var pendingItem)) {
                    yield return true;
                    continue;
                }

                var networkItem = pendingItem.NetworkItem;
                var itemName = session.Items.GetItemName(networkItem.Item);
                var itemDisplayName = itemName + " (" + networkItem.Item + ") at index " + pendingItem.ItemIndex;

                if (SaveFile.GetInt($"randomizer processed item index {pendingItem.ItemIndex}") == 1) {
                    incomingItems.TryDequeue(out _);
                    TunicArchipelago.Tracker.SetCollectedItem(itemName, false);
                    yield return true;
                    continue;
                }

                //ItemTracker.SaveTrackerFile();

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
                        lastProcessedItemIndex = pendingItem.ItemIndex;
                        SaveFile.SetInt("randomizer last processed item index", lastProcessedItemIndex);
                        SaveFile.SetInt($"randomizer processed item index {pendingItem.ItemIndex}", 1);
/*                        // Wait for animation to finish
                        var preInteractionStart = DateTime.Now;
                        while (DateTime.Now < preInteractionStart + TimeSpan.FromSeconds(1.0)) {
                            yield return true;
                        }*/

                        // Wait for all interactions to finish
                        while (
                            GenericMessage.instance.isActiveAndEnabled ||
                            GenericPrompt.instance.isActiveAndEnabled ||
                            ItemPresentation.instance.isActiveAndEnabled ||
                            PageDisplay.instance.isActiveAndEnabled ||
                            NPCDialogue.instance.isActiveAndEnabled || PlayerCharacter.InstanceIsDead) {
                            yield return true;
                        }

                        // Pause before processing next item
                        var postInteractionStart = DateTime.Now;
                        while (DateTime.Now < postInteractionStart + TimeSpan.FromSeconds(2.5)) {
                            yield return true;
                        }

                        break;

                    case ItemPatches.ItemResult.TemporaryFailure:
                        Logger.LogDebug("Will retrying processing item " + itemDisplayName);
                        break;

                    case ItemPatches.ItemResult.PermanentFailure:
                        Logger.LogWarning("Failed to process item " + itemDisplayName);
                        incomingItems.TryDequeue(out _);
                        lastProcessedItemIndex = pendingItem.ItemIndex;
                        SaveFile.SetInt("randomizer last processed item index", lastProcessedItemIndex);
                        break;
                }

                yield return true;
            }
        }

        private IEnumerator<bool> ProcessOutgoingItemsStateMachine() {
            while (!cancellationTokenSource.IsCancellationRequested) {
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
                Logger.LogDebug("Cannot enable death link service as it is null.");
            }
            
            Logger.LogDebug("Enabled death link service");
            deathLinkService.EnableDeathLink();
        }

        public void DisableDeathLink() {
            if (deathLinkService != null) {
                Logger.LogDebug("Cannot disable death link service as it is null.");
            }

            Logger.LogDebug("Disabled death link service");
            deathLinkService.DisableDeathLink();
        }

        public void SendDeathLink() {
            string Player = TunicArchipelago.Settings.ConnectionSettings.Player;
/*            List<string> Causes = new List<string>() {
                $"{Player} was chomped by Terry.",
                $"{Player} died to Siege Engine.",
                $"{Player} died to The Librarian.",
                $"{Player} died to Boss Scavenger.",
                $"{Player} died to The Heir.",
                $"{Player} didn't share their wisdom.",
                $"{Player}'s prayer went unheard.",
                $"{Player}'s HP fell to 0.",
                $"{Player} blew themself up.",
                $"{Player} died to frogs.",
                $"{Player} withered away from miasma.",
            };*/
            deathLinkService.SendDeathLink(new DeathLink(TunicArchipelago.Settings.ConnectionSettings.Player, "Was chomped by Terry."));
        }

        private void ShowNotification(string topLine, string bottomLine) {
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
