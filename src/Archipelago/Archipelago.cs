using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BepInEx.Logging;
using Archipelago.MultiClient.Net.Enums;
using Archipelago.MultiClient.Net;
using Archipelago.MultiClient.Net.Models;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace TunicArchipelago {
    public class Archipelago : MonoBehaviour {
        public static Archipelago instance { get; set; }

        public ArchipelagoIntegration integration;

        public void Start() {
            this.integration = new ArchipelagoIntegration();
        }

        public void Update() {
            this.integration.Update();
        }

        public void OnDestroy() {
            this.integration.TryDisconnect();
        }

        public void Connect() {
            this.integration.TryConnect();
        }

        public void Disconnect() {
            this.integration.TryDisconnect();
        }

        public void ActivateCheck(string LocationId) {
            this.integration.ActivateCheck(LocationId);
        }

        public Dictionary<string, object> GetPlayerSlotData() {
            return integration.slotData;
        }

        public int GetPlayerSlot() {
            return integration.session.ConnectionInfo.Slot;
        }

        public string GetPlayerName(int Slot) {
            return integration.session.Players.GetPlayerName(Slot);
        }

        public string GetPlayerGame(int Slot) {
            return integration.session.Players.Players[0][Slot].Game;
        }

        public string GetItemName(long id) {
            return integration.session.Items.GetItemName(id);
        }

        public string GetLocationName(long id) { 
            return integration.session.Locations.GetLocationNameFromId(id);
        }

    }
}
