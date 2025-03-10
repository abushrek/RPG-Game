﻿using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Script;
using Assets.Script.Extension;
using Assets.Script.InventoryFolder;
using Assets.Script.StatisticsFolder;
using Assets.Scripts.InventoryFolder.CraftFolder;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.InventoryFolder
{
    public enum EType
    {
        None,
        Usable,
        Consumable,
        Craftable,
        Armour,
        Weapon,
        Ammo
    }

    public enum ERank
    {
        Common,
        Uncommon,
        Rare,
        Epic,
        None
    }

    public enum ESubtype
    {
        None = 0,
        Head,
        Chest,
        Hands,
        Legs,
        Boots,
        Neck,
        Ring,
        Bracers,
        Waist,
        Weapon,
        Axe,
        Bow,
        Sword,
        Spear,
        Knife
    }

    public enum EProfession
    {
        None,
        Cooking,
        Fishing,
        Tailoring,
        Smithing,
        Crafting
    }

    public struct ItemStats
    {
        public ESkill ESkill { get; private set; }
        public int Value { get; private set; }
        public bool Wearing { get; set; }
        public ItemStats(ESkill eSkill, int value) : this()
        {
            ESkill = eSkill;
            Value = value;
            Wearing = false;
        }
    }

    public class NewItem
    {
        public int ID { get; set; }
        public Sprite Icon { get; private set; }
        public string Name { get; private set; }
        public EType Type { get; private set; }
        public ESubtype Subtype { get; private set; }
        public EProfession EProfession { get; private set; }
        public ERank ERank { get; private set; }
        public List<ItemStats> ItemStats { get; private set; }
        public int Chance { get; private set; }
        public List<CraftItem> CraftItems { get; set; }
        public int Loot { get; private set; }
        public int Quantity { get; private set; }
        public int MaximumStack { get; private set; }
        public int ActualStack { get; set; }
        public uint BuyPrice { get; private set; }
        public uint SellPrice { get; private set; }
        public int ProfesionLevelNeed { get; private set; }
        public int ProfesionExperiences { get; private set; }
        public GameObject CraftingObject;
        public bool Instantiate;
        public NewItem(Dictionary<string, string> itemDictionary)
        {
            ID = int.Parse(itemDictionary["ID"]);
            Name = itemDictionary["ItemName"];
            Icon = Resources.Load<Sprite>("Inventory/" + Name);
            CraftItems = new List<CraftItem>();
            ItemStats = new List<ItemStats>();
            if (itemDictionary["Type"] != "")
                Type = (EType)Enum.Parse(typeof(EType), itemDictionary["Type"], true);
            else
                Type = EType.None;
            if (itemDictionary["Subtype"] != "")
                Subtype = (ESubtype)Enum.Parse(typeof(ESubtype), itemDictionary["Subtype"], true);
            else
                Subtype = ESubtype.None;
            if (itemDictionary["Profession"] != "")
                EProfession = (EProfession)Enum.Parse(typeof(EProfession), itemDictionary["Profession"], true);
            else
                EProfession = EProfession.None;
            for (int i = 0; i < itemDictionary["Stats"].Split(',').Count(); i++)
            {

                if (itemDictionary["Stats"].Split(',')[i] != "" && itemDictionary["StatValues"].Split(',')[i] != "")
                    ItemStats.Add(new ItemStats((ESkill)Enum.Parse(typeof(ESkill), itemDictionary["Stats"].Split(',')[i], true), int.Parse(itemDictionary["StatValues"].Split(',')[i])));
            }
            if (itemDictionary["Chance"] != "")
                Chance = int.Parse(itemDictionary["Chance"]);
            /*if(itemDictionary["CraftItem"] != "" && itemDictionary["NumberOfItems"] != ""
                && itemDictionary["CraftItem"].Count() == itemDictionary["NumberOfItems"].Count())*/
            for (int i = 0; i < itemDictionary["CraftItem"].Split(',').Count(); i++)
            {
                if (itemDictionary["CraftItem"].Split(',')[i] != "" && itemDictionary["NumberOfItems"].Split(',')[i] != "")
                    CraftItems.Add(new CraftItem(int.Parse(itemDictionary["CraftItem"].Split(',')[i]), int.Parse(itemDictionary["NumberOfItems"].Split(',')[i])));
            }
            if (itemDictionary["Quantity"] != "")
                Quantity = int.Parse(itemDictionary["Quantity"]);
            if (itemDictionary["ProfessionLevelNeed"] != "")
                ProfesionLevelNeed = int.Parse(itemDictionary["ProfessionLevelNeed"]);
            if (itemDictionary["ProfessionExperiences"] != "")
                ProfesionExperiences = int.Parse(itemDictionary["ProfessionExperiences"]);
            if (itemDictionary["Loot"] != "")
                Loot = int.Parse(itemDictionary["Loot"]);
            if (itemDictionary["BuyPrice"] != "")
                BuyPrice = uint.Parse(itemDictionary["BuyPrice"]);
            if (itemDictionary["SellPrice"] != "")
                SellPrice = uint.Parse(itemDictionary["SellPrice"]);
            if (itemDictionary["MaximumStack"] != "")
                MaximumStack = int.Parse(itemDictionary["MaximumStack"]);
            if (itemDictionary["Rank"] != "")
                ERank = (ERank)Enum.Parse(typeof(ERank), itemDictionary["Rank"], true);
            else
                ERank = ERank.None;
            ActualStack = 1;
        }
        public NewItem(NewItem item)
        {
            if (item != null)
            {
                Icon = item.Icon;
                ID = item.ID;
                Icon = item.Icon;
                Name = item.Name;
                Type = item.Type;
                Subtype = item.Subtype;
                EProfession = item.EProfession;
                ItemStats = item.ItemStats;
                Chance = item.Chance;
                CraftItems = item.CraftItems;
                Quantity = item.Quantity;
                MaximumStack = item.MaximumStack;
                ActualStack = item.ActualStack;
                Loot = item.Loot;
                BuyPrice = item.BuyPrice;
                SellPrice = item.SellPrice;
                ProfesionExperiences = item.ProfesionExperiences;
                ProfesionLevelNeed = item.ProfesionLevelNeed;
                ERank = item.ERank;
            }
        }

        public static void SetStats(GameObject obj, NewItem item)
        {
            obj.GetComponent<Image>().sprite = item.Icon;
            ComponentItem objItem = obj.GetComponent<ComponentItem>();
            objItem.ID = item.ID;
            objItem.Icon = item.Icon;
            objItem.Name = item.Name;
            objItem.Type = item.Type;
            objItem.Subtype = item.Subtype;
            objItem.EProfession = item.EProfession;
            objItem.ItemStats = item.ItemStats;
            objItem.Chance = item.Chance;
            objItem.CraftItems = item.CraftItems;
            objItem.Quantity = item.Quantity;
            objItem.ActualStack = item.ActualStack;
            objItem.MaximumStack = item.MaximumStack;
            objItem.Loot = item.Loot;
            objItem.BuyPrice = item.BuyPrice;
            objItem.SellPrice = item.SellPrice;
            objItem.ProfesionExperiences = item.ProfesionExperiences;
            objItem.ProfesionLevelNeed = item.ProfesionLevelNeed;
            objItem.ERank = item.ERank;
        }

        public static NewItem IdToItem(int id)
        {
            return Database.ItemDatabase.Find(s => s.ID == id);
        }

        public static bool DoesItemExist(int id)
        {
            return Database.ItemDatabase.Any(s => s.ID == id);
        }

        public override string ToString()
        {
            return "ID:" + ID + " Name:" + Name + " Quantity:" + Quantity + " ActualStack:" + ActualStack +
                   " MaximumStack:" + MaximumStack + " BuyPrice:" + BuyPrice;
        }
    }
}