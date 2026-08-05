using System;

namespace APKToolGUI.ApkTool
{
    public class KeyProfile
    {
        public string ProfileName { get; set; } = "";
        public string KeyType { get; set; } = "JKS"; // "JKS" or "PK8_PEM"
        public string OutputPath { get; set; } = "";
        public string Alias { get; set; } = "key0";
        public string StorePass { get; set; } = "";
        public string KeyPass { get; set; } = "";
        public string Algorithm { get; set; } = "RSA";
        public int KeySize { get; set; } = 2048;
        public int ValidityYears { get; set; } = 25;
        public string CN { get; set; } = "";
        public string OU { get; set; } = "";
        public string O { get; set; } = "";
        public string L { get; set; } = "";
        public string ST { get; set; } = "";
        public string C { get; set; } = "IN";
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public override string ToString()
        {
            return string.IsNullOrWhiteSpace(ProfileName) ? OutputPath : ProfileName;
        }
    }
}
