USE webstore;

INSERT INTO AspNetRoles VALUES ('1', 'Admin', 'ADMIN', '1');
INSERT INTO AspNetUserRoles (UserId, RoleId) 
	SELECT us.Id, rol.Id FROM AspNetUsers us, AspNetRoles rol 
		WHERE us.UserName LIKE 'iduarte' 
		AND rol.Name LIKE 'Admin';
