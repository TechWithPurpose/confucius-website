# Confucius Classroom Website

> **Status: Discontinued / Incomplete Project**

A web application prototype developed for the Confucius Classroom at Ruse University "Angel Kanchev". The goal of the project was to create a dedicated online platform for presenting information, activities, events, and resources related to the Confucius Classroom.

The project was developed as a personal project over approximately two years on an intermittent basis. Although the application was not completed, a significant part of the planning, design, database architecture, and administration functionality was implemented.

## 📌 Project Overview

The Confucius Classroom at Ruse University "Angel Kanchev" did not have a dedicated website. This project was intended to provide a centralized platform where visitors could access information about the organization, its activities, events, courses, and other relevant content.

The application was designed with two primary areas:

* **Public/User Side** — accessible to visitors.
* **Administration Side** — used to manage website content and data.

At the current stage of development, the administration side is partially implemented and functional, while the public-facing side remains incomplete.

## 🎯 Project Objectives

The main objectives of the project were to:

* Create a dedicated online presence for the Confucius Classroom.
* Provide a centralized source of information for students and visitors.
* Allow administrators to manage website content.
* Apply software engineering practices during the planning and development process.
* Gain practical experience with ASP.NET Core MVC, C#, SQL Server, database design, and web application architecture.

## 🛠️ Technologies Used

| Technology                              | Purpose                                    |
| --------------------------------------- | ------------------------------------------ |
| **C#**                                  | Application development                    |
| **ASP.NET Core MVC**                    | Web application framework and architecture |
| **SQL Server**                          | Database management                        |
| **SQL Server Management Studio (SSMS)** | Database design and administration         |
| **HTML**                                | Page structure                             |
| **CSS**                                 | Styling and user interface                 |
| **JavaScript**                          | Client-side functionality                  |

## 🧩 Software Engineering & Planning

Before beginning the implementation, the project went through several planning and design stages.

### User Stories

The system requirements were initially defined through user stories describing the expected functionality from both user and administrator perspectives.

The user roles include:

* Visitor/User
* Administrator

## 📋 User Stories

The functional requirements for the system were initially defined using user stories.

![View the complete User Stories document](https://github.com/TechWithPurpose/confucius-website/blob/c8c34fe189573ca36f37a474b298d6cb77e4909a/docs/Confucius%20website%20-%20User%20Stories.png)

### Wireframes

Wireframes were created for the planned pages and interfaces before implementation.

These included designs for:

* [Home Page]
* [About Page]
* [Events Page]
* [Courses Page]
* [Contact Page]
* [Admin Dashboard]
* [Other pages]

Some of the wireframes:

![Example wireframe#1](https://github.com/TechWithPurpose/confucius-website/blob/91dfd2fff2ab0a48ef7d48a93eb01151fe7a1d35/screenshots/WireFrames%20(5).png)
![Example wireframe#2](https://github.com/TechWithPurpose/confucius-website/blob/91dfd2fff2ab0a48ef7d48a93eb01151fe7a1d35/screenshots/WireFrames%20(4).png)
![Example wireframe#3](https://github.com/TechWithPurpose/confucius-website/blob/91dfd2fff2ab0a48ef7d48a93eb01151fe7a1d35/screenshots/WireFrames%20(16).png)
![Example wireframe#4](https://github.com/TechWithPurpose/confucius-website/blob/91dfd2fff2ab0a48ef7d48a93eb01151fe7a1d35/screenshots/WireFrames%20(15).png)

📄 **[(Link to all of the Wireframes)]**(https://github.com/TechWithPurpose/confucius-website/tree/c8c34fe189573ca36f37a474b298d6cb77e4909a/docs/wireframes)  

## 🗄️ Database Design

The database was designed and implemented using Microsoft SQL Server.

The database schema was created to support the planned functionality of the application, including the management of [users / events / news / courses / other entities].

![Database Schema](https://github.com/TechWithPurpose/confucius-website/blob/91dfd2fff2ab0a48ef7d48a93eb01151fe7a1d35/docs/database-schema.png )



### Main Entities

The database includes entities such as:

* `Users`
* `Events`
* `Images`
* `ClassSchedule`

## 💻 Implemented Functionality

The project is incomplete; however, several components were implemented.

### Administration Area

The administration side is the most developed part of the application.

Implemented or partially implemented functionality includes:

* [ ] Administrator authentication
* [ ] Admin dashboard
* [ ] Create content
* [ ] Edit content
* [ ] Delete content
* [ ] Manage events
* [ ] Manage courses
* [ ] Manage news
* [ ] Manage users
* [ ] Upload/manage images
* [ ] Other functionality

### Public/User Area

The public-facing side of the website was planned but remains incomplete.

Planned functionality included:

* [ ] Viewing information about the Confucius Classroom
* [ ] Viewing news and announcements
* [ ] Viewing upcoming events
* [ ] Accessing information about courses and activities
* [ ] Contact information
* [ ] Signing up for classes

## 📸 Screenshots

### Administration Panel

#### [Admin Dashboard]

![Screenshot](https://github.com/TechWithPurpose/confucius-website/blob/65d8b6a5fd664ee5d112a1031ae20668701d777b/screenshots/Screenshot%202026-08-20%20105103.png)

**Description:** Displays the most recent activity updates.

---

#### [News Dashboard]

![Screenshot](https://github.com/TechWithPurpose/confucius-website/blob/65d8b6a5fd664ee5d112a1031ae20668701d777b/screenshots/Screenshot%202026-08-20%20105204.png)

**Description:** Shows all of the published and to-be published news articles.


#### [Edit news article]

![Screenshot](https://github.com/TechWithPurpose/confucius-website/blob/65d8b6a5fd664ee5d112a1031ae20668701d777b/screenshots/Screenshot%202026-08-20%20105228.png)

**Description:** Allows changes to be made on an already published or drafted article.

]## 📂 Project Structure

```text
ConfuciusWebsite/
│
├── Areas/
├── Controllers/
├── Data/
├── Models/
├── Properties/
├── Services/
├── ViewModels/
├── Views/
├── wwwroot/
├── ConfuciusWebsite.csproj
├── Program.cs
├── appsettings.Development.json
├── appsettings.json
│
docs/
screenshots/
.gitignore
ConfuciusWebsite.sln
PROJECT-NOTES
README.md
```

## 🚀 Getting Started

### Prerequisites

To run the project locally, you will need:

* [.NET 8.0]
* Microsoft SQL Server
* SQL Server Management Studio (optional)
* Visual Studio / Visual Studio Code

### Installation

1. Clone the repository.

```bash
git clone [repository-url]
```

2. Open the project in Visual Studio.

3. Configure the database connection string in:

```text
appsettings.json
```

4. Run the application.

5. Admin Demo
The project includes a functional administration area.

**Admin URL:**

`/Admin/Admin/Dashboard`

**Demo credentials:**

| Field | Value |
|---|---|
| Email | `admin@example.com` |
| Password | `Admin123!` |

> These are demonstration credentials for the local development version of the project and do not provide access to any production system or external service.

## ⚠️ Project Status

Development was discontinued because the scope of the project gradually became significantly larger and more complex than originally anticipated.

Rather than continuing to invest time into completing every planned feature, I decided to preserve the project as a record of my work and learning process.

The project demonstrates experience with:

* Requirements analysis using user stories.
* Wireframing and interface planning.
* Relational database design.
* SQL Server and SSMS.
* ASP.NET Core MVC architecture.
* C# web application development.
* CRUD operations and administrative functionality.
* Designing and developing a larger multi-component application.

## 📚 Documentation

The following project documentation is included in this repository:

* 📄 User Stories
* 🎨 Wireframes
* 🗄️ Database Schema
* 📸 Application Screenshots

## 🧠 What I Learned

This project provided practical experience with the full lifecycle of a software project, from the initial requirements and interface design to database architecture and implementation.

Some of the main lessons from this project include:

* The importance of properly defining and controlling project scope.
* How quickly feature requirements can increase the complexity of an application.
* The value of planning through user stories and wireframes before implementation.
* Designing relationships between entities in a relational database.
* Applying the MVC architectural pattern in a real project.
* The challenges involved in maintaining consistency between requirements, database design, and application logic.

## 👤 Author

**Nuray Salim**
