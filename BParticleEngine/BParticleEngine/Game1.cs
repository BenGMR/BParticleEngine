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
	{//Test for committing
		GraphicsDeviceManager graphics;
		SpriteBatch spriteBatch;

		Sprite testSprite;

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
			spriteBatch = new SpriteBatch (GraphicsDevice);

			testSprite = new Sprite (Content.Load<Texture2D> ("Square50x50"), Vector2.Zero, Color.White);
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
			#endif

			base.Update (gameTime);
		}
		protected override void Draw (GameTime gameTime)
		{
			graphics.GraphicsDevice.Clear (Color.CornflowerBlue);

			spriteBatch.Begin ();
			testSprite.Draw (spriteBatch);
			spriteBatch.End ();
            
			base.Draw (gameTime);
		}
	}
}

