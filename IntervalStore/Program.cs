using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntervalStore
{
    class Program
    {
        static void Main(string[] args)
        {
            var interval = new Interval { lowerBound = 1, upperBound = 3 };
            var intervalStore = new IntervalStore();
            intervalStore.AddInterval(interval);

            intervalStore.OutputInterval();
            Console.ReadKey();

            interval = new Interval { lowerBound = 5, upperBound = 7 };
            intervalStore.AddInterval(interval);

            intervalStore.OutputInterval();
            Console.ReadKey();

            interval = new Interval { lowerBound = 2, upperBound = 7 };
            intervalStore.AddInterval(interval);

            intervalStore.OutputInterval();
            Console.ReadKey();

            interval = new Interval { lowerBound = 98, upperBound = 99 };
            intervalStore.AddInterval(interval);

            intervalStore.OutputInterval();
            Console.ReadKey();
        }
    }

    public class IntervalStore
    {
        // use a List to store the indexes
        List<int> store = new List<int>();

        public void AddInterval(Interval interval)
        {
            if (store.Count > 0)
            {
                int upperBoundPos = -1;
                int lowerBoundPos = -1;
                bool lowerBoundFound = false;
                
                // now go find the positions for the lower and upper bound of the interval
                for (var i = 0; i < store.Count; i++)
                {
                    if (!lowerBoundFound && interval.lowerBound <= store[i])
                    {
                        lowerBoundPos = i;
                        lowerBoundFound = true;
                    }
                    if (lowerBoundFound && interval.upperBound <= store[i])
                    {
                        upperBoundPos = i;
                        break;
                    }
                }

                // Insert the interval based on the different cases of the positions of the lower and upper bound
                if (lowerBoundPos == -1)
                {
                    this.AddIntervalPlain(interval);
                }
                else if (lowerBoundPos == 0 && upperBoundPos == 0)
                {
                    if (interval.upperBound == store[0])
                    {
                        store[0] = interval.lowerBound;
                    }
                    else
                    {
                        store.Insert(0, interval.upperBound);
                        store.Insert(0, interval.lowerBound);
                    }
                }
                else if (lowerBoundPos == 0 && upperBoundPos == -1)
                {
                    store.Clear();
                    this.AddIntervalPlain(interval);
                }
                else
                {
                    if (lowerBoundPos == upperBoundPos)
                    {
                        if (lowerBoundPos % 2 == 0)
                        {
                            if (interval.upperBound == store[upperBoundPos])
                            {
                                store[upperBoundPos] = interval.lowerBound;
                            }
                            else
                            {
                                store.Insert(lowerBoundPos, interval.upperBound);
                                store.Insert(lowerBoundPos, interval.lowerBound);
                            }
                        }
                    }
                    else
                    {
                        // Every case down here needs to remove everything between the two levels
                        // Move this upfront
                        for (var i = lowerBoundPos; i < upperBoundPos; i++)
                        {
                            store.RemoveAt(lowerBoundPos);
                        }
                        if (lowerBoundPos % 2 == 0)
                        {
                            if (upperBoundPos % 2 == 0)
                            {
                                store.Insert(lowerBoundPos, interval.upperBound);
                                store.Insert(lowerBoundPos, interval.lowerBound);
                            }
                            else
                            {
                                store.Insert(lowerBoundPos, interval.lowerBound);
                            }
                        }
                        else
                        {
                            if (upperBoundPos % 2 == 0)
                            {
                                store.Insert(lowerBoundPos, interval.upperBound);
                            }
                        }
                    }
                }
            }
            else
            {
                this.AddIntervalPlain(interval);
            }
        }

        private void AddIntervalPlain(Interval interval)
        {
            store.Add(interval.lowerBound);
            store.Add(interval.upperBound);
        }

        public void OutputInterval()
        {
            StringBuilder stringBuilder = new StringBuilder();
            for (var i = 0; i < this.store.Count; i++)
            {
                if (i % 2 == 0)
                {
                    stringBuilder.Append("[");
                    stringBuilder.Append(this.store[i]);
                    stringBuilder.Append(",");
                }
                else
                {
                    stringBuilder.Append(this.store[i]);
                    stringBuilder.Append("],");
                }
            }
            if (stringBuilder.ToString() != string.Empty)
            {
                Console.WriteLine(stringBuilder.Remove(stringBuilder.Length - 1, 1).ToString());
            }
        }
    }

    public class Interval
    {
        public int lowerBound { get; set; }

        public int upperBound { get; set; }

        public Interval() { }
    }
}