using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Oxide.Core.Plugins;

namespace Oxide.Plugins
{
    [Info("SkyGameEvents", "JoyRax", "0.0.1")]
    [Description("Game events for players.")]
    class SkyGameEvents : RustPlugin
    {
        enum EventsName
        {
            None,
            KingOfTheHill
        }

        #region Initialization
        ConfigData configData;
        EventsName CurrentEvent;
        List<BasePlayer> ConnectedPlayers;

        void OnServerInitialized()
        {
            ConnectedPlayers = new List<BasePlayer>();
            CurrentEvent = EventsName.None;
        }
        #endregion



        #region Configuration
        class ConfigData
        {
            public bool Allow_KingOfTheHill { get; set; }
        }

        protected override void LoadDefaultConfig()
        {
            var config = new ConfigData
            {
                Allow_KingOfTheHill = true
            };

            SaveConfig(config);
        }

        void LoadVariables()
        {
            LoadConfigVariables();
            SaveConfig();
        }

        void LoadConfigVariables() => configData = Config.ReadObject<ConfigData>();

        void SaveConfig(ConfigData config) => Config.WriteObject(config, true);
        #endregion



        #region System Events
        void OnPlayerConnected(BasePlayer player)
        {
            if (player != null)
                ConnectedPlayers.Add(player);
            Debug.Log("INIT PLUGIN PIDARASTICHESKIY");
            Puts("INIT PLUGIN PIDARASTICHESKIY");
        }

        void OnPlayerDisconnected(BasePlayer player, string reason)
        {
            if (player != null)
                ConnectedPlayers.Remove(player);
        }
        #endregion



        #region Description of Events
        void StartEvent_KingOfTheHill()
        {
            if (!configData.Allow_KingOfTheHill) 
                return; // Event "King Of The Hill" disabled

            foreach(var player in ConnectedPlayers)
            {
                if (player == null && player.IsConnected)
                    continue;

                player.Message("Event King Of The Hill Starting!");
            }

            CurrentEvent = EventsName.KingOfTheHill;

            var countTick = 0;
            Dictionary<BasePlayer, int> retingPlayer = new Dictionary<BasePlayer, int>();

            Timer KingOfTheHill_Timer = timer.Every(1f, () =>
            {
                if (countTick < 10)
                {
                    foreach(var player in ConnectedPlayers)
                    {
                        if (player == null && player.IsConnected)
                            continue;

                        player.Message("До начала ивента King Of The Hill: {0}", (10 - countTick));
                    }
                }
                else if (countTick >= 10 && countTick <= 70)
                {
                    foreach(var player in ConnectedPlayers)
                    {
                        if (player == null && player.IsConnected)
                            continue;

                        var position = player.Position();
                        var coincidence;

                        foreach(var _player in retingPlayer.Keys)
                        {
                            if (_player == null && _player.IsConnected)
                                continue;

                            if (_player == player)
                            {
                                coincidence = true;
                                break;
                            }
                            else
                                coincidence = false;
                        }

                        if (!coincidence)
                            retingPlayer.Add(player, position.z);
                        else
                            retingPlayer[player] = position.z;
                    }

                    foreach(var player in ConnectedPlayers)
                    {
                        if (player == null && player.IsConnected)
                            continue;

                        foreach(var _player in retingPlayer.Keys)
                        {
                            if (_player == null && _player.IsConnected)
                                continue;

                            player.Message("Игрок: {0}, на высоте: {1}", _player.Name, retingPlayer[_player]);
                        }
                        player.Message("######################");
                        player.Message("До концв ивента осталось: {0}", (60 - (countTick - 10)));
                        player.Message("######################");
                    }
                    
                }
                else if (countTick > 70)
                {
                    BasePlayer Winner;
                    int Score;

                    retingPlayer = retingPlayer.OrderBy(pair => pair.Value).ToDictionary(pair => pair.Key, pair => pair.Value);

                    Winner = retingPlayer.Keys.Last();
                    Score = retingPlayer[Winner];

                    foreach(var player in ConnectedPlayers)
                    {
                        if (player == null && player.IsConnected)
                            continue;

                        if (Winner == null && Winner.IsConnected)
                            player.Message("Победитель ивента King Of The Hill: {0}, набравший высоту: {1}", Winner.Name, Score);
                    }
                    CurrentEvent = EventsName.None;
                    return;
                }
                

                countTick++;
            });
        }
        #endregion


        [ConsoleCommand("eventstest")]
        private void CmdConsole(ConsoleSystem.Arg args)
        {
            if (CurrentEvent != EventsName.None)
            {
                player.Message("Уже запущен другой ивент!");
                return;
            }

            StartEvent_KingOfTheHill();
        }
    }
}