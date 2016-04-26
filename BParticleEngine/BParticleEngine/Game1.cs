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
        Vector2 spriteSpeed = new Vector2(.5f, .5f);

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

            engine = new ParticleEngine(new TimeSpan(0, 0, 2), 
                Content.Load<Texture2D>("arrowDown"),
                Content.Load<Texture2D>("arrowUp"),
                Content.Load<Texture2D>("arrowLeft"),
                Content.Load<Texture2D>("arrowRight"),
                Content.Load<Texture2D>("audioOff"),
                Content.Load<Texture2D>("audioOn"),
                Content.Load<Texture2D>("barsHorizontal"),
                Content.Load<Texture2D>("barsVertical"),
                Content.Load<Texture2D>("button1"),
                Content.Load<Texture2D>("button2"),
                Content.Load<Texture2D>("button3"),
                Content.Load<Texture2D>("buttonA"),
                Content.Load<Texture2D>("buttonB"),
                Content.Load<Texture2D>("buttonL"),
                Content.Load<Texture2D>("buttonL1"),
                Content.Load<Texture2D>("buttonL2"),
                Content.Load<Texture2D>("buttonR"),
                Content.Load<Texture2D>("buttonR1"),
                Content.Load<Texture2D>("buttonR2"),
                Content.Load<Texture2D>("buttonSelect"),
                Content.Load<Texture2D>("buttonStart"),
                Content.Load<Texture2D>("buttonX"),
                Content.Load<Texture2D>("buttonY"),
                Content.Load<Texture2D>("checkmark"),
                Content.Load<Texture2D>("contrast"),
                Content.Load<Texture2D>("cross"),
                Content.Load<Texture2D>("down"),
                Content.Load<Texture2D>("downLeft"),
                Content.Load<Texture2D>("downRight"),
                Content.Load<Texture2D>("exclamation"),
                Content.Load<Texture2D>("exit"),
                Content.Load<Texture2D>("exitLeft"),
                Content.Load<Texture2D>("exitRight"),
                Content.Load<Texture2D>("export"),
                Content.Load<Texture2D>("fastForward"),
                Content.Load<Texture2D>("gamepad"),
                Content.Load<Texture2D>("gamepad1"),
                Content.Load<Texture2D>("gamepad2"),
                Content.Load<Texture2D>("gamepad3"),
                Content.Load<Texture2D>("gamepad4"));

            testSprite = new Sprite (Content.Load<Texture2D> ("Square50x50"), new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2), Color.White);
            testSprite.UseCenterOrigin = true;
            

            engine.RandomColors = true;
            engine.SpawnRate = new TimeSpan(0, 0, 0, 0, 100);
            engine.Scale = new Vector2(.1f, .1f);
            engine.FadeOut = true;
            engine.AutoSpawn = true;
            engine.SpawnCount = 5;
            engine.FollowItem = testSprite;
            engine.Tint = Color.Purple;
            engine.AngleToShoot = 45;
            engine.AngleDeviation = 360;

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

            testSprite.Position += spriteSpeed;

            if(InputManager.KeyJustPressed(Keys.O))
            {
                testSprite.UseCenterOrigin = !testSprite.UseCenterOrigin;
            }

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

