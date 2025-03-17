# IT Tools for Devs

## 📂 Cấu trúc thư mục
Dự án tuân theo mô hình 3 lớp để đảm bảo tính mở rộng và dễ bảo trì.

```
ITToolsApp/
│── Data/              # Tầng truy cập dữ liệu (DAL)
│   ├── DatabaseContext.cs    # Kết nối SQL Server
│   ├── PluginRepository.cs   # Xử lý truy vấn Plugin
│── Business/          # Tầng nghiệp vụ (BLL)
│   ├── Models/        # Các Model dữ liệu
│   │   ├── Plugin.cs  # Định nghĩa Plugin
│   │   ├── User.cs    # Định nghĩa User
│   ├── Services/      # Xử lý nghiệp vụ
│   │   ├── PluginService.cs  # Logic liên quan đến Plugin
│── Presentation/      # Tầng giao diện (UI - WinUI 3)
│   ├── Views/         # Giao diện XAML
│   ├── ViewModels/    # Xử lý dữ liệu cho UI
│── Plugins/           # Chứa DLL của các Plugin (Hot Plug)
│── README.md          # Tài liệu hướng dẫn
│── .gitignore         # Danh sách file cần bỏ qua khi commit
```

## 📜 Cách Commit
Dự án sử dụng quy chuẩn commit theo **Conventional Commits** để giúp quá trình theo dõi lịch sử thay đổi dễ dàng hơn.

### 🎯 Quy tắc đặt tên commit:
```
<type>(<scope>): <message>
```
- **type**: `feat` (chức năng mới), `fix` (sửa lỗi), `refactor` (cải tiến code), `docs` (cập nhật tài liệu), `test` (bổ sung test), `chore` (cấu hình)
- **scope**: Thành phần của dự án (vd: `UI`, `API`, `DB`, `Plugin`)
- **message**: Mô tả ngắn gọn thay đổi

### 🔥 Ví dụ commit chuẩn:
```bash
git commit -m "feat: Thêm màn hình quản lý Plugin"
git commit -m "fix: Sửa lỗi truy vấn khi lấy danh sách Plugin"
git commit -m "docs: Cập nhật hướng dẫn cài đặt"
```

## 🚀 Tech Stack
Dự án sử dụng các công nghệ sau:

| Thành phần  | Công nghệ |
|-------------|-----------|
| **Frontend**  | WinUI 3 (C#) |
| **Backend**  | Node.js + Express.js |
| **Database**  | SQL Server |
| **Plugin System** | Dynamic DLL loading (C# Reflection) |
| **Version Control** | Git + GitHub |

💡 **Lưu ý**: Hệ thống hỗ trợ **Hot Plug**, cho phép admin tải DLL vào runtime mà không cần khởi động lại ứng dụng.

---
✍️ **Contributor**: *[Tên nhóm của bạn]*
