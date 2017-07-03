-- Production to Staging
declare @OldDomain nvarchar(255) = 'miche.com'
declare @NewDomain nvarchar(255) = 'michestaging.com'
declare @OldMailDomain nvarchar(255) = 'mymiche.com'
declare @NewMailDomain nvarchar(255) = 'mymichestaging.com'

-- Update SiteUrls
update su set Url = REPLACE(Url, @OldDomain, @NewDomain) from SiteUrls su where su.Url like '%' + @OldDomain + '%'

-- Update ProxyLinks
update pl set Url = REPLACE(Url, @OldDomain, @NewDomain) from ProxyLinks pl where pl.URL like '%' + @OldDomain + '%'

-- Update MailDomains
update md set DomainName = REPLACE(DomainName, @OldMailDomain, @NewMailDomain) from MailDomains md where md.DomainName like '%' + @OldMailDomain + '%'

-- Update MailAccounts
update ma set EmailAddress = REPLACE(EmailAddress, @OldMailDomain, @NewMailDomain) from MailAccounts ma where ma.EmailAddress like '%' + @OldMailDomain + '%'

-- Update HTML elements
update he set he.Contents = REPLACE(he.Contents, 'http://workstation.' + @OldDomain + '/FileUploads/', '/FileUploads/')
	from Htmlelements he where he.Contents like '%http://workstation.' + @OldDomain + '/FileUploads/%'
update he set he.Contents = REPLACE(he.Contents, 'http://portal.' + @OldDomain + '/FileUploads/', '/FileUploads/')
	from Htmlelements he where he.Contents like '%http://portal.' + @OldDomain + '/FileUploads/%'
update he set he.Contents = REPLACE(he.Contents, 'http://base.' + @OldDomain + '/FileUploads/', '/FileUploads/')
	from Htmlelements he where he.Contents like '%http://base.' + @OldDomain + '/FileUploads/%'
update he set he.Contents = REPLACE(he.Contents, 'https://workstation.' + @OldDomain + '/FileUploads/', '/FileUploads/')
	from Htmlelements he where he.Contents like '%https://workstation.' + @OldDomain + '/FileUploads/%'
update he set he.Contents = REPLACE(he.Contents, 'https://portal.' + @OldDomain + '/FileUploads/', '/FileUploads/')
	from Htmlelements he where he.Contents like '%https://portal.' + @OldDomain + '/FileUploads/%'
update he set he.Contents = REPLACE(he.Contents, 'https://base.' + @OldDomain + '/FileUploads/', '/FileUploads/')
	from Htmlelements he where he.Contents like '%https://base.' + @OldDomain + '/FileUploads/%'
