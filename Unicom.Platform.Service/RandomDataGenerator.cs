using System;
using Unicom.Platform.Model;

namespace Unicom.Platform.Service
{
    public class RandomDataGenerator
    {
        public string DevId { get; }

        private int _lastGeneratedValue;

        private readonly int _rangeMinValue;

        private readonly int _rangeMaxValue;

        public RandomDataGenerator(EmsAutoDust autoDust)
        {
            DevId = autoDust.DevSystemCode;
            _rangeMaxValue = (int) autoDust.RangeMinValue;
            _rangeMinValue = (int) autoDust.RangeMaxValue;
            _lastGeneratedValue = new Random().Next(_rangeMaxValue, _rangeMaxValue);
        }

        public float NewValue()
        {
            _lastGeneratedValue += new Random().Next(-50, 50);
            if (_lastGeneratedValue < _rangeMinValue)
            {
                _lastGeneratedValue += 50;
            }
            if (_lastGeneratedValue > _rangeMaxValue)
            {
                _lastGeneratedValue -= 50;
            }

            return _lastGeneratedValue / 1000.0f;
        }
    }
}
