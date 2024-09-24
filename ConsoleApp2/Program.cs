using System.ComponentModel;
using System.ComponentModel.Design;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using TextRPG;


namespace TextRPG
{
    internal class Program
    {
        public enum Place { Town =0, Inn, Shop, Dungeon };
        public enum Weather {Sunny =0, Overcast, Rain, Mist};
        public enum Time {Day =0, Night };
        static void Main(string[] args)
        {
            Item items = new Item();
            Dictionary<int, Item> ItemCode = items.ItemCode();
            Shop shop = new Shop(ItemCode);
            Player p = new Player();
            Action.Intro(p);
            Place place = Place.Town;
            Weather weather = Weather.Sunny;
            Time time = Time.Day;
            Action.Town(weather, time);
            Dungeon dun = new Dungeon();

            while (p.hp > 0)
            {
                string input = Console.ReadLine();
                string[] commands = input.Split(' ');
                try
                {
                    if (commands[0] == "show")
                    {
                        if (commands[1] == "inventory" || commands[1] == "i")
                        {
                            p.ShowInventory();
                        }
                        else if (commands[1] == "stat" || commands[1] == "s" || commands[1] == "stats" || commands[1] == "status")
                        {
                            p.Status();
                        }
                        else if (place == Place.Shop && (commands[1] == "buy" || commands[1] == "b"))
                        {
                            int i = 1;
                            foreach (Item item in shop.sellList)
                            {
                                Console.Write($"{i}. ");
                                item.Info();
                                i++;
                            }
                        }
                        else if (place == Place.Shop && (commands[1] == "sell" || commands[1] == "s"))
                        {
                            int i = 1;
                            foreach (Item item in p.Inventory)
                            {
                                Console.Write($"{i}. ");
                                item.Info();
                                i++;
                            }
                        }
                        else
                        {
                            Console.WriteLine("명령어를 다시 입력해주세요.");
                        }

                    }
                    else if (commands[0] == "move")
                    {
                        if (commands[1] == "town" || commands[1] == "t")
                        {
                            place = Place.Town;
                            Action.Town(weather, time);
                        }
                        else if (commands[1] == "shop" || commands[1] == "s")
                        {
                            place = Place.Shop;

                            Action.Shop();
                        }
                        else if (commands[1] == "inn" || commands[1] == "i")
                        {
                            place = Place.Inn;
                            Action.Inn(p, weather, ref time);
                        }
                        else if (commands[1] == "dungeon" || commands[1] == "d")
                        {
                            place = Place.Dungeon;
                            Action.Dungeon();
                        }
                        else
                        {
                            Console.WriteLine("명령어를 다시 입력해주세요.");
                        }
                    }
                    else if (commands[0] == "?" || commands[0] == "/?")
                    {
                        Console.WriteLine("show 명령어를 통해 스테이터스와 인벤토리를 확인 가능합니다.");
                        Console.WriteLine("show inventory show status");
                        Console.WriteLine("move 명령어를 통해 이동 가능합니다.");
                        Console.WriteLine("move town, move shop, move inn, move dungeon");
                        Console.WriteLine("모든 하위 명령어는 약자로 실행 가능합니다.");
                        Console.WriteLine("show i, show s, move t, move s 등");
                        Console.WriteLine("인벤토리 내에서 equip (물품번호)를 통해서 아이템을 장착 가능합니다.");
                        if(place == Place.Shop)
                        {
                            Console.WriteLine("show buy를 입력해 구매 목록을, show sell을 입력해 판매 목록을 볼 수 있습니다.");
                            Console.WriteLine("구매목록에서는 buy (물품번호), 판매 목록에서는 sell (물품번호)를 입력하여 사거나 팔 수 있습니다.");
                        }
                        else if(place == Place.Dungeon)
                        {

                        }
                    }
                    else if (commands[0] == "buy" && place == Place.Shop)
                    {
                        if (int.TryParse(commands[1], out int output) == true)
                        {
                            shop.BuyItem(output, p);
                            Console.WriteLine($"현재 소지금 : {p.gold} G");
                        }
                    }
                    else if (commands[0] == "sell" && place == Place.Shop)
                    {
                        if (int.TryParse(commands[1], out int output) == true)
                        {
                            shop.SellItem(output, p);
                            Console.WriteLine($"현재 소지금 : {p.gold} G");
                        }
                    }
                    else if (commands[0] == "equip")
                    {
                        if(int.TryParse(commands[1],out int output) == true)
                        {
                            p.EquipItem(output);
                        }
                    }
                    else if (commands[0] == "enter" && place == Place.Dungeon)
                    {
                        if (int.TryParse(commands[1], out int output) == true)
                        {
                            dun.EnterDungeon(output, p);
                        }
                    }
                    else
                    {
                        Console.WriteLine("명령어를 다시 입력해주세요.");
                    }
                }
                catch (IndexOutOfRangeException)
                {
                    commands = new string[2];
                    Console.WriteLine("명령어가 너무 짧습니다.");
                }
                if (p.exp >= p.maxExp) p.LevelUp();
                
            }
        }
        public static class Action
        {
            public static void Intro(Player p)
            {
                Console.WriteLine("Sparta 던전에 오신 것을 환영합니다.");
                Console.WriteLine("당신의 이름을 알려주세요.");
                p.name = Console.ReadLine();
                Console.WriteLine("당신의 직업은?");
                Console.WriteLine("1. 전사, 2. 도적, 3. 수강생");
                switch (IsInt(Console.ReadLine(), 4))
                {
                    case 1: p.job = "전사"; p.defense = 5; break;
                    case 2: p.job = "도적"; p.damage = 15; break;
                    case 3: p.job = "수강생"; p.gold = 2000; break;
                    case 4: p.job = "튜터"; p.damage = 30; p.defense = 20; p.gold = 5000; break;
                }
                Console.WriteLine("튜토리얼을 보시겠습니까? (y/n)");
                string a = Console.ReadLine();
                if (a == "y")
                {
                    Console.WriteLine("이 게임은 던전에 진입으로 골드를 벌어서 그 골드로 아이템을 구입해 강해지는 성장형 RPG입니다.");
                    Console.WriteLine("방어력에 비례해 던전의 성공확률이 증가하며 공격력에 비례해 추가적인 보상을 얻을 수 있습니다.");
                    Console.WriteLine("게임의 진행은 명령어로 진행되며 자세한 내용은 플레이 중 ?를 입력하는 것으로 확인 할 수 있습니다.");
                    Console.WriteLine($"{p.name}님의 순탄한 여정을 빕니다.");
                }
                else Console.WriteLine($"{p.name}님의 순탄한 여정을 빕니다.");

            }
            public static void Town(Weather w, Time t) //마을에서 날씨 확인가능
            {
                if (w == Weather.Sunny)
                {
                    if(t == Time.Day)
                    {
                        Console.WriteLine("\r\n힘세고 강한 아침");
                        Console.WriteLine("눈부신 햇볕이 마을을 비춘다.");
                    }
                    else
                    {
                        Console.WriteLine("\r\n구름 한 점 없는 밤이다.");
                        Console.WriteLine("밤 하늘은 수 많은 별 들로 반짝인다.");
                    }
                }
                else if (w == Weather.Overcast)
                {
                    if (t == Time.Day)
                    {
                        Console.WriteLine("\r\n온 세상이 하얀 구름으로 뒤덮인 것 같다.");
                        Console.WriteLine("그렇게 어둡지는 않다.");
                    }
                    else
                    {
                        Console.WriteLine("\r\n새까만 밤이다.");
                        Console.WriteLine("빛이 없으면 한 치 앞이 보이지 않는다.");
                    }
                }
                else if (w == Weather.Rain)
                {
                    if (t == Time.Day)
                    {
                        Console.WriteLine("\r\n비 냄새가 온 사방에 가득하다.");
                        Console.WriteLine("습하고 축축하다.");
                    }
                    else
                    {
                        Console.WriteLine("\r\n어둡고 습하고 축축하다.");
                        Console.WriteLine("빗소리와 개구리 소리가 가득하다.");
                    }
                }
                else if (w == Weather.Mist)
                {
                    if (t == Time.Day)
                    {
                        Console.WriteLine("\r\n짙은 안개가 끼였다.");
                        Console.WriteLine("앞이 잘 안보인다. 여기가 어디지?");
                    }
                    else
                    {
                        Console.WriteLine("\r\n금방이라도 뭐가 튀어나올 듯한 으스스한 느낌이다.");
                        Console.WriteLine("이런 날에 굳이 밖으로 나가야할까?");
                    }
                }
                Console.WriteLine("\r\n그럼 이제 뭘 할까?");
                Console.WriteLine("명령어 확인은 '?'를 입력");
            }
            public static Weather Inn(Player p, Weather w, ref Time t) //휴식을 취하면 체력이 모두 회복되고 날씨가 바뀜
            {
                Console.WriteLine("모험자 여관에 온 것을 환영하네, 낯선 이여");
                Console.WriteLine("방 값은 500 G라네, 잠시 쉬고 가겠는가? (y/n)");
                Console.WriteLine($"현재 소지금 : {p.gold} G");
                bool isChar = char.TryParse(Console.ReadLine(), out char output);
                if(isChar == true)
                {
                    if(output == 'y')
                    {
                        if (p.gold >= 500)
                        {
                            p.gold -= 500;
                            Console.WriteLine("500G 치고 초라한 방이다.");
                            Console.WriteLine("체력이 모두 회복되었다.");
                            p.hp = p.maxHp;
                            if (t == Time.Day) t = Time.Night;
                            else t = Time.Day;
                            w = GetRandomWeather();
                            Town(w, t);
                            return w;
                        }
                        else
                        {
                            Console.WriteLine("돈이 모자란 손님은 받지 않는다!");
                            Console.WriteLine("당신은 매몰차게 쫒겨났다.");
                            Town(w, t);
                            return w;
                        }
                    }
                    else if(output == 'n')
                    {
                        Console.WriteLine("바쁜데 시간낭비 하지 말고 나가!");
                        Town(w, t);
                        return w;
                    }
                    else
                    {
                        Console.WriteLine("뭐라고 하는지 모르겠군 y/n으로 답하게");
                        return Inn(p, w,ref t);
                    }
                }
                else
                {
                    Console.WriteLine("뭐라고 하는지 모르겠군 y/n으로 답하게");
                    return Inn(p, w,ref t);
                }
                Weather GetRandomWeather()
                {
                    Random random = new Random();
                    var a = Enum.GetValues(enumType: typeof(Weather));
                    return (Weather)a.GetValue(random.Next(0, a.Length));
                }
            }

            public static void Shop() //상점에서 물건 구매와 판매 가능
            {
                Console.WriteLine("어서 오게나, 돈만 충분하다면 언제든지 환영일세");
                Console.WriteLine("무엇을 하겠는가?");
                Console.WriteLine("show buy를 입력해 구매목록을, show sell을 입력해 판매목록을 볼 수 있습니다. 다시 보려면 ?를 입력해주세요.");
            }

            public static void Dungeon() //던전은 3단계로 나뉨
            {
                Console.WriteLine("눈 앞에 3가지의 입구가 있다.");
                Console.WriteLine("차례대로 쉬움, 보통, 어려움이다.");
                Console.WriteLine("1. 쉬움 (권장 방어력 5)");
                Console.WriteLine("2. 보통 (권장 방어력 10)");
                Console.WriteLine("3. 어려움 (권장 방어력 20)");
                Console.WriteLine("던전에 입장하려면 enter (번호)를 입력하자.");
            }

            public static int IsInt(string input, int maxnum = 100)
            {
                bool isInt;
                int numb;
                isInt = int.TryParse(input, out numb);
                if (isInt == false)
                {
                    Console.WriteLine("숫자로 다시 입력해주세요.");
                    return IsInt(Console.ReadLine(), maxnum);
                }
                else if (numb > maxnum)
                {
                    Console.WriteLine("범위 내의 숫자로 다시 입력해주세요.");
                    return IsInt(Console.ReadLine(), maxnum);
                }
                else return numb;
            }

        }
    }
    
    public class Player //player 클래스
    {
        public string name;
        public int level = 1;
        public string job;
        public int damage = 10;
        private int addDamage = 0;
        public int defense = 5;
        private int addDefense = 0;
        public int maxHp = 100;
        public int hp = 100;
        public int gold = 1500;
        public int exp = 0;
        public int maxExp = 100;

        public List<Item> Inventory = new List<Item>(); //inventory
        public Item[] EquipItems = new Item[4]; //장착된 템을 끼워넣는 슬롯 무기, 머리, 갑옷, 장신구 순
        //인벤토리와 장착 슬롯을 별개로 표시?
        //한번에 표시하고 그 중 E 표시하기?
        //인벤토리 내부에서 EquipItems를 find해서 E표시하기?
        
        public void Status() //status 확인
        {
            Console.WriteLine("이  름 : {0}",name);
            Console.WriteLine("레  벨 : {0}",level);
            Console.WriteLine("직  업 : {0}",job);
            Console.WriteLine("공격력 : {0} ({1})",damage+addDamage, addDamage);
            Console.WriteLine("방어력 : {0} ({1})",defense+addDefense, addDefense);
            Console.WriteLine("체  력 : {0}",hp);
            Console.WriteLine("골  드 : {0} G",gold);
        }

        public void ShowInventory()
        {
            int i = 1;
            foreach (Item item in EquipItems)
            {
                if (EquipItems[i - 1] == null) continue;
                Console.Write("{0}. [E] ", i);
                item.Info();
                i++;
            }

            foreach (Item item in Inventory)
            {
                Console.Write("{0}. ", i);
                item.Info();
                i++;
            }
        }

        public void EquipItem(int numb)
        {
            //인벤토리를 열고
            //equip 번호 를 통해 해당 아이템을 장착
            if (EquipItems[0] != null && Inventory[numb-1].place == Item.EquipPlace.Weapon)
            {
                Console.WriteLine("이미 장착된 슬롯 입니다. 교체하시겠습니까? (y/n)");
                string input = Console.ReadLine();
                if (input == "y")
                {
                    Inventory.Add(EquipItems[0]);
                    EquipItems[0] = Inventory[numb - 1];
                    Inventory.Remove(Inventory[numb - 1]);
                    Console.WriteLine($"{Inventory[numb - 1].name}을(를) 장착했습니다.");
                }
            }
            else if (Inventory[numb-1].place == Item.EquipPlace.Weapon)
            {
                EquipItems[0] = Inventory[numb - 1];
                Inventory.Remove(Inventory[numb - 1]);
                Console.WriteLine($"{Inventory[numb - 1].name}을(를) 장착했습니다.");
            }
            if (EquipItems[1] == null && Inventory[numb-1].place == Item.EquipPlace.Helmet)
            {
                EquipItems[1] = Inventory[numb-1];
                Inventory.Remove(Inventory[numb - 1]);
                Console.WriteLine($"{Inventory[numb - 1].name}을(를) 장착했습니다.");
            }
            else if (EquipItems[1] != null && Inventory[numb-1].place == Item.EquipPlace.Helmet)
            {
                Console.WriteLine("이미 장착된 슬롯 입니다. 교체하시겠습니까? (y/n)");
                string input = Console.ReadLine();
                if (input == "y")
                {
                    Inventory.Add(EquipItems[1]);
                    EquipItems[1] = Inventory[numb-1];
                    Inventory.Remove(Inventory[numb - 1]);
                    Console.WriteLine($"{Inventory[numb - 1].name}을(를) 장착했습니다.");
                }
            }
            if (EquipItems[2] == null && Inventory[numb-1].place == Item.EquipPlace.Armor)
            {
                EquipItems[2] = Inventory[numb-1];
                Inventory.Remove(Inventory[numb - 1]);
                Console.WriteLine($"{Inventory[numb - 1].name}을(를) 장착했습니다.");
            }
            else if (EquipItems[2] != null && Inventory[numb-1].place == Item.EquipPlace.Armor)
            {
                Console.WriteLine("이미 장착된 슬롯 입니다. 교체하시겠습니까? (y/n)");
                string input = Console.ReadLine();
                if (input == "y")
                {
                    Inventory.Add(EquipItems[2]);
                    EquipItems[2] = Inventory[numb-1];
                    Inventory.Remove(Inventory[numb - 1]);
                    Console.WriteLine($"{Inventory[numb - 1].name}을(를) 장착했습니다.");
                }
            }
            if (EquipItems[3] == null && Inventory[numb-1].place == Item.EquipPlace.Accessory)
            {
                EquipItems[3] = Inventory[numb-1];
                Inventory.Remove(Inventory[numb - 1]);
                Console.WriteLine($"{Inventory[numb - 1].name}을(를) 장착했습니다.");
            }
            else if (EquipItems[3] != null && Inventory[numb-1].place == Item.EquipPlace.Accessory)
            {
                Console.WriteLine("이미 장착된 슬롯 입니다. 교체하시겠습니까? (y/n)");
                string input = Console.ReadLine();
                if (input == "y")
                {
                    Inventory.Add(EquipItems[3]);
                    EquipItems[3] = Inventory[numb-1];
                    Inventory.Remove(Inventory[numb - 1]);
                    Console.WriteLine($"{Inventory[numb - 1].name}을(를) 장착했습니다.");
                }
            }
            //아이템 수치 만큼 능력치 상승
            try
            {
                for (int i = 0; i < EquipItems.Length; i++)
                {
                    if (EquipItems[i] == null) continue;
                    addDamage += EquipItems[i].damage;
                    addDefense += EquipItems[i].defense;
                }
            }
            catch(NullReferenceException)
            {
                for(int i = 0; i < EquipItems.Length; i++)
                {
                    addDamage += EquipItems[i].damage;
                    addDefense += EquipItems[i].defense;
                }
            }
        }
        public void LevelUp()
        {
            exp = 0;
            maxExp += 50;
            level++;
            Console.WriteLine("레벨이 상승했습니다!");
            damage += 1;
            defense += 2;
        }
    }

    public class Shop
    {
        public List<Item> sellList = new List<Item>();
        bool[] isSold;
        public Shop(Dictionary<int, Item> ItemList)
        {
            foreach (KeyValuePair<int, Item> pair in ItemList)
            {
                sellList.Add(pair.Value);
            }
            isSold = new bool[sellList.Count];
        }

        public void BuyItem(int numb, Player p)
        {
            if(p.gold >= sellList[numb-1].price)
            {
                if (isSold[numb - 1] == true)
                {
                    Console.WriteLine("이미 구매한 물건일세.");
                }
                else
                {
                    p.Inventory.Add(sellList[numb - 1]);
                    p.gold -= sellList[numb-1].price;
                    isSold[numb - 1] = true;
                }
            }
            else
            {
                Console.WriteLine("돈이 부족한 것 같은데 지갑을 다시 확인해 보게나.");
                Console.WriteLine($"현재 소지금 : {p.gold} G");
            }
        }
        public void SellItem(int numb, Player p)
        {
            p.Inventory.Remove(p.Inventory[numb - 1]);
            p.gold += (p.Inventory[numb - 1].price)*80/100;
            Console.WriteLine($"현재 소지금 : {p.gold} G");
        }

    }
    public class Dungeon
    {
        public int difficult;
        Random rand = new Random();


        public void DungeonCheck(Player p)
        {
            if(p.defense > difficult)
            {
                float amends = difficult + p.damage / 100f;
                int dungeonDamage = p.defense - difficult + rand.Next(20, 36);
                if(dungeonDamage < 0) dungeonDamage = 0;
                p.hp -= dungeonDamage;
                p.exp += difficult;
                p.gold += (int)(100f*amends);
                Console.WriteLine("던전을 클리어하였습니다.");
            }
            else
            {
                if (rand.Next(1,101) > 60)
                {
                    p.defense += 5;
                    DungeonCheck(p);
                    p.defense -= 5;
                }
                else
                {
                    p.hp -= p.maxHp / 2;
                    Console.WriteLine("던전 공략에 실패했습니다.");
                }
            }
        }

        public void EnterDungeon(int numb, Player p)
        {
            switch (numb)
            {
                case 1: difficult = 5; break;
                case 2: difficult = 10; break;
                case 3: difficult = 20; break;
                default: Console.WriteLine("1~3사이의 숫자를 입력해주세요."); break;
            }
            DungeonCheck(p);
        }
    }
}
