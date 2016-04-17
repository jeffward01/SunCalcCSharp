using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SunCalc.Console
{
    public class SunCalc
    {
        public static double Pi = Math.PI;
        /*     public double sin = Math.Sin();
             public double cos = Math.PI;
             public double tan = Math.PI;
             public double asin = Math.PI;
             public double atan = Math.PI;
             public double acos = Math.Acos();*/
        public static double rad = Math.PI / 180;
        public static double e = rad * 23.4397;
        private static double Deg2Rad = Math.PI / 180.0;
        private static double Rad2Deg = 180.0 / Math.PI;

        public static double dayMs = 1000 * 60 * 60 * 24,
         J1970 = 2440588,
         J2000 = 2451545;

        public int DateToJulianDate(DateTime date)
        {
            int d = date.Day;
            int m = date.Month;
            int y = date.Year;

            int mm, yy;
            int k1, k2, k3;
            int j;

            yy = y - (int)((12 - m) / 10);
            mm = m + 9;
            if (mm >= 12)
            {
                mm = mm - 12;
            }
            k1 = (int)(365.25 * (yy + 4712));
            k2 = (int)(30.6001 * mm + 0.5);
            k3 = (int)((int)((yy / 100) + 49) * 0.75) - 38;
            // 'j' for dates in Julian calendar:
            j = k1 + k2 + d + 59;
            if (j > 2299160)
            {
                // For Gregorian calendar:
                j = j - k3; // 'j' is the Julian date at 12h UT (Universal Time)
            }
            return j;
        }

        public DateTime FromJulianDate(int x)
        {
            int jDate = Convert.ToInt32(x);
            int day = jDate % 1000;
            int year = (jDate - day) / 1000;
            var date1 = new DateTime(year, 1, 1);
            var result = date1.AddDays(day - 1);
            return result;
        }

        public int toDays(DateTime date)
        {
            return Convert.ToInt32(DateToJulianDate(date) - J2000);
        }

        public int rightAscension(double obliquity, double elipticalLongitude)
        {
            return Convert.ToInt32(Math.Atan2(
            Math.Cos(obliquity) * Math.Sin(elipticalLongitude),
            Math.Cos(elipticalLongitude)));

        }

        //Need to add methods for    // Solar Coordinates  
        public int getJulianCenturies(DateTime dateTime)
        {
            double julianDate = 367 * dateTime.Year -
            (int)((7.0 / 4.0) * (dateTime.Year +
            (int)((dateTime.Month + 9.0) / 12.0))) +
            (int)((275.0 * dateTime.Month) / 9.0) +
            dateTime.Day - 730531.5;

            return Convert.ToInt32(julianDate / 36525.0);
        }

        public int getSideRealTimeHours(DateTime dateTime)
        {
            var julianCenturies = getJulianCenturies(dateTime);
            return Convert.ToInt32(6.6974 + 2400.0513 * julianCenturies);
        }

        public int getSideRealTimeHours(int julianCenturies)
        {
            return Convert.ToInt32(6.6974 + 2400.0513 * julianCenturies);
        }

        public int sideRealTimeUT(DateTime date)
        {
            var julianCenturies = getJulianCenturies(date);
            var sideRealTimeHours = getSideRealTimeHours(julianCenturies);

            var sideRealTimeUT = sideRealTimeHours +
                (366.2422 / 365.2422) * (double)date.TimeOfDay.TotalHours;
            return Convert.ToInt32(sideRealTimeUT);
        }

        public int sideRealTimeUT(DateTime date, int sideRealTimeHours)
        {
            var sideRealTimeUT = sideRealTimeHours +
                (366.2422 / 365.2422) * (double)date.TimeOfDay.TotalHours;
            return Convert.ToInt32(sideRealTimeUT);
        }

        public int meanLongetitude(DateTime date)
        {
            var julianCenturies = getJulianCenturies(date);
            return Convert.ToInt32(CorrectAngle(Deg2Rad * (280.466 + 36000.77 * julianCenturies)));
        }

        public int meanLongetitude(int julianCenturies)
        {
            return Convert.ToInt32(CorrectAngle(Deg2Rad * (280.466 + 36000.77 * julianCenturies)));
        }

        public int meanAnomaly(DateTime date)
        {
            var julianCenturies = getJulianCenturies(date);
            return Convert.ToInt32(CorrectAngle(Deg2Rad * (357.529 + 35999.05 * julianCenturies)));
        }

        public int meanAnomaly(int julianCenturies)
        {
            return Convert.ToInt32(CorrectAngle(Deg2Rad * (357.529 + 35999.05 * julianCenturies)));
        }

        public int equationOfCenter(DateTime date, int meanAnomaly)
        {
            var julianCenturies = getJulianCenturies(date);
            return Convert.ToInt32(Deg2Rad * ((1.915 - 0.005 * julianCenturies) *
              Math.Sin(meanAnomaly) + 0.02 * Math.Sin(2 * meanAnomaly)));
        }

        public int equationOfCenter(int julianCenturies, int meanAnomaly)
        {
            return Convert.ToInt32(Deg2Rad * ((1.915 - 0.005 * julianCenturies) *
              Math.Sin(meanAnomaly) + 0.02 * Math.Sin(2 * meanAnomaly)));
        }

        public int eclipticLongitude(int meanLongitude, int equationOfCenter)
        {
            return Convert.ToInt32(CorrectAngle(meanLongitude + equationOfCenter));
        }

        public int obliquity(DateTime date)
        {
            var julianCenturies = getJulianCenturies(date);
            return Convert.ToInt32((23.439 - 0.013*julianCenturies)*Deg2Rad);
        }

        public int obliquity(int julianCenturies)
        {
            return Convert.ToInt32((23.439 - 0.013 * julianCenturies) * Deg2Rad);
        }

        public int declination(int rightAscention, int obliquity)
        {
            double rightAscention1 = Convert.ToDouble(rightAscention);
            double obliquity1 = Convert.ToDouble(obliquity);

            return Convert.ToInt32(Math.Asin(
                Math.Sin(rightAscention1) *Math.Sin(obliquity1)));
        }

        public int declination(double rightAscention, double obliquity)
        {
            return Convert.ToInt32(Math.Asin(
                Math.Sin(rightAscention) * Math.Sin(obliquity)));
        }

        public int hourAngle(int sideRealTime, int rightAscension)
        {
            var hourAngle = CorrectAngle(sideRealTime * Deg2Rad) - rightAscension;

            if (hourAngle > Math.PI)
            {
                hourAngle -= 2 * Math.PI;
            }
            return Convert.ToInt32(hourAngle);
        }

        public int altitude(int latitude, int hourAngle, int declination)
        {
            var altitude = Math.Asin(Math.Sin(latitude * Deg2Rad) *
             Math.Sin(declination) + Math.Cos(latitude * Deg2Rad) *
             Math.Cos(declination) * Math.Cos(hourAngle));
            return Convert.ToInt32(altitude);
        }

        public int aziNom(int hourAngle)
        {
            return Convert.ToInt32(-Math.Sin(hourAngle));
        }

        public int aziDenom(int declination, int latitude, int hourAngle)
        {
            double aziDenom =
            Math.Tan(declination) * Math.Cos(latitude * Deg2Rad) -
            Math.Sin(latitude * Deg2Rad) * Math.Cos(hourAngle);
            return Convert.ToInt32(aziDenom);
        }


        public int azimuth(int aziNom, int aziDenom)
        {

            double azimuth = Math.Atan(aziNom / aziDenom);

            if (aziDenom < 0) // In 2nd or 3rd quadrant  
            {
                azimuth += Math.PI;
            }
            else if (aziNom < 0) // In 4th quadrant  
            {
                azimuth += 2 * Math.PI;
            }
            return Convert.ToInt32(azimuth);
        }

        public int astroRefraction(int alititude)
        {
            if (alititude < 0)
            {
                alititude = 0;
            }
            return Convert.ToInt32(0.0002967/Math.Tan(alititude + 0.00312536/(alititude + 0.08901179)));
        }
        
        //TODO: LEft off here






/*! 
* \brief Calculates the sun light. 
* 
* CalcSunPosition calculates the suns "position" based on a 
* given date and time in local time, latitude and longitude 
* expressed in decimal degrees. It is based on the method 
* found here: 
* http://www.astro.uio.no/~bgranslo/aares/calculate.html 
* The calculation is only satisfiably correct for dates in 
* the range March 1 1900 to February 28 2100. 
* \param dateTime Time and date in local time. 
* \param latitude Latitude expressed in decimal degrees. 
* \param longitude Longitude expressed in decimal degrees. 
*/
public static void CalculateSunPosition(DateTime dateTime, double latitude, double longitude)
        {
            // Convert to UTC  
            dateTime = dateTime.ToUniversalTime();

            // Number of days from J2000.0.  
            double julianDate = 367 * dateTime.Year -
                (int)((7.0 / 4.0) * (dateTime.Year +
                (int)((dateTime.Month + 9.0) / 12.0))) +
                (int)((275.0 * dateTime.Month) / 9.0) +
                dateTime.Day - 730531.5;

            double julianCenturies = julianDate / 36525.0;

            // Sidereal Time  
            double siderealTimeHours = 6.6974 + 2400.0513 * julianCenturies;

            double siderealTimeUT = siderealTimeHours +
                (366.2422 / 365.2422) * (double)dateTime.TimeOfDay.TotalHours;

            double siderealTime = siderealTimeUT * 15 + longitude;

            // Refine to number of days (fractional) to specific time.  
            julianDate += (double)dateTime.TimeOfDay.TotalHours / 24.0;
            julianCenturies = julianDate / 36525.0;

            // Solar Coordinates  
            double meanLongitude = CorrectAngle(Deg2Rad *
                (280.466 + 36000.77 * julianCenturies));

            double meanAnomaly = CorrectAngle(Deg2Rad *
                (357.529 + 35999.05 * julianCenturies));

            double equationOfCenter = Deg2Rad * ((1.915 - 0.005 * julianCenturies) *
                Math.Sin(meanAnomaly) + 0.02 * Math.Sin(2 * meanAnomaly));

            double elipticalLongitude =
                CorrectAngle(meanLongitude + equationOfCenter);

            double obliquity = (23.439 - 0.013 * julianCenturies) * Deg2Rad;

            // Right Ascension  
            double rightAscension = Math.Atan2(
                Math.Cos(obliquity) * Math.Sin(elipticalLongitude),
                Math.Cos(elipticalLongitude));

            double declination = Math.Asin(
                Math.Sin(rightAscension) * Math.Sin(obliquity));

            // Horizontal Coordinates  
            double hourAngle = CorrectAngle(siderealTime * Deg2Rad) - rightAscension;

            if (hourAngle > Math.PI)
            {
                hourAngle -= 2 * Math.PI;
            }

            double altitude = Math.Asin(Math.Sin(latitude * Deg2Rad) *
                Math.Sin(declination) + Math.Cos(latitude * Deg2Rad) *
                Math.Cos(declination) * Math.Cos(hourAngle));

            // Nominator and denominator for calculating Azimuth  
            // angle. Needed to test which quadrant the angle is in.  
            double aziNom = -Math.Sin(hourAngle);
            double aziDenom =
                Math.Tan(declination) * Math.Cos(latitude * Deg2Rad) -
                Math.Sin(latitude * Deg2Rad) * Math.Cos(hourAngle);

            double azimuth = Math.Atan(aziNom / aziDenom);

            if (aziDenom < 0) // In 2nd or 3rd quadrant  
            {
                azimuth += Math.PI;
            }
            else if (aziNom < 0) // In 4th quadrant  
            {
                azimuth += 2 * Math.PI;
            }

            // Altitude  
            System.Console.WriteLine("Altitude: " + altitude * Rad2Deg);

            // Azimut  
            System.Console.WriteLine("Azimuth: " + azimuth * Rad2Deg);
        }

























        /*! 
    * \brief Corrects an angle. 
    * 
    * \param angleInRadians An angle expressed in radians. 
    * \return An angle in the range 0 to 2*PI. 
    */
        private static double CorrectAngle(double angleInRadians)
        {
            if (angleInRadians < 0)
            {
                return 2 * Math.PI - (Math.Abs(angleInRadians) % (2 * Math.PI));
            }
            else if (angleInRadians > 2 * Math.PI)
            {
                return angleInRadians % (2 * Math.PI);
            }
            else
            {
                return angleInRadians;
            }
        }

        private static double CorrectAngle(int angleInRadians)
        {
            double angleInRadians1 = Convert.ToDouble(angleInRadians);
            if (angleInRadians1 < 0)
            {
                return 2 * Math.PI - (Math.Abs(angleInRadians1) % (2 * Math.PI));
            }
            else if (angleInRadians1 > 2 * Math.PI)
            {
                return angleInRadians1 % (2 * Math.PI);
            }
            else
            {
                return angleInRadians1;
            }
        }

    }
}
