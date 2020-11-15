CREATE PROCEDURE [dbo].[AddNewContact]
	@FirstName varchar(30),
	@LastName varchar(30),
	@phoneNumber varchar(10),
	@Email varchar(50),
	@address varchar(60),
	@zip varchar(6),
	@city varchar(20),
	@state varchar(25),
	@addressBookName varchar(60),
	@contactType varchar(60)
AS
begin
begin try
begin transaction
Declare @NameTypeId int = null
Declare @zipexists int = null
Declare @contactCount int = null
-- Check if that addressbook and contact type exist or not and act accordingly
select @NameTypeId = count(*) from BookNameContactType where AddressBookName = @addressBookName and ContactType = @contactType
if(@NameTypeId = 0)
begin
insert into BookNameContactType values(@addressBookName, @contactType)
end
select @NameTypeId = NameTypeid from BookNameContactType where AddressBookName = @addressBookName and ContactType = @contactType
-- Check if zip already exists
select @zipexists = count(*) from ZipTable where zip = @zip
if(@zipexists = 0)
begin
insert into ZipTable values(@zip,@city,@state)
end
-- insert into contact details
select @contactCount = Count(*) from contactdetails where FirstName = @FirstName and LastName = @LastName
if(@contactCount = 0)
begin
insert into contactdetails values (@firstName,@LastName,@phoneNumber,@Email,@address,@zip, GETDATE())
end
insert into ContactTypeMap values (@FirstName,@LastName,@NameTypeId)
commit transaction
return 0
end try
begin catch
rollback transaction
end catch
end
RETURN -1
