#region File Description
//-----------------------------------------------------------------------------
// RetroScreen.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace Spacewar
{
    public class RetroScreen : SpacewarScreen
    {
        /// <summary>
        /// Creates a new SpacewarScreen
        /// </summary>
        public RetroScreen(Game game)
            : base(game)
        {
            //Retro
            backdrop = new SceneItem(game, new RetroStarfield(game));
            nextScene.Add(backdrop, drawScene);

            bullets = new RetroProjectiles(game);
            ship1 = new Ship(game, PlayerIndex.One, new Vector3(-250, 0, 0), bullets);
            ship1.Radius = 10f;
            nextScene.Add(ship1, drawScene);

            ship2 = new Ship(game, PlayerIndex.Two, new Vector3(250, 0, 0), bullets);
            ship2.Radius = 10f;
            nextScene.Add(ship2, drawScene);

            sun = new Sun(game, new RetroSun(game), new Vector3(SpacewarGame.Settings.SunPosition, 0.0f));
            nextScene.Add(sun, drawScene);

            nextScene.Add(bullets, drawScene);

            paused = false;
        }

        public override void Render()
        {
            Texture2D background = SpacewarGame.ContentManager.Load<Texture2D>(SpacewarGame.Settings.MediaPath + @"textures\retro_backdrop");
            IGraphicsDeviceService graphicsService = (IGraphicsDeviceService)GameInstance.Services.GetService(typeof(IGraphicsDeviceService));
            GraphicsDevice device = graphicsService.GraphicsDevice;

            //Backdrop
            SpriteBatch.Begin();
            SpriteBatch.Draw(background, new Vector2(0, 0), null, Color.White);
            SpriteBatch.End();

            //Always at the back so no need for zbuffer.
            device.RenderState.DepthBufferWriteEnable = false;
            device.RenderState.DepthBufferEnable = false;

            base.Render();

            Font.Begin();
            Font.Draw(FontStyle.WeaponLarge, 300, 15, player1Score);
            Font.Draw(FontStyle.WeaponLarge, 940, 15, player2Score);
            Font.End();
        }

        public override GameState Update(TimeSpan time, TimeSpan elapsedTime)
        {
            handleCollisions(time);

            return base.Update(time, elapsedTime);
        }

        public override void OnCreateDevice()
        {
            base.OnCreateDevice();

            bullets.OnCreateDevice();
        }
    }
}
