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
            rand = new Random();
            spawnTimer = timer;
            threshold = newThreshold;
        }

        public void Tick()
        {
            if (spawnTimer >= threshold)
            {
                IsReady = true;
            }
            else
            {
                IsReady = false;
            }
            spawnTimer++;
        }

        public IBaddie PopBaddie(int xPos, int yPos)
        {
            return new Baddie(xPos, yPos);
            IsReady = false;
            spawnTimer = 0;
        }
    }
}
