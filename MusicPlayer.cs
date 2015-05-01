using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WMPLib;
using ClientCore;

namespace ClientGUI
{
    /// <summary>
    /// A wrapper around Windows Media Player so the client doesn't crash
    /// when one doesn't have WMP installed.
    /// </summary>
    public class MusicPlayer
    {
        public MusicPlayer()
        {
        }

        WindowsMediaPlayer wmPlayer;

        public string Initialize()
        {
            try
            {
                wmPlayer = new WindowsMediaPlayer();
                wmPlayer.URL = ProgramConstants.gamepath + ProgramConstants.RESOURCES_DIR + "lobbymusic.wav";
                wmPlayer.settings.setMode("loop", true);
                if (DomainController.Instance().getMainMenuMusicStatus())
                {
                    wmPlayer.controls.play();
                    return "Music ON";
                }
                else
                {
                    wmPlayer.controls.stop();
                    return "Music OFF";
                }
            }
            catch
            {
                Logger.Log("Attempt to start background music failed (WMP missing?)");
                return "WMP Missing";
            }
        }

        public void StartMusic()
        {
            try
            {
                wmPlayer.URL = ProgramConstants.gamepath + ProgramConstants.RESOURCES_DIR + "lobbymusic.wav";
                wmPlayer.settings.setMode("loop", true);
                if (DomainController.Instance().getMainMenuMusicStatus())
                    wmPlayer.controls.play();
                else
                    wmPlayer.controls.stop();
            }
            catch
            {
                Logger.Log("Attempt to start background music failed (WMP missing?)");
            }
        }

        public void StopMusic()
        {
            try
            {
                wmPlayer.controls.stop();
            }
            catch
            {
                Logger.Log("Attempt to stop background music failed (WMP missing?)");
            }
        }

        public string ToggleMusic()
        {
            try
            {
                if (wmPlayer.playState == WMPPlayState.wmppsPlaying)
                {
                    wmPlayer.controls.stop();
                    DomainController.Instance().SaveLobbyMusicSettings(false);
                    return "Music OFF";
                }
                else
                {
                    wmPlayer.URL = ProgramConstants.gamepath + ProgramConstants.RESOURCES_DIR + "lobbymusic.wav";
                    wmPlayer.settings.setMode("loop", true);
                    wmPlayer.controls.play();
                    DomainController.Instance().SaveLobbyMusicSettings(true);
                    return "Music ON";
                }
            }
            catch
            {
                return "WMP Missing";
            }
        }
    }
}
