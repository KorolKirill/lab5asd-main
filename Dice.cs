using System;

namespace lab5asd
{
    public class Dice : ICloneable
    {
        public int value = 1;

        public int Roll()
        {
            var random = new Random();
            int value = random.Next(1,7);
            this.value = value;
            return value;
        }

        public object Clone()
        {
            return new Dice() {value = this.value};
        }
    }
}