using System;
using System.Text;
using System.Security.Cryptography;

namespace MtbBillCollectionLibrary.Security
{
    public class PublicKeyInfo
    {
        public string ChipheText { get; set; }
        public string Key { get; set; }
    }
    public class EncryptionDecryptionManager
    {

      public enum HashAlgorithmS
        {
          SHA1,
          SHA256,
          SHA384,
          SHA512,
          Hash 
        }

      public int PrivateKey { get; set; }

      public string  FloraIBankingEncrypt(string  chipperText)
      {
          string encryptText = "";
          char[] chipperArray = chipperText.ToCharArray();
          for(int i = 0; i < chipperArray.Length; i++)
             {
                 encryptText = encryptText + (chipperArray[i] + 80);
             }
          return encryptText;
      }

      public string FloraIBankingDecrypt(string encryptText)
      {
          string chipperText = "";
          char[] encryptTextInArray = encryptText.ToCharArray();
          for(int i = 0; i < encryptTextInArray.Length; i++)
          {
              int value =int.Parse(Char.ToString((char)encryptTextInArray[i]));
              while (value<80)
              {
                  if (i < encryptTextInArray.Length - 1)
                      i++;
                  else
                      return chipperText;

                  value = (value * 10) + int.Parse(Char.ToString((char)encryptTextInArray[i]));
              }
              chipperText = chipperText + (char)(value - 80);
          }
          return chipperText;
      }

      public PublicKeyInfo GtePublicKeyForRiaEncription(string plainText)
      {
          int p = GetPrimeNo(), q = GetPrimeNo();
         while(p==q)
         {
             q = GetPrimeNo();
         }
          if(p < q)
          {
              int temp = q;
              q = p;
              p = temp;
          }
          PublicKeyInfo publicPublicKey=new PublicKeyInfo();
          publicPublicKey.Key = GetPublicKey(p,q)+"#"+p * q;
          publicPublicKey.ChipheText = GetChipherTextByRia(plainText, publicPublicKey.Key);
          return publicPublicKey;
      }

      private int GetPublicKey(int p,int q)
      {
            UInt64  phiN = (UInt64)((p - 1) * (q - 1));
            int publicKey = 0;
            publicKey = GetCoPrime((int)phiN);
            //for (int i = (int)Math.Sqrt((int)phiN); i < (int)phiN; i++)
            //{
            //    if ((int)phiN % i != 0)
            //        return publicKey=i;
            //}
           return publicKey;
      }

      public string GetChipherTextByRia(string plainText, string key)
      {
          string asciiStream = "";
          try
          {
              short asciiValue = 0;
              
              char[] c = plainText.ToCharArray();
              string[] token = key.Split('#');
              int publicKey = int.Parse(token[0]);
              int mod = int.Parse(token[1]);
              for (int i = 0; i < plainText.Length; i++)
              {
                  asciiValue = (short) c[i];
                  if (i != 0)
                      asciiStream += "@";
                  asciiStream = asciiStream + Math.Pow(asciiValue, publicKey)%mod;
              }
          }catch(Exception e)
          {
              throw e;
          }
          return asciiStream;
      }

      public int GetPrimeNo()
      {
          Random r=new Random();
          bool isPrime = false;
          int prime = 0; 
          do
          {
              prime = r.Next(r.Next(5,20),r.Next(20,100));
              int loopMax = (int)Math.Sqrt(prime) + 1;
              isPrime = true;
              if (prime % 2 == 0)
                  isPrime = false;
              else
              {
                  for (int i = 3; i < loopMax && isPrime; )
                  {
                     if (prime % i == 0)
                          isPrime = false;
                      i = i + 2;
                  } 
              }
              
          } while (isPrime == false);
          return prime;
      }

      public int GetPrivateKey(int publicKey,int mod)
      {
          int privateKey=0;
          bool privateKeyFound = false;
          int i = 1;

          while(!privateKeyFound)
          {
              if ((publicKey * i - 1) % mod == 0)
              {
                  privateKey = i ;
                  privateKeyFound = true;
              }              
              i++;
          }
          return privateKey;
      }

      public string GetPlainText(string chipherText,int privateKey,int mod)
      {
          string[] token = chipherText.Split('@');
          string plaintText = "";
          for(int i=0;i<token.Length;i++)
          {
              int aciValue = (int)(Math.Pow(int.Parse(token[i]), privateKey) % mod);
              plaintText += (char) aciValue;
          }

          return plaintText;
      }

      public int GetCoPrime(int value)
      {
          for (int i = (int)Math.Sqrt((int)value); i < (int)value; i++)
          {
              if (value % i != 0)
              {
                  bool isCoprime = true;
                  for (int j = (int)Math.Sqrt(i); j < i && isCoprime; j++)
                  {
                      if (value % j == 0 && i % j == 0) 
                          isCoprime=false;
                  }
                  if (isCoprime)
                    return i;
              }
                  
          }
          return 1;
      }

      public string ComputeHash(string plainText,byte[] saltBytes)
      {
          // If salt is not specified, generate it on the fly.
          if (saltBytes == null)
          {
              // Define min and max salt sizes.
              int minSaltSize = 4;
              int maxSaltSize = 8;

              // Generate a random number for the size of the salt.
              Random random = new Random();
              int saltSize = random.Next(minSaltSize, maxSaltSize);

              // Allocate a byte array, which will hold the salt.
              saltBytes = new byte[saltSize];

              // Initialize a random number generator.
              RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();

              // Fill the salt with cryptographically strong byte values.
              rng.GetNonZeroBytes(saltBytes);
          }

          // Convert plain text into a byte array.
          byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);

          // Allocate array, which will hold plain text and salt.
          byte[] plainTextWithSaltBytes =
                  new byte[plainTextBytes.Length + saltBytes.Length];

          // Copy plain text bytes into resulting array.
          for (int i = 0; i < plainTextBytes.Length; i++)
              plainTextWithSaltBytes[i] = plainTextBytes[i];

          // Append salt bytes to the resulting array.
          for (int i = 0; i < saltBytes.Length; i++)
              plainTextWithSaltBytes[plainTextBytes.Length + i] = saltBytes[i];

          // Because we support multiple hashing algorithms, we must define
          // hash object as a common (abstract) base class. We will specify the
          // actual hashing algorithm class later during object creation.
          HashAlgorithm hash = new SHA256Managed();
          // Compute hash value of our plain text with appended salt.
          byte[] hashBytes = hash.ComputeHash(plainTextWithSaltBytes);

          // Create array which will hold hash and original salt bytes.
          byte[] hashWithSaltBytes = new byte[hashBytes.Length +
                                              saltBytes.Length];

          // Copy hash bytes into resulting array.
          for (int i = 0; i < hashBytes.Length; i++)
              hashWithSaltBytes[i] = hashBytes[i];

          // Append salt bytes to the result.
          for (int i = 0; i < saltBytes.Length; i++)
              hashWithSaltBytes[hashBytes.Length + i] = saltBytes[i];

          // Convert result into a base64-encoded string.
          string hashValue = Convert.ToBase64String(hashWithSaltBytes);

          // Return the result.
          return hashValue;
      }

      /// <summary>
      /// Compares a hash of the specified plain text value to a given hash
      /// value. Plain text is hashed with the same salt value as the original
      /// hash.
      /// </summary>
      /// <param name="plainText">
      /// Plain text to be verified against the specified hash. The function
      /// does not check whether this parameter is null.
      /// </param>
      /// <param name="hashAlgorithm">
      /// Name of the hash algorithm. Allowed values are: "MD5", "SHA1", 
      /// "SHA256", "SHA384", and "SHA512" (if any other value is specified,
      /// MD5 hashing algorithm will be used). This value is case-insensitive.
      /// </param>
      /// <param name="hashValue">
      /// Base64-encoded hash value produced by ComputeHash function. This value
      /// includes the original salt appended to it.
      /// </param>
      /// <returns>
      /// If computed hash mathes the specified hash the function the return
      /// value is true; otherwise, the function returns false.
      /// </returns>
      public bool VerifyHash(string plainText,string hashValue)
      {
          // Convert base64-encoded hash value into a byte array.
          byte[] hashWithSaltBytes = Convert.FromBase64String(hashValue);

          // We must know size of hash (without salt).
          int  hashSizeInBits = 256, hashSizeInBytes;

          // Make sure that hashing algorithm name is specified.
         HashAlgorithm hash = new SHA256Managed();
          // Convert size of hash from bits to bytes.
          hashSizeInBytes = hashSizeInBits / 8;

          // Make sure that the specified hash value is long enough.
          if (hashWithSaltBytes.Length < hashSizeInBytes)
              return false;

          // Allocate array to hold original salt bytes retrieved from hash.
          byte[] saltBytes = new byte[hashWithSaltBytes.Length -
                                      hashSizeInBytes];

          // Copy salt from the end of the hash to the new array.
          for (int i = 0; i < saltBytes.Length; i++)
              saltBytes[i] = hashWithSaltBytes[hashSizeInBytes + i];

          // Compute a new hash string.
          string expectedHashString =
                      ComputeHash(plainText , saltBytes);

          // If the computed hash matches the specified hash,
          // the plain text value must be correct.
          return (hashValue == expectedHashString);
      }
    }



}
