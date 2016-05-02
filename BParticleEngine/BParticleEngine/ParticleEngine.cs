using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using BLibMonoGame;

namespace BParticleEngine
{
	public class ParticleEngine
	{
        private Particle[] _particlePool;

        private List<Particle> _particles;
        public List<Particle> Particles 
        {
            get { return _particles; }
        }

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

        private float _rotation;
        public float Rotation
        {
            get{ return _rotation; }
            set{ _rotation = value; }
        }

        private Color _particleTint;
        public Color TintColor
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

        private float _targetAlpha;
        /// <summary>
        /// Gets or sets the target alpha. This value is clamped between 0, 255
        /// </summary>
        /// <value>The target alpha.</value>
        public float TargetAlpha
        {
            get{ return _targetAlpha; }
            set{ _targetAlpha = value; _targetAlpha = MathHelper.Clamp(_targetAlpha, 0, 255);}
        }

        private Vector2 _scale;
        public Vector2 Scale
        {
            get{ return _scale; }
            set{ _scale = value; }
        }

        private Vector2 _targetScale;
        public Vector2 TargetScale
        {
            get{ return _targetScale; }
            set{ _targetScale = value; }
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

        private float? _angleToShoot = null;
        public float? AngleToShoot
        {
            get{ return _angleToShoot.Value; }
            set{ _angleToShoot = value; }
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

        private float _targetRotation;
        public float TargetRotation
        {
            get{ return _targetRotation; }
            set{ _targetRotation = value; }
        }

        private float _gravityScale;
        public float GravityScale
        {
            get{ return _gravityScale; }
            set{ _gravityScale = value; }
        }

        private float _rotateToShootAngle;
        public float RotateTowardsShootAngle
        {
            get{ return _rotateToShootAngle; }
            set{ _rotateToShootAngle = value; }
        }

        private bool _randomizeSpeed;

        /// <summary>
        /// Gets or sets a value indicating whether the particle shoots at a random speed between 0, speed inclusive
        /// </summary>
        /// <value></value>
        public bool RandomSpeed
        {
            get{ return _randomizeSpeed; }
            set{ _randomizeSpeed = value; }
        }

        public int ParticlePoolCount
        {
            get{ return _particlePool.Length; }
        }

        public int MaxParticles
        {
            get
            { 
                if (LifeTime < SpawnRate)
                    return SpawnCount;
                return (LifeTime.TotalMilliseconds.ToInt() / (SpawnRate.TotalMilliseconds.ToInt())) * SpawnCount;
            }
        }

        private TimeSpan _lifeTimeDeviation;
        public TimeSpan LifeTimeDeviation
        {
            get{ return _lifeTimeDeviation; }
            set{ _lifeTimeDeviation = value; }
        }

        private Vector2 _particleSpeed = Vector2.Zero;

        public ParticleEngine (Vector2 position, Vector2 scale, Color tint, int particlesPerSpawn, TimeSpan particleLifeTime, TimeSpan spawnRate,  params Texture2D[] particleImages)
		{
            _position = position;
			_particles = new List<Particle> ();
            _particleImages = particleImages;
            _lifeTime = particleLifeTime;
            _scale = scale;
            _random = new Random();
            _autoSpawn = true;
            _spawnCount = particlesPerSpawn;
            _spawnRate = spawnRate;
            _speed = 3;
            _useGravity = false;
            _gravityScale = 1;
            _particleTint = tint;
            _targetAlpha = tint.A;
            _scale = scale;
            _targetScale = scale;
            _autoSpawn = true;
            _targetRotation = 0;
            _rotation = 0;
            _randomizeSpeed = false;

            //the pool of particles will be the 1.5 times the maximum number of particles will ever be on screen
            //with the current spawnrate, spawncount, and lifetime
            _particlePool = new Particle[0];

            ExpandParticlePool();
		}

        private Particle GetAvailableParticle()
        {
            for (int i = 0; i < _particlePool.Length; i++)
            {
                if (!_particlePool[i].InUse)
                {
                    _particlePool[i].InUse = true;
                    return _particlePool[i];
                }
            }
            //if we get here then there are no more particles to get.
            //Expand the array here?
            int newestParticle = _particlePool.Length;
            ExpandParticlePool();

            return _particlePool[newestParticle];
        }

        private void ExpandParticlePool()
        {
            //if this ever gets called then either SpawnCount, LifeTime, or SpawnRate were changed,
            //recalculate the new max size of the array
            Particle[] newPool = new Particle[(MaxParticles * 1.5f).ToInt()];
            _particlePool.CopyTo(newPool, 0);
            int newStartIndex = _particlePool.Length;
            _particlePool = newPool;
            for (int i = newStartIndex; i < _particlePool.Length; i++)
            {
                _particlePool[i] = new Particle(_particleImages[_random.Next(0, _particleImages.Length)], _position, _particleTint, _particleSpeed, _scale, _rotation, _lifeTime);
            }
        }

        public void AddParticle()
        {
            //has the programmer set an angle to shoot at?
            if (!_angleToShoot.HasValue)
            {
                //if not, set the particles speed to randomDirection * speed
                float randomAngle = _random.Next(0, 361);

                _particleSpeed = randomAngle.DegreeToVector();

                _particleSpeed.Normalize();

                _particleSpeed *= _speed;
            }
            else
            {
                //if the angle to shoot was set, shoot at that angle
                _particleSpeed = (_angleToShoot.Value + _random.Next(_angleDeviation * -1, _angleDeviation+1)%360).DegreeToVector();

                _particleSpeed.Normalize();
                if (!_randomizeSpeed)
                {
                    _particleSpeed *= _speed;
                }
                else
                {
                    _particleSpeed *= new Vector2(_random.Next(0, (int)_speed + 1), _random.Next(0, (int)_speed + 1));
                }
            }

            _particles.Add(GetAvailableParticle());

            Particle newParticle = _particles[_particles.Count - 1];

            newParticle.Texture = _particleImages[_random.Next(0, _particleImages.Length)];
            newParticle.Position = _position;
            newParticle.Tint = _particleTint;
            newParticle.Velocity = _particleSpeed;
            newParticle.Scale = _scale;
            newParticle.Rotation = _rotation;
            newParticle.LifeTime = _lifeTime + TimeSpan.FromMilliseconds(_random.Next((int)-LifeTimeDeviation.TotalMilliseconds, (int)LifeTimeDeviation.TotalMilliseconds));




            //make proper adjustments to the particle that was added
            newParticle.UseGravity = _useGravity;

            if (_useGravity)
            {
                newParticle.GravityForce *= _gravityScale;
            }

            if (_randomColors)
            {
                newParticle.Tint = new Color(_random.Next(0, 255), _random.Next(0, 255), _random.Next(0, 255), 255);
            }

            newParticle.TargetScale = _targetScale;

            newParticle.TargetAlpha = _targetAlpha;

            newParticle.TargetRotation = _targetRotation;
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
                if (!_particles[i].InUse)
                {
                    _particles[i].Reset();
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

