// Copyright (c) 2023  DigitalTwin Technology GmbH
// https://www.digitaltwin.technology/

namespace DigitalTwinTechnology
{
    public static class MathUtilities
    {
        /// <summary>
        /// Get the linear relation give an x value from a line y = mx + b; m = (yf - yo)/(xf - xo)
        /// </summary>
        /// <param name="x">Evaluation parameter</param>
        /// <param name="xo">Minimum x value</param>
        /// <param name="xf">Maximum x value</param>
        /// <param name="yo">Minimim y value</param>
        /// <param name="yf">Maximum y value</param>
        /// <returns></returns>
        public static float LinearRelation(float x, float xo, float xf, float yo, float yf)
        {
            if (xf != xo)
            {
                float m = (yf - yo) / (xf - xo);
                float b = yo - (m * xo);
                return (x * m) + b;
            }
            else
            {
                return yo;
            }
        }

        /// <summary>
        /// Return the linear relation give an x value from a line y = mx + b; m = (yf - yo)/(xf - xo), clamp to [yo, yf]
        /// </summary>
        /// <param name="x">Evaluation parameter</param>
        /// <param name="xo">Minimum x value</param>
        /// <param name="xf">Maximum x value</param>
        /// <param name="yo">Minimim y value</param>
        /// <param name="yf">Maximum y value</param>
        /// <returns></returns>
        public static float LinearRelationClamp(float x, float xo, float xf, float yo, float yf)
        {
            if (xf != xo)
            {
                float m = (yf - yo) / (xf - xo);
                float b = yo - (m * xo);
                float r = (x * m) + b;

                if (r > yf)
                {
                    return yf;
                }
                else if (r < yo)
                {
                    return yo;
                }
                else
                {
                    return r;
                }
            }
            else
            {
                return yo;
            }
        }
    }
}
