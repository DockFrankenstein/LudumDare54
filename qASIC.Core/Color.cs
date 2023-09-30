using qASIC.Communication;

namespace qASIC
{
    [Serializable]
    public struct Color : INetworkSerializable
    {
        public Color(byte red, byte green, byte blue) : this(red, green, blue, 255) { }

        public Color(byte red, byte green, byte blue, byte alpha)
        {
            this.red = red;
            this.green = green;
            this.blue = blue;
            this.alpha = alpha;
        }

        public byte red;
        public byte green;
        public byte blue;
        public byte alpha;

        public void Read(Packet packet)
        {
            red = packet.ReadByte();
            green = packet.ReadByte();
            blue = packet.ReadByte();
            alpha = packet.ReadByte();
        }

        public Packet Write(Packet packet) =>
            packet
            .Write(red)
            .Write(green)
            .Write(blue)
            .Write(alpha);
    }
}