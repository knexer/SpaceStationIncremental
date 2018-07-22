using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    public class Rocket
    {
        public readonly int Capacity = 100;
        private readonly List<ResourceDelta> cargo = new List<ResourceDelta>();
        public IEnumerable<ResourceDelta> Cargo => cargo;

        public bool TryAddCargo(IEnumerable<ResourceDelta> newCargo)
        {
            if (cargo.Sum(delta => delta.Amount) + newCargo.Sum(delta => delta.Amount) > Capacity)
            {
                return false;
            }

            cargo.AddRange(newCargo);
            return true;
        }
    }
}
