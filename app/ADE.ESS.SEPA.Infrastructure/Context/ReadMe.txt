

Update DBRepository from database
Open Package Manager Console and run below command
Scaffold-DbContext "Name=ConnectionStrings:ESSPrivateEDPEntities" Microsoft.EntityFrameworkCore.SqlServer -Project ADE.ESS.SEPA.Infrastructure -StartupProject ADE.ESS.SEPA.Client -OutputDir DBRepositories -ContextDir Context -Context "EssSepaDbContext" -UseDatabaseNames -Force