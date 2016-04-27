using System;
using Microsoft.Xna.Framework;
using BLibMonoGame;

namespace BParticleEngine
{
    public static class TestExtensions
    {
        public static Vector2 RadianToVector(this float radianToConvert)
        {

            return new Vector2((float)Math.Sin(radianToConvert), (float)-Math.Cos(radianToConvert));
        }
        public static Vector2 DegreeToVector(this float degreeToConvert)
        {
            degreeToConvert *= (Math.PI.ToFloat()/180);
            return new Vector2((float)Math.Sin(degreeToConvert), (float)-Math.Cos(degreeToConvert));
        }

        public static double VectorToRadianAngle(this Vector2 vectorToConvert)
        {
            return Math.Atan2(vectorToConvert.X, -vectorToConvert.Y);
        }

        public static double VectorToDegreeAngle(this Vector2 vectorToConvert)
        {
            return (Math.Atan2(vectorToConvert.X, -vectorToConvert.Y) * (180 / Math.PI.ToFloat()));
        }
    }
}

