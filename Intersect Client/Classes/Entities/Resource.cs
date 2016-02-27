﻿/*
    Intersect Game Engine (Server)
    Copyright (C) 2015  JC Snider, Joe Bridges
    
    Website: http://ascensiongamedev.com
    Contact Email: admin@ascensiongamedev.com 

    This program is free software; you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation; either version 2 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License along
    with this program; if not, write to the Free Software Foundation, Inc.,
    51 Franklin Street, Fifth Floor, Boston, MA 02110-1301 USA.
*/

using IntersectClientExtras.GenericClasses;
using Intersect_Client.Classes.Core;
using Intersect_Client.Classes.General;
using Intersect_Client.Classes.Misc;
using IntersectClientExtras.Graphics;

namespace Intersect_Client.Classes.Entities
{
    public class Resource : Entity
    {
        private bool _hasRenderBounds;
        FloatRect srcRectangle = new FloatRect();
        FloatRect destRectangle = new FloatRect();

        public Resource() : base()
        {

        }

        public void Load(ByteBuffer bf)
        {
            base.Load(bf);
            HideName = 1;
        }

        override public bool Update()
        {
            bool result = base.Update();
            if (!_hasRenderBounds) { CalculateRenderBounds(); }
            if (result && !GameGraphics.CurrentView.IntersectsWith(new FloatRect(destRectangle.Left,destRectangle.Top,destRectangle.Width,destRectangle.Height)))
            {
                if (RenderList != null)
                {
                    RenderList.Remove(this);
                }
            }
            return result;
        }

        private void CalculateRenderBounds()
        {
            if (!Globals.GameMaps.ContainsKey(CurrentMap)) return;
            int i = GetLocalPos(CurrentMap);
            if (i == -1)
            {
                return;
            }
            GameTexture srcTexture;
            if (GameGraphics.ResourceFileNames.IndexOf(MySprite) > -1)
            {
                srcTexture = GameGraphics.ResourceTextures[GameGraphics.ResourceFileNames.IndexOf(MySprite)];
                srcRectangle = new FloatRect(0, 0, GameGraphics.ResourceTextures[GameGraphics.ResourceFileNames.IndexOf(MySprite)].GetWidth(), GameGraphics.ResourceTextures[GameGraphics.ResourceFileNames.IndexOf(MySprite)].GetHeight());
                destRectangle.Y = (int)(Globals.GameMaps[CurrentMap].GetY() + CurrentY * Globals.Database.TileHeight + OffsetY);
                destRectangle.X = (int)(Globals.GameMaps[CurrentMap].GetX() + CurrentX * Globals.Database.TileWidth + OffsetX);
                if (srcRectangle.Height > 32) { destRectangle.Y -= srcRectangle.Height - 32; }
                if (srcRectangle.Width > 32) { destRectangle.X -= (srcRectangle.Width - 32) / 2; }
                destRectangle.Width = srcRectangle.Width;
                destRectangle.Height = srcRectangle.Height;
                _hasRenderBounds = true;
            }
        }

        //Rendering Resources
        override public void Draw()
        {
            int i = GetLocalPos(CurrentMap);
            if (i == -1)
            {
                return;
            }
            GameTexture srcTexture;
            if (GameGraphics.ResourceFileNames.IndexOf(MySprite) > -1)
            {
                srcTexture = GameGraphics.ResourceTextures[GameGraphics.ResourceFileNames.IndexOf(MySprite)];
                GameGraphics.DrawGameTexture(srcTexture, srcRectangle, destRectangle, Color.White);
            }
        }
    }
}
