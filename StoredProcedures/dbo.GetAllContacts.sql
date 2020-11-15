CREATE PROCEDURE [dbo].[GetAllContacts]
	@firstName varchar(30),
	@lastName varchar(30)
AS
begin
	SELECT CD.* , Z1.city,Z1.State,CM1.AddressBookName,CM1.ContactType from contactdetails CD inner join ZipTable Z1 on CD.Zip = Z1.Zip
	inner join(select AddressBookName,ContactType, FirstName,LastName from BookNameContactType BC 
				inner join ContactTypeMap CM on BC.NameTypeid = CM.NameTypeid) CM1 
    on CD.FirstName = CM1.FirstName and CD.LastName=CM1.LastName
end
RETURN 0

