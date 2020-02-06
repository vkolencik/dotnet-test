using System.Collections.Generic;
using System.Linq;

namespace Domain
{
    public class SortingHat
    {
        public IDictionary<Wizard, House> Sort(IList<Wizard> students)
        {
            return students.ToDictionary(w => w, w => House.Slytherin);
        }
    }
}