#region Using Statements
using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Input;

using BLibMonoGame;
#endregion

namespace BParticleEngine
{
	public class Game1 : Game
    {
		GraphicsDeviceManager graphics;
		SpriteBatch spriteBatch;

		Sprite testSprite;
        ParticleEngine engine;
        Vector2 spriteSpeed = new Vector2(0f, 0f);

        //TextSprite debugText;

		public Game1 ()
		{
			graphics = new GraphicsDeviceManager (this);
			Content.RootDirectory = "Content";
		}

		protected override void Initialize ()
		{
			
			base.Initialize ();
		}
			
		protected override void LoadContent ()
		{
            IsMouseVisible = true;
			spriteBatch = new SpriteBatch (GraphicsDevice);

            //debugText = new TextSprite(Content.Load<SpriteFont>("DebugFont"), "", Vector2.Zero, Color.Black);

            engine = new ParticleEngine(new TimeSpan(0, 0, 0, 2, 0), 
                Content.Load<Texture2D>("Square50x50"));

            testSprite = new Sprite (Content.Load<Texture2D> ("Square50x50"), new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2), Color.White);

            testSprite.UseCenterOrigin = true;

            engine.RandomColors = true;
            engine.SpawnRate = new TimeSpan(0, 0, 0, 0, 1000);
            engine.Scale = new Vector2(.5f, .5f);
            engine.FadeOut = true;
            engine.AutoSpawn = true;
            engine.SpawnCount = 5;
            engine.FollowItem = testSprite;
            engine.Tint = Color.Purple;
            engine.AngleToShoot = 0;
            engine.AngleDeviation = 0;
            //engine.UseGravity = true;
            //engine.GravityScale = 1;
            engine.ParticleSpeed = 0.1f;
		}

		protected override void Update (GameTime gameTime)
		{
			// For Mobile devices, this logic will close the Game when the Back button is pressed
			// Exit() is obsolete on iOS
			#if !__IOS__
			if (GamePad.GetState (PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
			    Keyboard.GetState ().IsKeyDown (Keys.Escape)) {
				Exit ();
			}

            InputManager.Update();

            engine.Update(gameTime);

            //engine.AngleToShoot =  180f.DegreeToVector().VectorToDegreeAngle().ToFloat();

            engine.AngleToShoot = spriteSpeed.VectorToDegreeAngle().ToFloat() + 180f;

            testSprite.Position += spriteSpeed;

            if(testSprite.Left <= 0 || testSprite.Right >= GraphicsDevice.Viewport.Width)
            {
                spriteSpeed.X *= -1;
            }
            if(testSprite.Top <= 0 || testSprite.Bottom >= GraphicsDevice.Viewport.Height)
            {
                spriteSpeed.Y *= -1;
            }
            
			#endif

			base.Update (gameTime);
		}
		protected override void Draw (GameTime gameTime)
		{
			graphics.GraphicsDevice.Clear (Color.CornflowerBlue);

            spriteBatch.Begin (blendState: BlendState.NonPremultiplied);
			testSprite.Draw (spriteBatch);
            engine.Draw(spriteBatch);
			spriteBatch.End ();
            
			base.Draw (gameTime);
		}
	}
}

