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
        int highestParticleCount = 0;
        TextSprite debugText;

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

            debugText = new TextSprite(Content.Load<SpriteFont>("DebugFont"), "", Vector2.Zero, Color.Black);

            engine = new ParticleEngine(new Vector2(100, 100), Vector2.One, Color.White, 3, new TimeSpan(0, 0, 0, 0, 1000), new TimeSpan(0, 0, 0, 0, 100),  
                Content.Load<Texture2D>("circle"),
                Content.Load<Texture2D>("diamond"),
                Content.Load<Texture2D>("star"));

            testSprite = new Sprite (Content.Load<Texture2D> ("Square50x50"), new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2), Color.White);

            testSprite.UseCenterOrigin = true;

            engine.SpawnCount = 20;
            engine.ParticleSpeed = 2f;
            engine.Scale = new Vector2(1f, 1f);
            engine.RandomColors = true;
            engine.TargetAlpha = 0;
            engine.FollowItem = testSprite;
            engine.LifeTimeDeviation = TimeSpan.FromMilliseconds(1000);
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

            debugText.Text = string.Format("Particles in use: {0}\nMax calculated Particles: {1}\nPool Count: {2}", engine.Particles.Count, engine.MaxParticles, engine.ParticlePoolCount);

            InputManager.Update();

            if(InputManager.KeyJustPressed(Keys.Up))
            {
                engine.SpawnCount++;
            }
            else if(InputManager.KeyJustPressed(Keys.Down))
            {
                engine.SpawnCount--;
            }

            if(InputManager.KeyJustPressed(Keys.P))
            {
                engine.TargetScale = Vector2.Zero;
            }
            engine.Update(gameTime);

            if(highestParticleCount < engine.Particles.Count)
            {
                highestParticleCount = engine.Particles.Count;
            }

            //engine.AngleToShoot =  180f.DegreeToVector().VectorToDegreeAngle().ToFloat();

            //engine.AngleToShoot = spriteSpeed.VectorToDegreeAngle().ToFloat() + 180f;

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
            engine.Draw(spriteBatch);
            testSprite.Draw (spriteBatch);
            debugText.Draw(spriteBatch);
			spriteBatch.End ();
            
			base.Draw (gameTime);
		}
	}
}

