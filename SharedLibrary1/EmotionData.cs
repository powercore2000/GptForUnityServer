using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SharedLibrary
{
    public class EmotionData
    {
        public string EmotionName { get; set; }
        public float EmotionValue { get; set; }
        public int Value
        {
            get { return Mathf.FloorToInt(EmotionValue); }
            set { EmotionValue = value; }

        }

        public EmotionData(string name, int value)
        {

            EmotionName = name;
            EmotionValue = value;
        }

        public EmotionData(string name, float value)
        {

            EmotionName = name;
            EmotionValue = value;
        }

        public int Compare(EmotionData x, EmotionData y)
        {
            // Compare based on the int values
            return x.EmotionValue.CompareTo(y.EmotionValue);
        }

        public override string ToString()
        {
            return ($"Emotion Name : {EmotionName} || EmotionValue : {EmotionValue} || Value : {Value}");
        }
    }
}
