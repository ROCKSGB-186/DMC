namespace DMC.Helper
{
    /// <summary>
    /// 编码帮助类
    /// </summary>
    public static class EncodingHelper
    {
        private static byte UTF8CharacterMask1Byte = 0b1000_0000;
        private static byte Valid1Byte = 0b0000_0000;//0b0xxx_xxxx

        private static byte UTF8CharacterMask2Byte = 0b1110_0000;
        private static byte Valid2Byte = 0b1100_0000;//0b110x_xxxx

        private static byte UTF8CharacterMask3Byte = 0b1111_0000;
        private static byte Valid3Byte = 0b1110_0000;//0b1110_xxxx

        private static byte UTF8CharacterMask4Byte = 0b1111_1000;
        private static byte Valid4Byte = 0b1111_0000;//0b1111_0xxx

        private static byte UTF8CharacterMaskForExtraByte = 0b1100_0000;
        private static byte ValidExtraByte = 0b1000_0000;//0b10xx_xxxx

        /// <summary>
        /// 验证字节数组是不是UTF-8编码
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static bool IsValidUTF8ByteArray(byte[] bytes)
        {
            short extraByteCount = 0;

            foreach (byte bt in bytes)
            {

                if (extraByteCount > 0)
                {
                    extraByteCount--;

                    // Extra Byte Pattern.
                    if ((bt & UTF8CharacterMaskForExtraByte) != ValidExtraByte)
                        return false;
                    continue;
                }
                else
                {
                    // 1 Byte Pattern.
                    if ((bt & UTF8CharacterMask1Byte) == Valid1Byte)
                    {
                        continue;
                    }

                    // 2 Bytes Pattern.
                    if ((bt & UTF8CharacterMask2Byte) == Valid2Byte)
                    {
                        extraByteCount = 1;
                        continue;
                    }

                    // 3 Bytes Pattern.
                    if ((bt & UTF8CharacterMask3Byte) == Valid3Byte)
                    {
                        extraByteCount = 2;
                        continue;
                    }

                    // 4 Bytes Pattern.
                    if ((bt & UTF8CharacterMask4Byte) == Valid4Byte)
                    {
                        extraByteCount = 3;
                        continue;
                    }

                    // invalid UTF8-Bytes.
                    return false;
                }
            }

            return extraByteCount == 0;
        }

    }
}
