# 🏠 RealEstateApp - Premium Property Listing Platform

A full-stack Real Estate management system built with **ASP.NET Core MVC** and **Entity Framework Core**. This application provides a seamless experience for buyers to browse properties, calculate mortgages, and contact agents, while offering a robust Admin Dashboard for managing listings, leads, and agents.

![Project Banner]<img width="1858" height="987" alt="Screenshot 2025-11-26 155402" src="https://github.com/user-attachments/assets/ea5d6968-a7ab-480c-9700-7a35dbe8ef0e" />

## 🚀 Tech Stack
* **Framework:** ASP.NET Core 8.0 (MVC)
* **Database:** SQL Server (SSMS) & Entity Framework Core (Code-First)
* **Authentication:** ASP.NET Core Identity (Role-Based Security)
* **Frontend:** Bootstrap 5, JavaScript, CSS3 (Custom Styling)
* **Tools:** Visual Studio 2022, Git

## ✨ Key Features

### 👤 For Users (Buyers/Renters)
* **Advanced Search:** Filter properties by Location, Price Range, and Type (Sale vs Rent).
* **Detailed Listings:** View high-res image carousels, agent details, and property specifications.
* **Mortgage Calculator:** Real-time JavaScript calculator to estimate monthly payments.
* **Favorites/Wishlist:** Save properties to a personal dashboard for later viewing.
* **Inquiries System:** Send messages directly to agents regarding specific properties.
* **User Dashboard:** Track sent inquiries and manage saved homes.

### 🛡️ For Admins
* **Property Management:** Create, Edit, and Delete listings with rich text descriptions.
* **Gallery Manager:** Upload multiple images per property and manage them individually.
* **Agent Management:** Add and manage consultant profiles displayed on the frontend.
* **Inquiry Tracking:** View incoming leads, archive old messages, and track user interest.
* **Role-Based Access:** Secure endpoints ensuring only Admins can access sensitive data.

## 📸 Screenshots

| Home Page | Property Details |
|:---:|:---:|
| ![Home]<img width="1857" height="992" alt="Screenshot 2025-11-26 155631" src="https://github.com/user-attachments/assets/02446a32-9776-418d-bafc-96805daf01ca" />
| ![Details](https://via.placeholder.com/400x200?<img width="1851" height="985" alt="Screenshot 2025-11-26 155720" src="https://github.com/user-attachments/assets/108b47d6-484a-4b2c-8023-283d342e697c" />


| Admin Dashboard | User Dashboard |
|:---:|:---:|
| ![Admin]<img width="1855" height="988" alt="Screenshot 2025-11-26 155915" src="https://github.com/user-attachments/assets/6247b0c4-f148-45b2-8262-a20691aa269e" />
| ![User]<img width="1859" height="991" alt="Screenshot 2025-11-26 155949" src="https://github.com/user-attachments/assets/e20ad8a4-4d45-4775-a636-9446e7299397" />


## 🛠️ Installation & Setup

1.  **Clone the repository**
    ```bash
    git clone [https://github.com/satish051/RealEstateApp.git](https://github.com/satish051/RealEstateApp.git)
    ```

2.  **Configure Database**
    * Open `appsettings.json`.
    * Update the `ConnectionStrings:DefaultConnection` to point to your local SQL Server instance.

3.  **Email Configuration**
    * This app uses Gmail SMTP. Update `EmailSettings` in `appsettings.json` with your credentials (or use a placeholder for testing).

4.  **Run Migrations**
    * Open Package Manager Console in Visual Studio.
    * Run the command:
        ```powershell
        Update-Database
        ```

5.  **Run the App**
    * Press `F5` or run via CLI: `dotnet run`.

## 🔐 Default Login Credentials

To test the Admin features, the application seeds a default admin user on the first run:

* **Admin Email:** `admin@realestate.com`
* **Password:** `Admin@123`

*(For standard user features, please register a new account via the "Register" button).*

## 🔮 Future Improvements
* [ ] Integration with Leaflet.js/Google Maps for property location visualization.
* [ ] Payment Gateway integration for "Featured Listing" payments.
* [ ] Live Chat functionality between Agents and Buyers.

## 📝 License
This project is open-source and available under the [MIT License](LICENSE).

---
**Built with ❤️ in Nepal.**  
