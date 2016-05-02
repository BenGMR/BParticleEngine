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
        private bool _inUse;

        public bool InUse
        {
            get { return _inUse; }
            set{ _inUse = true; }
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
            set{ _lifeTime = value; }
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

        private float _startAlpha;

        public float StartAlpha
        {
            get{ return _startAlpha; }
            set{ _startAlpha = value; }
        }

        private float _targetAlpha;

        public float TargetAlpha
        {
            get{ return _targetAlpha; }
            set
            {
                _targetAlpha = value;
                _targetAlpha = MathHelper.Clamp(_targetAlpha, 0, 255);
            }
        }

        private Vector2 _startScale;

        private Vector2 _targetScale;

        public Vector2 TargetScale
        {
            get{ return _targetScale; }
            set{ _targetScale = value; }
        }

        private float _startRotation;

        private float _targetRotation;
        public float TargetRotation
        {
            get{ return _targetRotation; }
            set{ _targetRotation = value; }
        }

        /// <summary>
        /// Gets the percentage of how close the sprite is to dying.
        /// </summary>
        /// <value>The percentage alive.</value>
        private float PercentageAlive
        {
            get{ return (float)(_timeAlive.TotalMilliseconds / _lifeTime.TotalMilliseconds); }
        }

        public void Reset()
        {
            _timeAlive = TimeSpan.Zero;
        }

        /// <summary>
        /// Initializes a single particle
        /// </summary>
        /// <param name="particleImage">Particle image.</param>
        /// <param name="startPosition">Start position.</param>
        /// <param name="tint">Color Tint.</param>
        /// <param name="velocity">Particle Velocity.</param>
        public Particle(Texture2D particleImage, Vector2 startPosition, Color tint, Vector2 velocity, Vector2 scale, float rotation, TimeSpan lifeTime)
            : base(particleImage, startPosition, tint)
        {
            _inUse = false;
            _velocity = velocity;
            _timeAlive = TimeSpan.Zero;
            _lifeTime = lifeTime;
            _fadeOut = false;
            _gravForce = .1f;
            _gravity = false;
            UseCenterOrigin = true;
            _startAlpha = tint.A;
            _rotation = rotation;
            _startRotation = rotation;
            _startScale = scale;
            _targetScale = scale;
            _scale = scale;
        }

        public override void Update(GameTime gameTime)
        {
            _position += _velocity;

            if (_gravity)
            {
                _velocity.Y += _gravForce;
            }

            _timeAlive += gameTime.ElapsedGameTime;

            _tint.A = (byte)MathHelper.Lerp(_startAlpha, _targetAlpha, PercentageAlive);

            _scale.X = MathHelper.Lerp(_startScale.X, _targetScale.X, PercentageAlive);
            _scale.Y = MathHelper.Lerp(_startScale.Y, _targetScale.Y, PercentageAlive);

            _rotation = MathHelper.Lerp(_startRotation, _targetRotation, PercentageAlive);

            if (_timeAlive >= _lifeTime)
            {
                _inUse = false;
            }

            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch batch)
        {
            if (_inUse)
            {
                base.Draw(batch);
            }
        }
    }
}

