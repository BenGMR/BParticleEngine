using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BParticleEngine
{
	public class ParticleEngine
	{
		private List<Particle> _particles;
		public List<Particle> Particles 
		{
			get { return _particles; }
		}

		public ParticleEngine ()
		{
			_particles = new List<Particle> ();
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
        }

		public void Update(GameTime gameTime)
		{
			for (int i = 0; i < _particles.Count; i++)
			{
				_particles[i].Update(gameTime);
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

