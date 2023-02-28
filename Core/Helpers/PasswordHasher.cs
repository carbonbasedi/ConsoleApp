﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Helpers
{
    public static class PasswordHasher
    {
        public static string Encrypt(string password)
        {
            try
            {
                byte[] encData_byte = new byte[password.Length];
                encData_byte = Encoding.UTF8.GetBytes(password);
                string encodedData = Convert.ToBase64String(encData_byte);
                return encodedData;
            }
            catch (Exception ex)
            {
                throw new Exception("Error in base64Encode" + ex.Message);
            }
        }
        public static string Decrypt(string hashedPassword)
        {
            UTF8Encoding encoder = new UTF8Encoding();
            Decoder utf8Decode = encoder.GetDecoder();
            byte[] toDecode_byte = Convert.FromBase64String(hashedPassword);
            int charCount = utf8Decode.GetCharCount(toDecode_byte, 0, toDecode_byte.Length);
            char[] decoded_char = new char[charCount];
            utf8Decode.GetChars(toDecode_byte, 0, toDecode_byte.Length, decoded_char, 0);
            string result = new String(decoded_char);
            return result;
        }
    }
}