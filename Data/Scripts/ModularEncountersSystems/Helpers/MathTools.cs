using System;
using System.Collections.Generic;
using System.Text;
using VRageMath;

namespace ModularEncountersSystems.Helpers {
    public static class MathTools {

        private static Random _rnd = new Random();

        public static void ApplyModifier(int value, ModifierEnum modifier, ref int target) {

            if (modifier == ModifierEnum.Set) {

                target = value;
                return;

            }

            if (modifier == ModifierEnum.Add) {

                target += value;
                return;

            }

            if (modifier == ModifierEnum.Subtract) {

                target -= value;
                return;

            }

            if (modifier == ModifierEnum.Multiply) {

                target *= value;
                return;

            }

            if (modifier == ModifierEnum.Divide) {

                target /= value;
                return;

            }

        }

        public static void ApplyModifier(long value, ModifierEnum modifier, ref long target) {

            if (modifier == ModifierEnum.Set) {

                target = value;
                return;

            }

            if (modifier == ModifierEnum.Add) {

                target += value;
                return;

            }

            if (modifier == ModifierEnum.Subtract) {

                target -= value;
                return;

            }

            if (modifier == ModifierEnum.Multiply) {

                target *= value;
                return;

            }

            if (modifier == ModifierEnum.Divide) {

                target /= value;
                return;

            }

        }

        public static void ApplyModifier(double value, ModifierEnum modifier, ref double target) {

            if (modifier == ModifierEnum.Set) {

                target = value;
                return;

            }

            if (modifier == ModifierEnum.Add) {

                target += value;
                return;

            }

            if (modifier == ModifierEnum.Subtract) {

                target -= value;
                return;

            }

            if (modifier == ModifierEnum.Multiply) {

                target *= value;
                return;

            }

            if (modifier == ModifierEnum.Divide) {

                target /= value;
                return;

            }

        }

        public static float ApplyNonFloatMultiplier(float value, float multiplier) {

            return value * multiplier / 100f;
        
        }

        public static float Average(params float[] numbers) {

            float total = 0;

            foreach (var number in numbers)
                total += number;

            return total / numbers.Length;
        
        }

        public static int ClampRandomEven(int value, int min, int max) {

            if (value % 2 != 0)
                return value;

            if (value - 1 < min)
                return value + 1;

            if (value + 1 > max)
                return value - 1;

            if(RandomBool())
                return value + 1;

            return value - 1;

        }

        public static int ClampRandomOdd(int value, int min, int max) {

            var oddValue = value;

            if (value % 2 == 0) {

                if (RandomBool())
                    oddValue++;
                else
                    oddValue--;
            
            }

            if (oddValue < min)
                return min;

            if (oddValue > max)
                return max;

            return oddValue;

        }

        public static bool CompareValues(float providedValue, float comparedValue, CounterCompareEnum compare) {

            if (compare == CounterCompareEnum.GreaterOrEqual)
                return providedValue >= comparedValue;

            if (compare == CounterCompareEnum.Greater)
                return providedValue > comparedValue;

            if (compare == CounterCompareEnum.Equal)
                return providedValue == comparedValue;

            if (compare == CounterCompareEnum.Less)
                return providedValue < comparedValue;

            if (compare == CounterCompareEnum.LessOrEqual)
                return providedValue <= comparedValue;

            return false;

        }

        public static bool CompareValues(int providedValue, int comparedValue, CounterCompareEnum compare) {

            if (compare == CounterCompareEnum.GreaterOrEqual)
                return providedValue >= comparedValue;

            if (compare == CounterCompareEnum.Greater)
                return providedValue > comparedValue;

            if (compare == CounterCompareEnum.Equal)
                return providedValue == comparedValue;

            if (compare == CounterCompareEnum.Less)
                return providedValue < comparedValue;

            if (compare == CounterCompareEnum.LessOrEqual)
                return providedValue <= comparedValue;

            return false;

        }

        public static double Average(params double[] values) {

            double result = 0;

            for (int i = 0; i < values.Length; i++)
                result += values[i];

            return result / values.Length;

        }

        public static int LowestCount(params int[] counts) {

            int lowestCount = -1;

            for (int i = 0; i < counts.Length; i++) {

                if (lowestCount == -1)
                    lowestCount = i;

                if(i < lowestCount)
                    lowestCount = i;

            }

            return lowestCount;
        
        }

        /// <summary>
        /// Calculates Acceleration from Provided Force and Mass
        /// </summary>
        /// <param name="forceNewtons">Total Force in Newtons</param>
        /// <param name="mass">Total Mass in Kilograms</param>
        /// <returns>Acceleration of the Force and Mass in m/s^2</returns>
        public static double CalculateAcceleration(double forceNewtons, double mass) {

            //A = F/M
            return forceNewtons / mass;

        }

        public static double Hypotenuse(double a, double b) {

            var valueA = a * a + b * b;
            return Math.Sqrt(valueA);

        }

        /// <summary>
        /// This method will calculate the distance that a ship will stop at whent Braking Acceleration is applied.
        /// </summary>
        /// <param name="brakingAcceleration">The acceleration amount provided by breaking force</param>
        /// <param name="currentVelocity">Current velocity in m/s</param>
        /// <param name="desiredStopSpeed">What velocity you want to end at (default 0)</param>
        /// <param name="gravityAcceleration">Gravity acceleration (in m/s) that will be factored into the calculation (assuming stopping is in same direction)</param>
        /// <returns>Distance in Meters that Braking Acceleration needs to be applied at</returns>
        public static double StoppingDistance(double brakingAcceleration, double currentVelocity, double desiredStopSpeed = 0, double gravityAcceleration = 0) {

            double acceleration = (Math.Abs(brakingAcceleration) - Math.Abs(gravityAcceleration)) * -1;

            if (acceleration > 0)
                return -1;

            var time = (desiredStopSpeed - currentVelocity) / acceleration;
            var maxDistanceInTime = time * currentVelocity;
            var timeSquared = time * time;
            var timeSqMultipliedByAccel = acceleration * timeSquared;
            var distance = maxDistanceInTime + timeSqMultipliedByAccel / 2;

            return distance;

        }

        /// <summary>
        /// This method will provide a linear interpolation (lerp) of a provided value between a min and max range.
        /// Ex: Providing a value of 5 between -10 and 10 would result in 0.75
        /// </summary>
        /// <param name="minValue">The lowest value used in the range</param>
        /// <param name="maxValue">The highest value used in the range</param>
        /// <param name="providedValue">A value that lives between the provided minValue and maxValue values</param>
        /// <returns>Returns the value in a range between 0-1</returns>
        public static double LerpToMultiplier(double minValue, double maxValue, double providedValue) {

            var range = maxValue - minValue;
            var unit = range / 100;
            var percent = providedValue * unit / maxValue;
            return percent;

        }

        /// <summary>
        /// This method will provide a linear interpolation (lerp) of a provided value between a min and max range.
        /// Ex: Providing a value of 0.25 between -10 and 10 would result in -5
        /// </summary>
        /// <param name="minValue">The lowest value used in the range</param>
        /// <param name="maxValue">The highest value used in the range</param>
        /// <param name="providedMultiplier">A value between 0-1</param>
        /// <returns>Returns the value in the provided range</returns>
        public static double LerpToValue(double minValue, double maxValue, double providedMultiplier) {

            var range = maxValue - minValue;
            var unit = providedMultiplier * range;
            var newValue = minValue + unit;
            return newValue;

        }

        public static void MinMaxRangeSafety(ref double min, ref double max) {

            if (min < max)
                return;

            if (min == max)
                max = min + 1;

            if (min > max) {

                var tempMin = min;
                var tempMax = max;
                min = tempMax;
                max = tempMin;

            }

        }

        public static void MinMaxRangeSafety(ref float min, ref float max) {

            if (min < max)
                return;

            if (min == max)
                max = min + 1;

            if (min > max) {

                var tempMin = min;
                var tempMax = max;
                min = tempMax;
                max = tempMin;

            }

        }

        public static void MinMaxRangeSafety(ref int min, ref int max) {

            if (min < max)
                return;

            if (min == max)
                max = min + 1;

            if (min > max) {

                var tempMin = min;
                var tempMax = max;
                min = tempMax;
                max = tempMin;

            }

        }

        public static int NextIndex(int currentIndex, int count) {

            int newIndex = currentIndex += 1;

            if (newIndex >= count)
                return 0;

            return newIndex;
        
        }

        public static int PreviousIndex(int currentIndex, int count) {

            int newIndex = currentIndex -= 1;

            if (newIndex < 0)
                return (count -= 1);

            return newIndex;

        }

        /// <summary>
        /// This method will convert a radian value into degrees
        /// </summary>
        /// <param name="radians">The value, in radians</param>
        /// <returns>Value in Degrees</returns>
        public static double RadiansToDegrees(double radians) {

            return 180 / Math.PI * radians;

        }

        /// <summary>
        /// This method calculates how long it will take to reach a desired velocity with the provided acceleration and current velocity.
        /// </summary>
        /// <param name="acceleration">Acceleration of object</param>
        /// <param name="targetVelocity">The desired velocity to reach</param>
        /// <param name="currentVelocity">The current velocity of the object</param>
        /// <returns></returns>
        public static double TimeToVelocity(double acceleration, double targetVelocity, double currentVelocity = 0) {

            return Math.Abs((targetVelocity - currentVelocity) / acceleration);

        }

        public static bool WithinMultiplierTolerance(double number, double multiplier, double target) {

            var check = target *= multiplier;

            if (number > check + target)
                return false;

            if (number < target - check)
                return false;

            return true;

        }

        public static bool WithinTolerance(double number, double target, double tolerance) {

            return !UnderTolerance(number, target, tolerance) && !OverTolerance(number, target, tolerance);

        }

        public static bool UnderTolerance(double number, double target, double tolerance) {

            return (number - tolerance) < target;

        }

        public static bool OverTolerance(double number, double target, double tolerance) {

            return (number + tolerance) > target;

        }

        public static int VariantValue(int existingValue, int variant) {

            return _rnd.Next(existingValue - variant, existingValue + variant + 1);

        }

        public static double ValueBetween(double a, double b) {

            if (a == b)
                return a;

            double min = a < b ? a : b;
            double max = a < b ? b : a;
            return min + (max - min) / 2;

        }

        public static int RandomBetween(int a, int b) {

            if (a == b)
                return a;

            int min = a < b ? a : b;
            int max = a < b ? b : a;
            return _rnd.Next(min, max);

        }

        public static float RandomBetween(float a, float b, float divideBy = 1) {

            if (a == b)
                return a;

            float min = a < b ? a : b;
            float max = a < b ? b : a;
            return _rnd.Next((int)min, (int)max) / divideBy;

        }

        public static double RandomBetween(double a, double b, double divideBy = 1) {

            if (a == b)
                return a;

            double min = a < b ? a : b;
            double max = a < b ? b : a;
            return _rnd.Next((int)min, (int)max) / divideBy;

        }

        public static bool RandomBool() {

            return _rnd.Next(0, 2) == 0;

        }

        public static bool RandomChance(int chance) {

            return _rnd.Next(0, chance) == 0;

        }

        public static bool RandomChance(int chance, int ceiling) {

            return _rnd.Next(0, ceiling + 1) <= chance;

        }

        public static int RandomSign() {

            return _rnd.Next(0, 2) == 0 ? -1 : 1;

        }

        public static Vector3I Vector3IPlus(Vector3I vec, int x = 0, int y = 0, int z = 0) {

            var newVec = vec;
            newVec.X += x;
            newVec.Y += y;
            newVec.Z += z;
            return newVec;
        
        }

        public static double GravityToDistance(double gravityMultiplier = 1, double maxGravity = 1, double falloff = 7, double minRadius = 59400, double maxRadius = 67200) {

            if (gravityMultiplier >= maxGravity)
                return maxRadius;

            var multiplier = gravityMultiplier / maxGravity;
            var distanceMultiplier = Math.Pow(multiplier, 1 / -falloff);
            return maxRadius * distanceMultiplier;

        }

        public static bool CloserToZero(int value, int target) {

            if (target >= 0)
                return value <= target;
            else
                return value >= target;
        
        }

        public static bool CloserToZero(float value, float target) {

            if (target >= 0)
                return value <= target;
            else
                return value >= target;

        }

        public static bool CloserToZero(double value, double target) {

            if (target >= 0)
                return value <= target;
            else
                return value >= target;

        }

        public static bool FurtherFromZero(int value, int target) {

            if (target >= 0)
                return value >= target;
            else
                return value <= target;

        }

        public static bool FurtherFromZero(float value, float target) {

            if (target >= 0)
                return value >= target;
            else
                return value <= target;

        }

        public static bool FurtherFromZero(double value, double target) {

            if (target >= 0)
                return value >= target;
            else
                return value <= target;

        }

        public static bool IsValueCloser(double target, double existing, double challenger) {

            var existingDiff = Math.Abs(target - existing);
            var challengeDiff = Math.Abs(target - challenger);
            return challenger < existing;
        
        }

    }
}
