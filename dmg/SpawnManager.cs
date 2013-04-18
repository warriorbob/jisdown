using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dmg.Domain;

namespace dmg
{
    public class SpawnManager
    {
        private Random rand;
        private int spawnTimer;
        private int threshold;
        public bool IsReady { get; private set; }

        public SpawnManager(int timer, int newThreshold)
        {
            //rand = new Random();
            spawnTimer = timer;
            threshold = newThreshold;
        }

        public void Tick()
        {
            spawnTimer++;
            if (spawnTimer >= threshold)
            {
                IsReady = true;
            }
            else
            {
                IsReady = false;
            }
        }

        public IBaddie PopBaddie(int xPos, int yPos)
        {
            IsReady = false;
            spawnTimer = 0;
            return new BigBaddie(xPos, yPos);
        }
    }
}
