// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("0wv2ZiUuEQ6z6baSaKdaYI0uLBrH7n0gdaV4Kgeu7S/w/uip7T7gbSqZu6a7y2ixF85TMioO0SNPfdefbFQjKuNT3TSC+COgkYLcZZ/0ZcG3NDo1Bbc0Pze3NDQ1ji97K1u8WwW3NBcFODM8H7N9s8I4NDQ0MDU2eX5igxBGzE3yk1FHc20OOd58BoLOGRB6fCGs7KqvcBmFVrWAmoewpnBS6sBoEe6OHeLW0Lr5+Yhs9FyEIuv/ZMoaZ1mi5tao/7iJwyHfdKHvDomR9pu3R4alU001y8oawp1Etk7CcQz+Fhv0iRfpQ614/C50E2dagdoFSP1c5fCzhJXD41f47Iya7v0QS6CzLHq4ksSlM+EhPniEkJyPR9ZUFDMrRJ4NPDc2NDU0");
        private static int[] order = new int[] { 2,5,8,3,5,8,13,12,8,9,13,12,13,13,14 };
        private static int key = 53;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
