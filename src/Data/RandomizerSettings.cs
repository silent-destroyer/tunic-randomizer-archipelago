using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TunicArchipelago {

    public class RandomizerSettings {

        public ConnectionSettings ConnectionSettings { get; set; }

        public enum RandomizerType { 
            SINGLEPLAYER,
            ARCHIPELAGO
        }

        public RandomizerType Mode {
            get;
            set;
        }

        // Single Player Settings
        public GameModes GameMode {
            get;
            set;
        }

        public bool KeysBehindBosses {
            get;
            set;
        }

        public bool StartWithSwordEnabled {
            get;
            set;
        }

        public bool SwordProgressionEnabled {
            get;
            set;
        }

        public bool ShuffleAbilities {
            get;
            set;
        }

        public bool EntranceRandoEnabled {
            get;
            set;
        }

        public bool ERFixedShop {
            get;
            set;
        }

        public int HexagonQuestGoal {
            get;
            set;
        }

        public int HexagonQuestExtraPercentage {
            get;
            set;
        }

        public bool Lanternless {
            get;
            set;
        }

        public bool Maskless {
            get;
            set;
        }

        public FixedLaurelsType FixedLaurelsOption {
            get;
            set;
        }

        public FoolTrapOption FoolTrapIntensity {
            get;
            set;
        }

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

        public bool UseTrunicTranslations {
            get;
            set;
        }

        public bool CreateSpoilerLog {
            get;
            set;
        }

        // Gameplay Settings
        public bool HeirAssistModeEnabled {
            get;
            set;
        }

        public bool ClearEarlyBushes {
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

        public enum GameModes {
            RANDOMIZER,
            HEXAGONQUEST,
            VANILLA
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

        public enum FixedLaurelsType {
            ANYWHERE,
            SIXCOINS,
            TENCOINS,
            TENFAIRIES,
        }

        public RandomizerSettings() {

            ConnectionSettings = new ConnectionSettings();
            Mode = RandomizerType.SINGLEPLAYER;

            // Single Player
            GameMode = GameModes.RANDOMIZER;
            KeysBehindBosses = false;
            SwordProgressionEnabled = true;
            StartWithSwordEnabled = false;
            ShuffleAbilities = false;
            EntranceRandoEnabled = false;
            ERFixedShop = false;
            HexagonQuestGoal = 20;
            HexagonQuestExtraPercentage = 50;
            FixedLaurelsOption = FixedLaurelsType.ANYWHERE;
            FoolTrapIntensity = FoolTrapOption.NORMAL;
            Lanternless = false;
            Maskless = false;

            // Archipelago 
            DeathLinkEnabled = false;
            CollectReflectsInWorld = false;
            SkipItemAnimations = false;
            SendHintsToServer = false;

            // Hints
            HeroPathHintsEnabled = true;
            GhostFoxHintsEnabled = true;
            ShowItemsEnabled = true;
            ChestsMatchContentsEnabled = true;
            UseTrunicTranslations = false;
            CreateSpoilerLog = true;

            // General
            HeirAssistModeEnabled = false;
            ClearEarlyBushes = false;
            CheaperShopItemsEnabled = true;
            BonusStatUpgradesEnabled = true;
            DisableChestInterruption = false;
            FasterUpgrades = false;
            MoreSkulls = false;
            ArachnophobiaMode = false;

            // Enemy Randomizer
            EnemyRandomizerEnabled = false;
            EnemyDifficulty = EnemyRandomizationType.BALANCED;
            EnemyGeneration = EnemyGenerationType.SEEDED;
            ExtraEnemiesEnabled = false;

            // Fox Customization
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
