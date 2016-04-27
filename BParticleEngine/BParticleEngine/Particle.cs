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
        private bool _dead;
        public bool Dead
        {
            get { return _dead; }
        }
            
		private bool _fadeOut;
		public bool FadeOut
        {
            get{ return _fadeOut; }
            set{ _fadeOut = value; }
        }

		private TimeSpan _timeAlive;
		private TimeSpan _lifeTime;
		public TimeSpan LifeTime
        {
			get{ return _lifeTime; }
			set{_lifeTime = value;}
		}
		private Vector2 _velocity;
		public Vector2 Velocity
		{
			get{ return _velocity; }
			set{ _velocity = value; }
		}

        private float _gravForce;
        public float GravityForce
        {
            get{ return _gravForce; }
            set{ _gravForce = value; }
        }
        private bool _gravity;
        public bool UseGravity
        {
            get{ return _gravity; }
            set{ _gravity = value; }
        }

		/// <summary>
		/// Initializes a single particle
		/// </summary>
		/// <param name="particleImage">Particle image.</param>
		/// <param name="startPosition">Start position.</param>
		/// <param name="tint">Color Tint.</param>
		/// <param name="velocity">Particle Velocity.</param>
        public Particle (Texture2D particleImage, Vector2 startPosition, Color tint, Vector2 velocity, TimeSpan lifeTime)
			: base (particleImage, startPosition, tint)
		{
			_velocity = velocity;
			_timeAlive = TimeSpan.Zero;
            _lifeTime = lifeTime;
            _fadeOut = false;
            _gravForce = .1f;
            _gravity = false;
            UseCenterOrigin = true;
		}

		public override void Update (GameTime gameTime)
		{
			_position += _velocity;

            if (_gravity)
            {
                _velocity.Y += _gravForce;
            }

			_timeAlive += gameTime.ElapsedGameTime;

			//fade lerping logic
            if (_fadeOut)
			{
                _tint.A = (byte)(255.0f - MathHelper.Lerp(0f, 255.0f, (float)(_timeAlive.TotalMilliseconds / _lifeTime.TotalMilliseconds)));

                if (_timeAlive >= _lifeTime)
                {
                    _dead = true;
                }
			}

			base.Update (gameTime);
		}

		public override void Draw (SpriteBatch batch)
		{
            if (!_dead)
            {
                base.Draw (batch);
            }
		}
	}
}

