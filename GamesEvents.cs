using System;
using System.Collections;
using System.Collections.Generic;
using Oxide.Core.Plugins;
using Oxide.Core.Libraries.Covalence;

namespace Oxide.Plugins
{
    [Info("Epic Stuff", "Unknown Author", "0.1.0")]
    [Description("Makes epic stuff happen")]
    class GamesEvents : CovalencePlugin
    {
        private void Init()
        {
            Puts("A baby plugin is born!");
        }

        enum EventsName
        {
            None,
            KingOfTheHill
        }

        EventsName CurrentEvent = EventsName.None;
        List<BasePlayer> ConnectedPlayers = new List<BasePlayer>();

        void OnPlayerConnected(BasePlayer player)
        {
            if (player != null)
            {
                ConnectedPlayers.Add(player);
            }
        }

        void OnPlayerDisconnected(BasePlayer player, string reason)
        {
            if (player != null)
            {
                ConnectedPlayers.Remove(player);
            }
        }

        [Command("testevents")]
        private void TestEventsCommand(IPlayer player, string command, string[] args)
        {
            player.Reply("Starting event!");
            StartEvent_KingOfTheHill();
        }

        private void StartEvent_KingOfTheHill()
        {
            foreach(var player in ConnectedPlayers)
            {
                if (player == null)
                {
                    continue;
                }

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
                        if (player == null)
                        {
                            continue;
                        }

                        player.Message("До начала ивента King Of The Hill: {0}", (10 - countTick));
                    }
                }
                else if (countTick >= 10 && countTick <= 70)
                {
                    foreach(var player in ConnectedPlayers)
                    {
                        if (player == null)
                        {
                            continue;
                        }

                        var position = player.Position();
                        var coincidence;

                        foreach(var _player in retingPlayer.Keys)
                        {
                            if (_player == null)
                            {
                                continue;
                            }

                            if (_player == player)
                            {
                                coincidence = true;
                                break;
                            }
                            else
                            {
                                coincidence = false;
                            }
                        }

                        if (!coincidence)
                        {
                            retingPlayer.Add(player, position.z);
                        }
                        else
                        {
                            retingPlayer[player] = position.z;
                        }
                    }

                    foreach(var player in ConnectedPlayers)
                    {
                        if (player == null)
                        {
                            continue;
                        }

                        foreach(var _player in retingPlayer.Keys)
                        {
                            if (_player == null
                            {
                                continue;
                            }

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
                        if (player == null)
                        {
                            continue;
                        }

                        if (Winner == null)
                        {
                            player.Message("Победитель ивента King Of The Hill: {0}, набравший высоту: {1}", Winner.Name, Score);
                        }
                    }
                    CurrentEvent = EventsName.None;
                    KingOfTheHill_Timer.Destroy();
                }
                countTick++;
            });
        }
    }
}


