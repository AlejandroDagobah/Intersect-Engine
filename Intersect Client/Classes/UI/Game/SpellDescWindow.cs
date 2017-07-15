﻿using Intersect;
using Intersect.Enums;
using Intersect.GameObjects;
using Intersect.Localization;
using IntersectClientExtras.File_Management;
using IntersectClientExtras.Gwen;
using IntersectClientExtras.Gwen.Control;
using Intersect_Client.Classes.General;

namespace Intersect_Client.Classes.UI.Game
{
    public class SpellDescWindow
    {
        ImagePanel _descWindow;

        public SpellDescWindow(int spellnum, int x, int y)
        {
            var spell = SpellBase.Lookup.Get<SpellBase>(spellnum);
            if (spell == null)
            {
                return;
            }
            _descWindow = new ImagePanel(Gui.GameUI.GameCanvas,"SpellDescWindow");

            ImagePanel icon = new ImagePanel(_descWindow,"SpellIcon");
            icon.Texture = Globals.ContentManager.GetTexture(GameContentManager.TextureType.Spell, spell.Pic);

            Label spellName = new Label(_descWindow,"SpellName");
            spellName.Text = spell.Name;
            Align.CenterHorizontally(spellName);

            Label spellType = new Label(_descWindow,"SpellType");
            spellType.Text = Strings.Get("spelldesc", "spelltype" + spell.SpellType);
            
            RichLabel spellDesc = new RichLabel(_descWindow,"SpellDesc");
            Gui.LoadRootUIData(_descWindow, "InGame.xml"); //Load this up now so we know what color to make the text when filling out the desc
            if (spell.Desc.Length > 0)
            {
                spellDesc.AddText(Strings.Get("spelldesc", "desc", spell.Desc), spellDesc.RenderColor);
                spellDesc.AddLineBreak();
                spellDesc.AddLineBreak();
            }

            if (spell.SpellType == (int) SpellTypes.CombatSpell)
            {
                spellType.Text = Strings.Get("spelldesc", "targettype" + spell.TargetType, spell.CastRange,spell.HitRadius);
            }
            if (spell.CastDuration > 0)
            {
                spellDesc.AddText(Strings.Get("spelldesc", "casttime", ((float) spell.CastDuration / 10f)),spellDesc.RenderColor);
                spellDesc.AddLineBreak();
                spellDesc.AddLineBreak();
            }
            if (spell.CooldownDuration > 0)
            {
                spellDesc.AddText(Strings.Get("spelldesc", "cooldowntime", ((float) spell.CooldownDuration / 10f)),spellDesc.RenderColor);
                spellDesc.AddLineBreak();
                spellDesc.AddLineBreak();
            }

            bool requirements = (spell.VitalCost[(int) Vitals.Health] > 0 || spell.VitalCost[(int) Vitals.Mana] > 0);

            if (requirements == true)
            {
                spellDesc.AddText(Strings.Get("spelldesc", "prereqs"), spellDesc.RenderColor);
                spellDesc.AddLineBreak();
                if (spell.VitalCost[(int)Vitals.Health] > 0)
                {
                    spellDesc.AddText(Strings.Get("spelldesc", "vital0cost", spell.VitalCost[(int)Vitals.Health]),spellDesc.RenderColor);
                    spellDesc.AddLineBreak();
                }
                if (spell.VitalCost[(int)Vitals.Mana] > 0)
                {
                    spellDesc.AddText(Strings.Get("spelldesc", "vital1cost", spell.VitalCost[(int)Vitals.Mana]),spellDesc.RenderColor);
                    spellDesc.AddLineBreak();
                }
                spellDesc.AddLineBreak();
            }


            string stats = "";
            if (spell.SpellType == (int) SpellTypes.CombatSpell)
            {
                stats = Strings.Get("spelldesc", "effects");
                spellDesc.AddText(stats, spellDesc.RenderColor);
                spellDesc.AddLineBreak();

                if (spell.Data3 > 0)
                {
                    spellDesc.AddText(Strings.Get("spelldesc", "effect" + spell.Data3), spellDesc.RenderColor);
                    spellDesc.AddLineBreak();
                }

                if (spell.VitalDiff[(int) Vitals.Health] != 0)
                {
                    stats = Strings.Get("spelldesc", "vital0",
                    (spell.VitalDiff[(int) Vitals.Health] > 0
                        ? Strings.Get("spelldesc", "addsymbol")
                        : Strings.Get("spelldecs", "removesymbol")), spell.VitalDiff[(int) Vitals.Health]);
                    spellDesc.AddText(stats, spellDesc.RenderColor);
                    spellDesc.AddLineBreak();
                }

                if (spell.VitalDiff[(int) Vitals.Mana] != 0)
                {
                    stats = Strings.Get("spelldesc", "vital1",
                    (spell.VitalDiff[(int) Vitals.Mana] > 0
                        ? Strings.Get("spelldesc", "addsymbol")
                        : Strings.Get("spelldesc", "removesymbol")), spell.VitalDiff[(int) Vitals.Mana]);
                    spellDesc.AddText(stats, spellDesc.RenderColor);
                    spellDesc.AddLineBreak();
                }

                if (spell.Data2 > 0)
                {
                    for (int i = 0; i < Options.MaxStats; i++)
                    {
                        if (spell.StatDiff[i] != 0)
                        {
                            spellDesc.AddText(Strings.Get("combat", "stat" + i) + ": " + (spell.StatDiff[i] > 0 ? "+ " : "") +spell.StatDiff[i], spellDesc.RenderColor);
                            spellDesc.AddLineBreak();
                        }
                    }
                    spellDesc.AddText(Strings.Get("spelldesc", "duration", (float) spell.Data2 / 10f),spellDesc.RenderColor);
                    spellDesc.AddLineBreak();
                }
            }
            //Load Again for positioning purposes.
            Gui.LoadRootUIData(_descWindow, "InGame.xml");
            spellDesc.SizeToChildren(false, true);
            _descWindow.SetPosition(x, y);
        }

        public void Dispose()
        {
            if (_descWindow == null)
            {
                return;
            }
            Gui.GameUI.GameCanvas.RemoveChild(_descWindow, false);
            _descWindow.Dispose();
        }
    }
}