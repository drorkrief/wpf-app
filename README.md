# 🎯 Birth Date Scanner & Dashboard (WPF App)

A sleek and modern WPF application designed for scanning user IDs and displaying a personalized dashboard experience. Built with love using C#, XAML, and good vibes. 🧠✨

---

## 🚀 Features

✅ **Card Scanning UI**  
Prompt the user to scan their card or input their key in a clean, Hebrew-friendly interface.

✅ **Modern Design**  
Gradient backgrounds, soft shadows, blurred glass panels — a clean, young UI you’ll enjoy using.

✅ **Loader Animation**  
A minimal loader lets the user know something’s happening in the background — no mystery waits.

✅ **View Switching**  
Everything happens in a single window with multiple modular views (`UserControls`). Fast and fluid!

---

## 🛠️ Tech Stack

- WPF (.NET)
- C#
- XAML
- MVVM (coming soon!)
- Custom UI Components

---

## ✨ Screenshots

> Coming soon – once the dashboard is fully styled! 😎

---

## 📌 How It Works

1. App launches and shows the **Card Scanner view**.
2. User enters their card ID.
3. If valid → show the **Dashboard view**.
4. If invalid → show error (and maybe a meme 👀).

---

## 🧩 Planned Features (To Be Added)

- 🔐 **WhatsApp Phone Verification**  
  Verify users via WhatsApp messages using `whatsapp-web.js`.

- 🗂️ **Dynamic Dashboard Content**  
  Show user-specific data pulled from a MongoDB database.

- 🌐 **Multi-language Support**  
  Hebrew, English, and maybe even Klingon if we’re feeling brave.

- 🧑‍💼 **Admin Panel**  
  View users, analytics, and logs through a special admin-only dashboard.

- 💬 **Bot Chat Integration**  
  Let users interact with a smart assistant directly in the app.

- 📦 **Portable .EXE Installer**  
  Easily install and distribute the app on Windows machines.

---

## 🧠 For Developers

Each view (Login, Dashboard, etc.) is a separate `UserControl`.  
Navigation is handled in `MainWindow.xaml.cs` using a simple method like:
```csharp
MainContent.Children.Clear();
MainContent.Children.Add(new DashboardView());

---

Let me know if you want to add badges (build status, .NET version, etc.) or want it in Hebrew too — I can remix it however you want!
