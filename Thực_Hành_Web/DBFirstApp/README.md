# DBFirstApp

Ứng dụng MVC minh họa Database First trên cùng cơ sở dữ liệu `School` mà dự án Code First tạo. Các entity và `SchoolContext` trong `Models` có cấu trúc tương đương mã sinh từ database, còn `LearnersController` cùng các Razor view cung cấp CRUD.

## Kết nối

Mặc định `appsettings.json` dùng Windows Authentication tới SQL Server cục bộ (`localhost`). Có thể thay `ConnectionStrings:SchoolContext` bằng SQL Server instance của máy. Không lưu mật khẩu thật vào file được commit.

## Sinh lại model từ database

Trong Package Manager Console, đặt project mặc định là `DBFirstApp` rồi chạy:

```powershell
Scaffold-DbContext "Server=localhost;Database=School;Trusted_Connection=True;TrustServerCertificate=True" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models -Context SchoolContext -Force
```

Gói `Microsoft.EntityFrameworkCore.Design` và provider SQL Server đã được khai báo trong project.
