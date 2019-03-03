using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheFeels {
    public class GameManager {
        public enum FeelsType {
            Good,
            Bad
        };

        public enum FeelsTileType : int {
            FeelsNeutral = 0,
            FeelsGood = 1,
            FeelsBad = 2,
            FeelsNothing = -1
        };
    }
}
