/// @author Rampastring
/// @version 28. 12. 2014
/// http://www.moddb.com/members/rampastring

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Drawing;
using ClientCore;
using ClientCore.cncnet5;

namespace ClientGUI
{
    /// <summary>
    /// A static class holding UI-related functions useful for both the Skirmish and the CnCNet Game lobby.
    /// </summary>
    static class SharedUILogic
    {
        const int COOP_BRIEFING_WIDTH = 488;
        const int COOP_BRIEFING_HEIGHT = 200;

        /// <summary>
        /// Gathers the list of allowed sides.
        /// </summary>
        /// <param name="comboBoxes">ComboBoxes of the user interface.</param>
        /// <param name="sideComboboxPrerequisites">SideComboboxPrerequisites.</param>
        /// <returns>A list of allowed side indexes.</returns>
        public static List<int> getAllowedSides(List<LimitedComboBox> comboBoxes,
            List<SideComboboxPrerequisite> sideComboboxPrerequisites)
        {
            // Check which sides are pickable (prevent Allies and Soviet in Classic Mode)
            List<int> AllowedSidesToRandomizeTo = new List<int>();
            for (int rSideId = 0; rSideId < sideComboboxPrerequisites.Count; rSideId++)
            {
                SideComboboxPrerequisite prereq = sideComboboxPrerequisites[rSideId];
                if (prereq.Valid)
                {
                    int cmbId = prereq.ComboBoxId;
                    int indexId = prereq.RequiredIndexId;

                    if (comboBoxes[cmbId].SelectedIndex == indexId)
                        AllowedSidesToRandomizeTo.Add(rSideId);
                }
                else
                    AllowedSidesToRandomizeTo.Add(rSideId);
            }

            return AllowedSidesToRandomizeTo;
        }

        /// <summary>
        /// Randomizes player options.
        /// </summary>
        /// <param name="players">List of human players in the game.</param>
        /// <param name="aiPlayers">List of AI players in the game.</param>
        /// <param name="map">The map.</param>
        /// <param name="seed">The seed number used for randomizing.</param>
        /// <param name="PlayerSides">List of player side indexes.</param>
        /// <param name="isPlayerSpectator">Determines whether a player at a specific index is a spectator.</param>
        /// <param name="PlayerColors">List of player color indexes.</param>
        /// <param name="PlayerStartingLocs">The list of player starting location indexes.</param>
        /// <param name="AllowedSidesToRandomizeTo">List of allowed sides.</param>
        /// <param name="sideCount">The amount of sides in the game.</param>
        public static void Randomize(List<PlayerInfo> players, List<PlayerInfo> aiPlayers, Map map, int seed,
            List<int> PlayerSides, List<bool> isPlayerSpectator, List<int> PlayerColors, List<int> PlayerStartingLocs,
            List<int> AllowedSidesToRandomizeTo, int sideCount)
        {
            Random random = new Random(seed);

            Logger.Log("Randomizing sides.");

            int previousSide = AllowedSidesToRandomizeTo[random.Next(0, AllowedSidesToRandomizeTo.Count)];

            foreach (PlayerInfo player in players)
            {
                if (player.SideId == 0)
                {
                    int side = AllowedSidesToRandomizeTo[random.Next(0, AllowedSidesToRandomizeTo.Count)];
                    PlayerSides.Add(side);
                    previousSide = side;
                    isPlayerSpectator.Add(false);
                }
                else if (player.SideId == sideCount + 1)
                {
                    PlayerSides.Add(previousSide);
                    isPlayerSpectator.Add(true);
                }
                else
                {
                    PlayerSides.Add(player.SideId - 1);
                    previousSide = player.SideId - 1;
                    isPlayerSpectator.Add(false);
                }
            }

            for (int pId = 0; pId < players.Count; pId++)
            {
                if (isPlayerSpectator[pId])
                {
                    PlayerSides[pId] = previousSide;
                }
            }

            Logger.Log("Randomizing AI sides.");

            foreach (PlayerInfo player in aiPlayers)
            {
                if (player.SideId == 0)
                {
                    PlayerSides.Add(AllowedSidesToRandomizeTo[random.Next(0, AllowedSidesToRandomizeTo.Count)]);
                    isPlayerSpectator.Add(false);
                }
                else if (player.SideId == sideCount + 1)
                {
                    PlayerSides.Add(0);
                    isPlayerSpectator.Add(true);
                }
                else
                {
                    PlayerSides.Add(player.SideId - 1);
                    isPlayerSpectator.Add(false);
                }
            }

            Logger.Log("Generated sides:");
            for (int pid = 0; pid < players.Count; pid++)
            {
                Logger.Log("PlayerID " + pid + ": sideId " + PlayerSides[pid]);
            }

            List<int> freeColors = new List<int>();
            for (int cId = 1; cId < 9; cId++)
                freeColors.Add(cId);

            for (int pId = 0; pId < players.Count; pId++)
                freeColors.Remove(players[pId].ColorId);

            for (int aiId = 0; aiId < aiPlayers.Count; aiId++)
                freeColors.Remove(aiPlayers[aiId].ColorId);

            // Randomize colors

            Logger.Log("Randomizing colors.");

            foreach (PlayerInfo player in players)
            {
                if (player.ColorId == 0 && player.ForcedColor == 0)
                {
                    int randomizedColorIndex = random.Next(0, freeColors.Count);

                    if (randomizedColorIndex > -1)
                    {
                        PlayerColors.Add(freeColors[randomizedColorIndex] - 1);
                        freeColors.RemoveAt(randomizedColorIndex);
                    }
                    else
                        throw new Exception("Unable to find valid color for player " + player.Name);
                }
                else if (player.ForcedColor > 0)
                {
                    PlayerColors.Add(player.ForcedColor);
                    Logger.Log("Forced Color for " + player.Name + ": " + player.ForcedColor);
                }
                else
                    PlayerColors.Add(player.ColorId - 1);
            }

            Logger.Log("Randomizing AI colors.");

            foreach (PlayerInfo player in aiPlayers)
            {
                if (player.ColorId == 0)
                {
                    int randomizedColorIndex = random.Next(0, freeColors.Count);

                    if (randomizedColorIndex > -1)
                    {
                        PlayerColors.Add(freeColors[randomizedColorIndex] - 1);
                        freeColors.RemoveAt(randomizedColorIndex);
                    }
                    else
                        PlayerColors.Add(0);
                }
                else
                    PlayerColors.Add(player.ColorId - 1);
            }

            List<int> freeStartingLocs = new List<int>();

            for (int sId = 1; sId <= map.AmountOfPlayers; sId++)
                freeStartingLocs.Add(sId);

            for (int pId = 0; pId < players.Count; pId++)
                freeStartingLocs.Remove(players[pId].StartingLocation);

            for (int aiId = 0; aiId < aiPlayers.Count; aiId++)
                freeStartingLocs.Remove(aiPlayers[aiId].StartingLocation);

            // Randomize starting locations

            Logger.Log("Randomizing starting locations.");

            int sLocPId = 0;

            foreach (PlayerInfo player in players)
            {
                sLocPId++;

                if (isPlayerSpectator[sLocPId - 1])
                {
                    PlayerStartingLocs.Add(9);
                    sLocPId++;
                    continue;
                }

                if (player.StartingLocation == 0)
                {
                    if (freeStartingLocs.Count > 1)
                    {
                        int index = random.Next(0, freeStartingLocs.Count);
                        PlayerStartingLocs.Add(freeStartingLocs[index] - 1);
                        freeStartingLocs.RemoveAt(index);
                    }
                    else if (freeStartingLocs.Count == 1)
                    {
                        PlayerStartingLocs.Add(freeStartingLocs[0] - 1);
                        freeStartingLocs.RemoveAt(0);
                    }
                    else // if freeStartingLocs.Count == 0
                    {
                        PlayerStartingLocs.Add(random.Next(0, map.AmountOfPlayers));
                    }
                }
                else
                    PlayerStartingLocs.Add(player.StartingLocation - 1);
            }

            Logger.Log("Randomizing AI starting locations.");

            foreach (PlayerInfo player in aiPlayers)
            {
                if (player.StartingLocation == 0)
                {
                    if (freeStartingLocs.Count > 1)
                    {
                        int index = random.Next(0, freeStartingLocs.Count);
                        PlayerStartingLocs.Add(freeStartingLocs[index] - 1);
                        freeStartingLocs.RemoveAt(index);
                    }
                    else if (freeStartingLocs.Count == 1)
                    {
                        PlayerStartingLocs.Add(freeStartingLocs[0] - 1);
                        freeStartingLocs.RemoveAt(0);
                    }
                    else // if freeStartingLocs.Count == 0
                    {
                        PlayerStartingLocs.Add(random.Next(0, map.AmountOfPlayers));
                    }
                }
                else
                    PlayerStartingLocs.Add(player.StartingLocation - 1);
            }
        }

        /// <summary>
        /// Writes spawn.ini. See NGameLobby and SkirmishLobby for usage examples.
        /// </summary>
        public static void WriteSpawnIni(List<PlayerInfo> players, List<PlayerInfo> aiPlayers, Map map, string gameMode, int seed, 
            int numberOfLoadingScreens, bool isHost, List<int> playerPorts, string tunnelAddress, int tunnelPort,
            List<LimitedComboBox> ComboBoxes, List<UserCheckBox> CheckBoxes, List<bool> IsCheckBoxReversed, List<string> AssociatedCheckBoxSpawnIniOptions, 
            List<string> AssociatedComboBoxSpawnIniOptions, List<DataWriteMode> ComboBoxDataWriteModes, 
            List<int> PlayerSides, List<bool> isPlayerSpectator, List<int> PlayerColors, List<int> PlayerStartingLocs,
            out List<int> MultiCmbIndexes)
        {
            File.Delete(ProgramConstants.gamepath + ProgramConstants.SPAWNMAP_INI);
            File.Delete(ProgramConstants.gamepath + ProgramConstants.SPAWNER_SETTINGS);

            string mapCodePath = map.Path.Substring(0, map.Path.Length - 3) + "ini";
            IniFile mapCodeIni = new IniFile(mapCodePath);

            if (map.IsCoop)
            {
                foreach (PlayerInfo pInfo in players)
                    pInfo.TeamId = 1;

                foreach (PlayerInfo pInfo in aiPlayers)
                    pInfo.TeamId = 1;
            }

            // Write spawn.ini
            StreamWriter sw = new StreamWriter(File.Create(ProgramConstants.gamepath + ProgramConstants.SPAWNER_SETTINGS));
            sw.WriteLine("[Settings]");
            sw.WriteLine("Name=" + ProgramConstants.CNCNET_PLAYERNAME);
            sw.WriteLine("Scenario=" + ProgramConstants.SPAWNMAP_INI);
            int myIndex = players.FindIndex(c => c.Name == ProgramConstants.CNCNET_PLAYERNAME);
            int sideId = PlayerSides[myIndex];
            sw.WriteLine("Side=" + sideId);
            sw.WriteLine("IsSpectator=" + isPlayerSpectator[myIndex]);
            sw.WriteLine("Color=" + PlayerColors[myIndex]);
            sw.WriteLine("CustomLoadScreen=" + LoadingScreenController.GetLoadScreenName(sideId, numberOfLoadingScreens));
            sw.WriteLine("AIPlayers=" + aiPlayers.Count);
            sw.WriteLine("Host=" + isHost);
            sw.WriteLine("Seed=" + seed);
            sw.WriteLine("GameID=9001");
            for (int chkId = 0; chkId < CheckBoxes.Count; chkId++)
            {
                string option = AssociatedCheckBoxSpawnIniOptions[chkId];
                if (option != "none")
                {
                    Logger.Log("CheckBox " + CheckBoxes[chkId].Name + " associated spawn.ini option: " + option);
                    if (!IsCheckBoxReversed[chkId])
                        sw.WriteLine(option + "=" + CheckBoxes[chkId].Checked);
                    else
                        sw.WriteLine(option + "=" + !CheckBoxes[chkId].Checked);
                }
            }
            for (int cmbId = 0; cmbId < ComboBoxes.Count; cmbId++)
            {
                DataWriteMode dwMode = ComboBoxDataWriteModes[cmbId];
                string option = AssociatedComboBoxSpawnIniOptions[cmbId];
                if (option != "none")
                {
                    Logger.Log("ComboBox " + ComboBoxes[cmbId].Name + " associated spawn.ini option: " + option);

                    if (dwMode == DataWriteMode.BOOLEAN)
                    {
                        if (ComboBoxes[cmbId].SelectedIndex > 0)
                            sw.WriteLine(option + "=Yes");
                        else
                            sw.WriteLine(option + "=No");
                    }
                    else if (dwMode == DataWriteMode.INDEX)
                    {
                        sw.WriteLine(option + "=" + ComboBoxes[cmbId].SelectedIndex);
                    }
                    else // if dwMode == DataWriteMode.String
                    {
                        sw.WriteLine(option + "=" + ComboBoxes[cmbId].SelectedItem.ToString());
                    }
                }
            }

            Logger.Log("Writing forced spawn.ini options from GameOptions.ini.");

            IniFile goIni = DomainController.Instance().gameOptions_ini;

            if (goIni.SectionExists("ForcedSpawnIniOptions"))
            {
                List<string> keys = goIni.GetSectionKeys("ForcedSpawnIniOptions");
                foreach (string key in keys)
                {
                    sw.WriteLine(key + "=" + goIni.GetStringValue("ForcedSpawnIniOptions", key, "give me a value, noob"));
                }
            }
            sw.Close();

            Logger.Log("Writing game mode forced spawn.ini options from INI\\" + gameMode + "_spawn.ini");

            IniFile spawnIni = new IniFile(ProgramConstants.gamepath + ProgramConstants.SPAWNER_SETTINGS);

            if (File.Exists(ProgramConstants.gamepath + "INI\\" + gameMode + "_spawn.ini"))
            {
                IniFile gameModeSpawnIni = new IniFile(ProgramConstants.gamepath + "INI\\" + gameMode + "_spawn.ini");
                if (gameModeSpawnIni.SectionExists("Settings"))
                {
                    List<string> keys = gameModeSpawnIni.GetSectionKeys("Settings");
                    foreach (string key in keys)
                    {
                        spawnIni.SetStringValue("Settings", key, gameModeSpawnIni.GetStringValue("Settings", key, "give me a value, noob"));
                    }
                }
                else
                    Logger.Log("WARNING: Game mode spawn.ini options file doesn't contain the Settings section. Ignoring.");
            }
            else
                Logger.Log("WARNING: Game mode spawn.ini options file doesn't exist.");

            Logger.Log("Writing forced spawn.ini options from the map settings INI file.");

            if (mapCodeIni.SectionExists("ForcedSpawnIniOptions"))
            {
                List<string> keys = mapCodeIni.GetSectionKeys("ForcedSpawnIniOptions");
                foreach (string key in keys)
                {
                    spawnIni.SetStringValue("Settings", key, mapCodeIni.GetStringValue("ForcedSpawnIniOptions", key, "No value specified!"));
                }
            }

            spawnIni.WriteIniFile();

            StreamWriter sw2 = new StreamWriter(File.OpenWrite(ProgramConstants.gamepath + ProgramConstants.SPAWNER_SETTINGS));
            sw2.BaseStream.Position = sw2.BaseStream.Length - 1;
            sw2.WriteLine();
            sw2.WriteLine();
            if (players.Count > 1)
            {
                sw2.WriteLine("Port=" + playerPorts[players.FindIndex(c => c.Name == ProgramConstants.CNCNET_PLAYERNAME)]);
                sw2.WriteLine();
                sw2.WriteLine("[Tunnel]");
                sw2.WriteLine("Ip=" + tunnelAddress);
                sw2.WriteLine("Port=" + tunnelPort);
                sw2.WriteLine();
            }

            int otherId = 1;

            for (int pId = 0; pId < players.Count; pId++)
            {
                if (players[pId].Name != ProgramConstants.CNCNET_PLAYERNAME)
                {
                    sw2.WriteLine("[Other" + otherId + "]");
                    sw2.WriteLine("Name=" + players[pId].Name);
                    sw2.WriteLine("Side=" + PlayerSides[pId]);
                    sw2.WriteLine("IsSpectator=" + isPlayerSpectator[pId]);
                    sw2.WriteLine("Color=" + PlayerColors[pId]);
                    sw2.WriteLine("Ip=0.0.0.0");
                    sw2.WriteLine("Port=" + playerPorts[pId]);
                    otherId++;
                    sw2.WriteLine();
                }
            }

            // Create the list of MultiX indexes according to color indexes
            MultiCmbIndexes = new List<int>();

            for (int cId = 0; cId < 8; cId++)
            {
                for (int pId = 0; pId < players.Count; pId++)
                {
                    if (PlayerColors[pId] == cId)
                        MultiCmbIndexes.Add(pId);
                }
            }

            // Secret color logic ;)
            for (int pId = 0; pId < players.Count; pId++)
            {
                if (PlayerColors[pId] > 7)
                    MultiCmbIndexes.Add(pId);
            }

            if (aiPlayers.Count > 0)
            {
                sw2.WriteLine("[HouseHandicaps]");
                int multiId = MultiCmbIndexes.Count + 1;

                for (int aiId = 0; aiId < aiPlayers.Count; aiId++)
                {
                    if (aiPlayers[aiId].Name.Contains("Easy"))
                        sw2.WriteLine(string.Format("Multi{0}=2", multiId));
                    else if (aiPlayers[aiId].Name.Contains("Medium"))
                        sw2.WriteLine(string.Format("Multi{0}=1", multiId));
                    else // if (aiPlayers[aiId].Name.Contains("Hard"))
                        sw2.WriteLine(string.Format("Multi{0}=0", multiId));

                    multiId++;
                }

                sw2.WriteLine();

                sw2.WriteLine("[HouseCountries]");
                multiId = MultiCmbIndexes.Count + 1;

                for (int aiId = 0; aiId < aiPlayers.Count; aiId++)
                {
                    sw2.WriteLine(string.Format("Multi{0}={1}", multiId, PlayerSides[players.Count + aiId]));
                    multiId++;
                }

                sw2.WriteLine();

                sw2.WriteLine("[HouseColors]");
                multiId = MultiCmbIndexes.Count + 1;

                for (int aiId = 0; aiId < aiPlayers.Count; aiId++)
                {
                    sw2.WriteLine(string.Format("Multi{0}={1}", multiId, PlayerColors[players.Count + aiId]));
                    multiId++;
                }
            }

            sw2.WriteLine();

            sw2.WriteLine("[IsSpectator]");
            for (int multiId = 0; multiId < MultiCmbIndexes.Count; multiId++)
            {
                int pIndex = MultiCmbIndexes[multiId];
                if (isPlayerSpectator[pIndex])
                    sw2.WriteLine(string.Format("Multi{0}=Yes", multiId + 1));
            }

            sw2.WriteLine();

            // Set alliances

            List<int> Team1MultiMemberIds = new List<int>();
            List<int> Team2MultiMemberIds = new List<int>();
            List<int> Team3MultiMemberIds = new List<int>();
            List<int> Team4MultiMemberIds = new List<int>();

            for (int pId = 0; pId < players.Count; pId++)
            {
                int teamId = players[pId].TeamId;

                if (teamId > 0)
                {
                    switch (teamId)
                    {
                        case 1:
                            Team1MultiMemberIds.Add(MultiCmbIndexes.FindIndex(c => c == pId) + 1);
                            break;
                        case 2:
                            Team2MultiMemberIds.Add(MultiCmbIndexes.FindIndex(c => c == pId) + 1);
                            break;
                        case 3:
                            Team3MultiMemberIds.Add(MultiCmbIndexes.FindIndex(c => c == pId) + 1);
                            break;
                        case 4:
                            Team4MultiMemberIds.Add(MultiCmbIndexes.FindIndex(c => c == pId) + 1);
                            break;
                    }
                }
            }

            int mId2 = MultiCmbIndexes.Count + 1;

            for (int aiId = 0; aiId < aiPlayers.Count; aiId++)
            {
                int teamId = aiPlayers[aiId].TeamId;

                if (teamId > 0)
                {
                    switch (teamId)
                    {
                        case 1:
                            Team1MultiMemberIds.Add(mId2);
                            break;
                        case 2:
                            Team2MultiMemberIds.Add(mId2);
                            break;
                        case 3:
                            Team3MultiMemberIds.Add(mId2);
                            break;
                        case 4:
                            Team4MultiMemberIds.Add(mId2);
                            break;
                    }
                }

                mId2++;
            }

            foreach (int houseId in Team1MultiMemberIds)
            {
                sw2.WriteLine("[Multi" + houseId + "_Alliances]");
                bool selfFound = false;

                for (int allyId = 0; allyId < Team1MultiMemberIds.Count; allyId++)
                {
                    int allyHouseId = Team1MultiMemberIds[allyId];

                    if (allyHouseId == houseId)
                        selfFound = true;
                    else
                    {
                        sw2.WriteLine(string.Format("HouseAlly{0}={1}", getHouseAllyIndexFromInt(allyId, selfFound), allyHouseId - 1));
                    }
                }

                sw2.WriteLine();
            }

            foreach (int houseId in Team2MultiMemberIds)
            {
                sw2.WriteLine("[Multi" + houseId + "_Alliances]");
                bool selfFound = false;

                for (int allyId = 0; allyId < Team2MultiMemberIds.Count; allyId++)
                {
                    int allyHouseId = Team2MultiMemberIds[allyId];

                    if (allyHouseId == houseId)
                        selfFound = true;
                    else
                    {
                        sw2.WriteLine(string.Format("HouseAlly{0}={1}", getHouseAllyIndexFromInt(allyId, selfFound), allyHouseId - 1));
                    }
                }

                sw2.WriteLine();
            }

            foreach (int houseId in Team3MultiMemberIds)
            {
                sw2.WriteLine("[Multi" + houseId + "_Alliances]");
                bool selfFound = false;

                for (int allyId = 0; allyId < Team3MultiMemberIds.Count; allyId++)
                {
                    int allyHouseId = Team3MultiMemberIds[allyId];

                    if (allyHouseId == houseId)
                        selfFound = true;
                    else
                    {
                        sw2.WriteLine(string.Format("HouseAlly{0}={1}", getHouseAllyIndexFromInt(allyId, selfFound), allyHouseId - 1));
                    }
                }

                sw2.WriteLine();
            }

            foreach (int houseId in Team4MultiMemberIds)
            {
                sw2.WriteLine("[Multi" + houseId + "_Alliances]");
                bool selfFound = false;

                for (int allyId = 0; allyId < Team4MultiMemberIds.Count; allyId++)
                {
                    int allyHouseId = Team4MultiMemberIds[allyId];

                    if (allyHouseId == houseId)
                        selfFound = true;
                    else
                    {
                        sw2.WriteLine(string.Format("HouseAlly{0}={1}", getHouseAllyIndexFromInt(allyId, selfFound), allyHouseId - 1));
                    }
                }

                sw2.WriteLine();
            }

            mId2 = 1;

            sw2.WriteLine("[SpawnLocations]");
            for (int pId = 0; pId < players.Count; pId++)
            {
                sw2.WriteLine(string.Format("Multi{0}={1}", mId2, PlayerStartingLocs[MultiCmbIndexes[pId]]));
                mId2++;
            }

            for (int aiId = 0; aiId < aiPlayers.Count; aiId++)
            {
                sw2.WriteLine(string.Format("Multi{0}={1}", mId2, PlayerStartingLocs[players.Count + aiId]));
                mId2++;
            }

            sw2.WriteLine();
            sw2.WriteLine();

            sw2.Close();
        }

        private static string getHouseAllyIndexFromInt(int allyId, bool selfFound)
        {
            if (selfFound)
                allyId = allyId - 1;

            switch (allyId)
            {
                case 0:
                    return "One";
                case 1:
                    return "Two";
                case 2:
                    return "Three";
                case 3:
                    return "Four";
                case 4:
                    return "Five";
                case 5:
                    return "Six";
                case 6:
                    return "Seven";
            }

            return "None" + allyId;
        }

        /// <summary>
        /// Generic preview painter for the Skirmish and CnCNet Game Lobbies.
        /// See NGameLobby and SkirmishLobby for usage examples.
        /// </summary>
        public static void PaintPreview(Map map, Rectangle imageRectangle, PaintEventArgs e,
            Font coopBriefingFont, Font playerNameFont, Color coopBriefingForeColor, bool displayCoopBriefing,
            double previewRatioY, double previewRatioX, List<string>[] playerNamesOnPlayerLocations,
            List<Color> mpColors, List<int>[] playerColorsOnPlayerLocations, Image[] startingLocationIndicators,
            Image enemyStartingLocationIndicator)
        {
            if (map.StartingLocationsX.Count > 0)
            {
                for (int pId = 0; pId < map.StartingLocationsX.Count; pId++)
                {
                    int xPos = map.StartingLocationsX[pId];
                    int yPos = map.StartingLocationsY[pId];

                    Image startingLocationIndicator;
                    if (pId < map.AmountOfPlayers)
                        startingLocationIndicator = startingLocationIndicators[pId];
                    else
                        startingLocationIndicator = enemyStartingLocationIndicator;

                    int halfWidth = startingLocationIndicator.Width / 2;
                    int halfHeight = startingLocationIndicator.Height / 2;

                    int x;
                    int y;
                    if (!map.StaticPreviewSize)
                    {
                        x = Convert.ToInt32(xPos * previewRatioX);
                        y = Convert.ToInt32(yPos * previewRatioY);
                    }
                    else
                    {
                        x = xPos;
                        y = yPos;
                    }

                    e.Graphics.DrawImage(startingLocationIndicator,
                        new Rectangle(imageRectangle.X + x - (halfWidth), imageRectangle.Y + y - (halfHeight),
                            startingLocationIndicator.Width, startingLocationIndicator.Height),
                        new Rectangle(0, 0, startingLocationIndicator.Width, startingLocationIndicator.Height),
                        GraphicsUnit.Pixel);

                    if (playerNamesOnPlayerLocations == null)
                        continue;

                    int id = 0;

                    foreach (string playerName in playerNamesOnPlayerLocations[pId])
                    {
                        float basePositionY = imageRectangle.Y + Convert.ToSingle(y) + (startingLocationIndicator.Height * id) - (halfHeight / 1.5f);

                        e.Graphics.DrawString(playerName, playerNameFont,
                            new SolidBrush(Color.Black),
                            new PointF(imageRectangle.X + Convert.ToSingle(x) + halfWidth + 4,
                            basePositionY + 1f));

                        e.Graphics.DrawString(playerName, playerNameFont,
                            new SolidBrush(mpColors[playerColorsOnPlayerLocations[pId][id]]),
                            new PointF(imageRectangle.X + Convert.ToSingle(x) + halfWidth + 3,
                                basePositionY));

                        id++;
                    }
                }
            }

            // Draw co-op mission briefing if we should do so
            if (!String.IsNullOrEmpty(map.Briefing) && displayCoopBriefing)
            {
                int briefingBoxY = (imageRectangle.Height - COOP_BRIEFING_HEIGHT) / 2;
                int briefingBoxX = (imageRectangle.Width - COOP_BRIEFING_WIDTH) / 2;

                e.Graphics.DrawRectangle(new Pen(new SolidBrush(coopBriefingForeColor)),
                    new Rectangle(imageRectangle.X + briefingBoxX, imageRectangle.Y + briefingBoxY,
                    COOP_BRIEFING_WIDTH, COOP_BRIEFING_HEIGHT));

                e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(245, 0, 0, 0)),
                    new Rectangle(imageRectangle.X + briefingBoxX + 1, imageRectangle.Y + briefingBoxY + 1,
                    COOP_BRIEFING_WIDTH - 2, COOP_BRIEFING_HEIGHT - 2));

                e.Graphics.DrawString(map.Briefing, coopBriefingFont,
                    new SolidBrush(Color.Black),
                    new PointF(imageRectangle.X + briefingBoxX + 4, imageRectangle.Y + briefingBoxY + 4));

                e.Graphics.DrawString(map.Briefing, coopBriefingFont,
                    new SolidBrush(coopBriefingForeColor),
                    new PointF(imageRectangle.X + briefingBoxX + 3, imageRectangle.Y + briefingBoxY + 3));
            }
        }

        /// <summary>
        /// Parses and applies forced game options from an INI file.
        /// </summary>
        /// <param name="lockedOptionsIni">The INI file to read and apply the forced game options from.</param>
        public static void ParseLockedOptionsFromIni(List<UserCheckBox> CheckBoxes,
            List<LimitedComboBox> ComboBoxes, IniFile lockedOptionsIni)
        {
            List<string> keys = lockedOptionsIni.GetSectionKeys("ForcedOptions");
            if (keys == null)
            {
                Logger.Log("Unable to parse ForcedOptions from forced game options ini.");
                return;
            }

            foreach (string key in keys)
            {
                UserCheckBox chkBox = CheckBoxes.Find(c => c.Name == key);

                if (chkBox != null)
                {
                    chkBox.Checked = lockedOptionsIni.GetBooleanValue("ForcedOptions", key, false);
                    chkBox.Enabled = false;
                }

                LimitedComboBox cmbBox = ComboBoxes.Find(c => c.Name == key);

                if (cmbBox != null)
                {
                    cmbBox.SelectedIndex = lockedOptionsIni.GetIntValue("ForcedOptions", key, 0);
                    cmbBox.Enabled = false;
                }
            }
        }

        /// <summary>
        /// Parses and applies various theme-related INI keys from DTACnCNetClient.ini.
        /// Enables editing attributes of individual controls in DTACnCNetClient.ini.
        /// </summary>
        public static void ParseClientThemeIni(Form form)
        {
            IniFile clientThemeIni = new IniFile(ProgramConstants.gamepath + ProgramConstants.RESOURCES_DIR + "DTACnCNetClient.ini");

            List<string> sections = clientThemeIni.GetSections();

            foreach (string section in sections)
            {
                Control[] controls = form.Controls.Find(section, true);

                if (controls.Length == 0)
                    continue;

                Control control = controls[0];

                List<string> keys = clientThemeIni.GetSectionKeys(section);

                foreach (string key in keys)
                {
                    string keyValue = clientThemeIni.GetStringValue(section, key, String.Empty);

                    switch (key)
                    {
                        case "Font":
                            control.Font = SharedLogic.getFont(keyValue);
                            break;
                        case "ForeColor":
                            control.ForeColor = getColorFromString(keyValue);
                            break;
                        case "BackColor":
                            control.BackColor = getColorFromString(keyValue);
                            break;
                        case "Size":
                            string[] sizeArray = keyValue.Split(',');
                            control.Size = new Size(Convert.ToInt32(sizeArray[0]), Convert.ToInt32(sizeArray[1]));
                            break;
                        case "Location":
                            string[] locationArray = keyValue.Split(',');
                            control.Location = new Point(Convert.ToInt32(locationArray[0]), Convert.ToInt32(locationArray[1]));
                            break;
                        case "Text":
                            control.Text = keyValue;
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// Gets a color from a RGB color string (example: 255,255,255)
        /// </summary>
        /// <param name="colorString">The color string.</param>
        /// <returns>The color.</returns>
        public static Color getColorFromString(string colorString)
        {
            string[] colorArray = colorString.Split(',');
            Color color = Color.FromArgb(Convert.ToByte(colorArray[0]), Convert.ToByte(colorArray[1]), Convert.ToByte(colorArray[2]));
            return color;
        }
    }
}
