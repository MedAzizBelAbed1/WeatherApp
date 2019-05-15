using System;
using Xamarin.Forms;

namespace WeatherApp.Services
{
    public class UIServices
    {

        /// <summary>
        /// zoom in & out with scale option to generate animation.
        /// </summary>
        public static void ScaleImage(Image img)
        {
            if (img.Scale > 0.9)
            {
                img.ScaleTo(0.6, 1950);
            }
            else
            {
                img.ScaleTo(1, 1950);
            }
        }
    }
}
