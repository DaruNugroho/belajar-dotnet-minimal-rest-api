# ---------------------
- ubuntu 22.04 LTS
- 8.0.110 LTS

# package yang perlu di install
- dotnet dev-certs https --trust
- dotnet add package Microsoft.EntityFrameworkCore.InMemory --prerelease
- dotnet add package Microsoft.AspNetCore.Diagnostics.EntityFrameworkCore --prerelease
- dotnet add package NSwag.AspNetCore

# jalankan project
- dotnet run

# uji api dengan tools swagger
buka link 
- https://localhost:<port_aplikasi>/swagger

# link tutorial
- [https://learn.microsoft.com/aspnet/core/tutorials/min-web-api?WT.mc_id=dotnet-35129-website]
