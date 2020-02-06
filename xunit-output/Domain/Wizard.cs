using System;

namespace Domain
{
    public class Wizard
    {
        public Wizard(string name)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }

        public string Name { get; }

        public override bool Equals(object obj)
        {
            switch (obj)
            {
                case null: return false;
                case Wizard other: return this.Name.Equals(other.Name);
                default: return false;
            }
        }

        public override int GetHashCode() => Name.GetHashCode();

        public override string ToString() => Name;
    }
}
