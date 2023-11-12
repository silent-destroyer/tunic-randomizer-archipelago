# Tunic Randomizer Archipelago Client
This is the Archipelago version of the TUNIC Randomizer mod. If you are looking for the standalone, single-player version of the mod, you can find it [here](https://github.com/silent-destroyer/tunic-randomizer).

This mod features randomization of every item in the game through Archipelago, as well as enemy randomization, enhanced fox customization, custom items, and much, much more!

For questions, feedback, or discussion related to the randomizer, please visit the dedicated randomizer channel in the [Tunic Speedrunning Discord](https://discord.gg/HXkztJgQWj)! 

The Archipelago version of the TUNIC Randomizer is still very much a work-in-progress, so expect bugs to happen and things to not work correctly. The quickest way to report bugs is to message me (silentdestroyer) directly on Discord.

## Installation
- Must use a compatible PC version of TUNIC on the latest update. The mod has been tested on Steam and PC Game Pass versions, but should realistically work on any PC version (including Steam Deck).
    - If playing on Steam Deck, first follow this guide to [setting up BepInEx on Steam Deck via Proton](https://docs.bepinex.dev/articles/advanced/proton_wine.html).

- Download the correct build/version of BepInEx from here: https://builds.bepinex.dev/projects/bepinex_be/572/BepInEx_UnityIL2CPP_x64_9c2b17f_6.0.0-be.572.zip, or alternatively search for it yourself by going to https://builds.bepinex.dev/projects/bepinex_be, finding <b>Artifact #572</b>, and downloading the <b>"BepInEx Unity IL2CPP for Windows (x64) games"</b> build. 

  ![image](https://user-images.githubusercontent.com/110704408/188519149-d9476aa9-55f6-4f38-9ce9-93d137fa71af.png)
- Extract the zip folder you downloaded from the previous step into your game's install directory (For example: C:\Program Files (x86)\Steam\steamapps\common\TUNIC)
  - For the PC Game Pass version, extract the zip into the `Content` folder, i.e. C:\XboxGames\Tunic\Content
- Launch the game and close it. This will finalize the BepInEx installation.
- [Download and extract the TunicArchipelago.zip file from the latest release.](https://github.com/silent-destroyer/tunic-randomizer-archipelago/releases/latest)
  - Copy the `Tunic Archipelago` folder from the release zip into BepInEx/plugins under your game's install directory.
    - This folder contains an `ArchipelagoSettings.json` file, which is where you will fill in your PlayerName, Hostname, Port, and Password (if needed).
  - The release download contains two files called `tunic.apworld` and `tunic.yaml`. These are not needed for the mod itself but are used in the next section for setting up a multiworld game. See "Generating a Multiworld".
- Launch the game again and you should see `Randomizer + Archipelago Mod Ver. x.y.z` on the top left of the title screen!
- To uninstall the mod, either remove/delete the `Tunic Archipelago` folder from the plugins folder or rename the winhttp.dll file located in the game's root directory (this will disable all installed mods from running).


## Generating a Multiworld
- In order to setup a multiworld you must first install the latest release of [Archipelago](https://github.com/ArchipelagoMW/Archipelago/releases/latest).
  - When running the Archipelago Setup exe, you'll at least want to install the Generator and Text Client, but can install any other components you may need as well for other games.
- After installing, run `Archipelago Launcher` and click on `Browse Files`.
  - This will open the local file directory for your Archipelago installation.
- Place `tunic.yaml` from the TunicArchipelago release inside of the `Players` folder.
  - `tunic.yaml` needs to be configured before you can generate a game. Open the file in a text editor and fill in your player name and choose the settings you want.
  - You will also need to get the .yaml files for everyone who is joining your multiworld and place them inside of the `Players` folder as well.
- Place `tunic.apworld` from the TunicArchipelago release inside of `lib/worlds`.
- Once all of the .yaml fies have been configured and placed into `Players`, click `Generate` in the Archipelago Launcher.
- A .zip file will be created in the `output` folder containing the information for your multiworld. In order to host your game, go to [https://archipelago.gg/uploads](https://archipelago.gg/uploads) and upload the .zip file, then click `Create New Room`.
- Configure your `ArchipelagoSettings.json` from before with your player name and the hostname/port you see in the newly created room (Ex: archipelago.gg:32517, the hostname is `archipelago.gg` and the port is `32517`).
- All that's left is to launch the game and, if it says `Connected`, start a New Game and being your game!
- For more information, see the official [Archipelago Setup Guide](https://archipelago.gg/tutorial/Archipelago/setup/en).

## Helpful Tips
- Regarding Logic:
  - West Garden is logically accessible with either the Lantern or Hero's Laurels.
  - The Eastern Vault Fortress is accessible with either the Lantern (and Prayer) or Hero's Laurels (with or without Prayer).
  - The Library is accessible with either the Hero's Laurels or the Magic Orb.
  - The Cathedral is accessible during the day by using the Hero's Laurels to access and activate the Overworld fuse near the Swamp entrance.
  - The Scavenger's Mask is in logic to not be shuffled into the lower areas of Quarry or within the Ziggurat.
  - The Holy Cross chest from the dancing fox ghost in East Forest can be spawned in during the daytime. 
  - The Secret Legend chest in the Cathedral can similarly be obtained during the day by opening the Holy Cross door from the outside.
  - Nighttime is not considered in logic. Every check in the game is logically obtainable before the first visit to The Heir, and since nighttime restricts access to several locations it would limit the amount of options overall if certain items were required to always be obtainable at night. The game will automatically switch back to daytime if you die to The Heir on the first visit, but you can optionally switch to nighttime at any point after that by interacting with a special object in the Old House.
  - For Entrance Rando:
    - You can use the Torch to return to the Overworld save point at any time.
    - The dancing ghost fox area in East Forest can be accessed using the Hero's Laurels from the gate entrance of Guard House 1.
    - Using the Hero's Laurels, you can dash through the gate in the Bottom of the Well boss room and Monastery.
    - Activating a fuse to turn on a Prayer teleporter also activates its counterpart in the Far Shore.
    - The West Garden fuse can be activated from below.
    - You can reach the main part of Ruined Atoll from the lower entrance area using the Hero's Laurels.
    - The elevators in Ziggurat only go down.
    - The portal in the trophy room in the back of the Old House is active from the start.
    - The elevator in Cathedral is immediately accessible without activating the fuse. Activating the fuse does nothing.
- Regarding Hints:
  - The Mailbox will hint the general location of the Lantern.
  - The Hero's Grave in the Swamp, Monastery, and Library will hint the location of the three Hexagon keys.
  - The Hero's Grave in East Forest, West Garden, and the Eastern Vault Fortress are "Path of the Hero" hints, which hint towards a major progression item, such as Grapple, Lantern, Fire Wand, Ice Dagger, and/or the Prayer/Holy Cross pages (if playing with Ability Shuffling).
  - The statue in the Sealed Temple will always hint the general location of the Hero's Laurels.
- The "Fairy Seeker" Holy Cross spell (ULURDR) can now be used to seek out all items in an area, instead of just fairies. If all items in an area have been found, the fairy seeker will seek out the closest load zone that has items immediately beyond it. Useful for finding missing checks in areas with lots of obscured or hidden items!
- Save files created by the randomizer will be marked with a "randomizer" tag in the file select screen to help differentiate them from your vanilla save files while the mod is loaded.
- The Randomizer will routinely write to a couple of files in the Randomizer folder under the game's AppData directory (typically C:\Users\You\AppData\LocalLow\Andrew Shouldice\Secret Legend\Randomizer):
  - Spoiler.log - This file lists every check in the game and what item they contain. It will also mark off which checks you have collected.
  - ItemTracker.json - This file contains information such as the current seed number, what important items have been obtained, and a running list of all checks that have been found. Can be useful for creating external programs that interface with the randomizer, such as this [Tunic Rando Tracker](https://github.com/radicoon/tunic-rando-tracker) by Radicoon.
- Custom seeds can be set on the title screen before starting a New Game, or can be changed by modifying the seed value in the save file created after starting a New Game. Pressing '2' on keyboard while in-game will display the current seed and settings being used.
- The Randomizer will notify you on the title screen if a new update is available for the mod.
- An [EmoTracker Package](https://github.com/SapphireSapphic/TunicTracker) exists for this game with a full map tracker, created by SapphireSapphic and ScoutJD.

## Settings
With the exception of the Logic settings (which are determined in your Archipelago yaml settings), all options can be freely toggled or changed at any point while playing.
### Archipelago
- Death Link
  - Want a more chaotice experience? When a player with Death Link enabled dies, everyone else with the setting on *also* dies. Be careful!
- Auto-open !collect-ed Checks
  - With this enabled, chests and other item pickup locations that you haven't already found but were retrieved by another player via !collect will appear as open/already picked up.
  - This will also reflect the item counts on the inventory screen and on the end summary screen.
  - This setting can be toggled on/off at will, and the world will update/revert accordingly on the next scene transition.
### Logic
- Game Mode
  - Choose between a classic randomizer experience or Hexagon Quest, a separate game mode inspired by Triforce Hunt in Zelda randomizers.
    - Classic Randomizer: Find the three Hexagon Keys and defeat The Heir or Share Your Wisdom to win.
    - Hexagon Quest: 30 Gold Hexagons are shuffled into the item pool. Find 20 of them and visit The Heir to win.
- Keys Behind Bosses 
  - Choose if the three Hexagon Keys are randomly shuffled or placed behind their respective bossfight.
  - In Hexagon Quest, this option guarantees a Gold Hexagon is placed behind each of the three major bosses.
- Sword Progression
  - Replaces the stick and swords in the item pool with four Sword Upgrades that progressively level up as you find them.
  - Level 1: Stick -> Level 2: Sword -> Level 3: Librarian's Sword -> Level 4: Heir's Sword
  - The Level 3 and 4 Swords are custom swords that offer extended reach compared to the standard Sword and a free +1 to your Attack level when found.
- Start With Sword
  - The player will spawn with a Sword in the inventory on New Game. Does not count towards Sword Progression.
- Shuffle Abilities
  - Locks the ability to use Prayer, most Holy Cross codes*, and the Ice Rod combo technique until the respective manual page for each ability is found.
  - Prayer is unlocked by Page 24, Holy Cross is unlocked by Page 43, and Ice Rod is unlocked by Page 53.
  - *This option only locks Holy Cross codes that block access to checks in the randomizer. The free bomb codes and other player-facing codes like Big Head Mode, Sunglasses, Fairy Seeker, etc. are still usable from the start.
  - This option does not currently apply when playing Hexagon Quest, as all pages are given from the start in that mode.
### Hints
- Path of the Hero
  - Places a major hint at specific locations around the world, including the Mailbox, the Hero Graves, and the statue in the Sealed Temple. These hint towards major progression items, such as Magic Items, Laurels, Hexagons, and more.
- Ghost Foxes
  - Spawns 15 Ghost Fox NPCs around the world that give minor hints. These hints include the locations of useful non-progression items, items in hard-to-reach locations, and barren locations.
  - There are over 50 unique Ghost Fox spawns, all with their own custom Trunic dialogue!
- Freestanding Items and Chests Match Contents
  - All freestanding items (Item Pickups, Page Pickups, Shop Items, Hero's Grave Relics, etc.) will have their model swapped to appear as the item they are randomized as.
  - Chest textures will be swapped to indicate what item is in them. Currently, the items with different chest textures are Fairies, Golden Trophies, the three Hexagons, and the Hero's Laurels.
### General
- Easier Heir Fight
  - Attacks deal additional damage to The Heir based on the total number of checks found.
- Cheaper Shop Items
  - Reduces the cost of the four randomized Shop items from 1000 to 300.
- Bonus Upgrades
  - Makes the Golden Trophy items give free Level Ups for certain stats, allowing you to get up to +8 in every stat in a single playthrough when combined with the regular stat upgrades and the +2 Attack levels from Sword Progression.
  - Note: Bonus upgrades will not be retroactively awarded if this setting is turned on after obtaining Golden Trophies with it disabled. The +2 Attack Levels from Sword Progression are always awarded and not affected by this setting.
- Disable Chest Interruption
  - Enemies will not be able to interrupt you while opening chests if this option is turned on.
- Fool Traps
  - Enables fool traps, which replace low-value money rewards when enabled and apply damage or other negative effects to the player.
  - The different options determine how many fools are present, with None/Normal/Double/Onslaught containing 0/15/32/50 fools respectively.
- ???
  - !esirprus a rof no nruT
### Enemy Randomization 
- Enemy Randomizer
  - Randomly swaps out enemies with new ones when you load into a scene. See below settings for ways to affect enemy difficulty/generation.
  - There is currently no logic for certain edge cases where grappling to an enemy may be required to reach a check (ex. East Forest Slime, Turret in Overworld/Frog's Domain). The Enemy Randomizer can be toggled on or off at any point while playing however, so if you find yourself unable to reach certain checks it is recommended to briefly turn the setting off to get the check and then turn it back on afterwards.
  - Enemy Randomization is still a work-in-progress feature and may randomly stop working or cause the game to stutter/lag after a while. If either of these situations occur, restarting the game should temporarily fix the issue.
- Extra Enemies
  - Enables certain NG+ and nighttime enemy slots to add more enemies into the mix for increased chaos.
- Enemy Difficulty
  - Random: No balancing is performed, and all new enemy spawns are chosen randomly from the full pool of enemies.
  - Balanced: Enemies will only be replaced with an enemy of similar difficulty.
- Enemy Generation
  - Random: Enemies will change every time you leave and come back to an area.
  - Seeded: Enemy spawns will remain consistent per area.
### Fox Customization 🦊
- Random Fox Colors
  - Fox colors will randomize everytime you enter a new scene, rest at a shrine, or load from the menu.
  - Pressing '5' on keyboard will randomize fox colors on demand.
- Keepin' It real
  - Toggles Sunglasses on/off without needing to use the Holy Cross code.
- Fox Color Editor
  - Opens an in-depth fox palette editor that allows you to create/save/load a custom color palette texture for the fox.
- Use Custom Texture
  - When enabled, will always apply the saved custom texture to the fox. Note: Random Fox Colors should be disabled when using this setting or it may not work properly.
  - The custom texture can be found under AppData in the same folder as the Spoiler log and Item Tracker file.

## Custom Items
### Dath Stone
![image](https://github.com/silent-destroyer/tunic-randomizer/assets/110704408/a5797e96-66c6-4abd-ba94-ca61019a79d8)
- This item combines two unused items in the game into one and allows you to warp back to the last statue you saved at when used.
- What does Dath mean? The trunic characters on the item icon were presumably meant to spell "Dash", but instead they spell out "Dath", so the community has dubbed this item the "Dath Stone".

### Librarian and Heir Swords
![image](https://github.com/silent-destroyer/tunic-randomizer/assets/110704408/412935ea-b5ab-4d96-b181-678a39025901) ![image](https://github.com/silent-destroyer/tunic-randomizer/assets/110704408/92e6b5d3-1d30-49ff-a344-1d8295e593dd)
- These familiar swords are now usable in the game as part of the Sword Progression mode!
- The Librarian Sword (Level 3) offers additional reach over the base sword, as well as a free +1 to your Attack stat level.
- the Heir Sword (Level 4) offers slightly more range than the Librarian Sword, another free +1 Attack, and also has the added bonus of ignoring collision with metalllic objecs, including enemy shields.

## Golden Hexagons
![image](https://github.com/silent-destroyer/tunic-randomizer/assets/110704408/361efdaa-1849-44ee-9b2c-504d7f05c46e)
- These appear when playing the Hexagon Quest mode. Finding 20 of them allows you to end the game by visiting The Heir.

## ???
![image](https://github.com/silent-destroyer/tunic-randomizer/assets/110704408/ed015b2c-7b93-4d23-8728-b36b5cba36d1)
- ???
  
## Credits
- Glace and RisingStar111 for helping research how to mod this game, and Jabberrock for creating an initial Archipelago integration.
- Radicoon, kingsamps0n, Landie, JimTheEternal, SapphireSapphic, ScoutJD, Scipio, and many others for playtesting and helping to improve the mod.
- Andrew Shouldice, Kevin Regamey, Finji, and everyone else involved in making this wonderful game.
