# Event-management-system 📅

A full-stack solution for managing events and attendees with ease and reliability! Built with **.NET (C#)**, **MySQL**, and **React**, this project demonstrates best practices in architecture, clean code, and user experience.

---

## 🚀 Project Overview

The Event Management System enables organizations to efficiently create, update, and manage events while streamlining attendee registration and analytics. Designed for scalability and maintainability, the system uses a layered backend and a responsive, professional frontend.

- **Objective:** Simplify event organization, maximize attendee management, and provide analytics.
- **Target Users:** Event organizers and administration.
- **Technologies:** .NET 8 (C#), React, MySQL, Entity Framework Core, SCSS.

---

## ✨ Key Features

### Event Management
- Full CRUD: Create, Read, Update (future events only), Delete
- Pagination, search, and advanced filtering by date, location, or tags
- Tag events for easy categorization
- Input validation and error handling for all API endpoints

### Attendee Management
- Register attendees to events with validation for duplicates, event capacity, and event status (no registration for full or past events)
- View all registered attendees for any event

### Analytics & Reporting
- Real-time analytics: total attendees, capacity utilization per event

### UI/UX & Frontend
- Responsive design using React and SCSS
- Reusable components
- Confirmation modals for critical actions

---

## 🏗️ Architecture

The system follows a **Layered Architecture** for clarity and maintainability:

- **Presentation Layer:** React frontend for UI and UX
- **Controller Layer:** ASP.NET Core controllers for routing and API
- **Service Layer:** Business logic encapsulated in services
- **Data Access Layer:** Entity Framework Core for MySQL interactions
- **Database Layer:** MySQL database with tables for events, attendees, and registrations

---

## 💻 Technology Stack

| Component        | Technology              |
|------------------|------------------------|
| Programming      | C# (.NET 8)            |
| API Framework    | ASP.NET Core Web API    |
| Frontend         | React (JavaScript/TS)   |
| Database         | MySQL (EF Core Object relational mapping)     |
| Styling          | SCSS                    |

---

## 🚀 Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Node.js](https://nodejs.org/)
- [MySQL Community Server](https://dev.mysql.com/downloads/mysql/)
- (Optional) [EF Core CLI](https://learn.microsoft.com/en-us/ef/core/cli/dotnet)  
  Install via terminal:  
  ```sh
  dotnet tool install --global dotnet-ef
  ```

### Installation

1. **Clone the Repository**
    ```sh
    git clone https://github.com/vidura13/event-management-system
    cd event-management-system
    ```

2. **Set Up MySQL**
    - Create a database (e.g., `event_management`).
    - Update your connection string in `api/appsettings.json`:
      ```json
      "ConnectionStrings": {
        "DefaultConnection": "server=localhost;port=3306;database=event_management;user=root;password=YOUR_SERVER_PASSWORD"
      }
      ```

3. **Backend Setup (.NET 8)**
    ```sh
    cd api
    dotnet restore
    dotnet ef database update
    dotnet run
    ```
    - The API runs at `https://localhost:5001` by default.

4. **Frontend Setup (React)**
    ```sh
    cd client
    npm install
    npm start
    ```
    - The app opens at [http://localhost:3000](http://localhost:3000). (Default)
    - Ensure your API URL is set in `client/.env`:
      ```
      REACT_APP_API_URL=https://localhost:5001
      ```

---

## 📚 API Endpoints

### Events
- `GET    /api/events` – List all events (pagination, filtering)
- `POST   /api/events` – Create new event
- `PUT    /api/events/{id}` – Update event (future events only)
- `DELETE /api/events/{id}` – Delete event (cascade delete attendees)
- `GET    /api/events/{id}` – Get event details

### Attendees
- `POST   /api/events/{eventId}/register` – Register attendee (with validations)
- `GET    /api/events/{eventId}/attendees` – List attendees for an event

### Analytics
- `GET    /api/events/{eventId}/analytics` – Get event analytics (total attendees, capacity utilization)

_All endpoints validate input and return clear JSON messages._

---

## 🖥️ Project Structure

```
/event-management-system
  /EventManagementSystem.API              # .NET 8 backend (C#)
    /Controllers
    /DTOs
    /Models
    /Services
    appsettings.json
    ...
  /event-management-frontend           # React frontend
    /components
    /pages
    /services
    /styles
    .env
    ...
  README.md
```

---

## 📝 Design Decisions

- **Separation of Concerns:** Controllers, services, DTOs, and models are cleanly separated
- **DTOs:** Used for secure and maintainable data transfer
- **Service Layer:** Centralized business logic
- **Reusable Components:** Modular React components for UI
- **Responsive Design:** Mobile-first and accessible

---

## 🙌 Acknowledgments

Thanks to the open-source community for tools like .NET, React, MySQL, and more.

---

## 📬 Contact

For questions or support, please reach out at viduravd@gmailcom.

---

## 📸 Screenshots


---

## 🏷️ Version

Version: 1.0.0
Last Updated: June 9, 2025
