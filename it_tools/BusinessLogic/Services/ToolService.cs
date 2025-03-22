using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using ToolLib;

namespace it_tools.BusinessLogic.Services
{
    class ToolService
    {
        public static ITool? LoadToolFromDll(string dllPath)
        {
            try
            {
                // 🔹 Lấy thư mục gốc của ứng dụng
                string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
                string absolutePath = Path.Combine("C:\\Users\\dient\\source\\repos\\it_tools\\it_tools\\bin\\x86\\Debug\\net6.0-windows10.0.19041.0\\win10-x86\\RandomStringTool.dll", "");

                Debug.WriteLine($"🔹 Loading plugin from: {absolutePath}");

                // ❌ Kiểm tra xem file có tồn tại không
                if (!File.Exists(absolutePath))
                {
                    Debug.WriteLine($"❌ DLL không tồn tại: {absolutePath}");
                    return null;
                }

                // ✅ Load DLL
                Assembly assembly = Assembly.LoadFrom(absolutePath);

                // 🔍 Debug: Liệt kê tất cả các type có trong DLL
                foreach (var type in assembly.GetTypes())
                {
                    Debug.WriteLine($"🔹 Found type: {type.FullName}");
                }

                // 🔍 Tìm class implement `ITool`
                var toolTypes = assembly.GetTypes()
                    .Where(t => typeof(ITool).IsAssignableFrom(t) && !t.IsInterface)
                    .ToList();

                if (!toolTypes.Any())
                {
                    Debug.WriteLine("❌ Không tìm thấy class nào implement ITool trong DLL.");
                    return null;
                }

                // ✅ Chọn class đầu tiên
                Type toolType = toolTypes.First();
                Debug.WriteLine($"✅ Found ITool implementation: {toolType.FullName}");

                // 🔍 Kiểm tra constructor có hợp lệ không
                var constructor = toolType.GetConstructor(Type.EmptyTypes);
                if (constructor == null)
                {
                    Debug.WriteLine($"❌ Class {toolType.Name} không có constructor mặc định.");
                    return null;
                }

                // ✅ Tạo instance từ class
                try
                {
                    var instance = (ITool)Activator.CreateInstance(toolType)!;
                    Debug.WriteLine($"✅ Successfully created instance of {toolType.FullName}");
                    return instance;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"❌ Lỗi khi tạo instance của {toolType.FullName}: {ex.Message}");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"❌ Lỗi khi load tool từ {dllPath}: {ex.Message}");
            }

            return null;
        }
    }
}
