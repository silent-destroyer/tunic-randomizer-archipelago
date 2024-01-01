using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TunicArchipelago {

    public class RandomizerSettings {

        public ConnectionSettings ConnectionSettings { get; set; }

        // Archipelago Settings
        public bool DeathLinkEnabled {
            get;
            set;
        }

        public bool CollectReflectsInWorld {
            get;
            set;
        }

        public bool SkipItemAnimations {
            get;
            set;
        }

        public bool SendHintsToServer {
            get;
            set;
        }

        // Hint Settings
        public bool HeroPathHintsEnabled {
            get;
            set;
        }

        public bool GhostFoxHintsEnabled {
            get;
            set;
        }

        public bool ShowItemsEnabled {
            get;
            set;
        }

        public bool ChestsMatchContentsEnabled {
            get;
            set;
        }

        // Gameplay Settings
        public bool HeirAssistModeEnabled {
            get;
            set;
        }

        public bool CheaperShopItemsEnabled {
            get;
            set;
        }

        public bool BonusStatUpgradesEnabled {
            get;
            set;
        }

        public bool DisableChestInterruption {
            get;
            set;
        }

        public bool FasterUpgrades {
            get;
            set;
        }

        public bool MoreSkulls {
            get;
            set;
        }

        public bool ArachnophobiaMode {
            get;
            set;
        }

        // Enemy Randomization Settings
        public bool EnemyRandomizerEnabled {
            get;
            set;
        }

        public EnemyRandomizationType EnemyDifficulty {
            get;
            set;
        }

        public EnemyGenerationType EnemyGeneration {
            get;
            set;
        }

        public bool ExtraEnemiesEnabled {
            get;
            set;
        }


        // Fox Settings
        public bool RandomFoxColorsEnabled {
            get;
            set;
        }

        public bool RealestAlwaysOn {
            get;
            set;
        }

        public bool UseCustomTexture {
            get;
            set;
        }

        public enum FoolTrapOption {
            NONE,
            NORMAL,
            DOUBLE,
            ONSLAUGHT,
        }

        public enum EnemyGenerationType {
            RANDOM,
            SEEDED
        }

        public enum EnemyRandomizationType {
            RANDOM,
            BALANCED
        }

        public RandomizerSettings() {

            ConnectionSettings = new ConnectionSettings();
/*            GameMode = GameModes.RANDOMIZER;
            KeysBehindBosses = false;
            SwordProgressionEnabled = true;
            StartWithSwordEnabled = false;
            ShuffleAbilities = false;*/
            DeathLinkEnabled = false;
            CollectReflectsInWorld = false;
            SkipItemAnimations = false;
            SendHintsToServer = false;

            HeroPathHintsEnabled = true;
            GhostFoxHintsEnabled = true;
            ShowItemsEnabled = true;
            ChestsMatchContentsEnabled = true;

            HeirAssistModeEnabled = false;
            CheaperShopItemsEnabled = true;
            BonusStatUpgradesEnabled = true;
            DisableChestInterruption = false;
            FasterUpgrades = false;
            MoreSkulls = false;
            ArachnophobiaMode = false;

            EnemyRandomizerEnabled = false;
            EnemyDifficulty = EnemyRandomizationType.BALANCED;
            EnemyGeneration = EnemyGenerationType.SEEDED;
            ExtraEnemiesEnabled = false;

            RandomFoxColorsEnabled = true;
            RealestAlwaysOn = false;
            UseCustomTexture = false;
        }

        public RandomizerSettings(bool hintsEnabled, bool randomFoxColorsEnabled) {
            HeroPathHintsEnabled = hintsEnabled;
            RandomFoxColorsEnabled = randomFoxColorsEnabled;
        }

        public RandomizerSettings(bool hintsEnabled, bool randomFoxColorsEnabled, bool heirAssistEnaled) {
            HeroPathHintsEnabled = hintsEnabled;
            RandomFoxColorsEnabled = randomFoxColorsEnabled;
            HeirAssistModeEnabled = heirAssistEnaled;
        }
    }
}
