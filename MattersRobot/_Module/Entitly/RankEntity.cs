using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MattersRobot._Module.Entitly
{
    class RankEntity
    {
        public RankEntity(string id,string displayname,int amount)
        {
            this.id = id;
            this.displayname = displayname;
            this.amount = amount;
        }
        public string id { get; set; }
        public string displayname { get; set; }
        public int amount { get; set; }

        public static int findIndex(List<RankEntity> array, string id)
        {
            for(int i = 0; i < array.Count; i++)
            {
                if (array[i].id == id)
                {
                    return i;
                }
            }
            
            return -1;
        }
       public static List<RankEntity> sortRank(List<RankEntity> rankEntities)
        {
            List<RankEntity> sorted = new List<RankEntity>(rankEntities.Count);
            
            Console.WriteLine(sorted.Count);
            while (rankEntities.Count > 0)
            {
                int n = ExtractMaxIndex(rankEntities);
                sorted.Add(rankEntities[n]);
            }
            return rankEntities;
        }

        


        public static void Sort(int[] array)
        {
            List<int> sorted = new List<int>(array.Length);
            List<int> unsorted = array.ToList();
            Console.WriteLine("已排序 "+sorted.Count());
            foreach(int x in sorted)
            {
                Console.Write(x+", ");
            }
            Console.WriteLine("未排序" + unsorted.Count());
            foreach (int x in unsorted)
            {
                Console.Write(x + ", ");
            }
            
            while (unsorted.Count > 0)
            {
                int n = ExtractMin(unsorted);
                sorted.Add(n);
            }
            sorted.ToArray().CopyTo(array, 0);
        }

        private static int ExtractMin(List<int> unsorted)
        {
            int index = 0, min = unsorted[0];
            for (int i = 0; i < unsorted.Count; ++i)
            {
                if (unsorted[i] < min)
                {
                    index = i;
                    min = unsorted[i];
                }
            }
            unsorted.RemoveAt(index);
            return min;
        }
        private static int ExtractMaxIndex(List<RankEntity> unsorted)
        {
            int index = 0, max = unsorted[0].amount;

            for (int i = 0; i < unsorted.Count; ++i)
            {
                if (unsorted[i].amount > max)
                {
                    index = i;

                }
            }
            unsorted.RemoveAt(index);
            return index;
        }

    }
}
