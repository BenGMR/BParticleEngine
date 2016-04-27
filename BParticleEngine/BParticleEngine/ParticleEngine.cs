using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using BLibMonoGame;

namespace BParticleEngine
{
	public class ParticleEngine
	{
        private Random _random;

        private TimeSpan _elapsedTime;

        private bool _useGravity;
        public bool UseGravity
        {
            get { return _useGravity; }
            set { _useGravity = value; }
        }

        private bool _autoSpawn;

        public bool AutoSpawn
        {
            get{ return _autoSpawn; }
            set{ _autoSpawn = value; }
        }

        private IPositionable _followItem;

        public IPositionable FollowItem
        {
            get{ return _followItem; }
            set{ _followItem = value; }
        }

        private Color _particleTint;
        public Color Tint
        {
            get{ return _particleTint; }
            set{ _particleTint = value; }
        }

        private bool _randomColors;
        
        public bool RandomColors
        {
            get{ return _randomColors; }
            set{ _randomColors = value; }
        }

        private bool _fadeOut;
        public bool FadeOut
        {
            get{ return _fadeOut; }
            set{ _fadeOut = value; }
        }

        private Vector2 _scale;
        public Vector2 Scale
        {
            get{ return _scale; }
            set{ _scale = value; }
        }

        private TimeSpan _spawnRate;
        public TimeSpan SpawnRate
        {
            get{ return _spawnRate; }
            set{ _spawnRate = value; }
        }
           
        private int _spawnCount;
        public int SpawnCount
        {
            get{ return _spawnCount; }
            set{ _spawnCount = value; }
        }
            
        private Texture2D[] _particleImages;

        private TimeSpan _lifeTime;
        public TimeSpan LifeTime
        {
            get { return _lifeTime; }
            set { _lifeTime = value; }
        }

        private Vector2 _position;
        public Vector2 Position
        {
            get{ return _position; }
            set{ _position = value; }
        }

		private List<Particle> _particles;
		public List<Particle> Particles 
		{
			get { return _particles; }
		}

        private float? angleToShoot = null;
        public float? AngleToShoot
        {
            get{ return angleToShoot.Value; }
            set{ angleToShoot = value; }
        }

        private int _angleDeviation;
        public int AngleDeviation
        {
            get{ return _angleDeviation; }
            set{ _angleDeviation = value; }
        }

        private float _speed;
        public float ParticleSpeed
        {
            get{ return _speed; }
            set { _speed = value; }
        }

        private float _gravityScale;
        public float GravityScale
        {
            get{ return _gravityScale; }
            set{ _gravityScale = value; }
        }

        public ParticleEngine (TimeSpan particleLifeTime, params Texture2D[] particleImages)
		{
			_particles = new List<Particle> ();
            _particleImages = particleImages;
            _lifeTime = particleLifeTime;
            _scale = Vector2.One;
            _random = new Random();
            _autoSpawn = true;
            _fadeOut = true;
            _spawnCount = 1;
            _spawnRate = new TimeSpan(0, 0, 0, 0, 100);
            _speed = 3;
            _useGravity = false;
            _gravityScale = 1;
		}

        public void AddParticle()
        {
            Vector2 particleSpeed = Vector2.Zero;

            //has the programmer set an angle to shoot at?
            if (!angleToShoot.HasValue)
            {
                //if not, set the particles speed to randomDirection * speed
                float randomAngle = _random.Next(0, 361);

                particleSpeed = randomAngle.DegreeToVector();

                particleSpeed.Normalize();

                particleSpeed *= _speed;
            }
            else
            {
                //if the angle to shoot was set, shoot at that angle
                particleSpeed = (angleToShoot.Value + _random.Next(_angleDeviation * -1, _angleDeviation+1)%360).DegreeToVector();

                particleSpeed.Normalize();

                particleSpeed *= _speed;
            }

            _particles.Add(new Particle(_particleImages[_random.Next(0, _particleImages.Length)], _position, _particleTint, particleSpeed, _lifeTime));

            Particle newParticle = _particles[_particles.Count - 1];

            newParticle.UseGravity = _useGravity;

            if (_useGravity)
            {
                newParticle.GravityForce *= _gravityScale;
            }

            if (_randomColors)
            {
                newParticle.Tint = new Color(_random.Next(0, 255), _random.Next(0, 255), _random.Next(0, 255), 255);
            }
            newParticle.Scale = _scale;
            newParticle.FadeOut = _fadeOut;

           
        }

		public void Update(GameTime gameTime)
		{
            //is the engine auto spawning particles?
            if (_autoSpawn)
            {
                //keep track of time
                _elapsedTime += gameTime.ElapsedGameTime;

                //is it time to shoot out particles?
                if (_elapsedTime >= _spawnRate)
                {
                    //reset elapsedTime
                    _elapsedTime = TimeSpan.Zero;

                    //spawn X many particles
                    for (int i = 0; i < _spawnCount; i++)
                    {
                        AddParticle();
                    }
                }
            }

            //is there a sprite the engine should follow?
            if (_followItem != null)
            {
                _position = _followItem.Position;
            }
                
			for (int i = 0; i < _particles.Count; i++)
			{
				_particles[i].Update(gameTime);
                if (_particles[i].Dead)
                {
                    _particles.Remove(_particles[i]);
                    i--;
                }
			}
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			for (int i = 0; i < _particles.Count; i++)
			{
				_particles[i].Draw(spriteBatch);
			}
		}
	}
}

