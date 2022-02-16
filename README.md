<<<<<<< HEAD
# invoice
=======
# invoice

## Logging
Logging uchun quyidagi qoidalarga amal qiling.
- readonly logging objectni `_logger` deb nomlang
- `$""` string interpolation ishlatmang. o'rniga Loggging params[] ni ishlating
  - masalan: _User created with ilmhub.uz@gmail.com_ degan habarni `_logger.LogInformation("New user created", user.email);` deb log qilish kerak.
- Exception yuz berganda log qilayotganingizda exception objectni ham Loggerga berish kerak
  - masalan: `catch(Exception exception) { _logger.LogError(exception, "bla bla bla"); }`
- 
>>>>>>> main
