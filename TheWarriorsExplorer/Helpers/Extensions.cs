namespace TheWarriorsExplorer
{
    public static class ByteArrayExtensions
    {
        public static byte[] PadToMultipleOf(this byte[] source, int multiple, byte paddingByte = 0)
        {
            int length = source.Length;
            int remainder = length % multiple;

            if (remainder == 0)
            {
                return source;
            }

            int paddingLength = multiple - remainder;
            byte[] paddedArray = new byte[length + paddingLength];

            Array.Copy(source, paddedArray, length);

            for (int iterator = length; iterator < length + paddingLength; iterator++)
            {
                paddedArray[iterator] = paddingByte;
            }

            return paddedArray;
        }
    }
}
