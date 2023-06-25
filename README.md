dotnet tool install --global dotnet-ef

dotnet ef migrations add "Init Database" --project Infrastructure --startup-project WebUI --output-dir Persistence\Migrations

dotnet ef database update --project Infrastructure --startup-project WebUI

#Oracle DB Info:

Host: localhost
Service Name: ORCLCDB
Port: 1521
Username: sys
Pass: 123Aa123Bb
Role: SYSDB

#Web Info:

Username: super_admin@gmail.com
Pass: 123@123Aa

#Database in Web:

SYS

Tables:

SELECT * FROM "ApplicationUsers"
SELECT * FROM "ApplicationRoles"
SELECT * FROM "ApplicationUserClaims"
SELECT * FROM "ApplicationUserRoles"
SELECT * FROM "ApplicationUserLogins"
SELECT * FROM "ApplicationRoleClaims"
SELECT * FROM "ApplicationUserTokens"
SELECT * FROM "Categories"
SELECT * FROM "Orders"
SELECT * FROM "OrderItems"
SELECT * FROM "Payments"
SELECT * FROM "Products"
SELECT * FROM "Tables"
SELECT * FROM "TableOrders"
SELECT * FROM "Units"
SELECT * FROM "TableOrderRequests"
SELECT * FROM "TableBookings"


password mã hoá HASH
