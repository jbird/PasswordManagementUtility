# Password Management Utility
A password manager program to store and maintain your passwords securely.

### Database ###
Passwords are compressed with GZip and encrypted using AES-256 in CBC mode. 

### Master Key ###
The master key is generated by passing the password into a PBKDF2 (Password-Based Key Derivation Function 2) function, with eight thousand iterations, the output is then hashed using SHA256.
