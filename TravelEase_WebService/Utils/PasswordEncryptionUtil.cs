/*
------------------------------------------------------------------------------
 File: PasswordEncryptionUtil.cs
 Purpose: This file contains the PasswordEncryptionUtil class, which provides
 utility methods for password hashing and verification in the TravelEase_WebService project.
 Author: IT20122096
 Date: 2023-10-13
------------------------------------------------------------------------------
*/
namespace TravelEase_WebService.Utils
{
    public class PasswordEncryptionUtil
	{
        //------------------------------------------------------------------------------
        // Method: HashPassword
        // Purpose: Hashes the given password.
        //------------------------------------------------------------------------------
        public string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        //------------------------------------------------------------------------------
        // Method: VerifyPassword
        // Purpose: Verifies the entered password against the hashed password.
        //------------------------------------------------------------------------------
        public bool VerifyPassword(string enteredPassword, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(enteredPassword, hashedPassword);
        }
    }
}

