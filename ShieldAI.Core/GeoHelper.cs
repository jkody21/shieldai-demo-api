using System;

namespace ShieldAI.Core
{
    public static class GeoHelper
    {
        public enum UnitOfDistance {
            NotSet,
            Kilometers,
            Miles
        }

        /// <summary>
        /// Gets the bounding box.
        /// </summary>
        /// <param name="longitude">The longitude.</param>
        /// <param name="latitude">The latitude.</param>
        /// <param name="distance">The distance.</param>
        /// <param name="unit">The unit.</param>
        /// <returns></returns>
        public static BoundingBox GetBoundingBox(
            double longitude, 
            double latitude, 
            double distance, 
            UnitOfDistance unit = UnitOfDistance.Miles) {

            double latMin;
            double latMax;
            double lonMin;
            double lonMax;
            double lat1R;
            double lon1R;
            double lat2R;
            double lon2R;
            double lat3R = 0.0;
            double dLonR;
            double maxDistR;
            double dirR;

            var box = new BoundingBox();
            box.StartLatitude = latitude;
            box.StartLongitude = longitude;

            lat1R = Deg2Rad(latitude);
            lon1R = Deg2Rad(longitude);

            if (unit == UnitOfDistance.Kilometers)
                maxDistR = distance * Math.PI / 20001.6;
            else
                maxDistR = distance * Math.PI / 12428.418038654259126700071581961;

            //---------------------------------------------------------------------------------|
            //Determine minimum and maximum latitude and longitude
            //Calculate latitude of north boundary
            dirR = Deg2Rad(0e0);
            lat2R = Math.Asin(Math.Sin(lat1R) * Math.Cos(maxDistR)
                        + Math.Cos(lat1R) * Math.Sin(maxDistR) * Math.Cos(dirR));

            //Convert back to degrees
            latMax = Rad2Deg(lat2R);

            //On northern hemisphere, go east/west from northernmost point
            if (latitude > 0)
                lat3R = lat2R;
            //---------------------------------------------------------------------------------|



            //---------------------------------------------------------------------------------|
            //Calculate latitude of south boundary
            dirR = Deg2Rad(180e0);
            lat2R = Math.Asin(Math.Sin(lat1R) * Math.Cos(maxDistR)
                        + Math.Cos(lat1R) * Math.Sin(maxDistR) * Math.Cos(dirR));

            //Convert back to degrees
            latMin = Rad2Deg(lat2R);

            //On southern hemisphere, go east/west from southernmost point
            if (latitude <= 0)
                lat3R = lat2R;
            //---------------------------------------------------------------------------------|



            //---------------------------------------------------------------------------------|
            //Calculate longitude of west boundary
            dirR = Deg2Rad(90e0);

            //Need latitude first
            lat2R = Math.Asin(Math.Sin(lat3R) * Math.Cos(maxDistR)
                        + Math.Cos(lat3R) * Math.Sin(maxDistR) * Math.Cos(dirR));

            //Calculate longitude difference.
            dLonR = Math.Atan2(Math.Sin(dirR) * Math.Sin(maxDistR) * Math.Cos(lat3R),
            Math.Cos(maxDistR) - Math.Sin(lat3R) * Math.Sin(lat2R));

            //Calculate longitude of new point - ensure result is between -PI and PI.
            lon2R = (Convert.ToDouble(lon1R - dLonR + Math.PI))
                        % Convert.ToDouble(2 * Math.PI) - Math.PI;

            //Convert back to degrees
            lonMin = Rad2Deg(lon2R);
            //---------------------------------------------------------------------------------|


            //---------------------------------------------------------------------------------|
            dirR = Deg2Rad(-90e0);

            //Need latitude first
            lat2R = Math.Asin(Math.Sin(lat3R) * Math.Cos(maxDistR)
                        + Math.Cos(lat3R) * Math.Sin(maxDistR) * Math.Cos(dirR));

            //Calculate longitude difference.
            dLonR = Math.Atan2(Math.Sin(dirR) * Math.Sin(maxDistR) * Math.Cos(lat3R),
                        Math.Cos(maxDistR) - Math.Sin(lat3R) * Math.Sin(lat2R));

            //Calculate longitude of new point - ensure result is between -PI and PI.
            lon2R = (Convert.ToDouble(lon1R - dLonR + Math.PI))
                        % Convert.ToDouble(2 * Math.PI) - Math.PI;

            //Convert back to degrees
            lonMax = Rad2Deg(lon2R);

            //---------------------------------------------------------------------------------|

            box.MaximumLongitude = lonMax;
            box.MinimumLongitude = lonMin;
            box.MinimumLatitude = latMin;
            box.MaximumLatitude = latMax;

            return box;
        }


        /// <summary>
        /// Gets the miles by feet.
        /// </summary>
        /// <param name="feet">The feet.</param>
        /// <returns></returns>
        public static double GetMilesByFeet(double feet) {
            var m = 1.0 / 5280.0 * feet;
            return m;
        }


        /// <summary>
        /// Distances the between locations.
        /// </summary>
        /// <param name="lat1Degrees">The lat1 degrees.</param>
        /// <param name="lon1Degrees">The lon1 degrees.</param>
        /// <param name="lat2Degrees">The lat2 degrees.</param>
        /// <param name="lon2Degrees">The lon2 degrees.</param>
        /// <returns></returns>
        public static double DistanceBetweenLocations(double lat1Degrees,
                                              double lon1Degrees,
                                              double lat2Degrees,
                                              double lon2Degrees) {
            double angle = AngleBetweenLocations(
                lat1Degrees,
                lon1Degrees,
                lat2Degrees,
                lon2Degrees);

            const double circumference = 24830.0; // miles at equator

            return circumference * angle / (2.0 * Math.PI);
        }


        /// <summary>
        /// Angles the between locations.
        /// </summary>
        /// <param name="lat1Degrees">The lat1 degrees.</param>
        /// <param name="lon1Degrees">The lon1 degrees.</param>
        /// <param name="lat2Degrees">The lat2 degrees.</param>
        /// <param name="lon2Degrees">The lon2 degrees.</param>
        /// <returns></returns>
        public static double AngleBetweenLocations(double lat1Degrees,
                                           double lon1Degrees,
                                           double lat2Degrees,
                                           double lon2Degrees) {

            var lat1Radians = Deg2Rad(lat1Degrees);
            var lon1Radians = Deg2Rad(lon1Degrees);
            var lat2Radians = Deg2Rad(lat2Degrees);
            var lon2Radians = Deg2Rad(lon2Degrees);

            var a = lon1Radians - lon2Radians;

            if (a < 0.0) {
                a = -a;
            }

            if (a > Math.PI) {
                a = 2.0 * Math.PI - a;
            }

            return Math.Acos(
                  Math.Sin(lat2Radians) * Math.Sin(lat1Radians) +
                  Math.Cos(lat2Radians) * Math.Cos(lat1Radians) * Math.Cos(a)
                  );
        }


        /// <summary>
        /// Deg2rads the specified deg.
        /// </summary>
        /// <param name="deg">The deg.</param>
        /// <returns></returns>
        private static double Deg2Rad(double deg) {
            return (deg * Math.PI / 180.0);
        }


        /// <summary>
        /// Rad2degs the specified RAD.
        /// </summary>
        /// <param name="rad">The RAD.</param>
        /// <returns></returns>
        private static double Rad2Deg(double rad) {
            return (rad / Math.PI * 180.0);
        }


        /// <summary>
        /// Converts miles to meters
        /// </summary>
        /// <param name="distance"></param>
        /// <returns></returns>
        public static double ConvertMilesToMeters(double distance) {
            return Math.Round((double)(distance) * 1609.34, 2);
        }

    }
}
