using System.Linq;

namespace Utilities {
    public class DeterministicHashCode {
        public static int Hash(string str) {
            return str.Select((char ch) => (int)ch).Sum();
        }
    }
}