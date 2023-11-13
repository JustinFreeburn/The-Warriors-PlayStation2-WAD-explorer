namespace TheWarriorsExplorer
{
    public static class Endianness
    {
        public static uint CheckEndianness(uint uiVariable_)
        {
            if (BitConverter.IsLittleEndian)
            {
                uiVariable_ = uiVariable_ << 8 & 0xFF00FF00 | uiVariable_ >> 8 & 0xFF00FF;
                return uiVariable_ << 16 | uiVariable_ >> 16;
            }
            else
            {
                return uiVariable_;
            }
        }
    }
}
