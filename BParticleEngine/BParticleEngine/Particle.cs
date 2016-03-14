using System;
using BLibMonoGame;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BParticleEngine
{
	/*
	 * What does a particle need?
	 * Force
	 * Lifetime
	 * Angle
	 * It is a sprite
	 * */
	public class Particle : Sprite
	{
		private bool _fadeAway;
		public bool FadeAway
        {
            get{ return _fadeAway; }
            set{ _fadeAway = value; }
        }

		private TimeSpan _timeAlive;
		private TimeSpan _lifeTime;
		public TimeSpan LifeTime {
			get{ return _lifeTime; }
			set{_lifeTime = value;}
		}
		private Vector2 _velocity;
		public Vector2 Velocity
		{
			get{ return _velocity; }
			set{ _velocity = value; }
		}

		/// <summary>
		/// Initializes a single particle
		/// </summary>
		/// <param name="particleImage">Particle image.</param>
		/// <param name="startPosition">Start position.</param>
		/// <param name="tint">Color Tint.</param>
		/// <param name="velocity">Particle Velocity.</param>
		public Particle (Texture2D particleImage, Vector2 startPosition, Color tint, Vector2 velocity)
			: base (particleImage, startPosition, tint)
		{
			_velocity = velocity;
			_timeAlive = TimeSpan.Zero;
		}

		public override void Update (GameTime gameTime)
		{
			_position += _velocity;

			_timeAlive += gameTime.ElapsedGameTime;

			//fade lerping logic?
			if (FadeAway)
			{
				_tint.A = (byte)(255.0f / (float)(LifeTime.TotalMilliseconds / _timeAlive.TotalMilliseconds));
			}

			base.Update (gameTime);
		}

		public override void Draw (SpriteBatch batch)
		{
			base.Draw (batch);
		}
	}
}

