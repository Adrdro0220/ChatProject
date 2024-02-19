using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace User
{
    internal class ByteReader
    {
        public static async Task<byte[]> GetSpecificBYtes(byte[] array, int offset, int arrayLenght)
        {

            byte[] data = new byte[arrayLenght];

            for (int i = 0; i < arrayLenght; i++)
            {
                data[i] = array[offset + i];
            }
            return data;
        }

        public static async Task<int> GetId(byte[] array)
        {
            return BitConverter.ToInt32(array, 4);
        }

        public static async Task<int> GetPayloadLenght(byte[] array)
        {
            return BitConverter.ToInt32(array, 8);
        }
    }
}
