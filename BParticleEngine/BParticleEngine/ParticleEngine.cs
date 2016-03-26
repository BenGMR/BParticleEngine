using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BParticleEngine
{
	public class ParticleEngine
	{
        private Random _random;

        private TimeSpan _elapsedTime;

        private bool _autoSpawn;

        public bool AutoSpawn
        {
            get{ return _autoSpawn; }
            set{ _autoSpawn = value; }
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

        public ParticleEngine (TimeSpan particleLifeTime,params Texture2D[] particleImages)
		{
			_particles = new List<Particle> ();
            _particleImages = particleImages;
            _lifeTime = particleLifeTime;
            _scale = Vector2.One;
            _random = new Random();
            _autoSpawn = true;
            _fadeOut = true;
		}

        public void AddParticle()
        {
            /*
             * What do we need for this?
             * texture <- need a list of textures for this engine. randomly choose a texture
             * where will it shoot? <- some angle
             * time? <- comes from particle engine itself
             * 
             * */

            _particles.Add(new Particle(_particleImages[_random.Next(0, _particleImages.Length)], _position, Color.Red, new Vector2(_random.Next(-5, 5), _random.Next(-5, 5)), _lifeTime));

            Particle newParticle = _particles[_particles.Count - 1];
            if (_randomColors)
            {
                newParticle.Tint = new Color(_random.Next(0, 255), _random.Next(0, 255), _random.Next(0, 255), 255);
            }
            newParticle.Scale = _scale;
            newParticle.FadeOut = _fadeOut;
        }

		public void Update(GameTime gameTime)
		{
            if (_autoSpawn)
            {
                _elapsedTime += gameTime.ElapsedGameTime;
                if (_elapsedTime >= _spawnRate)
                {
                    _elapsedTime = TimeSpan.Zero;
                    AddParticle();
                }
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

