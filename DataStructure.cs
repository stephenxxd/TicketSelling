using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tickets
{
    public class TotalSelling
    {
        public List<TicketsPerSeat> Sellings { get; set; }
        public bool SellOut { get; set; }
        
        public int UnservicableCount { get; set; }  // the count for continuingly not servicable, if exceed the set threshold, assuming no tickets could be selled any more.
    }
    public class TicketsPerSeat :IComparable
    {
        public List<Ticket> Selled { get; set; }

        public int SeatSerial { get; set; }

        public int MinAvailableStop
        {
            get
            {
                if (Selled.Any())
                {
                    return Selled[Selled.Count() - 1].End;
                }
                return 1;
            }
        }

        int IComparable.CompareTo(object obj)
        {
            var toCompare = obj as TicketsPerSeat;
            if (toCompare == null)
                return -1;
            return toCompare.MinAvailableStop -this.MinAvailableStop ;
        }
    }
    public class Ticket 
    {
        public int Start { get; set; }
        public int End { get; set; }
         
       
    }


}
