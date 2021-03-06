﻿using LeagueSharp;
using LeagueSharp.Common;
using System;
using System.Linq;

namespace SFSeries
{
    class Kennen
    {

        #region Declares
                public static string Name = "Kennen";
                public static Orbwalking.Orbwalker Orbwalker ;
                public static Obj_AI_Hero Player = ObjectManager.Player;
                public static Spell Q, W, R;
                public static Items.Item Dfg;

                public static Menu Sf;

                    public Kennen()
                    {
                        Game_OnGameLoad();
                    }

        private void Game_OnGameLoad()
        {
            Q = new Spell(SpellSlot.Q, 1050);
            W = new Spell(SpellSlot.W, 800);
            R = new Spell(SpellSlot.R, 550);

            Q.SetSkillshot(0.65f, 50f, 1700f, true, SkillshotType.SkillshotLine);
            //Base menu
            Sf = new Menu("SFSeries", "SFSeries", true);
            //Orbwalker and menu

            //moment :D

            var orbwalkerMenu = new Menu("LX Orbwalker", "LX_Orbwalker");
            Orbwalker = new Orbwalking.Orbwalker(orbwalkerMenu);
            Sf.AddSubMenu(orbwalkerMenu);
            //Target selector and menu  y thats all 
            var ts = new Menu("Target Selector", "Target Selector");
            SimpleTs.AddToMenu(ts);
            Sf.AddSubMenu(ts);
            //Combo menu
            Sf.AddSubMenu(new Menu("Combo", "Combo"));
            Sf.SubMenu("Combo").AddItem(new MenuItem("useQ", "Use Q?").SetValue(true));
            Sf.SubMenu("Combo").AddItem(new MenuItem("useW", "Use W?").SetValue(true));
            Sf.SubMenu("Combo").AddItem(new MenuItem("useR", "Use R?").SetValue(true));
            var harras = new Menu("Harras", "Harras");
            harras.AddItem(new MenuItem("useQH", "Use Q?").SetValue(true));
            Sf.AddSubMenu(harras);
            //Exploits
            Sf.AddItem(new MenuItem("NFE", "No-Face").SetValue(false));
            //Make the menu visible
            Sf.AddToMainMenu();

            Drawing.OnDraw += Drawing_OnDraw; // Add onDraw
            Game.OnGameUpdate += Game_OnGameUpdate; // adds OnGameUpdate (Same as onTick in bol)
            Game.PrintChat("SFSeries loaded! By iSnorflake");


        }

        private static void Drawing_OnDraw(EventArgs args)
        {
            
        }

        private static void Game_OnGameUpdate(EventArgs args)
        {
            switch (Orbwalker.ActiveMode)
            {
                case Orbwalking.OrbwalkingMode.Combo:
                    Combo();
                    break;
                case Orbwalking.OrbwalkingMode.Mixed:
                    Harras();
                    break;
            }
            AlwaysW();
        }

        private static void AlwaysW()
        {
// ReSharper disable once UnusedVariable
            foreach (var target in ObjectManager.Get<Obj_AI_Hero>().Where(target => target.IsValidTarget(W.Range)).Where(target => target.HasBuff("kennenmarkofstorm", true)))
            {
                W.Cast();
            }
        }

        private static void Harras()
        {
            var target = SimpleTs.GetTarget(Q.Range, SimpleTs.DamageType.Magical);
            if (target.IsValidTarget(Q.Range) && Q.IsReady())
            {
                Q.CastIfHitchanceEquals(target, HitChance.High, Sf.Item("NFE").GetValue<bool>());
            }

            
        }

        private static void Combo()
        {
            var target = SimpleTs.GetTarget(Q.Range, SimpleTs.DamageType.Magical);
            if (target.IsValidTarget(Q.Range) && Q.IsReady())
            {
                Q.CastIfHitchanceEquals(target,HitChance.High,Sf.Item("NFE").GetValue<bool>());
            }
            if (target.IsValidTarget(R.Range) & R.IsReady())
            {
                R.Cast(target, Sf.Item("NFE").GetValue<bool>());
            }
        }

        #endregion
    }
}
