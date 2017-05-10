using System;
using Unicom.Platform.Model;

namespace Unicom.Platform.Service
{
    public class RandomDataGenerator
    {
        public string DevId { get; }

        private int _lastGeneratedValue;

        public RandomDataGenerator(EmsAutoDust autoDust)
        {
            DevId = autoDust.DevSystemCode;
            _lastGeneratedValue = new Random().Next((int)autoDust.RangeMinValue, (int)autoDust.RangeMaxValue);
        }

        public float NewValue()
        {
            _lastGeneratedValue += new Random().Next(-50, 50);
            if (_lastGeneratedValue < 0)
            {
                _lastGeneratedValue += 200;
            }
            if (_lastGeneratedValue > 1000)
            {
                _lastGeneratedValue -= 200;
            }

            return _lastGeneratedValue / 1000.0f;
        }
    }
}
