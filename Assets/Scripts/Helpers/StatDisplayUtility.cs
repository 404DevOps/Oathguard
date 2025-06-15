

//using System;
//using UnityEngine;

//public static class StatDisplayUtility
//{
//    public static string GetStatDisplayName(StatModifier statMod)
//    {
//        switch (statMod.StatType)
//        {
//            case StatType.MaxHealth:
//                return "Max Health";
//            case StatType.Attack:
//                return "Attack Power";
//            case StatType.Defense:
//                return "Defense";
//            case StatType.ElementModifier:
//                return statMod.ElementType.ToString() + " Defense";
//            case StatType.CritChance:
//                return "Critical Hit Chance";
//            case StatType.MoveSpeed:
//                return "Movement Speed";
//            case StatType.DodgeCD:
//                return "Dodge Cooldown";
//            case StatType.StaggerMultiplier:
//                return "Stagger";
//            default:
//                return "";
//        }
//    }
//    public static string GetStatDisplayName(StatInfo statInfo)
//    {
//        var statMod = new StatModifier("", statInfo.StatType, OperatorType.AddPercentage, statInfo.Value, ModifierSourceType.Equipment, statInfo.Element);
//        return GetStatDisplayName(statMod);
//    }

//    public static string GetOperatorDisplayValue(StatModifier statMod)
//    {
//        switch (statMod.Operator)
//        {
//            case OperatorType.AddPercentage:
//                return "+" + statMod.Value.ToString() + "%";
//            case OperatorType.SubtractPercentage:
//                return "-" + statMod.Value.ToString() + "%";
//            case OperatorType.Multiply:
//                return "*" + statMod.Value.ToString();
//            case OperatorType.Divide:
//                return "/" + statMod.Value.ToString();
//            case OperatorType.Add:
//                return "+" + statMod.Value.ToString();
//            case OperatorType.Subtract:
//                return "-" + statMod.Value.ToString();
//            default:
//                Debug.LogError("No DisplayValue found for OperatorType " + statMod.Operator.ToString());
//                return "";
//        }
//    }

//    public static string GetElementModifierDisplayValue(StatInfo info)
//    {
//        return $"{info.Value:F0}% {info.Element.ToString()} Defense";
//    }

//    public static string GetPlayerStatDisplayValue(StatInfo statInfo)
//    {
//        switch (statInfo.StatType)
//        {
//            case StatType.CritChance:
//            case StatType.MoveSpeed:
//            case StatType.ElementModifier:
//                return statInfo.Value.ToString() + "%";
//            case StatType.DodgeCD:
//                return statInfo.Value.ToString() + "s";
//            default:
//                return statInfo.Value.ToString();
//        }
//    }
//}