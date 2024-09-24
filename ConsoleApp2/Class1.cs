using System;
using System.Collections.Generic;
using TextRPG;

public class Item
{
    public enum EquipPlace { Weapon, Armor, Helmet, Accessory };
    public bool isSold = false;
    public string name;
    public int damage = 0;
    public int defense = 0;
    public int price;
    public string description;
    public EquipPlace place = new EquipPlace();

    public Dictionary<int, Item> ItemCode()
    {
        Dictionary<int, Item> itemcode = new Dictionary<int, Item>();

        Item woodenSword = new Item();
        woodenSword.name = "목검";
        woodenSword.damage = 5;
        woodenSword.price = 300;
        woodenSword.description = "오래된 목검이다. 없는 것보단 낫다.";
        place = EquipPlace.Weapon;
        itemcode.Add(101, woodenSword);

        Item axe = new Item();
        axe.name = "벌목 도끼";
        axe.damage = 10;
        axe.price = 500;
        axe.description = "날이 잘 서있는 도끼다.";
        place = EquipPlace.Weapon;
        itemcode.Add(102, axe);

        Item lance = new Item();
        lance.name = "기사의 랜스";
        lance.damage = 15;
        lance.price = 1000;
        lance.description = "흑색에 금색 문양이 새겨진 랜스";
        place = EquipPlace.Weapon;
        itemcode.Add(103, lance);

        Item cloth = new Item();
        cloth.name = "천 옷";
        cloth.defense = 3;
        cloth.price = 200;
        cloth.description = "이런 천 쪼가리에 방어력을 기대하지 말자.";
        place = EquipPlace.Armor;
        itemcode.Add(104, cloth);

        Item leatherArmor = new Item();
        leatherArmor.name = "가죽 갑옷";
        leatherArmor.defense = 6;
        leatherArmor.price = 600;
        leatherArmor.description = "매끈한 가죽 갑옷, 생각보다 튼튼하다.";
        place = EquipPlace.Armor;
        itemcode.Add(105, leatherArmor);

        Item chainMail = new Item();
        chainMail.name = "사슬 갑옷";
        chainMail.defense = 10;
        chainMail.price = 1200;
        chainMail.description = "아주 무겁고 아주 튼튼하다.";
        place = EquipPlace.Armor;
        itemcode.Add(106, chainMail);

        Item bandana = new Item();
        bandana.name = "두건";
        bandana.defense = 1;
        bandana.price = 100;
        bandana.description = "정말 이게 뭘 막을 수 있을 것 같아?";
        place = EquipPlace.Helmet;
        itemcode.Add(107, bandana);

        Item letherHelmet = new Item();
        letherHelmet.name = "가죽 투구";
        letherHelmet.defense = 3;
        letherHelmet.price = 400;
        letherHelmet.description = "통풍이 되지 않아 쓰고 있으면 덥다.";
        place = EquipPlace.Helmet;
        itemcode.Add(108, letherHelmet);

        Item knightHelm = new Item();
        knightHelm.name = "기사의 투구";
        knightHelm.defense = 7;
        knightHelm.price = 800;
        knightHelm.description = "튼튼하지만 시야가 좁다.";
        place = EquipPlace.Helmet;
        itemcode.Add(109, knightHelm);

        Item bronzeRing = new Item();
        bronzeRing.name = "동 반지";
        bronzeRing.damage = 2;
        bronzeRing.price = 300;
        bronzeRing.description = "왠지 모르게 손에 힘이 들어간다.";
        place = EquipPlace.Accessory;
        itemcode.Add(110, bronzeRing);

        Item silverRing = new Item();
        silverRing.name = "은 반지";
        silverRing.defense = 5;
        silverRing.price = 800;
        silverRing.description = "살균 효과 덕분에 건강해진다.";
        place = EquipPlace.Accessory;
        itemcode.Add(111, silverRing);

        Item goldRing = new Item();
        goldRing.name = "금 반지";
        goldRing.damage = 10;
        goldRing.price = 2000;
        goldRing.description = "자신감은 지갑에서 나온다.";
        place = EquipPlace.Accessory;
        itemcode.Add(112, goldRing);

        return itemcode;
    }

    public void Info()
    {
        Console.Write($"{name} | ");
        if(damage > 0)
        {
            Console.Write($"공격력 : {damage} | ");
        }
        if(defense > 0)
        {
            Console.Write($"방어력 : {defense} | ");
        }
        Console.Write($"{description} | ");
        if (isSold == false)
        {
            Console.WriteLine($"{price} G");
        }
        else Console.WriteLine("이미 구매한 상품입니다.");
    }
}

public class Table
{
    public List<List<Item>> IEtable = new List<List<Item>>();
    public List<Item> Itable = new List<Item>();
    public List<Item> Etable = new List<Item>();

    public Table(List<Item> itable, List<Item> etable)
    {
        Itable = itable;
        Etable = etable;
        IEtable[0] = Itable;
        IEtable[1] = Etable;
    }

}