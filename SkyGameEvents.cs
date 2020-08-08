using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Linq;
using UnityEngine;
using Oxide.Game.Rust.Cui;
using Oxide.Core.Plugins;
using Oxide.Core.Libraries.Covalence;

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

public enum ItemTypes
        {
            Item,
            Command
        }
        #endregion

        #region Config
        private static Configuration config;

        private class Configuration
        {
            [JsonProperty("Использовать автоматический запуск ивентов?")]
            public bool useTimer = true;

            [JsonProperty("Задержка между эвентами")]
            public int Delay = 3600;

            [JsonProperty("Отклонение от стандартного времени (рандомное время)")]
            public int RandomTime = 900;

            [JsonProperty("Максимальная длина никнейма")]
            public int NameLenght = 16;
            
            [JsonProperty("Эвенты доступные для игроков", ObjectCreationHandling = ObjectCreationHandling.Replace)]
            public List<string> EnabledEvents = new List<string>
            {
                "KingMountain", "CollectionResources", "HuntAnimal", "HelicopterPet", "LookingLoot", "SpecialCargo", "FoundationDrop"
            };

            [JsonProperty("Право на запуск ивента")]
            public string perm_admin = "multievents.admin";

            [JsonProperty("Команда открытия склада")]
            public string cmdStorage = "storage";

            [JsonProperty("Включить оповещение о получении награды?")]
            public bool EnableRewardAlert = true;

            [JsonProperty("Настройка эвента ЛЮБИМЧИК ВЕРТОЛЁТА")]
            public EventSettings HelicopterPet = new EventSettings
            {
                MinPlayers = 4,
                TimeDelay = 300,
                ImageUrl = "https://i.imgur.com/owDanC1.png",
                ui = new Interface
                {
                    color = "0.024 0.016 0.17 0.7",
                    oMin = "-225 -305",
                    oMax = "-5 -5"
                },
                loot = new List<Loot>
                    {
                        new Loot
                        {
                            itemenabled = true,
                            cashenabled = true,
                            items = new List<List<Items>>
                            {
                                new List<Items>
                                {
                                    new Items
                                    {
                                        Type = ItemTypes.Item,
                                        Item = new ItemConfClass
                                        {
                                            ShortName = "autoturret",
                                            SkinID = 0,
                                            minAmount = 1,
                                            maxAmount = 1
                                        },
                                        DisplayName = string.Empty,
                                        Command = string.Empty,
                                        ImageURL = string.Empty
                                    },
                                    new Items
                                    {
                                        Type = ItemTypes.Command,
                                        Item = new ItemConfClass
                                        {
                                            SkinID = 0,
                                            ShortName = string.Empty,
                                            minAmount = 1,
                                            maxAmount = 1
                                        },
                                        Command = "addgroup %steamid% vip 1d",
                                        DisplayName = "VIP на 1 день",
                                        ImageURL = "https://i.imgur.com/GuJUyLH.png"
                                    }
                                }
                            },
                            cash = new Cash
                            {
                                function = "Deposit",
                                plugin = "Economics",
                                amount = 400,
                            }
                        },
                    },
            };

            [JsonProperty("Настройка эвента ОХОТА НА ЖИВОТНЫХ")]
            public HuntAnimalSettings HuntAnimal = new HuntAnimalSettings
            {
                MinPlayers = 4,
                TimeDelay = 300,
                ImageUrl = "https://i.imgur.com/9gTw3kN.png",
                chicken = 1,
                wolf = 4,
                boar = 4,
                deer = 4,
                horse = 4,
                bear = 10,
                ui = new Interface
                {
                    color = "0.024 0.016 0.17 0.7",
                    oMin = "-225 -305",
                    oMax = "-5 -5"
                },
                loot = new List<Loot>
                    {
                        new Loot
                        {
                            itemenabled = true,
                            cashenabled = true,
                            items = new List<List<Items>>
                            {
                                new List<Items>
                                {
                                    new Items
                                    {
                                        Type = ItemTypes.Item,
                                        Item = new ItemConfClass
                                        {
                                            ShortName = "autoturret",
                                            SkinID = 0,
                                            minAmount = 1,
                                            maxAmount = 1
                                        },
                                        DisplayName = string.Empty,
                                        Command = string.Empty,
                                        ImageURL = string.Empty
                                    },
                                    new Items
                                    {
                                        Type = ItemTypes.Command,
                                        Item = new ItemConfClass
                                        {
                                            SkinID = 0,
                                            ShortName = string.Empty,
                                            minAmount = 1,
                                            maxAmount = 1
                                        },
                                        Command = "addgroup %steamid% vip 1d",
                                        DisplayName = "VIP на 1 день",
                                        ImageURL = "https://i.imgur.com/GuJUyLH.png"
                                    }
                                }
                            },
                            cash = new Cash
                            {
                                function = "Deposit",
                                plugin = "Economics",
                                amount = 400,
                            }
                        },
                        new Loot
                        {
                            itemenabled = true,
                            cashenabled = true,
                            items = new List<List<Items>>
                            {
                                new List<Items>
                                {
                                    new Items
                                    {
                                        Type = ItemTypes.Item,
                                        Item = new ItemConfClass
                                        {
                                            ShortName = "autoturret",
                                            SkinID = 0,
                                            minAmount = 1,
                                            maxAmount = 1
                                        },
                                        DisplayName = string.Empty,
                                        Command = string.Empty,
                                        ImageURL = string.Empty
                                    },
                                    new Items
                                    {
                                        Type = ItemTypes.Command,
                                        Item = new ItemConfClass
                                        {
                                            SkinID = 0,
                                            ShortName = string.Empty,
                                            minAmount = 1,
                                            maxAmount = 1
                                        },
                                        Command = "addgroup %steamid% vip 1d",
                                        DisplayName = "VIP на 1 день",
                                        ImageURL = "https://i.imgur.com/GuJUyLH.png"
                                    }
                                }
                            },
                            cash = new Cash
                            {
                                function = "Deposit",
                                plugin = "Economics",
                                amount = 400,
                            }
                        },
                        new Loot
                        {
                            itemenabled = true,
                            cashenabled = true,
                            items = new List<List<Items>>
                            {
                                new List<Items>
                                {
                                    new Items
                                    {
                                        Type = ItemTypes.Item,
                                        Item = new ItemConfClass
                                        {
                                            ShortName = "autoturret",
                                            SkinID = 0,
                                            minAmount = 1,
                                            maxAmount = 1
                                        },
                                        DisplayName = string.Empty,
                                        Command = string.Empty,
                                        ImageURL = string.Empty
                                    },
                                    new Items
                                    {
                                        Type = ItemTypes.Command,
                                        Item = new ItemConfClass
                                        {
                                            SkinID = 0,
                                            ShortName = string.Empty,
                                            minAmount = 1,
                                            maxAmount = 1
                                        },
                                        Command = "addgroup %steamid% vip 1d",
                                        DisplayName = "VIP на 1 день",
                                        ImageURL = "https://i.imgur.com/GuJUyLH.png"
                                    }
                                }
                            },
                            cash = new Cash
                            {
                                function = "Deposit",
                                plugin = "Economics",
                                amount = 400,
                            }
                        },
                    },
            };

            [JsonProperty("Настройка эвента КОРОЛЬ ГОРЫ")]
            public KingMountainSettings KingMountain = new KingMountainSettings
            {
                checkDuels = true,
                checkHotair = true,
                checkMinicopter = true,
                checkScrapheli = true,
                checkBuilding = true,
                MinPlayers = 4,
                TimeDelay = 300,
                ImageUrl = "https://i.imgur.com/dLWgxg7.png",
                ui = new Interface
                {
                    color = "0.024 0.016 0.17 0.7",
                    oMin = "-225 -305",
                    oMax = "-5 -5"
                },
                loot = new List<Loot>
                    {
                        new Loot
                        {
                            itemenabled = true,
                            cashenabled = true,
                            items = new List<List<Items>>
                            {
                                new List<Items>
                                {
                                    new Items
                                    {
                                        Type = ItemTypes.Item,
                                        Item = new ItemConfClass
                                        {
                                            ShortName = "autoturret",
                                            SkinID = 0,
                                            minAmount = 1,
                                            maxAmount = 1
                                        },
                                        DisplayName = string.Empty,
                                        Command = string.Empty,
                                        ImageURL = string.Empty
                                    },
                                    new Items
                                    {
                                        Type = ItemTypes.Command,
                                        Item = new ItemConfClass
                                        {
                                            SkinID = 0,
                                            ShortName = string.Empty,
                                            minAmount = 1,
                                            maxAmount = 1
                                        },
                                        Command = "addgroup %steamid% vip 1d",
                                        DisplayName = "VIP на 1 день",
                                        ImageURL = "https://i.imgur.com/GuJUyLH.png"
                                    }
                                }
                            },
                            cash = new Cash
                            {
                                function = "Deposit",
                                plugin = "Economics",
                                amount = 400,
                            }
                        }
                    },
            };

            [JsonProperty("Настройка эвента СБОР РЕСУРСОВ")]
            public CollectionResourcesSettings CollectionResources = new CollectionResourcesSettings()
            {
                MinPlayers = 4,
                TimeDelay = 300,
                ImageUrl = "https://i.imgur.com/FggJeaH.png",
                ui = new Interface
                {
                    color = "0.024 0.016 0.17 0.7",
                    oMin = "-225 -305",
                    oMax = "-5 -5"
                },
                BlockItems =  new List<string>
                {
                    "jackhammer", "chainsaw"  
                },
                loot = new List<Loot>
                    {
                        new Loot
                        {
                            itemenabled = true,
                            cashenabled = true,
                            items = new List<List<Items>>
                            {
                                new List<Items>
                                {
                                    new Items
                                    {
                                        Type = ItemTypes.Item,
                                        Item = new ItemConfClass
                                        {
                                            ShortName = "autoturret",
                                            SkinID = 0,
                                            minAmount = 1,
                                            maxAmount = 1
                                        },
                                        DisplayName = string.Empty,
                                        Command = string.Empty,
                                        ImageURL = string.Empty
                                    },
                                    new Items
                                    {
                                        Type = ItemTypes.Command,
                                        Item = new ItemConfClass
                                        {
                                            SkinID = 0,
                                            ShortName = string.Empty,
                                            minAmount = 1,
                                            maxAmount = 1
                                        },
                                        Command = "addgroup %steamid% vip 1d",
                                        DisplayName = "VIP на 1 день",
                                        ImageURL = "https://i.imgur.com/GuJUyLH.png"
                                    }
                                }
                            },
                            cash = new Cash
                            {
                                function = "Deposit",
                                plugin = "Economics",
                                amount = 400,
                            }
                        },
                        new Loot
                        {
                            itemenabled = true,
                            cashenabled = true,
                            items = new List<List<Items>>
                            {
                                new List<Items>
                                {
                                    new Items
                                    {
                                        Type = ItemTypes.Item,
                                        Item = new ItemConfClass
                                        {
                                            ShortName = "autoturret",
                                            SkinID = 0,
                                            minAmount = 1,
                                            maxAmount = 1
                                        },
                                        DisplayName = string.Empty,
                                        Command = string.Empty,
                                        ImageURL = string.Empty
                                    },
                                    new Items
                                    {
                                        Type = ItemTypes.Command,
                                        Item = new ItemConfClass
                                        {
                                            SkinID = 0,
                                            ShortName = string.Empty,
                                            minAmount = 1,
                                            maxAmount = 1
                                        },
                                        Command = "addgroup %steamid% vip 1d",
                                        DisplayName = "VIP на 1 день",
                                        ImageURL = "https://i.imgur.com/GuJUyLH.png"
                                    }
                                }
                            },
                            cash = new Cash
                            {
                                function = "Deposit",
                                plugin = "Economics",
                                amount = 400,
                            }
                        },
                        new Loot
                        {
                            itemenabled = true,
                            cashenabled = true,
                            items = new List<List<Items>>
                            {
                                new List<Items>
                                {
                                    new Items
                                    {
                                        Type = ItemTypes.Item,
                                        Item = new ItemConfClass
                                        {
                                            ShortName = "autoturret",
                                            SkinID = 0,
                                            minAmount = 1,
                                            maxAmount = 1
                                        },
                                        DisplayName = string.Empty,
                                        Command = string.Empty,
                                        ImageURL = string.Empty
                                    },
                                    new Items
                                    {
                                        Type = ItemTypes.Command,
                                        Item = new ItemConfClass
                                        {
                                            SkinID = 0,
                                            ShortName = string.Empty,
                                            minAmount = 1,
                                            maxAmount = 1
                                        },
                                        Command = "addgroup %steamid% vip 1d",
                                        DisplayName = "VIP на 1 день",
                                        ImageURL = "https://i.imgur.com/GuJUyLH.png"
                                    }
                                }
                            },
                            cash = new Cash
                            {
                                function = "Deposit",
                                plugin = "Economics",
                                amount = 400,
                            }
                        },
                    },
            };

            [JsonProperty("Настройка эвента В ПОИСКАХ ЛУТА")]
            public LookingLootSettings LookingLoot = new LookingLootSettings
            {
                MinPlayers = 4,
                TimeDelay = 300,
                ImageUrl = "https://i.imgur.com/4ZOIs33.png",
                Barrels = new List<string>
                    {
                        "assets/bundled/prefabs/autospawn/resource/loot/loot-barrel-1.prefab",
                        "assets/bundled/prefabs/autospawn/resource/loot/loot-barrel-2.prefab",
                        "assets/bundled/prefabs/radtown/loot_barrel_1.prefab",
                        "assets/bundled/prefabs/radtown/loot_barrel_2.prefab",
                        "assets/bundled/prefabs/radtown/oil_barrel.prefab"
                    },
                ui = new Interface
                {
                    color = "0.024 0.016 0.17 0.7",
                    oMin = "-225 -305",
                    oMax = "-5 -5"
                },
                loot = new List<Loot>
                    {
                        new Loot
                        {
                            itemenabled = true,
                            cashenabled = true,
                            items = new List<List<Items>>
                            {
                                new List<Items>
                                {
                                    new Items
                                    {
                                        Type = ItemTypes.Item,
                                        Item = new ItemConfClass
                                        {
                                            ShortName = "autoturret",
                                            SkinID = 0,
                                            minAmount = 1,
                                            maxAmount = 1
                                        },
                                        DisplayName = string.Empty,
                                        Command = string.Empty,
                                        ImageURL = string.Empty
                                    },
                                    new Items
                                    {
                                        Type = ItemTypes.Command,
                                        Item = new ItemConfClass
                                        {
                                            SkinID = 0,
                                            ShortName = string.Empty,
                                            minAmount = 1,
                                            maxAmount = 1
                                        },
                                        Command = "addgroup %steamid% vip 1d",
                                        DisplayName = "VIP на 1 день",
                                        ImageURL = "https://i.imgur.com/GuJUyLH.png"
                                    }
                                }
                            },
                            cash = new Cash
                            {
                                function = "Deposit",
                                plugin = "Economics",
                                amount = 400,
                            }
                        },
                    },
            };

            [JsonProperty("Настройка эвента ДОСТАВКА ОСОБОГО ГРУЗА")]
            public SpecialCargoSettings SpecialCargo = new SpecialCargoSettings
            {
                tpBlock = true,
                MarkerName = "ОСОБЫЙ ГРУЗ",
                MinPlayers = 4,
                TimeDelay = 1800,
                ImageUrl = "https://i.imgur.com/Dp9bMSl.png",
                timeNewRunner = 6,
                timeStart = 4,
                blockMonuments = new List<string>
                    {
                        "oilrig", "cave", "power_sub"
                    },
                ui = new Interface
                {
                    color = "0.024 0.016 0.17 0.7",
                    oMin = "-225 -305",
                    oMax = "-5 -5"
                },
                loot = new List<Loot>
                    {
                        new Loot
                        {
                            itemenabled = true,
                            cashenabled = true,
                            items = new List<List<Items>>
                            {
                                new List<Items>
                                {
                                    new Items
                                    {
                                        Type = ItemTypes.Item,
                                        Item = new ItemConfClass
                                        {
                                            ShortName = "autoturret",
                                            SkinID = 0,
                                            minAmount = 1,
                                            maxAmount = 1
                                        },
                                        DisplayName = string.Empty,
                                        Command = string.Empty,
                                        ImageURL = string.Empty
                                    },
                                    new Items
                                    {
                                        Type = ItemTypes.Command,
                                        Item = new ItemConfClass
                                        {
                                            SkinID = 0,
                                            ShortName = string.Empty,
                                            minAmount = 1,
                                            maxAmount = 1
                                        },
                                        Command = "addgroup %steamid% vip 1d",
                                        DisplayName = "VIP на 1 день",
                                        ImageURL = "https://i.imgur.com/GuJUyLH.png"
                                    }
                                }
                            },
                            cash = new Cash
                            {
                                function = "Deposit",
                                plugin = "Economics",
                                amount = 400,
                            }
                        },
                    },
            };

            [JsonProperty("Настройка эвента ПАДАЮЩИЕ ПЛАТФОРМЫ")]
            public FoundationDropSettings FoundationDrop = new FoundationDropSettings
            {
                loot = new List<Loot>
                    {
                        new Loot
                        {
                            itemenabled = true,
                            cashenabled = true,
                            items = new List<List<Items>>
                            {
                                new List<Items>
                                {
                                    new Items
                                    {
                                        Type = ItemTypes.Item,
                                        Item = new ItemConfClass
                                        {
                                            ShortName = "autoturret",
                                            SkinID = 0,
                                            minAmount = 1,
                                            maxAmount = 1
                                        },
                                        DisplayName = string.Empty,
                                        Command = string.Empty,
                                        ImageURL = string.Empty
                                    },
                                    new Items
                                    {
                                        Type = ItemTypes.Command,
                                        Item = new ItemConfClass
                                        {
                                            SkinID = 0,
                                            ShortName = string.Empty,
                                            minAmount = 1,
                                            maxAmount = 1
                                        },
                                        Command = "addgroup %steamid% vip 1d",
                                        DisplayName = "VIP на 1 день",
                                        ImageURL = "https://i.imgur.com/GuJUyLH.png"
                                    }
                                }
                            },
                            cash = new Cash
                            {
                                function = "Deposit",
                                plugin = "Economics",
                                amount = 400,
                            }
                        },
                    },
                MinPlayers = 4,
                TimeDelay = 1800,
                ImageUrl = "https://i.imgur.com/B4DCBs2.png",
                ArenaSize = 10,
                DelayDestroy = 5f,
                WaitTime = 60,
                IntensityRadiation = 10f,
                DisableDefaultRadiation = false,
                ui = new Interface
                {
                    color = "0.024 0.016 0.17 0.7",
                    oMin = "-225 -305",
                    oMax = "-5 -5"
                },
                infoUI = new Interface
                {
                    color = "0.024 0.016 0.17 0.7",
                    oMin = "-225 -100",
                    oMax = "-5 -5"
                },
                commands = new List<string>
                    {
                        "bp",
                        "backpack",
                        "skin",
                        "skinbox",
                        "rec",
                        "tpa",
                        "tpr",
                        "sethome",
                        "home",
                        "kit",
                        "remove"
                    }
            };
        }

        public class Interface
        {
            [JsonProperty("Цвет фона")]
            public string color;
            [JsonProperty("Offset Min")]
            public string oMin;
            [JsonProperty("Offset Max")]
            public string oMax;
        }

        public class EventSettings
        {
            [JsonProperty("Настройка интерфейса ивента")]
            public Interface ui;
            [JsonProperty("Сколько игроков требуется для начала эвента?")]
            public int MinPlayers;
            [JsonProperty("Время продолжительности эвента (в секундах)")]
            public int TimeDelay;
            [JsonProperty("Ссылка на картинку")]
            public string ImageUrl;
            [JsonProperty("Настройка приза для победителя", ObjectCreationHandling = ObjectCreationHandling.Replace)]
            public List<Loot> loot;
        }
        
        public class HuntAnimalSettings : EventSettings
        {
            [JsonProperty("Сколько очков даётся за курицу?")]
            public int chicken;
            [JsonProperty("Сколько очков даётся за волка?")]
            public int wolf;
            [JsonProperty("Сколько очков даётся за кабана?")]
            public int boar;
            [JsonProperty("Сколько очков даётся за оленя?")]
            public int deer;
            [JsonProperty("Сколько очков даётся за лошадь?")]
            public int horse;
            [JsonProperty("Сколько очков даётся за медведя?")]
            public int bear;
        }

        public class CollectionResourcesSettings : EventSettings
        {
            [JsonProperty("Список предметов, добыча при помощи которых не учитывается")]
            public List<string> BlockItems;
        }
        
        public class KingMountainSettings : EventSettings
        {
            [JsonProperty("Блокировать людей на дуэдли (Duels)")]
            public bool checkDuels;

            [JsonProperty("Блокировать людей в миникоптере")]
            public bool checkMinicopter;

            [JsonProperty("Блокировать людей в скрап вертолёте")]
            public bool checkScrapheli;

            [JsonProperty("Блокировать людей в воздушномшаре")]
            public bool checkHotair;

            [JsonProperty("Блокировать людей в билде (когда игрок авторизован в шкафу)")]
            public bool checkBuilding;
        }

        public class LookingLootSettings : EventSettings
        {
            [JsonProperty("Лутание каких бочек считаются в эвенте?", ObjectCreationHandling = ObjectCreationHandling.Replace)]
            public List<string> Barrels;
        }

        public class SpecialCargoSettings : EventSettings
        {
            [JsonProperty("Время показа уведомления о запуске ивента")]
            public float timeStart;

            [JsonProperty("Время показа уведомления об объявлении бегущего")]
            public float timeNewRunner;

            [JsonProperty("Название маркера для карты")]
            public string MarkerName;

            [JsonProperty("Блокировать телепортацию для бегущего?")]
            public bool tpBlock;

            [JsonProperty("Запрещённые монументы для игры", ObjectCreationHandling = ObjectCreationHandling.Replace)]
            public List<string> blockMonuments;
        }
        
        public class FoundationDropSettings : EventSettings
        {
            [JsonProperty("Настройка интерфейса с информацией о количестве игроков и блоков")]
            public Interface infoUI;
            [JsonProperty("Размер арены в квадратах")]
            public int ArenaSize;
            [JsonProperty("Интвервал между удалениями блоков")]
            public float DelayDestroy;
            [JsonProperty("Время ожидания игроков с момента объявления ивента")]
            public int WaitTime;
            [JsonProperty("Интенсивность созданой радиации")]
            public float IntensityRadiation;
            [JsonProperty("Отключить стандартную радиацию на РТ (Это нужно в случае если у Вас отключена радиация, плагин включит её обратно но уберёт на РТ)")]
            public bool DisableDefaultRadiation;
            [JsonProperty("Заблокированные команды", ObjectCreationHandling = ObjectCreationHandling.Replace)]
            public List<string> commands;
        }
        
        public class Loot
        {
            [JsonProperty("Выдавать предмет?")]
            public bool itemenabled;
            [JsonProperty("Выдавать баланс?")]
            public bool cashenabled;
            [JsonProperty("Настройка предмета", ObjectCreationHandling = ObjectCreationHandling.Replace)]
            public List<List<Items>> items;
            [JsonProperty("Настройка баланса")]
            public Cash cash;
        }

        public class ItemConfClass : ItemClass
        {
            [JsonProperty(PropertyName = "Минимальное количество предмета")]
            public int minAmount;

            [JsonProperty(PropertyName = "Максимальное количество предмета")]
            public int maxAmount;
        }

        public class Items
        {
            [JsonProperty(PropertyName = "Тип предмета")]
            [JsonConverter(typeof(StringEnumConverter))]
            public ItemTypes Type;

            [JsonProperty(PropertyName = "Предмет")]
            public ItemConfClass Item;

            [JsonProperty(PropertyName = "Команда")]
            public string Command;

            [JsonProperty(PropertyName = "Отображаемое имя (если ковычки - дефолт из игры)")]
            public string DisplayName;

            [JsonProperty(PropertyName = "Картинка (если пусто - по шортнейму)")]
            public string ImageURL;

            public DataItem ToDataItem()
            {
                return new DataItem
                {
                    Type = Type,
                    item = new ItemClass
                    {
                        ShortName = Item.ShortName,
                        SkinID = Item.SkinID
                    },
                    Command = Command,
                    DisplayName = DisplayName,
                    Amount = Type == ItemTypes.Item ? UnityEngine.Random.Range(Item.minAmount, Item.maxAmount) : 1,
                    ImageURL = ImageURL
                };
            }
        }

        public class Cash
        {
            [JsonProperty("Название функции для вызова")]
            public string function;
            [JsonProperty("Название плагина")]
            public string plugin;
            [JsonProperty("Количество денег")]
            public int amount;
        }

        protected override void LoadConfig()
        {
            base.LoadConfig();
            try
            {
                config = Config.ReadObject<Configuration>();
                if (config == null) throw new Exception();
                SaveConfig();
            }
            catch
            {
                PrintError("Your configuration file contains an error. Using default configuration values.");
                LoadDefaultConfig();
            }
        }

        protected override void SaveConfig() => Config.WriteObject(config);

        protected override void LoadDefaultConfig() => config = new Configuration();
        #endregion

        #region Data
        private static PluginData _data;

        private void SaveData() => Oxide.Core.Interface.Oxide.DataFileSystem.WriteObject(Name, _data);

        private void LoadData()
        {
            try
            {
                _data = Oxide.Core.Interface.Oxide.DataFileSystem.ReadObject<PluginData>(Name);
            }
            catch (Exception e)
            {
                PrintError(e.ToString());
            }

            if (_data == null) _data = new PluginData();
        }

        private class PluginData
        {
            [JsonProperty(PropertyName = "Players Data", ObjectCreationHandling = ObjectCreationHandling.Replace)]
            public Dictionary<ulong, List<DataItem>> PlayersData = new Dictionary<ulong, List<DataItem>>();
        }

        public class DataItem
        {
            [JsonProperty(PropertyName = "Тип предмета")]
            public ItemTypes Type;

            [JsonProperty(PropertyName = "Предмет")]
            public ItemClass item;

            [JsonProperty(PropertyName = "Команда")]
            public string Command;

            [JsonProperty(PropertyName = "Отображаемое имя (если ковычки - дефолт из игры)")]
            public string DisplayName;

            [JsonProperty(PropertyName = "Количество")]
            public int Amount;

            [JsonProperty(PropertyName = "Картинка (если пусто - по шортнейму)")]
            public string ImageURL;

            public Item ToItem()
            {
                var newItem = ItemManager.CreateByName(item.ShortName, Amount, item.SkinID);

                if (newItem == null)
                {
                    instance?.PrintError($"Error creating item with shortname '{item.ShortName}'");
                    return null;
                }

                if (!string.IsNullOrEmpty(DisplayName)) newItem.name = DisplayName;
                return newItem;
            }

            public void GiveCmd(BasePlayer player)
            {
                string command = Command.Replace("\n", "|").Replace("%steamid%", player.UserIDString, StringComparison.OrdinalIgnoreCase).Replace("%username%", player.displayName, StringComparison.OrdinalIgnoreCase);
                instance?.Server.Command(command);
            }
        }

        public class ItemClass
        {
            [JsonProperty(PropertyName = "Короткое название предмета")]
            public string ShortName;

            [JsonProperty(PropertyName = "Skin ID предмета")]
            public ulong SkinID;
        }
        #endregion



        #region System Events
        void OnPlayerConnected(BasePlayer player)
        {
            if (player != null)
                ConnectedPlayers.Add(player);

            PrintWarning("INIT PLUGIN PIDARASTICHESKIY");
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