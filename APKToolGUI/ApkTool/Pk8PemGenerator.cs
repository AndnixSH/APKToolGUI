using System;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace APKToolGUI.ApkTool
{
    public class Pk8PemGenerator
    {
        public static (bool Success, string Message) GeneratePk8Pem(KeyProfile profile, string outputPk8Path, string outputPemPath)
        {
            try
            {
                int keySize = profile.KeySize > 0 ? profile.KeySize : 2048;
                int years = Math.Max(1, profile.ValidityYears);

                using (RSA rsa = RSA.Create(keySize))
                {
                    string dname = KeyToolWrapper.BuildDName(profile);
                    var req = new CertificateRequest(
                        new X500DistinguishedName(dname),
                        rsa,
                        HashAlgorithmName.SHA256,
                        RSASignaturePadding.Pkcs1);

                    var cert = req.CreateSelfSigned(
                        DateTimeOffset.UtcNow.AddDays(-1),
                        DateTimeOffset.UtcNow.AddYears(years));

                    // 1. Export Certificate as X.509 PEM (.x509.pem)
                    byte[] certBytes = cert.Export(X509ContentType.Cert);
                    string b64Cert = Convert.ToBase64String(certBytes, Base64FormattingOptions.InsertLineBreaks);
                    string pemContent = $"-----BEGIN CERTIFICATE-----\n{b64Cert}\n-----END CERTIFICATE-----\n";
                    File.WriteAllText(outputPemPath, pemContent, Encoding.ASCII);

                    // 2. Export Private Key as PKCS#8 DER (.pk8)
                    byte[] pk8Bytes = EncodePkcs8PrivateKey(rsa.ExportParameters(true));
                    File.WriteAllBytes(outputPk8Path, pk8Bytes);

                    return (true, "PK8 and PEM key pair generated successfully.");
                }
            }
            catch (Exception ex)
            {
                return (false, "PK8/PEM generation failed: " + ex.Message);
            }
        }

        #region ASN.1 PKCS#8 DER Encoding for RSA

        private static byte[] EncodePkcs8PrivateKey(RSAParameters p)
        {
            // Build RSAPrivateKey (PKCS#1) DER Sequence
            byte[] pkcs1 = BuildSeq(
                BuildInt(new byte[] { 0 }), // version 0
                BuildInt(p.Modulus),
                BuildInt(p.Exponent),
                BuildInt(p.D),
                BuildInt(p.P),
                BuildInt(p.Q),
                BuildInt(p.DP),
                BuildInt(p.DQ),
                BuildInt(p.InverseQ)
            );

            // rsaEncryption OID: 1.2.840.113549.1.1.1 + NULL
            byte[] rsaOid = new byte[] { 0x30, 0x0D, 0x06, 0x09, 0x2A, 0x86, 0x48, 0x86, 0xF7, 0x0D, 0x01, 0x01, 0x01, 0x05, 0x00 };

            // PrivateKeyInfo (PKCS#8) DER Sequence
            byte[] pkcs8 = BuildSeq(
                BuildInt(new byte[] { 0 }), // version 0
                rsaOid,
                BuildOctetString(pkcs1)
            );

            return pkcs8;
        }

        private static byte[] BuildInt(byte[] data)
        {
            if (data == null || data.Length == 0) return new byte[] { 0x02, 0x01, 0x00 };

            int start = 0;
            while (start < data.Length - 1 && data[start] == 0) start++;

            bool needPad = (data[start] & 0x80) != 0;
            int len = (data.Length - start) + (needPad ? 1 : 0);

            byte[] body = new byte[len];
            if (needPad)
            {
                body[0] = 0x00;
                Array.Copy(data, start, body, 1, data.Length - start);
            }
            else
            {
                Array.Copy(data, start, body, 0, data.Length - start);
            }

            return BuildDerElement(0x02, body);
        }

        private static byte[] BuildOctetString(byte[] data)
        {
            return BuildDerElement(0x04, data);
        }

        private static byte[] BuildSeq(params byte[][] items)
        {
            int totalLen = 0;
            foreach (var item in items) totalLen += item.Length;

            byte[] content = new byte[totalLen];
            int pos = 0;
            foreach (var item in items)
            {
                Array.Copy(item, 0, content, pos, item.Length);
                pos += item.Length;
            }
            return BuildDerElement(0x30, content);
        }

        private static byte[] BuildDerElement(byte tag, byte[] content)
        {
            byte[] lenBytes;
            if (content.Length < 128)
            {
                lenBytes = new byte[] { (byte)content.Length };
            }
            else if (content.Length <= 0xFF)
            {
                lenBytes = new byte[] { 0x81, (byte)content.Length };
            }
            else if (content.Length <= 0xFFFF)
            {
                lenBytes = new byte[] { 0x82, (byte)(content.Length >> 8), (byte)(content.Length & 0xFF) };
            }
            else
            {
                lenBytes = new byte[] { 0x83, (byte)(content.Length >> 16), (byte)((content.Length >> 8) & 0xFF), (byte)(content.Length & 0xFF) };
            }

            byte[] res = new byte[1 + lenBytes.Length + content.Length];
            res[0] = tag;
            Array.Copy(lenBytes, 0, res, 1, lenBytes.Length);
            Array.Copy(content, 0, res, 1 + lenBytes.Length, content.Length);
            return res;
        }

        #endregion
    }
}
