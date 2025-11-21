# 🏠 RealEstateApp - Premium Property Listing Platform

A full-stack Real Estate management system built with **ASP.NET Core MVC** and **Entity Framework Core**. This application provides a seamless experience for buyers to browse properties, calculate mortgages, and contact agents, while offering a robust Admin Dashboard for managing listings, leads, and agents.
<img width="1854" height="989" alt="homepage" src="https://github.com/user-attachments/assets/10fa5466-ff04-4f01-879b-b554c776c639" />

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
| ![Home](https://via.placeholder.com/400x200?text=Home+Page) | ![Details](https://via.placeholder.com/400x200?text=Details+Page) |

| Admin Dashboard | User Dashboard |<img width="1855" height="904" alt="user dashboard" src="https://github.com/user-attachments/assets/feec324a-1e92-46b4-b827-a87d1b3345ac" />
<img width="1854" height="989" alt="homepage" src="https://github.com/user-attachments/assets/5cf40a2b-0409-4938-966d-fb98046080b8" />
<img width="1852" height="986" alt="detalispage" src="https://github.com/user-attachments/assets/02584a46-a6af-49af-857c-74602ccc1a7a" />
<img width="1852" height="991" alt="admin dashboard" src="https://github.com/user-attachments/assets/448bc0da-8874-41d6-b150-8962c5a56cba" />

|:---:|:---:|
| ![Admin](https://via.placeholder.com/400x200?text=Admin+Dashboard) | ![User](https://via.placeholder.com/400x200?text=User+Dashboard) |

## 🛠️ Installation & Setup

1.  **Clone the repository**
    ```bash
    git clone [https://github.com/satish051/RealEstateApp.git](https://github.com/YOUR-USERNAME/RealEstateApp.git)
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
