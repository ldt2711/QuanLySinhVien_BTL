# 📚 HỆ THỐNG QUẢN LÝ SINH VIÊN  
### Dự án môn học: Lập trình Web – ASP.NET Core MVC  

## 🧭 Giới thiệu  
Dự án **Quản lý Sinh viên** được xây dựng nhằm hỗ trợ các hoạt động quản lý trong môi trường đại học bao gồm:  
- Quản lý **Sinh viên**, **Giảng viên**, **Lớp học phần**, **Khoa**, **Ngành**  
- Chức năng **đăng nhập, phân quyền** cho từng vai trò (Admin / Giảng viên / Sinh viên)  
- Hỗ trợ **đăng ký lớp học phần**, **nhập điểm**, **xem điểm** và **quản lý thông tin cá nhân**  

Hệ thống được phát triển theo mô hình **MVC (Model – View – Controller)** sử dụng **ASP.NET Core MVC**, **Entity Framework Core**, và **SQL Server**.  

## ⚙️ Công nghệ sử dụng  
| Thành phần | Công nghệ |
|-------------|------------|
| Frontend | HTML5, CSS3, Bootstrap, Razor View |
| Backend | ASP.NET Core MVC 8.0 |
| ORM | Entity Framework Core |
| Database | Microsoft SQL Server |
| Authentication | ASP.NET Identity |
| IDE khuyến nghị | Visual Studio 2022 / Rider |

## 🚀 Hướng dẫn cài đặt  
### 1️⃣ Clone dự án  

```bash
git clone https://github.com/<tên-người-dùng>/<tên-repo>.git
cd <tên-repo>
```

### 2️⃣ Cấu hình CSDL

```bash
Chỉnh appsettings.json:

"ConnectionStrings": {
  "DefaultConnection": "Server=<tên-server-của-bạn>;Database=QuanLySinhVien;Trusted_Connection=True;TrustServerCertificate=True;"
}
```

### 3️⃣ Khởi tạo Database

```bash
update-database
```

### 4️⃣ Chạy ứng dụng

```bash
dotnet run
```

### 5️⃣ Tài khoản admin

```bash
Email: admin@qlsv.com
Admin123!
```
