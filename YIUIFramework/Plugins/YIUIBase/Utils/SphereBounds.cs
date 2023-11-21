using UnityEngine;

namespace YIUIFramework
{
    /// <summary>
    /// The sphere bounds.
    /// </summary>
    public struct SphereBounds
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SphereBounds"/> struct.
        /// </summary>
        public SphereBounds(Vector3 center, float radius)
        {
            Center = center;
            Radius = radius;
        }

        /// <summary>
        /// Gets or sets the center of this sphere.
        /// </summary>
        public Vector3 Center { get; set; }

        /// <summary>
        /// Gets or sets the radius of this sphere.
        /// </summary>
        public float Radius { get; set; }

        /// <summary>
        /// Check whether intersects with other sphere.
        /// </summary>
        public bool Intersects(SphereBounds bounds)
        {
            var sqrDistance = (Center - bounds.Center).sqrMagnitude;
            var minDistance = Radius + bounds.Radius;
            return sqrDistance <= minDistance * minDistance;
        }

        /// <summary>
        /// Check whether intersects with other AABB.
        /// </summary>
        public bool Intersects(Bounds bounds)
        {
            // Check if the sphere is inside the AABB
            if (bounds.Contains(Center))
            {
                return true;
            }

            // Check if the sphere and the AABB intersect.
            var boundsMin = bounds.min;
            var boundsMax = bounds.max;

            float s = 0.0f;
            float d = 0.0f;
            if (Center.x < boundsMin.x)
            {
                s = Center.x - boundsMin.x;
                d += s * s;
            }
            else if (Center.x > boundsMax.x)
            {
                s = Center.x - boundsMax.x;
                d += s * s;
            }

            if (Center.y < boundsMin.y)
            {
                s = Center.y - boundsMin.y;
                d += s * s;
            }
            else if (Center.y > boundsMax.y)
            {
                s = Center.y - boundsMax.y;
                d += s * s;
            }

            if (Center.z < boundsMin.z)
            {
                s = Center.z - boundsMin.z;
                d += s * s;
            }
            else if (Center.z > boundsMax.z)
            {
                s = Center.z - boundsMax.z;
                d += s * s;
            }

            return d <= Radius * Radius;
        }
    }
}