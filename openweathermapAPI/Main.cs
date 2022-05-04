using System;

namespace openweathermapAPI
{
    public class Main
    {


        // Temp is in Kelvin
        // conversion K -> C

        private float _temp = 0;
        public float temp
        {

            get{ return (float)Math.Round(_temp - 273.15, 2); }

            set { _temp = value; }
        }

        private float _feels_like = 0;
        public float feels_like
        {
            get { return (float) Math.Round(_feels_like - 273.15, 2); }
            set { _feels_like = value; }
        }

        private float _temp_min = 0;
        public float temp_min
        {
            get{return (float)Math.Round(_temp_min - 273.15, 2); }
            set { _temp_min = value; }
        }

        private float _temp_max = 0;
        public float temp_max
        {
            get { return (float)Math.Round(_temp_max - 273.15, 2); }
            set { _temp_max = value; }
        }

        
        public float pressure { get; set; }
        public float humidity { get; set; }
    }
}